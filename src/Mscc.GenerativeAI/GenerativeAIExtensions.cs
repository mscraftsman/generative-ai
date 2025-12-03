using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Authentication;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

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
            if (apiKey.Length == 39 && !apiKey.Substring(0, 4).Equals("AIza", StringComparison.OrdinalIgnoreCase))
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

            if (!allowedMimeTypes.Contains(mimeType.ToLowerInvariant()))
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
        public static void GuardMimeType(string mimeType)
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
                throw new NotSupportedException($"The mime type `{mimeType}` is not supported by the API.");
        }

        /// <summary>
        /// A comprehensive and complete list of MIME types supported by the 
        /// Gemini API's File Search feature. This list has been verified to
        /// include all text/x-* types.
        /// Source: https://ai.google.dev/gemini-api/docs/file-search#supported-files
        /// </summary>
        public static void GuardMimeTypeFileSearchStore(string mimeType)
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
                throw new NotSupportedException($"The mime type `{mimeType}` is not supported by the API.");
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
            if (value.Contains("..") || value.Contains("?") || value.Contains("&"))
            {
                throw new ArgumentException($"invalid characters in name `{value}`.");
            }
        }
        
        public static string SanitizeModelName(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            GuardInvalidStringsInName(value);

            if (value.StartsWith("tuned", StringComparison.InvariantCultureIgnoreCase))
            {
                return value;
            }

            if (!value.StartsWith("model", StringComparison.InvariantCultureIgnoreCase))
            {
                return $"models/{value}";
            }

            return value;
        }

        public static string SanitizeFileName(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            GuardInvalidStringsInName(value);

            if (!value.StartsWith("file", StringComparison.InvariantCultureIgnoreCase))
            {
                return $"files/{value}";
            }

            return value;
        }

        public static string SanitizeGeneratedFileName(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            GuardInvalidStringsInName(value);

            if (!value.StartsWith("generatedFile", StringComparison.InvariantCultureIgnoreCase))
            {
                return $"generatedFiles/{value}";
            }

            return value;
        }

        public static string SanitizeCachedContentName(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            GuardInvalidStringsInName(value);

            if (!value.StartsWith("cachedContent", StringComparison.InvariantCultureIgnoreCase))
            {
                return $"cachedContents/{value}";
            }

            return value;
        }

        public static string SanitizeBatchesName(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            GuardInvalidStringsInName(value);

            if (!value.StartsWith("batch", StringComparison.InvariantCultureIgnoreCase))
            {
                return $"batches/{value}";
            }

            return value;
        }

        public static string SanitizeTuningJobsName(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            GuardInvalidStringsInName(value);

            if (!value.StartsWith("tuningJob", StringComparison.InvariantCultureIgnoreCase))
            {
                return $"tuningJobs/{value}";
            }

            return value;
        }

        public static string SanitizeEndpointName(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            GuardInvalidStringsInName(value);

            if (value.StartsWith("endpoint", StringComparison.InvariantCultureIgnoreCase))
            {
                return value;
            }

            return value;
        }

        public static string SanitizeFileSearchStoreName(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            GuardInvalidStringsInName(value);

            if (!value.StartsWith("fileSearchStore", StringComparison.InvariantCultureIgnoreCase))
            {
                return $"fileSearchStores/{value}";
            }

            return value;
        }

        public static string SanitizeCorporaName(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            GuardInvalidStringsInName(value);

            if (!value.StartsWith("corpora", StringComparison.InvariantCultureIgnoreCase))
            {
                return $"corpora/{value}";
            }

            return value;
        }

        public static string SanitizeDocumentName(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            GuardInvalidStringsInName(value);

            if (!value.StartsWith("document", StringComparison.InvariantCultureIgnoreCase))
            {
                return $"documents/{value}";
            }

            return value;
        }

        public static Part FromBytes(this Part part, string value, string mimeType)
        {
            if (string.IsNullOrEmpty(value)) return part;

            part.Text = value;
            return part;
        }

        public static Part FromCodeExecutionResult(this Part part, string outcome, string output)
        {
            if (string.IsNullOrEmpty(outcome)) return part;

            part.Text = outcome;
            return part;
        }

        public static Part FromExecutableCode(this Part part, string code, Language language)
        {
            if (string.IsNullOrEmpty(code)) return part;

            part.Text = code;
            return part;
        }

        public static Part FromFunctionCall(this Part part, string name, string[] args)
        {
            if (string.IsNullOrEmpty(name)) return part;

            part.Text = name;
            return part;
        }

        public static Part FromFunctionResponse(this Part part, string name, dynamic response)
        {
            if (string.IsNullOrEmpty(name)) return part;

            part.Text = name;
            return part;
        }

        public static Part FromText(this Part part, string value)
        {
            if (string.IsNullOrEmpty(value)) return part;

            part.Text = value;
            return part;
        }

        public static Part FromUri(this Part part, string uri, string? mimeType)
        {
            if (string.IsNullOrEmpty(uri)) return part;

            mimeType ??= GetMimeType(uri);
            part.FileData = new FileData() { FileUri = uri, MimeType = mimeType };
            return part;
        }

        public static Part FromVideoMetadata(this Part part, string startOffset, string endOffset, double fps)
        {
            part.VideoMetadata ??= new();
            part.VideoMetadata.StartOffset = startOffset;
            part.VideoMetadata.EndOffset = endOffset;
            part.VideoMetadata.Fps = fps;
            return part;
        }

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
            if (response.PromptFeedback is { BlockReason: not BlockReason.BlockedReasonUnspecified })
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

        internal static async Task<byte[]> ReadImageFileAsync(string url)
        {
            return await ReadImageFileAsync(url, CancellationToken.None);
        }

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

        internal static async Task<string> ReadImageFileBase64Async(string url,
            CancellationToken cancellationToken = default)
        {
            byte[] imageBytes = await ReadImageFileAsync(url, cancellationToken);
            return Convert.ToBase64String(imageBytes);
        }

        internal static string GetMimeType(string uri)
        {
            if (uri == null) throw new ArgumentNullException(nameof(uri));

            var extension = Path.GetExtension(uri).ToLower();
            if (extension.StartsWith(".", StringComparison.OrdinalIgnoreCase))
                extension = extension.Substring(1);

            switch (extension)
            {
                #region Big freaking list of mime types

                case "323": return "text/h323";
                case "3g2": return "video/3gpp2";
                case "3gp": return "video/3gpp";
                case "3gp2": return "video/3gpp2";
                case "3gpp": return "video/3gpp";
                case "7z": return "application/x-7z-compressed";
                case "aa": return "audio/audible";
                case "aac": return "audio/aac";
                case "aaf": return "application/octet-stream";
                case "aax": return "audio/vnd.audible.aax";
                case "ac3": return "audio/ac3";
                case "aca": return "application/octet-stream";
                case "accda": return "application/msaccess.addin";
                case "accdb": return "application/msaccess";
                case "accdc": return "application/msaccess.cab";
                case "accde": return "application/msaccess";
                case "accdr": return "application/msaccess.runtime";
                case "accdt": return "application/msaccess";
                case "accdw": return "application/msaccess.webapplication";
                case "accft": return "application/msaccess.ftemplate";
                case "acx": return "application/internet-property-stream";
                case "addin": return "text/xml";
                case "ade": return "application/msaccess";
                case "adobebridge": return "application/x-bridge-url";
                case "adp": return "application/msaccess";
                case "adt": return "audio/vnd.dlna.adts";
                case "adts": return "audio/aac";
                case "afm": return "application/octet-stream";
                case "ai": return "application/postscript";
                case "aif": return "audio/x-aiff";
                case "aifc": return "audio/aiff";
                case "aiff": return "audio/aiff";
                case "air": return "application/vnd.adobe.air-application-installer-package+zip";
                case "amc": return "application/x-mpeg";
                case "application": return "application/x-ms-application";
                case "art": return "image/x-jg";
                case "asa": return "application/xml";
                case "asax": return "application/xml";
                case "ascx": return "application/xml";
                case "asd": return "application/octet-stream";
                case "asf": return "video/x-ms-asf";
                case "ashx": return "application/xml";
                case "asi": return "application/octet-stream";
                case "asm": return "text/plain";
                case "asmx": return "application/xml";
                case "aspx": return "application/xml";
                case "asr": return "video/x-ms-asf";
                case "asx": return "video/x-ms-asf";
                case "atom": return "application/atom+xml";
                case "au": return "audio/basic";
                case "avi": return "video/x-msvideo";
                case "axs": return "application/olescript";
                case "bas": return "text/plain";
                case "bcpio": return "application/x-bcpio";
                case "bin": return "application/octet-stream";
                case "bmp": return "image/bmp";
                case "c": return "text/plain";
                case "cab": return "application/octet-stream";
                case "caf": return "audio/x-caf";
                case "calx": return "application/vnd.ms-office.calx";
                case "cat": return "application/vnd.ms-pki.seccat";
                case "cc": return "text/plain";
                case "cd": return "text/plain";
                case "cdda": return "audio/aiff";
                case "cdf": return "application/x-cdf";
                case "cer": return "application/x-x509-ca-cert";
                case "chm": return "application/octet-stream";
                case "class": return "application/x-java-applet";
                case "clp": return "application/x-msclip";
                case "cmx": return "image/x-cmx";
                case "cnf": return "text/plain";
                case "cod": return "image/cis-cod";
                case "config": return "application/xml";
                case "contact": return "text/x-ms-contact";
                case "coverage": return "application/xml";
                case "cpio": return "application/x-cpio";
                case "cpp": return "text/plain";
                case "crd": return "application/x-mscardfile";
                case "crl": return "application/pkix-crl";
                case "crt": return "application/x-x509-ca-cert";
                case "cs": return "text/plain";
                case "csdproj": return "text/plain";
                case "csh": return "application/x-csh";
                case "csproj": return "text/plain";
                case "css": return "text/css";
                case "csv": return "text/csv";
                case "cur": return "application/octet-stream";
                case "cxx": return "text/plain";
                case "dat": return "application/octet-stream";
                case "datasource": return "application/xml";
                case "dbproj": return "text/plain";
                case "dcr": return "application/x-director";
                case "def": return "text/plain";
                case "deploy": return "application/octet-stream";
                case "der": return "application/x-x509-ca-cert";
                case "dgml": return "application/xml";
                case "dib": return "image/bmp";
                case "dif": return "video/x-dv";
                case "dir": return "application/x-director";
                case "disco": return "text/xml";
                case "dll": return "application/x-msdownload";
                case "dll.config": return "text/xml";
                case "dlm": return "text/dlm";
                case "doc": return "application/msword";
                case "docm": return "application/vnd.ms-word.document.macroenabled.12";
                case "docx": return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                case "dot": return "application/msword";
                case "dotm": return "application/vnd.ms-word.template.macroenabled.12";
                case "dotx": return "application/vnd.openxmlformats-officedocument.wordprocessingml.template";
                case "dsp": return "application/octet-stream";
                case "dsw": return "text/plain";
                case "dtd": return "text/xml";
                case "dtsconfig": return "text/xml";
                case "dv": return "video/x-dv";
                case "dvi": return "application/x-dvi";
                case "dwf": return "drawing/x-dwf";
                case "dwp": return "application/octet-stream";
                case "dxr": return "application/x-director";
                case "eml": return "message/rfc822";
                case "emz": return "application/octet-stream";
                case "eot": return "application/octet-stream";
                case "eps": return "application/postscript";
                case "etl": return "application/etl";
                case "etx": return "text/x-setext";
                case "evy": return "application/envoy";
                case "exe": return "application/octet-stream";
                case "exe.config": return "text/xml";
                case "fdf": return "application/vnd.fdf";
                case "fif": return "application/fractals";
                case "filters": return "application/xml";
                case "fla": return "application/octet-stream";
                case "flr": return "x-world/x-vrml";
                case "flv": return "video/x-flv";
                case "fsscript": return "application/fsharp-script";
                case "fsx": return "application/fsharp-script";
                case "generictest": return "application/xml";
                case "gif": return "image/gif";
                case "group": return "text/x-ms-group";
                case "gsm": return "audio/x-gsm";
                case "gtar": return "application/x-gtar";
                case "gz": return "application/x-gzip";
                case "h": return "text/plain";
                case "hdf": return "application/x-hdf";
                case "hdml": return "text/x-hdml";
                case "hhc": return "application/x-oleobject";
                case "hhk": return "application/octet-stream";
                case "hhp": return "application/octet-stream";
                case "hlp": return "application/winhlp";
                case "hpp": return "text/plain";
                case "hqx": return "application/mac-binhex40";
                case "hta": return "application/hta";
                case "htc": return "text/x-component";
                case "htm": return "text/html";
                case "html": return "text/html";
                case "htt": return "text/webviewhtml";
                case "hxa": return "application/xml";
                case "hxc": return "application/xml";
                case "hxd": return "application/octet-stream";
                case "hxe": return "application/xml";
                case "hxf": return "application/xml";
                case "hxh": return "application/octet-stream";
                case "hxi": return "application/octet-stream";
                case "hxk": return "application/xml";
                case "hxq": return "application/octet-stream";
                case "hxr": return "application/octet-stream";
                case "hxs": return "application/octet-stream";
                case "hxt": return "text/html";
                case "hxv": return "application/xml";
                case "hxw": return "application/octet-stream";
                case "hxx": return "text/plain";
                case "i": return "text/plain";
                case "ico": return "image/x-icon";
                case "ics": return "application/octet-stream";
                case "idl": return "text/plain";
                case "ief": return "image/ief";
                case "iii": return "application/x-iphone";
                case "inc": return "text/plain";
                case "inf": return "application/octet-stream";
                case "inl": return "text/plain";
                case "ins": return "application/x-internet-signup";
                case "ipa": return "application/x-itunes-ipa";
                case "ipg": return "application/x-itunes-ipg";
                case "ipproj": return "text/plain";
                case "ipsw": return "application/x-itunes-ipsw";
                case "iqy": return "text/x-ms-iqy";
                case "isp": return "application/x-internet-signup";
                case "ite": return "application/x-itunes-ite";
                case "itlp": return "application/x-itunes-itlp";
                case "itms": return "application/x-itunes-itms";
                case "itpc": return "application/x-itunes-itpc";
                case "ivf": return "video/x-ivf";
                case "jar": return "application/java-archive";
                case "java": return "application/octet-stream";
                case "jck": return "application/liquidmotion";
                case "jcz": return "application/liquidmotion";
                case "jfif": return "image/pjpeg";
                case "jnlp": return "application/x-java-jnlp-file";
                case "jpb": return "application/octet-stream";
                case "jpe": return "image/jpeg";
                case "jpeg": return "image/jpeg";
                case "jpg": return "image/jpeg";
                case "js": return "application/x-javascript";
                case "jsx": return "text/jscript";
                case "jsxbin": return "text/plain";
                case "latex": return "application/x-latex";
                case "library-ms": return "application/windows-library+xml";
                case "lit": return "application/x-ms-reader";
                case "loadtest": return "application/xml";
                case "lpk": return "application/octet-stream";
                case "lsf": return "video/x-la-asf";
                case "lst": return "text/plain";
                case "lsx": return "video/x-la-asf";
                case "lzh": return "application/octet-stream";
                case "m13": return "application/x-msmediaview";
                case "m14": return "application/x-msmediaview";
                case "m1v": return "video/mpeg";
                case "m2t": return "video/vnd.dlna.mpeg-tts";
                case "m2ts": return "video/vnd.dlna.mpeg-tts";
                case "m2v": return "video/mpeg";
                case "m3u": return "audio/x-mpegurl";
                case "m3u8": return "audio/x-mpegurl";
                case "m4a": return "audio/m4a";
                case "m4b": return "audio/m4b";
                case "m4p": return "audio/m4p";
                case "m4r": return "audio/x-m4r";
                case "m4v": return "video/x-m4v";
                case "mac": return "image/x-macpaint";
                case "mak": return "text/plain";
                case "man": return "application/x-troff-man";
                case "manifest": return "application/x-ms-manifest";
                case "map": return "text/plain";
                case "master": return "application/xml";
                case "mda": return "application/msaccess";
                case "mdb": return "application/x-msaccess";
                case "mde": return "application/msaccess";
                case "mdp": return "application/octet-stream";
                case "me": return "application/x-troff-me";
                case "mfp": return "application/x-shockwave-flash";
                case "mht": return "message/rfc822";
                case "mhtml": return "message/rfc822";
                case "mid": return "audio/mid";
                case "midi": return "audio/mid";
                case "mix": return "application/octet-stream";
                case "mk": return "text/plain";
                case "mmf": return "application/x-smaf";
                case "mno": return "text/xml";
                case "mny": return "application/x-msmoney";
                case "mod": return "video/mpeg";
                case "mov": return "video/quicktime";
                case "movie": return "video/x-sgi-movie";
                case "mp2": return "video/mpeg";
                case "mp2v": return "video/mpeg";
                case "mp3": return "audio/mpeg";
                case "mp4": return "video/mp4";
                case "mp4v": return "video/mp4";
                case "mpa": return "video/mpeg";
                case "mpe": return "video/mpeg";
                case "mpeg": return "video/mpeg";
                case "mpf": return "application/vnd.ms-mediapackage";
                case "mpg": return "video/mpeg";
                case "mpp": return "application/vnd.ms-project";
                case "mpv2": return "video/mpeg";
                case "mqv": return "video/quicktime";
                case "ms": return "application/x-troff-ms";
                case "msi": return "application/octet-stream";
                case "mso": return "application/octet-stream";
                case "mts": return "video/vnd.dlna.mpeg-tts";
                case "mtx": return "application/xml";
                case "mvb": return "application/x-msmediaview";
                case "mvc": return "application/x-miva-compiled";
                case "mxp": return "application/x-mmxp";
                case "nc": return "application/x-netcdf";
                case "nsc": return "video/x-ms-asf";
                case "nws": return "message/rfc822";
                case "ocx": return "application/octet-stream";
                case "oda": return "application/oda";
                case "odc": return "text/x-ms-odc";
                case "odh": return "text/plain";
                case "odl": return "text/plain";
                case "odp": return "application/vnd.oasis.opendocument.presentation";
                case "ods": return "application/oleobject";
                case "odt": return "application/vnd.oasis.opendocument.text";
                case "one": return "application/onenote";
                case "onea": return "application/onenote";
                case "onepkg": return "application/onenote";
                case "onetmp": return "application/onenote";
                case "onetoc": return "application/onenote";
                case "onetoc2": return "application/onenote";
                case "orderedtest": return "application/xml";
                case "osdx": return "application/opensearchdescription+xml";
                case "p10": return "application/pkcs10";
                case "p12": return "application/x-pkcs12";
                case "p7b": return "application/x-pkcs7-certificates";
                case "p7c": return "application/pkcs7-mime";
                case "p7m": return "application/pkcs7-mime";
                case "p7r": return "application/x-pkcs7-certreqresp";
                case "p7s": return "application/pkcs7-signature";
                case "pbm": return "image/x-portable-bitmap";
                case "pcast": return "application/x-podcast";
                case "pct": return "image/pict";
                case "pcx": return "application/octet-stream";
                case "pcz": return "application/octet-stream";
                case "pdf": return "application/pdf";
                case "pfb": return "application/octet-stream";
                case "pfm": return "application/octet-stream";
                case "pfx": return "application/x-pkcs12";
                case "pgm": return "image/x-portable-graymap";
                case "pic": return "image/pict";
                case "pict": return "image/pict";
                case "pkgdef": return "text/plain";
                case "pkgundef": return "text/plain";
                case "pko": return "application/vnd.ms-pki.pko";
                case "pls": return "audio/scpls";
                case "pma": return "application/x-perfmon";
                case "pmc": return "application/x-perfmon";
                case "pml": return "application/x-perfmon";
                case "pmr": return "application/x-perfmon";
                case "pmw": return "application/x-perfmon";
                case "png": return "image/png";
                case "pnm": return "image/x-portable-anymap";
                case "pnt": return "image/x-macpaint";
                case "pntg": return "image/x-macpaint";
                case "pnz": return "image/png";
                case "pot": return "application/vnd.ms-powerpoint";
                case "potm": return "application/vnd.ms-powerpoint.template.macroenabled.12";
                case "potx": return "application/vnd.openxmlformats-officedocument.presentationml.template";
                case "ppa": return "application/vnd.ms-powerpoint";
                case "ppam": return "application/vnd.ms-powerpoint.addin.macroenabled.12";
                case "ppm": return "image/x-portable-pixmap";
                case "pps": return "application/vnd.ms-powerpoint";
                case "ppsm": return "application/vnd.ms-powerpoint.slideshow.macroenabled.12";
                case "ppsx": return "application/vnd.openxmlformats-officedocument.presentationml.slideshow";
                case "ppt": return "application/vnd.ms-powerpoint";
                case "pptm": return "application/vnd.ms-powerpoint.presentation.macroenabled.12";
                case "pptx": return "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                case "prf": return "application/pics-rules";
                case "prm": return "application/octet-stream";
                case "prx": return "application/octet-stream";
                case "ps": return "application/postscript";
                case "psc1": return "application/powershell";
                case "psd": return "application/octet-stream";
                case "psess": return "application/xml";
                case "psm": return "application/octet-stream";
                case "psp": return "application/octet-stream";
                case "pub": return "application/x-mspublisher";
                case "pwz": return "application/vnd.ms-powerpoint";
                case "qht": return "text/x-html-insertion";
                case "qhtm": return "text/x-html-insertion";
                case "qt": return "video/quicktime";
                case "qti": return "image/x-quicktime";
                case "qtif": return "image/x-quicktime";
                case "qtl": return "application/x-quicktimeplayer";
                case "qxd": return "application/octet-stream";
                case "ra": return "audio/x-pn-realaudio";
                case "ram": return "audio/x-pn-realaudio";
                case "rar": return "application/octet-stream";
                case "ras": return "image/x-cmu-raster";
                case "rat": return "application/rat-file";
                case "rc": return "text/plain";
                case "rc2": return "text/plain";
                case "rct": return "text/plain";
                case "rdlc": return "application/xml";
                case "resx": return "application/xml";
                case "rf": return "image/vnd.rn-realflash";
                case "rgb": return "image/x-rgb";
                case "rgs": return "text/plain";
                case "rm": return "application/vnd.rn-realmedia";
                case "rmi": return "audio/mid";
                case "rmp": return "application/vnd.rn-rn_music_package";
                case "roff": return "application/x-troff";
                case "rpm": return "audio/x-pn-realaudio-plugin";
                case "rqy": return "text/x-ms-rqy";
                case "rtf": return "application/rtf";
                case "rtx": return "text/richtext";
                case "ruleset": return "application/xml";
                case "s": return "text/plain";
                case "safariextz": return "application/x-safari-safariextz";
                case "scd": return "application/x-msschedule";
                case "sct": return "text/scriptlet";
                case "sd2": return "audio/x-sd2";
                case "sdp": return "application/sdp";
                case "sea": return "application/octet-stream";
                case "searchconnector-ms": return "application/windows-search-connector+xml";
                case "setpay": return "application/set-payment-initiation";
                case "setreg": return "application/set-registration-initiation";
                case "settings": return "application/xml";
                case "sgimb": return "application/x-sgimb";
                case "sgml": return "text/sgml";
                case "sh": return "application/x-sh";
                case "shar": return "application/x-shar";
                case "shtml": return "text/html";
                case "sit": return "application/x-stuffit";
                case "sitemap": return "application/xml";
                case "skin": return "application/xml";
                case "sldm": return "application/vnd.ms-powerpoint.slide.macroenabled.12";
                case "sldx": return "application/vnd.openxmlformats-officedocument.presentationml.slide";
                case "slk": return "application/vnd.ms-excel";
                case "sln": return "text/plain";
                case "slupkg-ms": return "application/x-ms-license";
                case "smd": return "audio/x-smd";
                case "smi": return "application/octet-stream";
                case "smx": return "audio/x-smd";
                case "smz": return "audio/x-smd";
                case "snd": return "audio/basic";
                case "snippet": return "application/xml";
                case "snp": return "application/octet-stream";
                case "sol": return "text/plain";
                case "sor": return "text/plain";
                case "spc": return "application/x-pkcs7-certificates";
                case "spl": return "application/futuresplash";
                case "src": return "application/x-wais-source";
                case "srf": return "text/plain";
                case "ssisdeploymentmanifest": return "text/xml";
                case "ssm": return "application/streamingmedia";
                case "sst": return "application/vnd.ms-pki.certstore";
                case "stl": return "application/vnd.ms-pki.stl";
                case "sv4cpio": return "application/x-sv4cpio";
                case "sv4crc": return "application/x-sv4crc";
                case "svc": return "application/xml";
                case "swf": return "application/x-shockwave-flash";
                case "t": return "application/x-troff";
                case "tar": return "application/x-tar";
                case "tcl": return "application/x-tcl";
                case "testrunconfig": return "application/xml";
                case "testsettings": return "application/xml";
                case "tex": return "application/x-tex";
                case "texi": return "application/x-texinfo";
                case "texinfo": return "application/x-texinfo";
                case "tgz": return "application/x-compressed";
                case "thmx": return "application/vnd.ms-officetheme";
                case "thn": return "application/octet-stream";
                case "tif": return "image/tiff";
                case "tiff": return "image/tiff";
                case "tlh": return "text/plain";
                case "tli": return "text/plain";
                case "toc": return "application/octet-stream";
                case "tr": return "application/x-troff";
                case "trm": return "application/x-msterminal";
                case "trx": return "application/xml";
                case "ts": return "video/vnd.dlna.mpeg-tts";
                case "tsv": return "text/tab-separated-values";
                case "ttf": return "application/octet-stream";
                case "tts": return "video/vnd.dlna.mpeg-tts";
                case "txt": return "text/plain";
                case "u32": return "application/octet-stream";
                case "uls": return "text/iuls";
                case "user": return "text/plain";
                case "ustar": return "application/x-ustar";
                case "vb": return "text/plain";
                case "vbdproj": return "text/plain";
                case "vbk": return "video/mpeg";
                case "vbproj": return "text/plain";
                case "vbs": return "text/vbscript";
                case "vcf": return "text/x-vcard";
                case "vcproj": return "application/xml";
                case "vcs": return "text/plain";
                case "vcxproj": return "application/xml";
                case "vddproj": return "text/plain";
                case "vdp": return "text/plain";
                case "vdproj": return "text/plain";
                case "vdx": return "application/vnd.ms-visio.viewer";
                case "vml": return "text/xml";
                case "vscontent": return "application/xml";
                case "vsct": return "text/xml";
                case "vsd": return "application/vnd.visio";
                case "vsi": return "application/ms-vsi";
                case "vsix": return "application/vsix";
                case "vsixlangpack": return "text/xml";
                case "vsixmanifest": return "text/xml";
                case "vsmdi": return "application/xml";
                case "vspscc": return "text/plain";
                case "vss": return "application/vnd.visio";
                case "vsscc": return "text/plain";
                case "vssettings": return "text/xml";
                case "vssscc": return "text/plain";
                case "vst": return "application/vnd.visio";
                case "vstemplate": return "text/xml";
                case "vsto": return "application/x-ms-vsto";
                case "vsw": return "application/vnd.visio";
                case "vsx": return "application/vnd.visio";
                case "vtx": return "application/vnd.visio";
                case "wav": return "audio/wav";
                case "wave": return "audio/wav";
                case "wax": return "audio/x-ms-wax";
                case "wbk": return "application/msword";
                case "wbmp": return "image/vnd.wap.wbmp";
                case "wcm": return "application/vnd.ms-works";
                case "wdb": return "application/vnd.ms-works";
                case "wdp": return "image/vnd.ms-photo";
                case "webarchive": return "application/x-safari-webarchive";
                case "webtest": return "application/xml";
                case "wiq": return "application/xml";
                case "wiz": return "application/msword";
                case "wks": return "application/vnd.ms-works";
                case "wlmp": return "application/wlmoviemaker";
                case "wlpginstall": return "application/x-wlpg-detect";
                case "wlpginstall3": return "application/x-wlpg3-detect";
                case "wm": return "video/x-ms-wm";
                case "wma": return "audio/x-ms-wma";
                case "wmd": return "application/x-ms-wmd";
                case "wmf": return "application/x-msmetafile";
                case "wml": return "text/vnd.wap.wml";
                case "wmlc": return "application/vnd.wap.wmlc";
                case "wmls": return "text/vnd.wap.wmlscript";
                case "wmlsc": return "application/vnd.wap.wmlscriptc";
                case "wmp": return "video/x-ms-wmp";
                case "wmv": return "video/x-ms-wmv";
                case "wmx": return "video/x-ms-wmx";
                case "wmz": return "application/x-ms-wmz";
                case "wpl": return "application/vnd.ms-wpl";
                case "wps": return "application/vnd.ms-works";
                case "wri": return "application/x-mswrite";
                case "wrl": return "x-world/x-vrml";
                case "wrz": return "x-world/x-vrml";
                case "wsc": return "text/scriptlet";
                case "wsdl": return "text/xml";
                case "wvx": return "video/x-ms-wvx";
                case "x": return "application/directx";
                case "xaf": return "x-world/x-vrml";
                case "xaml": return "application/xaml+xml";
                case "xap": return "application/x-silverlight-app";
                case "xbap": return "application/x-ms-xbap";
                case "xbm": return "image/x-xbitmap";
                case "xdr": return "text/plain";
                case "xht": return "application/xhtml+xml";
                case "xhtml": return "application/xhtml+xml";
                case "xla": return "application/vnd.ms-excel";
                case "xlam": return "application/vnd.ms-excel.addin.macroenabled.12";
                case "xlc": return "application/vnd.ms-excel";
                case "xld": return "application/vnd.ms-excel";
                case "xlk": return "application/vnd.ms-excel";
                case "xll": return "application/vnd.ms-excel";
                case "xlm": return "application/vnd.ms-excel";
                case "xls": return "application/vnd.ms-excel";
                case "xlsb": return "application/vnd.ms-excel.sheet.binary.macroenabled.12";
                case "xlsm": return "application/vnd.ms-excel.sheet.macroenabled.12";
                case "xlsx": return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                case "xlt": return "application/vnd.ms-excel";
                case "xltm": return "application/vnd.ms-excel.template.macroenabled.12";
                case "xltx": return "application/vnd.openxmlformats-officedocument.spreadsheetml.template";
                case "xlw": return "application/vnd.ms-excel";
                case "xml": return "text/xml";
                case "xmta": return "application/xml";
                case "xof": return "x-world/x-vrml";
                case "xoml": return "text/plain";
                case "xpm": return "image/x-xpixmap";
                case "xps": return "application/vnd.ms-xpsdocument";
                case "xrm-ms": return "text/xml";
                case "xsc": return "application/xml";
                case "xsd": return "text/xml";
                case "xsf": return "text/xml";
                case "xsl": return "text/xml";
                case "xslt": return "text/xml";
                case "xsn": return "application/octet-stream";
                case "xss": return "application/xml";
                case "xtp": return "application/octet-stream";
                case "xwd": return "image/x-xwindowdump";
                case "z": return "application/x-compress";
                case "zip": return "application/x-zip-compressed";

                #endregion

                default: return "application/octet-stream";
            }
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

        internal static string GetNormalizedName(this Delegate callback)
        {
            return callback.Method.Name.ToSnakeCase();
        }

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
	        public void UseJsonMode()
	        {
		        request.GenerationConfig ??= new GenerationConfig();
		        request.GenerationConfig.ResponseMimeType ??= Constants.MediaType;
	        }
	        
	        public void UseGoogleSearch()
	        {
		        Tools defaultTools = [new Tool { GoogleSearch = new() }];
		        request.Tools ??= defaultTools;
		        if (request.Tools != null && !request.Tools.Any(t => t.GoogleSearch is not null))
		        {
			        request.Tools.AddRange(defaultTools);
		        }
	        }

	        public void UseGoogleMaps(bool? enableWidget = false)
	        {
		        Tools defaultTools = [new Tool() { GoogleMaps = new() { EnableWidget = enableWidget } }];
		        request.Tools ??= defaultTools;
		        if (request.Tools != null && !request.Tools.Any(t => t.GoogleMaps is not null))
		        {
			        request.Tools.AddRange(defaultTools);
		        }
	        }

	        public void UseGrounding()
	        {
		        Tools defaultTools = [new Tool { GoogleSearchRetrieval = new() }];
		        request.Tools ??= defaultTools;
		        if (request.Tools != null && !request.Tools.Any(t => t.GoogleSearchRetrieval is not null))
		        {
			        request.Tools.AddRange(defaultTools);
		        }
	        }

	        public void UseCodeExecution()
	        {
		        Tools defaultTools = [new Tool { CodeExecution = new() }];
		        request.Tools ??= defaultTools;
		        if (request.Tools != null && !request.Tools.Any(t => t.CodeExecution is not null))
		        {
			        request.Tools.AddRange(defaultTools);
		        }
	        }
        }
    }
}