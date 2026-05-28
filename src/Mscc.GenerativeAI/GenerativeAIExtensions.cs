using Microsoft.Extensions.Logging;
using Mscc.GenerativeAI.Types;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Security.Authentication;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Mscc.GenerativeAI
{
    public static class GenerativeAIExtensions
    {
        private static readonly HashSet<string> SensitiveHeaders = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "Authorization",
            "X-API-Key",
            "Proxy-Authorization",
            "x-goog-api-key",
            "x-goog-user-project"
            // Add any other sensitive header names here
        };

        private static readonly JsonSerializerOptions SCredentialOptions = CreateCredentialOptions();

        private static JsonSerializerOptions CreateCredentialOptions()
        {
            var options = new JsonSerializerOptions(JsonSerializerDefaults.Web)
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                NumberHandling = JsonNumberHandling.AllowReadingFromString,
                PropertyNameCaseInsensitive = true,
            };
            options.Converters.Add(new FlexibleEnumConverterFactory());
            options.Converters.Add(new DateTimeFormatJsonConverter());
            return options;
        }

#if NET472_OR_GREATER || NETSTANDARD2_0
        private static readonly Version _httpVersion = HttpVersion.Version11;
        private static readonly HttpClient Client = new HttpClient(new HttpClientHandler
        {
            SslProtocols = SslProtocols.Tls12
        });
#else
        private static readonly Version _httpVersion = HttpVersion.Version11;
        private static readonly HttpClient Client =
            new HttpClient(new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(30), EnableMultipleHttp2Connections = true
            })
            {
                DefaultRequestVersion = _httpVersion, DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrHigher
            };
#endif

        /// <summary>
        /// Checks whether the API key has the right conditions.
        /// </summary>
        /// <param name="apiKey">API key for the Gemini API.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="apiKey"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the <paramref name="apiKey"/> is empty.</exception>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="apiKey"/> has extra whitespace at the start or end, doesn't start with 'AIza', or has the wrong length.</exception>
        public static string? GuardApiKey(this string? apiKey)
        {
            if (apiKey == null) return null;
            if (apiKey.Trim() != apiKey)
                throw new ArgumentException("API key has extra whitespace at the start or end", nameof(apiKey));
            if (apiKey.Length == 39 && !apiKey.StartsWith("AIza", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("API key should start with 'AIza'", nameof(apiKey));
            if (apiKey.Length is not (39 or 53))
                throw new ArgumentException("API key has not the correct length", nameof(apiKey));

            return apiKey;
        }

        /// <summary>
        /// Checks if the functionality is supported by the model.
        /// </summary>
        /// <param name="model">Model to use.</param>
        /// <param name="message">Message to use.</param>
        /// <exception cref="NotSupportedException">Thrown when the functionality is not supported by the model.</exception>
        public static void GuardSupported(this GenerativeModel model, string? message = null)
        {
            message ??= $"Vertex AI or the model `{model.Name}` does not support this functionality.";
            if (model.IsVertexAI) throw new NotSupportedException(message);
        }

        /// <summary>
        /// Checks if the IANA standard MIME type is supported by the model.
        /// </summary>
        /// <remarks>
        /// See <see href="https://ai.google.dev/gemini-api/docs/vision"/> for a list of supported image data and video format MIME types.
        /// See <see href="https://ai.google.dev/gemini-api/docs/audio"/> for a list of supported audio format MIME types.
        /// </remarks>
        /// <param name="mimeType">The IANA standard MIME type to check.</param>
        /// <exception cref="NotSupportedException">Thrown when the <paramref name="mimeType"/> is not supported by the API.</exception>
        public static void GuardInlineDataMimeType(string mimeType)
        {
            string[] allowedMimeTypes =
            [
                "image/jpeg", "image/png", "image/heif", "image/heic", "image/webp",
                "audio/wav", "audio/mp3", "audio/mpeg", "audio/aiff", "audio/aac", "audio/ogg", "audio/flac"
            ];

            if (!allowedMimeTypes.Contains(mimeType, StringComparer.OrdinalIgnoreCase))
                throw new NotSupportedException($"The mime type `{mimeType}` is not supported by the API.");
        }

        /// <summary>
        /// Checks if the IANA standard MIME type is supported by the model.
        /// </summary>
        /// <remarks>
        /// See <see href="https://ai.google.dev/gemini-api/docs/vision"/> for a list of supported image data and video format MIME types.
        /// See <see href="https://ai.google.dev/gemini-api/docs/audio"/> for a list of supported audio format MIME types.
        /// See also <seealso href="https://ai.google.dev/gemini-api/docs/document-processing"/> for a list of supported MIME types for document processing.
        /// Ref: https://developer.mozilla.org/en-US/docs/Web/HTTP/MIME_types/Common_types
        /// </remarks>
        /// <param name="mimeType">The IANA standard MIME type to check.</param>
        /// <exception cref="NotSupportedException">Thrown when the <paramref name="mimeType"/> is not supported by the API.</exception>
        public static void GuardMimeType(string mimeType, ILogger? logger = null)
        {
            ReadOnlyCollection<string> allowedMimeTypes = new List<string>
            {
                "image/jpeg", "image/png", "image/heif", "image/heic", "image/webp",
                "audio/wav", "audio/mp3", "audio/mpeg", "audio/aiff", "audio/aac", "audio/ogg", "audio/flac",
                "video/mp4", "video/mpeg", "video/mov", "video/avi", "video/x-flv", "video/mpg", "video/webm",
                "video/wmv", "video/3gpp",
                "application/pdf", "application/x-javascript", "text/javascript", "application/x-python",
                "application/rtf",
                "text/x-python", "text/plain", "text/html", "text/css", "text/md", "text/csv", "text/xml", "text/rtf"
            }.AsReadOnly();

            if (!allowedMimeTypes.Contains(mimeType, StringComparer.OrdinalIgnoreCase))
            {
                logger?.LogMimeTypeNotVerified(mimeType);
            }
        }

        /// <summary>
        /// A comprehensive and complete list of MIME types supported by the 
        /// Gemini API's File Search feature. This list has been verified to
        /// include all text/x-* types.
        /// Source: https://ai.google.dev/gemini-api/docs/file-search#supported-files
        /// </summary>
        public static void GuardMimeTypeFileSearchStore(string mimeType, ILogger? logger = null)
        {
            ReadOnlyCollection<string> allowedMimeTypes = new List<string>
            {
                // --- Application file types ---
                "application/dart",
                "application/ecmascript",
                "application/json",
                "application/ms-java",
                "application/msword",
                "application/pdf",
                "application/sql",
                "application/typescript",
                "application/vnd.curl",
                "application/vnd.dart",
                "application/vnd.ibm.secure-container",
                "application/vnd.jupyter",
                "application/vnd.ms-excel",
                "application/vnd.oasis.opendocument.text",
                "application/vnd.openxmlformats-officedocument.presentationml.presentation",
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                "application/vnd.openxmlformats-officedocument.wordprocessingml.template",
                "application/x-csh",
                "application/x-hwp",
                "application/x-hwp-v5",
                "application/x-latex",
                "application/x-php",
                "application/x-powershell",
                "application/x-sh",
                "application/x-shellscript",
                "application/x-tex",
                "application/x-zsh",
                "application/xml",
                "application/zip",

                // --- Text file types (Complete) ---
                "text/1d-interleaved-parityfec",
                "text/RED",
                "text/SGML",
                "text/cache-manifest",
                "text/calendar",
                "text/cql",
                "text/cql-extension",
                "text/cql-identifier",
                "text/css",
                "text/csv",
                "text/csv-schema",
                "text/dns",
                "text/encaprtp",
                "text/enriched",
                "text/example",
                "text/fhirpath",
                "text/flexfec",
                "text/fwdred",
                "text/gff3",
                "text/grammar-ref-list",
                "text/hl7v2",
                "text/html",
                "text/javascript",
                "text/jcr-cnd",
                "text/jsx",
                "text/markdown",
                "text/mizar",
                "text/n3",
                "text/parameters",
                "text/parityfec",
                "text/php",
                "text/plain",
                "text/provenance-notation",
                "text/prs.fallenstein.rst",
                "text/prs.lines.tag",
                "text/prs.prop.logic",
                "text/raptorfec",
                "text/rfc822-headers",
                "text/rtf",
                "text/rtp-enc-aescm128",
                "text/rtploopback",
                "text/rtx",
                "text/sgml",
                "text/shaclc",
                "text/shex",
                "text/spdx",
                "text/strings",
                "text/t140",
                "text/tab-separated-values",
                "text/texmacs",
                "text/troff",
                "text/tsv",
                "text/tsx",
                "text/turtle",
                "text/ulpfec",
                "text/uri-list",
                "text/vcard",
                "text/vnd.DMClientScript",
                "text/vnd.IPTC.NITF",
                "text/vnd.IPTC.NewsML",
                "text/vnd.a",
                "text/vnd.abc",
                "text/vnd.ascii-art",
                "text/vnd.curl",
                "text/vnd.debian.copyright",
                "text/vnd.dvb.subtitle",
                "text/vnd.esmertec.theme-descriptor",
                "text/vnd.exchangeable",
                "text/vnd.familysearch.gedcom",
                "text/vnd.ficlab.flt",
                "text/vnd.fly",
                "text/vnd.fmi.flexstor",
                "text/vnd.gml",
                "text/vnd.graphviz",
                "text/vnd.hans",
                "text/vnd.hgl",
                "text/vnd.in3d.3dml",
                "text/vnd.in3d.spot",
                "text/vnd.latex-z",
                "text/vnd.motorola.reflex",
                "text/vnd.ms-mediapackage",
                "text/vnd.net2phone.commcenter.command",
                "text/vnd.radisys.msml-basic-layout",
                "text/vnd.senx.warpscript",
                "text/vnd.sosi",
                "text/vnd.sun.j2me.app-descriptor",
                "text/vnd.trolltech.linguist",
                "text/vnd.wap.si",
                "text/vnd.wap.sl",
                "text/vnd.wap.wml",
                "text/vnd.wap.wmlscript",
                "text/vtt",
                "text/wgsl",
                "text/x-asm",
                "text/x-bibtex",
                "text/x-boo",
                "text/x-c",
                "text/x-c++hdr",
                "text/x-c++src",
                "text/x-cassandra",
                "text/x-chdr",
                "text/x-coffeescript",
                "text/x-component",
                "text/x-csh",
                "text/x-csharp",
                "text/x-csrc",
                "text/x-cuda",
                "text/x-d",
                "text/x-diff",
                "text/x-dsrc",
                "text/x-emacs-lisp",
                "text/x-erlang",
                "text/x-fortran",
                "text/x-gss",
                "text/x-go",
                "text/x-golo",
                "text/x-groovy",
                "text/x-haskell",
                "text/x-handlebars-template",
                "text/x-haxe",
                "text/x-hh",
                "text/x-idl",
                "text/x-inform",
                "text/x-ini",
                "text/x-java",
                "text/x-java-source",
                "text/x-jruby",
                "text/x-jsex",
                "text/x-kotlin",
                "text/x-less",
                "text/x-lilypond",
                "text/x-lisp",
                "text/x-livescript",
                "text/x-log",
                "text/x-lua",
                "text/x-lyx",
                "text/x-makefile",
                "text/x-markdown",
                "text/x-mathematica",
                "text/x-matlab",
                "text/x-nim",
                "text/x-nix",
                "text/x-objcsrc",
                "text/x-ocaml",
                "text/x-opencl-src",
                "text/x-pascal",
                "text/x-perl",
                "text/x-processing",
                "text/x-prolog",
                "text/x-properties",
                "text/x-protobuf",
                "text/x-puppet",
                "text/x-python",
                "text/x-qml",
                "text/x-r",
                "text/x-r-doc",
                "text/x-r-source",
                "text/x-ruby",
                "text/x-rustsrc",
                "text/x-sass",
                "text/x-scala",
                "text/x-scheme",
                "text/x-scss",
                "text/x-sfd",
                "text/x-sh",
                "text/x-smalltalk",
                "text/x-sql",
                "text/x-stylus",
                "text/x-swift",
                "text/x-systemd-unit",
                "text/x-tcl",
                "text/x-tex",
                "text/x-toml",
                "text/x-typescript",
                "text/x-uri",
                "text/x-uuencode",
                "text/x-vala",
                "text/x-vcalendar",
                "text/x-vcard",
                "text/x-verilog",
                "text/x-vhdl",
                "text/x-web-config",
                "text/x-yaml",
                "text/yaml"
            }.AsReadOnly();

            if (!allowedMimeTypes.Contains(mimeType, StringComparer.OrdinalIgnoreCase))
            {
                logger?.LogMimeTypeNotVerified(mimeType);
            }
        }

        /// <summary>
        /// Checks if the language is supported by the model.
        /// </summary>
        /// <param name="language">Language to use.</param>
        /// <exception cref="NotSupportedException">Thrown when the <paramref name="language"/> is not supported by the API.</exception>
        public static void GuardSupportedLanguage(this string language)
        {
            string[] supportedLanguages = { "en", "de", "fr", "it", "es" };
            if (!supportedLanguages.Contains(language, StringComparer.OrdinalIgnoreCase))
                throw new NotSupportedException($"The language `{language}` is not supported by the API.");
        }

        /// <summary>
        /// Checks if invalid characters are part of the name of an entity.
        /// </summary>
        /// <param name="value">The name of the URL resource.</param>
        /// <exception cref="ArgumentException">Thrown when <see cref="value"/> contains invalid characters.</exception>
        internal static void GuardInvalidStringsInName(this string value)
        {
            if (value.Contains("..", StringComparison.OrdinalIgnoreCase) 
                || value.Contains("?", StringComparison.OrdinalIgnoreCase) 
                || value.Contains("&", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException($"invalid characters in name `{value}`.");
            }
        }

        /// <summary>
        /// Reads credentials from a specified JSON file.
        /// </summary>
        /// <remarks>This is typically used for reading service account credentials from Google Cloud Platform.</remarks>
        /// <param name="credentialsFile">The path to the credentials file.</param>
        /// <returns>A <see cref="Credentials"/> object if the file exists and is valid; otherwise, <c>null</c>.</returns>
        internal static Credentials? GetCredentialsFromFile(string credentialsFile)
        {
            Credentials? credentials = null;
            if (File.Exists(credentialsFile))
            {
                using var stream = new FileStream(credentialsFile, FileMode.Open, FileAccess.Read);
                credentials = JsonSerializer.Deserialize<Credentials>(stream, SCredentialOptions);
            }

            return credentials;
        }

        /// <summary>
        /// Retrieves an access token from Application Default Credentials (ADC) using the gcloud command-line tool.
        /// This method is specific to Google Cloud Platform.
        /// </summary>
        /// <returns>The access token as a string, or an empty string if it fails.</returns>
        /// <seealso href="https://cloud.google.com/docs/authentication"/>
        internal static string GetAccessTokenFromAdc(ILogger? logger = null)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return RunExternalExe("cmd.exe", "/c gcloud auth application-default print-access-token", logger).TrimEnd();
            }
            else
            {
                return RunExternalExe("gcloud", "auth application-default print-access-token", logger).TrimEnd();
            }
        }

        /// <summary>
        /// Executes an external command-line application.
        /// </summary>
        /// <param name="filename">The command or application to run.</param>
        /// <param name="arguments">Optional arguments to pass to the application.</param>
        /// <param name="logger">Optional. Logger instance used for logging.</param>
        /// <returns>The standard output from the application.</returns>
        /// <exception cref="Exception">Thrown if the process exits with a non-zero code.</exception>
        private static string RunExternalExe(string filename, string arguments, ILogger? logger = null)
        {
            var process = new Process();
            var stdOutput = new StringBuilder();
            var stdError = new StringBuilder();

            process.StartInfo.FileName = filename;
            if (!string.IsNullOrEmpty(arguments))
            {
                process.StartInfo.Arguments = arguments;
            }

            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.UseShellExecute = false;

            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardOutput = true;
            // Use AppendLine rather than Append since args.Data is one line of output, not including the newline character.
            process.OutputDataReceived += (sender, args) =>
            {
                if (args.Data != null) stdOutput.AppendLine(args.Data);
            };
            process.ErrorDataReceived += (sender, args) =>
            {
                if (args.Data != null) stdError.AppendLine(args.Data);
            };

            try
            {
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();
            }
            catch (Exception e)
            {
                logger?.LogWarning($"OS error while executing {Format(filename, arguments)}: {e.Message}");
                return string.Empty;
            }

            if (process.ExitCode == 0)
            {
                return stdOutput.ToString();
            }
            else
            {
                var message = new StringBuilder();

                if (stdError.Length > 0)
                {
                    message.AppendLine("Err output:");
                    message.AppendLine(stdError.ToString());
                }

                if (stdOutput.Length != 0)
                {
                    message.AppendLine("Std output:");
                    message.AppendLine(stdOutput.ToString());
                }

                var exceptionMessage = Format(filename, arguments) + " finished with exit code = " + process.ExitCode +
                                    ": " + message;
                logger?.LogWarning(exceptionMessage);
                return string.Empty;
            }
        }

        /// <summary>
        /// Formats a command and its arguments for logging purposes.
        /// </summary>
        /// <param name="filename">The command or application that was run.</param>
        /// <param name="arguments">The arguments passed to the application.</param>
        /// <returns>A formatted string containing the command and arguments.</returns>
        private static string Format(string filename, string? arguments)
        {
            return "'" + filename +
                   ((string.IsNullOrEmpty(arguments)) ? string.Empty : " " + arguments) +
                   "'";
        }

        /// <summary>
        /// Sanitizes the model name by ensuring it starts with "models/" unless it is a tuned model.
        /// </summary>
        /// <param name="value">The model name to sanitize.</param>
        /// <returns>The sanitized model name.</returns>
        public static string SanitizeModelName(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            GuardInvalidStringsInName(value);

            if (value.StartsWith("tuned", StringComparison.OrdinalIgnoreCase))
            {
                return value;
            }

            if (!value.StartsWith("model", StringComparison.OrdinalIgnoreCase))
            {
                return $"models/{value}";
            }

            return value;
        }

        /// <summary>
        /// Sanitizes the file name by ensuring it starts with "files/".
        /// </summary>
        /// <param name="value">The file name to sanitize.</param>
        /// <returns>The sanitized file name.</returns>
        public static string SanitizeFileName(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            GuardInvalidStringsInName(value);

            if (!value.StartsWith("file", StringComparison.OrdinalIgnoreCase))
            {
                return $"files/{value}";
            }

            return value;
        }

        /// <summary>
        /// Sanitizes the generated file name by ensuring it starts with "generatedFiles/".
        /// </summary>
        /// <param name="value">The generated file name to sanitize.</param>
        /// <returns>The sanitized generated file name.</returns>
        public static string SanitizeGeneratedFileName(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            GuardInvalidStringsInName(value);

            if (!value.StartsWith("generatedFile", StringComparison.OrdinalIgnoreCase))
            {
                return $"generatedFiles/{value}";
            }

            return value;
        }

        /// <summary>
        /// Sanitizes the cached content name by ensuring it starts with "cachedContents/".
        /// </summary>
        /// <param name="value">The cached content name to sanitize.</param>
        /// <returns>The sanitized cached content name.</returns>
        public static string SanitizeCachedContentName(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            GuardInvalidStringsInName(value);

            if (!value.StartsWith("cachedContent", StringComparison.OrdinalIgnoreCase))
            {
                return $"cachedContents/{value}";
            }

            return value;
        }

        /// <summary>
        /// Sanitizes the batch name by ensuring it starts with "batches/".
        /// </summary>
        /// <param name="value">The batch name to sanitize.</param>
        /// <returns>The sanitized batch name.</returns>
        public static string SanitizeBatchesName(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            GuardInvalidStringsInName(value);

            if (!value.StartsWith("batch", StringComparison.OrdinalIgnoreCase))
            {
                return $"batches/{value}";
            }

            return value;
        }

        /// <summary>
        /// Sanitizes the tuning job name by ensuring it starts with "tuningJobs/".
        /// </summary>
        /// <param name="value">The tuning job name to sanitize.</param>
        /// <returns>The sanitized tuning job name.</returns>
        public static string SanitizeTuningJobsName(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            GuardInvalidStringsInName(value);

            if (!value.StartsWith("tuningJob", StringComparison.OrdinalIgnoreCase))
            {
                return $"tuningJobs/{value}";
            }

            return value;
        }

        /// <summary>
        /// Sanitizes the endpoint name.
        /// </summary>
        /// <param name="value">The endpoint name to sanitize.</param>
        /// <returns>The sanitized endpoint name.</returns>
        public static string SanitizeEndpointName(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            GuardInvalidStringsInName(value);

            if (value.StartsWith("endpoint", StringComparison.OrdinalIgnoreCase))
            {
                return value;
            }

            return value;
        }

        /// <summary>
        /// Sanitizes the file search store name by ensuring it starts with "fileSearchStores/".
        /// </summary>
        /// <param name="value">The file search store name to sanitize.</param>
        /// <returns>The sanitized file search store name.</returns>
        public static string SanitizeFileSearchStoreName(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            GuardInvalidStringsInName(value);

            if (!value.StartsWith("fileSearchStore", StringComparison.OrdinalIgnoreCase))
            {
                return $"fileSearchStores/{value}";
            }

            return value;
        }

        /// <summary>
        /// Sanitizes the corpora name by ensuring it starts with "corpora/".
        /// </summary>
        /// <param name="value">The corpora name to sanitize.</param>
        /// <returns>The sanitized corpora name.</returns>
        public static string SanitizeCorporaName(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            GuardInvalidStringsInName(value);

            if (!value.StartsWith("corpora", StringComparison.OrdinalIgnoreCase))
            {
                return $"corpora/{value}";
            }

            return value;
        }

        /// <summary>
        /// Sanitizes the document name by ensuring it starts with "documents/".
        /// </summary>
        /// <param name="value">The document name to sanitize.</param>
        /// <returns>The sanitized document name.</returns>
        public static string SanitizeDocumentName(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            GuardInvalidStringsInName(value);

            if (!value.StartsWith("document", StringComparison.OrdinalIgnoreCase))
            {
                return $"documents/{value}";
            }

            return value;
        }

        /// <summary>
        /// Populates a Part from a byte array string and mime type.
        /// </summary>
        /// <param name="part">The part to populate.</param>
        /// <param name="value">The string representation of the byte array.</param>
        /// <param name="mimeType">The mime type of the data.</param>
        /// <returns>The populated part.</returns>
        public static Part FromBytes(this Part part, string value, string mimeType)
        {
            if (string.IsNullOrEmpty(value)) return part;

            part.Text = value;
            return part;
        }

        /// <summary>
        /// Populates a Part from a code execution result.
        /// </summary>
        /// <param name="part">The part to populate.</param>
        /// <param name="outcome">The outcome of the code execution.</param>
        /// <param name="output">The output of the code execution.</param>
        /// <returns>The populated part.</returns>
        public static Part FromCodeExecutionResult(this Part part, string outcome, string output)
        {
            if (string.IsNullOrEmpty(outcome)) return part;

            part.Text = outcome;
            return part;
        }

        /// <summary>
        /// Populates a Part from executable code.
        /// </summary>
        /// <param name="part">The part to populate.</param>
        /// <param name="code">The executable code.</param>
        /// <param name="language">The language of the code.</param>
        /// <returns>The populated part.</returns>
        public static Part FromExecutableCode(this Part part, string code, Language language)
        {
            if (string.IsNullOrEmpty(code)) return part;

            part.Text = code;
            return part;
        }

        /// <summary>
        /// Populates a Part from a function call.
        /// </summary>
        /// <param name="part">The part to populate.</param>
        /// <param name="name">The name of the function.</param>
        /// <param name="args">The arguments for the function call.</param>
        /// <returns>The populated part.</returns>
        public static Part FromFunctionCall(this Part part, string name, string[] args)
        {
            if (string.IsNullOrEmpty(name)) return part;

            part.Text = name;
            return part;
        }

        /// <summary>
        /// Populates a Part from a function response.
        /// </summary>
        /// <param name="part">The part to populate.</param>
        /// <param name="name">The name of the function.</param>
        /// <param name="response">The response from the function.</param>
        /// <returns>The populated part.</returns>
        public static Part FromFunctionResponse(this Part part, string name, dynamic response)
        {
            if (string.IsNullOrEmpty(name)) return part;

            part.Text = name;
            return part;
        }

        /// <summary>
        /// Populates a Part from a text string.
        /// </summary>
        /// <param name="part">The part to populate.</param>
        /// <param name="value">The text value.</param>
        /// <returns>The populated part.</returns>
        public static Part FromText(this Part part, string value)
        {
            if (string.IsNullOrEmpty(value)) return part;

            part.Text = value;
            return part;
        }

        /// <summary>
        /// Populates a Part from a URI.
        /// </summary>
        /// <param name="part">The part to populate.</param>
        /// <param name="uri">The URI of the file data.</param>
        /// <param name="mimeType">The mime type of the file. If null, it will be inferred from the URI.</param>
        /// <returns>The populated part.</returns>
        public static Part FromUri(this Part part, string uri, string? mimeType)
        {
            if (string.IsNullOrEmpty(uri)) return part;

            mimeType ??= GetMimeType(uri);
            part.FileData = new FileData() { FileUri = uri, MimeType = mimeType };
            return part;
        }

        /// <summary>
        // Populates a Part with video metadata.
        /// </summary>
        /// <param name="part">The part to populate.</param>
        /// <param name="startOffset">The start offset of the video.</param>
        /// <param name="endOffset">The end offset of the video.</param>
        /// <param name="fps">The frames per second of the video.</param>
        /// <returns>The populated part.</returns>
        public static Part FromVideoMetadata(this Part part, string startOffset, string endOffset, double fps)
        {
            part.VideoMetadata ??= new();
            part.VideoMetadata.StartOffset = startOffset;
            part.VideoMetadata.EndOffset = endOffset;
            part.VideoMetadata.Fps = fps;
            return part;
        }

        /// <summary>
        /// Creates an Image object from a file URI.
        /// </summary>
        /// <param name="uri">The URI of the image file.</param>
        /// <param name="mimeType">The mime type of the image. If null, it will be inferred from the URI.</param>
        /// <returns>An Image object.</returns>
        public static Image FromFile(string uri, string? mimeType = null)
        {
            mimeType ??= GetMimeType(uri);
            return new Image() { ImageBytes = File.ReadAllBytes(uri), MimeType = mimeType };
        }

        public static string GetValue(this JsonElement element, string key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            string? result = "";
            if (element.TryGetProperty(key, out JsonElement value))
            {
                result = value.GetString();
            }

            return result ?? "";
        }

        public static void ReadDotEnv(string dotEnvFile = ".env")
        {
            if (!File.Exists(dotEnvFile)) return;

            foreach (var line in File.ReadAllLines(dotEnvFile))
            {
                var parts = line.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length != 2) continue;

                Environment.SetEnvironmentVariable(parts[0], parts[1]);
            }
        }

        public static string AddQueryString(this string requestUri, Dictionary<string, string?> queryStringParams)
        {
            bool startingQuestionMarkAdded = false;
            var sb = new StringBuilder();
            sb.Append(requestUri);
            foreach (var parameter in queryStringParams)
            {
                if (string.IsNullOrEmpty(parameter.Value))
                {
                    continue;
                }

                sb.Append(startingQuestionMarkAdded ? '&' : '?');
                sb.Append($"{parameter.Key}={parameter.Value}");
                startingQuestionMarkAdded = true;
            }

            return sb.ToString();
        }

        public static void CheckResponse(this GenerateContentResponse response, bool stream = false)
        {
            if (response.PromptFeedback is { BlockReason: not BlockReason.BlockReasonUnspecified })
            {
                throw new BlockedPromptException(response.PromptFeedback!);
            }

            if (!stream)
            {
                if (response.Candidates!.FirstOrDefault()!.FinishReason is
                    FinishReason.MaxTokens or
                    FinishReason.Safety or
                    FinishReason.Recitation or
                    FinishReason.Other)
                {
                    throw new StopCandidateException(response.Candidates[0]);
                }
            }
        }

        /// <summary>
        /// Checks if a string is a valid Base64 string.
        /// </summary>
        /// <param name="value">The string to check.</param>
        /// <returns>True if the string is a valid Base64 string, otherwise false.</returns>
        internal static bool IsValidBase64String(this string value)
        {
#if NET8_0_OR_GREATER
            return System.Buffers.Text.Base64.IsValid(value.AsSpan());
#else
            try
            {
                _ = Convert.FromBase64String(value);
                return true;
            }
            catch
            {
                return false;
            }
#endif
        }

        /// <summary>
        /// Checks if a string is valid JSON.
        /// </summary>
        /// <param name="value">The string to check.</param>
        /// <returns>True if the string is valid JSON, otherwise false.</returns>
        internal static bool IsValidJson(this string value)
        {
            try
            {
                using JsonDocument doc = JsonDocument.Parse(value);
                return true; // doc is not null;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Throws an exception if the IsSuccessStatusCode property for the HTTP response is false.
        /// </summary>
        /// <param name="response">The HTTP response message to check.</param>
        /// <param name="errorMessage">Custom error message to prepend the <see cref="HttpRequestException"/> message."/></param>
        /// <param name="includeResponseContent">Include the response content in the error message.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The HTTP response message if the call is successful.</returns>
        /// <exception cref="HttpRequestException"></exception>
        internal static async Task<HttpResponseMessage> EnsureSuccessAsync(this HttpResponseMessage response,
            string errorMessage,
            bool includeResponseContent = true,
            CancellationToken cancellationToken = default)
        {
            if (response.IsSuccessStatusCode) return response;

            errorMessage = !string.IsNullOrEmpty(errorMessage)
                ? errorMessage
                : Constants.RequestFailed;
#if NET472_OR_GREATER || NETSTANDARD2_0
            string errorMessageContent = includeResponseContent
                ? Environment.NewLine + await response.Content.ReadAsStringAsync()
                : string.Empty;
#else
            string errorMessageContent = includeResponseContent
                ? Environment.NewLine + await response.Content.ReadAsStringAsync(cancellationToken)
                : string.Empty;
#endif

#if NET8_0_OR_GREATER
            throw new HttpRequestException(
                $"{errorMessage}{Environment.NewLine}{Constants.RequestFailedWithStatusCode}{response.StatusCode}{errorMessageContent.Truncate()}",
                inner: null, statusCode: response.StatusCode);
#else
            throw new HttpRequestException($"{errorMessage}{Environment.NewLine}{Constants.RequestFailedWithStatusCode}{response.StatusCode}{errorMessageContent.Truncate()}");
#endif
        }

        /// <summary>
        /// Throws an exception if the <see cref="HttpResponseMessage.IsSuccessStatusCode"/> property for the HTTP response is false.
        /// </summary>
        /// <param name="response">The HTTP response.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <exception cref="GeminiApiException">The HTTP response was not successful.</exception>
        public static async Task<HttpResponseMessage> EnsureSuccessAsync(this HttpResponseMessage response,
            CancellationToken cancellationToken = default)
        {
            if (response.IsSuccessStatusCode) return response;

            var message = response.ReasonPhrase;
            if (response.Content != null)
            {
#if NET472_OR_GREATER || NETSTANDARD2_0
                message = await response.Content.ReadAsStringAsync();
#else
                message = await response.Content.ReadAsStringAsync(cancellationToken);
#endif
            }

            throw new GeminiApiException($"The request was not successful. Last API response:\n{message}", response);
        }

        /// <summary>
        /// Reads an image file from a URL as a byte array.
        /// </summary>
        /// <param name="url">The URL of the image file.</param>
        /// <returns>A byte array of the image file.</returns>
        internal static async Task<byte[]> ReadImageFileAsync(string url)
        {
            return await ReadImageFileAsync(url, CancellationToken.None);
        }

        /// <summary>
        /// Reads an image file from a URL as a byte array.
        /// </summary>
        /// <param name="url">The URL of the image file.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A byte array of the image file.</returns>
        internal static async Task<byte[]> ReadImageFileAsync(string url,
            CancellationToken cancellationToken = default)
        {
            using var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);
            using var response = await Client.SendAsync(httpRequest, HttpCompletionOption.ResponseContentRead,
                cancellationToken);
            await response.EnsureSuccessAsync($"Download of '{url}' failed", cancellationToken: cancellationToken);
#if NET472_OR_GREATER || NETSTANDARD2_0
            byte[] imageBytes = await response.Content.ReadAsByteArrayAsync();
#else
            byte[] imageBytes = await response.Content.ReadAsByteArrayAsync(cancellationToken);
#endif
            return imageBytes;
        }

        /// <summary>
        /// Reads an image file from a URL and returns it as a Base64 encoded string.
        /// </summary>
        /// <param name="url">The URL of the image file.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A Base64 encoded string of the image file.</returns>
        internal static async Task<string> ReadImageFileBase64Async(string url,
            CancellationToken cancellationToken = default)
        {
            byte[] imageBytes = await ReadImageFileAsync(url, cancellationToken);
            return Convert.ToBase64String(imageBytes);
        }

        /// <summary>
        /// Gets the MIME type from a file extension in a URI.
        /// </summary>
        /// <param name="uri">The URI of the file.</param>
        /// <returns>The inferred MIME type, or "application/octet-stream" if not found.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="uri"/> is null.</exception>
        private static readonly Dictionary<string, string> MimeTypeMap = new(StringComparer.OrdinalIgnoreCase)
        {
            { "323", "text/h323" },
            { "3g2", "video/3gpp2" },
            { "3gp", "video/3gpp" },
            { "3gp2", "video/3gpp2" },
            { "3gpp", "video/3gpp" },
            { "7z", "application/x-7z-compressed" },
            { "aa", "audio/audible" },
            { "aac", "audio/aac" },
            { "aaf", "application/octet-stream" },
            { "aax", "audio/vnd.audible.aax" },
            { "ac3", "audio/ac3" },
            { "aca", "application/octet-stream" },
            { "accda", "application/msaccess.addin" },
            { "accdb", "application/msaccess" },
            { "accdc", "application/msaccess.cab" },
            { "accde", "application/msaccess" },
            { "accdr", "application/msaccess.runtime" },
            { "accdt", "application/msaccess" },
            { "accdw", "application/msaccess.webapplication" },
            { "accft", "application/msaccess.ftemplate" },
            { "acx", "application/internet-property-stream" },
            { "addin", "text/xml" },
            { "ade", "application/msaccess" },
            { "adobebridge", "application/x-bridge-url" },
            { "adp", "application/msaccess" },
            { "adt", "audio/vnd.dlna.adts" },
            { "adts", "audio/aac" },
            { "afm", "application/octet-stream" },
            { "ai", "application/postscript" },
            { "aif", "audio/x-aiff" },
            { "aifc", "audio/aiff" },
            { "aiff", "audio/aiff" },
            { "air", "application/vnd.adobe.air-application-installer-package+zip" },
            { "amc", "application/x-mpeg" },
            { "application", "application/x-ms-application" },
            { "art", "image/x-jg" },
            { "asa", "application/xml" },
            { "asax", "application/xml" },
            { "ascx", "application/xml" },
            { "asd", "application/octet-stream" },
            { "asf", "video/x-ms-asf" },
            { "ashx", "application/xml" },
            { "asi", "application/octet-stream" },
            { "asm", "text/plain" },
            { "asmx", "application/xml" },
            { "aspx", "application/xml" },
            { "asr", "video/x-ms-asf" },
            { "asx", "video/x-ms-asf" },
            { "atom", "application/atom+xml" },
            { "au", "audio/basic" },
            { "avi", "video/x-msvideo" },
            { "axs", "application/olescript" },
            { "bas", "text/plain" },
            { "bcpio", "application/x-bcpio" },
            { "bin", "application/octet-stream" },
            { "bmp", "image/bmp" },
            { "c", "text/plain" },
            { "cab", "application/octet-stream" },
            { "caf", "audio/x-caf" },
            { "calx", "application/vnd.ms-office.calx" },
            { "cat", "application/vnd.ms-pki.seccat" },
            { "cc", "text/plain" },
            { "cd", "text/plain" },
            { "cdda", "audio/aiff" },
            { "cdf", "application/x-cdf" },
            { "cer", "application/x-x509-ca-cert" },
            { "chm", "application/octet-stream" },
            { "class", "application/x-java-applet" },
            { "clp", "application/x-msclip" },
            { "cmx", "image/x-cmx" },
            { "cnf", "text/plain" },
            { "cod", "image/cis-cod" },
            { "config", "application/xml" },
            { "contact", "text/x-ms-contact" },
            { "coverage", "application/xml" },
            { "cpio", "application/x-cpio" },
            { "cpp", "text/plain" },
            { "crd", "application/x-mscardfile" },
            { "crl", "application/pkix-crl" },
            { "crt", "application/x-x509-ca-cert" },
            { "cs", "text/plain" },
            { "csdproj", "text/plain" },
            { "csh", "application/x-csh" },
            { "csproj", "text/plain" },
            { "css", "text/css" },
            { "csv", "text/csv" },
            { "cur", "application/octet-stream" },
            { "cxx", "text/plain" },
            { "dat", "application/octet-stream" },
            { "datasource", "application/xml" },
            { "dbproj", "text/plain" },
            { "dcr", "application/x-director" },
            { "def", "text/plain" },
            { "deploy", "application/octet-stream" },
            { "der", "application/x-x509-ca-cert" },
            { "dgml", "application/xml" },
            { "dib", "image/bmp" },
            { "dif", "video/x-dv" },
            { "dir", "application/x-director" },
            { "disco", "text/xml" },
            { "dll", "application/x-msdownload" },
            { "dll.config", "text/xml" },
            { "dlm", "text/dlm" },
            { "doc", "application/msword" },
            { "docm", "application/vnd.ms-word.document.macroenabled.12" },
            { "docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
            { "dot", "application/msword" },
            { "dotm", "application/vnd.ms-word.template.macroenabled.12" },
            { "dotx", "application/vnd.openxmlformats-officedocument.wordprocessingml.template" },
            { "dsp", "application/octet-stream" },
            { "dsw", "text/plain" },
            { "dtd", "text/xml" },
            { "dtsconfig", "text/xml" },
            { "dv", "video/x-dv" },
            { "dvi", "application/x-dvi" },
            { "dwf", "drawing/x-dwf" },
            { "dwp", "application/octet-stream" },
            { "dxr", "application/x-director" },
            { "eml", "message/rfc822" },
            { "emz", "application/octet-stream" },
            { "eot", "application/octet-stream" },
            { "eps", "application/postscript" },
            { "etl", "application/etl" },
            { "etx", "text/x-setext" },
            { "evy", "application/envoy" },
            { "exe", "application/octet-stream" },
            { "exe.config", "text/xml" },
            { "fdf", "application/vnd.fdf" },
            { "fif", "application/fractals" },
            { "filters", "application/xml" },
            { "fla", "application/octet-stream" },
            { "flr", "x-world/x-vrml" },
            { "flv", "video/x-flv" },
            { "fsscript", "application/fsharp-script" },
            { "fsx", "application/fsharp-script" },
            { "generictest", "application/xml" },
            { "gif", "image/gif" },
            { "group", "text/x-ms-group" },
            { "gsm", "audio/x-gsm" },
            { "gtar", "application/x-gtar" },
            { "gz", "application/x-gzip" },
            { "h", "text/plain" },
            { "hdf", "application/x-hdf" },
            { "hdml", "text/x-hdml" },
            { "hhc", "application/x-oleobject" },
            { "hhk", "application/octet-stream" },
            { "hhp", "application/octet-stream" },
            { "hlp", "application/winhlp" },
            { "hpp", "text/plain" },
            { "hqx", "application/mac-binhex40" },
            { "hta", "application/hta" },
            { "htc", "text/x-component" },
            { "htm", "text/html" },
            { "html", "text/html" },
            { "htt", "text/webviewhtml" },
            { "hxa", "application/xml" },
            { "hxc", "application/xml" },
            { "hxd", "application/octet-stream" },
            { "hxe", "application/xml" },
            { "hxf", "application/xml" },
            { "hxh", "application/octet-stream" },
            { "hxi", "application/octet-stream" },
            { "hxk", "application/xml" },
            { "hxq", "application/octet-stream" },
            { "hxr", "application/octet-stream" },
            { "hxs", "application/octet-stream" },
            { "hxt", "text/html" },
            { "hxv", "application/xml" },
            { "hxw", "application/octet-stream" },
            { "hxx", "text/plain" },
            { "i", "text/plain" },
            { "ico", "image/x-icon" },
            { "ics", "application/octet-stream" },
            { "idl", "text/plain" },
            { "ief", "image/ief" },
            { "iii", "application/x-iphone" },
            { "inc", "text/plain" },
            { "inf", "application/octet-stream" },
            { "inl", "text/plain" },
            { "ins", "application/x-internet-signup" },
            { "ipa", "application/x-itunes-ipa" },
            { "ipg", "application/x-itunes-ipg" },
            { "ipproj", "text/plain" },
            { "ipsw", "application/x-itunes-ipsw" },
            { "iqy", "text/x-ms-iqy" },
            { "isp", "application/x-internet-signup" },
            { "ite", "application/x-itunes-ite" },
            { "itlp", "application/x-itunes-itlp" },
            { "itms", "application/x-itunes-itms" },
            { "itpc", "application/x-itunes-itpc" },
            { "ivf", "video/x-ivf" },
            { "jar", "application/java-archive" },
            { "java", "application/octet-stream" },
            { "jck", "application/liquidmotion" },
            { "jcz", "application/liquidmotion" },
            { "jfif", "image/pjpeg" },
            { "jnlp", "application/x-java-jnlp-file" },
            { "jpb", "application/octet-stream" },
            { "jpe", "image/jpeg" },
            { "jpeg", "image/jpeg" },
            { "jpg", "image/jpeg" },
            { "js", "application/x-javascript" },
            { "jsx", "text/jscript" },
            { "jsxbin", "text/plain" },
            { "latex", "application/x-latex" },
            { "library-ms", "application/windows-library+xml" },
            { "lit", "application/x-ms-reader" },
            { "loadtest", "application/xml" },
            { "lpk", "application/octet-stream" },
            { "lsf", "video/x-la-asf" },
            { "lst", "text/plain" },
            { "lsx", "video/x-la-asf" },
            { "lzh", "application/octet-stream" },
            { "m13", "application/x-msmediaview" },
            { "m14", "application/x-msmediaview" },
            { "m1v", "video/mpeg" },
            { "m2t", "video/vnd.dlna.mpeg-tts" },
            { "m2ts", "video/vnd.dlna.mpeg-tts" },
            { "m2v", "video/mpeg" },
            { "m3u", "audio/x-mpegurl" },
            { "m3u8", "audio/x-mpegurl" },
            { "m4a", "audio/m4a" },
            { "m4b", "audio/m4b" },
            { "m4p", "audio/m4p" },
            { "m4r", "audio/x-m4r" },
            { "m4v", "video/x-m4v" },
            { "mac", "image/x-macpaint" },
            { "mak", "text/plain" },
            { "man", "application/x-troff-man" },
            { "manifest", "application/x-ms-manifest" },
            { "map", "text/plain" },
            { "master", "application/xml" },
            { "mda", "application/msaccess" },
            { "mdb", "application/x-msaccess" },
            { "mde", "application/msaccess" },
            { "mdp", "application/octet-stream" },
            { "me", "application/x-troff-me" },
            { "mfp", "application/x-shockwave-flash" },
            { "mht", "message/rfc822" },
            { "mhtml", "message/rfc822" },
            { "mid", "audio/mid" },
            { "midi", "audio/mid" },
            { "mix", "application/octet-stream" },
            { "mk", "text/plain" },
            { "mmf", "application/x-smaf" },
            { "mno", "text/xml" },
            { "mny", "application/x-msmoney" },
            { "mod", "video/mpeg" },
            { "mov", "video/quicktime" },
            { "movie", "video/x-sgi-movie" },
            { "mp2", "video/mpeg" },
            { "mp2v", "video/mpeg" },
            { "mp3", "audio/mpeg" },
            { "mp4", "video/mp4" },
            { "mp4v", "video/mp4" },
            { "mpa", "video/mpeg" },
            { "mpe", "video/mpeg" },
            { "mpeg", "video/mpeg" },
            { "mpf", "application/vnd.ms-mediapackage" },
            { "mpg", "video/mpeg" },
            { "mpp", "application/vnd.ms-project" },
            { "mpv2", "video/mpeg" },
            { "mqv", "video/quicktime" },
            { "ms", "application/x-troff-ms" },
            { "msi", "application/octet-stream" },
            { "mso", "application/octet-stream" },
            { "mts", "video/vnd.dlna.mpeg-tts" },
            { "mtx", "application/xml" },
            { "mvb", "application/x-msmediaview" },
            { "mvc", "application/x-miva-compiled" },
            { "mxp", "application/x-mmxp" },
            { "nc", "application/x-netcdf" },
            { "nsc", "video/x-ms-asf" },
            { "nws", "message/rfc822" },
            { "ocx", "application/octet-stream" },
            { "oda", "application/oda" },
            { "odc", "text/x-ms-odc" },
            { "odh", "text/plain" },
            { "odl", "text/plain" },
            { "odp", "application/vnd.oasis.opendocument.presentation" },
            { "ods", "application/oleobject" },
            { "odt", "application/vnd.oasis.opendocument.text" },
            { "one", "application/onenote" },
            { "onea", "application/onenote" },
            { "onepkg", "application/onenote" },
            { "onetmp", "application/onenote" },
            { "onetoc", "application/onenote" },
            { "onetoc2", "application/onenote" },
            { "orderedtest", "application/xml" },
            { "osdx", "application/opensearchdescription+xml" },
            { "p10", "application/pkcs10" },
            { "p12", "application/x-pkcs12" },
            { "p7b", "application/x-pkcs7-certificates" },
            { "p7c", "application/pkcs7-mime" },
            { "p7m", "application/pkcs7-mime" },
            { "p7r", "application/x-pkcs7-certreqresp" },
            { "p7s", "application/pkcs7-signature" },
            { "pbm", "image/x-portable-bitmap" },
            { "pcast", "application/x-podcast" },
            { "pct", "image/pict" },
            { "pcx", "application/octet-stream" },
            { "pcz", "application/octet-stream" },
            { "pdf", "application/pdf" },
            { "pfb", "application/octet-stream" },
            { "pfm", "application/octet-stream" },
            { "pfx", "application/x-pkcs12" },
            { "pgm", "image/x-portable-graymap" },
            { "pic", "image/pict" },
            { "pict", "image/pict" },
            { "pkgdef", "text/plain" },
            { "pkgundef", "text/plain" },
            { "pko", "application/vnd.ms-pki.pko" },
            { "pls", "audio/scpls" },
            { "pma", "application/x-perfmon" },
            { "pmc", "application/x-perfmon" },
            { "pml", "application/x-perfmon" },
            { "pmr", "application/x-perfmon" },
            { "pmw", "application/x-perfmon" },
            { "png", "image/png" },
            { "pnm", "image/x-portable-anymap" },
            { "pnt", "image/x-macpaint" },
            { "pntg", "image/x-macpaint" },
            { "pnz", "image/png" },
            { "pot", "application/vnd.ms-powerpoint" },
            { "potm", "application/vnd.ms-powerpoint.template.macroenabled.12" },
            { "potx", "application/vnd.openxmlformats-officedocument.presentationml.template" },
            { "ppa", "application/vnd.ms-powerpoint" },
            { "ppam", "application/vnd.ms-powerpoint.addin.macroenabled.12" },
            { "ppm", "image/x-portable-pixmap" },
            { "pps", "application/vnd.ms-powerpoint" },
            { "ppsm", "application/vnd.ms-powerpoint.slideshow.macroenabled.12" },
            { "ppsx", "application/vnd.openxmlformats-officedocument.presentationml.slideshow" },
            { "ppt", "application/vnd.ms-powerpoint" },
            { "pptm", "application/vnd.ms-powerpoint.presentation.macroenabled.12" },
            { "pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation" },
            { "prf", "application/pics-rules" },
            { "prm", "application/octet-stream" },
            { "prx", "application/octet-stream" },
            { "ps", "application/postscript" },
            { "psc1", "application/powershell" },
            { "psd", "application/octet-stream" },
            { "psess", "application/xml" },
            { "psm", "application/octet-stream" },
            { "psp", "application/octet-stream" },
            { "pub", "application/x-mspublisher" },
            { "pwz", "application/vnd.ms-powerpoint" },
            { "qht", "text/x-html-insertion" },
            { "qhtm", "text/x-html-insertion" },
            { "qt", "video/quicktime" },
            { "qti", "image/x-quicktime" },
            { "qtif", "image/x-quicktime" },
            { "qtl", "application/x-quicktimeplayer" },
            { "qxd", "application/octet-stream" },
            { "ra", "audio/x-pn-realaudio" },
            { "ram", "audio/x-pn-realaudio" },
            { "rar", "application/octet-stream" },
            { "ras", "image/x-cmu-raster" },
            { "rat", "application/rat-file" },
            { "rc", "text/plain" },
            { "rc2", "text/plain" },
            { "rct", "text/plain" },
            { "rdlc", "application/xml" },
            { "resx", "application/xml" },
            { "rf", "image/vnd.rn-realflash" },
            { "rgb", "image/x-rgb" },
            { "rgs", "text/plain" },
            { "rm", "application/vnd.rn-realmedia" },
            { "rmi", "audio/mid" },
            { "rmp", "application/vnd.rn-rn_music_package" },
            { "roff", "application/x-troff" },
            { "rpm", "audio/x-pn-realaudio-plugin" },
            { "rqy", "text/x-ms-rqy" },
            { "rtf", "application/rtf" },
            { "rtx", "text/richtext" },
            { "ruleset", "application/xml" },
            { "s", "text/plain" },
            { "safariextz", "application/x-safari-safariextz" },
            { "scd", "application/x-msschedule" },
            { "sct", "text/scriptlet" },
            { "sd2", "audio/x-sd2" },
            { "sdp", "application/sdp" },
            { "sea", "application/octet-stream" },
            { "searchconnector-ms", "application/windows-search-connector+xml" },
            { "setpay", "application/set-payment-initiation" },
            { "setreg", "application/set-registration-initiation" },
            { "settings", "application/xml" },
            { "sgimb", "application/x-sgimb" },
            { "sgml", "text/sgml" },
            { "sh", "application/x-sh" },
            { "shar", "application/x-shar" },
            { "shtml", "text/html" },
            { "sit", "application/x-stuffit" },
            { "sitemap", "application/xml" },
            { "skin", "application/xml" },
            { "sldm", "application/vnd.ms-powerpoint.slide.macroenabled.12" },
            { "sldx", "application/vnd.openxmlformats-officedocument.presentationml.slide" },
            { "slk", "application/vnd.ms-excel" },
            { "sln", "text/plain" },
            { "slupkg-ms", "application/x-ms-license" },
            { "smd", "audio/x-smd" },
            { "smi", "application/octet-stream" },
            { "smx", "audio/x-smd" },
            { "smz", "audio/x-smd" },
            { "snd", "audio/basic" },
            { "snippet", "application/xml" },
            { "snp", "application/octet-stream" },
            { "sol", "text/plain" },
            { "sor", "text/plain" },
            { "spc", "application/x-pkcs7-certificates" },
            { "spl", "application/futuresplash" },
            { "src", "application/x-wais-source" },
            { "srf", "text/plain" },
            { "ssisdeploymentmanifest", "text/xml" },
            { "ssm", "application/streamingmedia" },
            { "sst", "application/vnd.ms-pki.certstore" },
            { "stl", "application/vnd.ms-pki.stl" },
            { "sv4cpio", "application/x-sv4cpio" },
            { "sv4crc", "application/x-sv4crc" },
            { "svc", "application/xml" },
            { "swf", "application/x-shockwave-flash" },
            { "t", "application/x-troff" },
            { "tar", "application/x-tar" },
            { "tcl", "application/x-tcl" },
            { "testrunconfig", "application/xml" },
            { "testsettings", "application/xml" },
            { "tex", "application/x-tex" },
            { "texi", "application/x-texinfo" },
            { "texinfo", "application/x-texinfo" },
            { "tgz", "application/x-compressed" },
            { "thmx", "application/vnd.ms-officetheme" },
            { "thn", "application/octet-stream" },
            { "tif", "image/tiff" },
            { "tiff", "image/tiff" },
            { "tlh", "text/plain" },
            { "tli", "text/plain" },
            { "toc", "application/octet-stream" },
            { "tr", "application/x-troff" },
            { "trm", "application/x-msterminal" },
            { "trx", "application/xml" },
            { "ts", "video/vnd.dlna.mpeg-tts" },
            { "tsv", "text/tab-separated-values" },
            { "ttf", "application/octet-stream" },
            { "tts", "video/vnd.dlna.mpeg-tts" },
            { "txt", "text/plain" },
            { "u32", "application/octet-stream" },
            { "uls", "text/iuls" },
            { "user", "text/plain" },
            { "ustar", "application/x-ustar" },
            { "vb", "text/plain" },
            { "vbdproj", "text/plain" },
            { "vbk", "video/mpeg" },
            { "vbproj", "text/plain" },
            { "vbs", "text/vbscript" },
            { "vcf", "text/x-vcard" },
            { "vcproj", "application/xml" },
            { "vcs", "text/plain" },
            { "vcxproj", "application/xml" },
            { "vddproj", "text/plain" },
            { "vdp", "text/plain" },
            { "vdproj", "text/plain" },
            { "vdx", "application/vnd.ms-visio.viewer" },
            { "vml", "text/xml" },
            { "vscontent", "application/xml" },
            { "vsct", "text/xml" },
            { "vsd", "application/vnd.visio" },
            { "vsi", "application/ms-vsi" },
            { "vsix", "application/vsix" },
            { "vsixlangpack", "text/xml" },
            { "vsixmanifest", "text/xml" },
            { "vsmdi", "application/xml" },
            { "vspscc", "text/plain" },
            { "vss", "application/vnd.visio" },
            { "vsscc", "text/plain" },
            { "vssettings", "text/xml" },
            { "vssscc", "text/plain" },
            { "vst", "application/vnd.visio" },
            { "vstemplate", "text/xml" },
            { "vsto", "application/x-ms-vsto" },
            { "vsw", "application/vnd.visio" },
            { "vsx", "application/vnd.visio" },
            { "vtx", "application/vnd.visio" },
            { "wav", "audio/wav" },
            { "wave", "audio/wav" },
            { "wax", "audio/x-ms-wax" },
            { "wbk", "application/msword" },
            { "wbmp", "image/vnd.wap.wbmp" },
            { "wcm", "application/vnd.ms-works" },
            { "wdb", "application/vnd.ms-works" },
            { "wdp", "image/vnd.ms-photo" },
            { "webarchive", "application/x-safari-webarchive" },
            { "webtest", "application/xml" },
            { "wiq", "application/xml" },
            { "wiz", "application/msword" },
            { "wks", "application/vnd.ms-works" },
            { "wlmp", "application/wlmoviemaker" },
            { "wlpginstall", "application/x-wlpg-detect" },
            { "wlpginstall3", "application/x-wlpg3-detect" },
            { "wm", "video/x-ms-wm" },
            { "wma", "audio/x-ms-wma" },
            { "wmd", "application/x-ms-wmd" },
            { "wmf", "application/x-msmetafile" },
            { "wml", "text/vnd.wap.wml" },
            { "wmlc", "application/vnd.wap.wmlc" },
            { "wmls", "text/vnd.wap.wmlscript" },
            { "wmlsc", "application/vnd.wap.wmlscriptc" },
            { "wmp", "video/x-ms-wmp" },
            { "wmv", "video/x-ms-wmv" },
            { "wmx", "video/x-ms-wmx" },
            { "wmz", "application/x-ms-wmz" },
            { "wpl", "application/vnd.ms-wpl" },
            { "wps", "application/vnd.ms-works" },
            { "wri", "application/x-mswrite" },
            { "wrl", "x-world/x-vrml" },
            { "wrz", "x-world/x-vrml" },
            { "wsc", "text/scriptlet" },
            { "wsdl", "text/xml" },
            { "wvx", "video/x-ms-wvx" },
            { "x", "application/directx" },
            { "xaf", "x-world/x-vrml" },
            { "xaml", "application/xaml+xml" },
            { "xap", "application/x-silverlight-app" },
            { "xbap", "application/x-ms-xbap" },
            { "xbm", "image/x-xbitmap" },
            { "xdr", "text/plain" },
            { "xht", "application/xhtml+xml" },
            { "xhtml", "application/xhtml+xml" },
            { "xla", "application/vnd.ms-excel" },
            { "xlam", "application/vnd.ms-excel.addin.macroenabled.12" },
            { "xlc", "application/vnd.ms-excel" },
            { "xld", "application/vnd.ms-excel" },
            { "xlk", "application/vnd.ms-excel" },
            { "xll", "application/vnd.ms-excel" },
            { "xlm", "application/vnd.ms-excel" },
            { "xls", "application/vnd.ms-excel" },
            { "xlsb", "application/vnd.ms-excel.sheet.binary.macroenabled.12" },
            { "xlsm", "application/vnd.ms-excel.sheet.macroenabled.12" },
            { "xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
            { "xlt", "application/vnd.ms-excel" },
            { "xltm", "application/vnd.ms-excel.template.macroenabled.12" },
            { "xltx", "application/vnd.openxmlformats-officedocument.spreadsheetml.template" },
            { "xlw", "application/vnd.ms-excel" },
            { "xml", "text/xml" },
            { "xmta", "application/xml" },
            { "xof", "x-world/x-vrml" },
            { "xoml", "text/plain" },
            { "xpm", "image/x-xpixmap" },
            { "xps", "application/vnd.ms-xpsdocument" },
            { "xrm-ms", "text/xml" },
            { "xsc", "application/xml" },
            { "xsd", "text/xml" },
            { "xsf", "text/xml" },
            { "xsl", "text/xml" },
            { "xslt", "text/xml" },
            { "xsn", "application/octet-stream" },
            { "xss", "application/xml" },
            { "xtp", "application/octet-stream" },
            { "xwd", "image/x-xwindowdump" },
            { "z", "application/x-compress" },
            { "zip", "application/x-zip-compressed" }
        };

        internal static string GetMimeType(string uri)
        {
            if (uri == null) throw new ArgumentNullException(nameof(uri));

            var extension = Path.GetExtension(uri).ToLowerInvariant();
            if (extension.StartsWith(".", StringComparison.OrdinalIgnoreCase))
                extension = extension.Substring(1);

            if (MimeTypeMap.TryGetValue(extension, out var mimeType))
            {
                return mimeType;
            }

            return "application/octet-stream";
        }

        /// <summary>
        /// Truncates/abbreviates a string and places a user-facing indicator at the end.
        /// </summary>
        /// <param name="value">The string to truncate.</param>
        /// <param name="maxLength">Maximum length of the resulting string.</param>
        /// <param name="suffix">Optional. Indicator to use, by default the ellipsis …</param>
        /// <returns>The truncated string</returns>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="suffix"/> parameter is null or empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the length of the <paramref name="suffix"/> is larger than the <paramref name="maxLength"/>.</exception>
        internal static string Truncate(this string value,
            int maxLength = 4096, string suffix = "…")
        {
            if (string.IsNullOrWhiteSpace(value)) return value;
            if (string.IsNullOrEmpty(suffix)) throw new ArgumentException(nameof(suffix));
            if (suffix.Length > maxLength)
                throw new ArgumentOutOfRangeException(nameof(suffix),
                    $"Suffix '{suffix}' (length {suffix.Length} cannot be larger than maximal length {maxLength}.");

            if (maxLength - suffix.Length >= 0 && maxLength - suffix.Length <= value.Length)
            {
#if NET472_OR_GREATER || NETSTANDARD2_0
                value = value.Length >= maxLength
                    ? value.Substring(0, maxLength - suffix.Length) + suffix
                    : value;
#else
                value = value.Length >= maxLength
                    ? value[..(maxLength - suffix.Length)] + suffix
                    : value;
#endif
            }

            return value;
        }

        /// <summary>
        /// Gets the normalized (snake_case) name of a delegate's method.
        /// </summary>
        /// <param name="callback">The delegate to get the name from.</param>
        /// <returns>The snake_case method name.</returns>
        internal static string GetNormalizedName(this Delegate callback)
        {
            return callback.Method.Name.ToSnakeCase();
        }

        /// <summary>
        /// Converts HTTP headers to a formatted string, redacting sensitive information.
        /// </summary>
        /// <param name="headers">The HttpHeaders to format.</param>
        /// <returns>A formatted string representation of the headers.</returns>
        public static string ToFormattedString(this HttpHeaders headers)
        {
            if (headers == null)
            {
                return string.Empty;
            }

            var sb = new StringBuilder();
            foreach (var header in headers)
            {
                sb.AppendLine(SensitiveHeaders.Contains(header.Key)
                    ? $"{header.Key}: [redacted]"
                    : $"{header.Key}: {string.Join(", ", header.Value)}");
            }

            return sb.ToString();
        }

        extension(GenerateContentRequest request)
        {
	        /// <summary>
	        /// Configures the request to expect a JSON response.
	        /// </summary>
	        public void UseJsonMode()
	        {
		        request.GenerationConfig ??= new GenerationConfig();
		        request.GenerationConfig.ResponseMimeType ??= Constants.MediaType;
	        }

	        /// <summary>
	        /// Gets the tools for the request, initializing the collection if it's null.
	        /// </summary>
	        /// <returns>The list of tools for the request.</returns>
	        public Tools WithTools()
	        {
		        request.Tools ??= [];
		        return request.Tools;
	        }
        }

        extension(Tools tools)
        {
	        /// <summary>
	        /// Adds the Google Search tool if it's not already present.
	        /// </summary>
	        /// <returns>The updated list of tools.</returns>
	        public Tools UseGoogleSearch()
	        {
		        Tools defaultTools = [new Tool { GoogleSearch = new() }];
		        tools ??= defaultTools;
		        if (tools != null && !tools.Any(t => t.GoogleSearch is not null))
		        {
			        tools.AddRange(defaultTools);
		        }

		        return tools;
	        }

	        /// <summary>
	        /// Adds the Google Maps tool if it's not already present.
	        /// </summary>
	        /// <param name="enableWidget">Whether to enable the widget for Google Maps.</param>
	        /// <returns>The updated list of tools.</returns>
	        public Tools UseGoogleMaps(bool? enableWidget = false)
	        {
		        Tools defaultTools = [new Tool() { GoogleMaps = new() { EnableWidget = enableWidget } }];
		        tools ??= defaultTools;
		        if (tools != null && !tools.Any(t => t.GoogleMaps is not null))
		        {
			        tools.AddRange(defaultTools);
		        }

		        return tools;
	        }

	        /// <summary>
	        /// Adds the Google Search Retrieval tool for grounding if it's not already present.
	        /// </summary>
	        /// <returns>The updated list of tools.</returns>
	        public Tools UseGrounding()
	        {
		        Tools defaultTools = [new Tool { GoogleSearchRetrieval = new() }];
		        tools ??= defaultTools;
		        if (tools != null && !tools.Any(t => t.GoogleSearchRetrieval is not null))
		        {
			        tools.AddRange(defaultTools);
		        }

		        return tools;
	        }

	        /// <summary>
	        /// Adds the Code Execution tool if it's not already present.
	        /// </summary>
	        /// <returns>The updated list of tools.</returns>
	        public Tools UseCodeExecution()
	        {
		        Tools defaultTools = [new Tool { CodeExecution = new() }];
		        tools ??= defaultTools;
		        if (tools != null && !tools.Any(t => t.CodeExecution is not null))
		        {
			        tools.AddRange(defaultTools);
		        }

		        return tools;
	        }
	        
	        /// <summary>
	        /// Adds the URL Context tool if it's not already present.
	        /// </summary>
	        /// <returns>The updated list of tools.</returns>
	        public Tools UseUrlContext()
	        {
		        Tools defaultTools = [new Tool { UrlContext = new() }];
		        tools ??= defaultTools;
		        if (tools != null && !tools.Any(t => t.UrlContext is not null))
		        {
			        tools.AddRange(defaultTools);
		        }

		        return tools;
	        }

	        public Tools UseFileSearch(string[] storeNames)
	        {
		        Tools defaultTools = [new Tool { FileSearch = new FileSearch { Stores = [.. storeNames] } }];
		        tools ??= defaultTools;
		        if (tools != null && !tools.Any(t => t.FileSearch is not null))
		        {
			        tools.AddRange(defaultTools);
		        }

		        return tools;
	        }
        }
    }
}