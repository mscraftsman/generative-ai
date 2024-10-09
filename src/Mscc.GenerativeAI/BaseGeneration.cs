#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Authentication;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
#endif
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Text;

namespace Mscc.GenerativeAI
{
    public abstract class BaseGeneration : GenerationBase
    {
        protected readonly string _region = "us-central1";
        protected readonly string _publisher = "google";
        protected readonly JsonSerializerOptions _options;
        internal readonly Credentials? _credentials;

        protected string _model;
        protected string? _apiKey;
        protected string? _accessToken;
        protected string? _projectId;

#if NET472_OR_GREATER || NETSTANDARD2_0
        protected static readonly Version _httpVersion = HttpVersion.Version11;
        protected static readonly HttpClient Client = new HttpClient(new HttpClientHandler
        {
            SslProtocols = SslProtocols.Tls12,
        });
#else
        protected static readonly Version _httpVersion = HttpVersion.Version30;
        protected static readonly HttpClient Client = new HttpClient(new SocketsHttpHandler
        {
            PooledConnectionLifetime = TimeSpan.FromMinutes(30),
            EnableMultipleHttp2Connections = true,
        })
        {
            DefaultRequestVersion = _httpVersion
        };
#endif

        protected string Version => ApiVersion.V1;

        protected string Model
        {
            set
            {
                if (value != null)
                {
                    _model = value;
                }
            }
        }

        /// <summary>
        /// Returns the name of the model. 
        /// </summary>
        /// <returns>Name of the model.</returns>
        public string Name => _model;

        /// <summary>
        /// Sets the API key to use for the request.
        /// </summary>
        /// <remarks>
        /// The value can only be set or modified before the first request is made.
        /// </remarks>
        public string? ApiKey
        {
            set
            {
                _apiKey = value;
                if (!string.IsNullOrEmpty(_apiKey))
                {
                    if (Client.DefaultRequestHeaders.Contains("x-goog-api-key"))
                    {
                        Client.DefaultRequestHeaders.Remove("x-goog-api-key");
                    }
                    Client.DefaultRequestHeaders.Add("x-goog-api-key", _apiKey);
                }
            }
        }
        
        /// <summary>
        /// Sets the project ID to use for the request.
        /// </summary>
        /// <remarks>
        /// The value can only be set or modified before the first request is made.
        /// </remarks>
        public string? ProjectId
        {
            set
            {
                _projectId = value;
                if (!string.IsNullOrEmpty(_projectId))
                {
                    if (Client.DefaultRequestHeaders.Contains("x-goog-user-project"))
                    {
                        Client.DefaultRequestHeaders.Remove("x-goog-user-project");
                    }
                    Client.DefaultRequestHeaders.Add("x-goog-user-project", _projectId);
                }
            }
        }
        
        /// <summary>
        /// Sets the access token to use for the request.
        /// </summary>
        public string? AccessToken
        {
            set
            {
                _accessToken = value;
                if (value != null)
                    Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
            }
        }

        /// <summary>
        /// Gets or sets the timespan to wait before the request times out.
        /// </summary>
        public TimeSpan Timeout
        {
            get => Client.Timeout;
            set => Client.Timeout = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger">Optional. Logger instance used for logging</param>
        public BaseGeneration(ILogger? logger = null) : base(logger)
        {
            _options = DefaultJsonSerializerOptions();
            GenerativeAIExtensions.ReadDotEnv();
            var credentialsFile = 
                Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS") ?? 
                Environment.GetEnvironmentVariable("GOOGLE_WEB_CREDENTIALS") ?? 
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "gcloud",
                    "application_default_credentials.json");
            _credentials = GetCredentialsFromFile(credentialsFile);
            
            ApiKey = Environment.GetEnvironmentVariable("GOOGLE_API_KEY");
            AccessToken = Environment.GetEnvironmentVariable("GOOGLE_ACCESS_TOKEN"); // ?? GetAccessTokenFromAdc();
            Model = Environment.GetEnvironmentVariable("GOOGLE_AI_MODEL") ?? 
                    GenerativeAI.Model.Gemini15Pro;
            _region = Environment.GetEnvironmentVariable("GOOGLE_REGION") ?? _region;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="region"></param>
        /// <param name="model"></param>
        /// <param name="logger">Optional. Logger instance used for logging</param>
        public BaseGeneration(string? projectId = null, string? region = null, 
            string? model = null, ILogger? logger = null) : this(logger)
        {
            AccessToken = Environment.GetEnvironmentVariable("GOOGLE_ACCESS_TOKEN") ?? 
                          GetAccessTokenFromAdc();
            ProjectId = projectId ??
                        Environment.GetEnvironmentVariable("GOOGLE_PROJECT_ID") ??
                        _credentials?.ProjectId ?? 
                        _projectId;
            _region = region ?? _region;
            Model = model ?? _model;
        }
        
        /// <summary>
        /// Parses the URL template and replaces the placeholder with current values.
        /// Given two API endpoints for Google AI Gemini and Vertex AI Gemini this
        /// method uses regular expressions to replace placeholders in a URL template with actual values.
        /// </summary>
        /// <param name="url">API endpoint to parse.</param>
        /// <param name="method">Method part of the URL to inject</param>
        /// <returns></returns>
        protected string ParseUrl(string url, string? method = default)
        {
            var replacements = GetReplacements();
            replacements.Add("method", method);

            var urlParsed = Regex.Replace(url, @"\{(?<name>.*?)\}",
                match => replacements.TryGetValue(match.Groups["name"].Value, out var value) ? value : "");

            return urlParsed;

            Dictionary<string, string> GetReplacements()
            {
                return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    { "version", Version },
                    { "model", _model },
                    { "apikey", _apiKey },
                    { "projectid", _projectId },
                    { "region", _region },
                    { "location", _region },
                    { "publisher", _publisher }
                };
            }
        }

        /// <summary>
        /// Return serialized JSON string of request payload.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected string Serialize<T>(T request)
        {
            return JsonSerializer.Serialize(request, _options);
        }

        /// <summary>
        /// Return deserialized object from JSON response.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response"></param>
        /// <returns></returns>
        protected async Task<T> Deserialize<T>(HttpResponseMessage? response)
        {
#if NET472_OR_GREATER || NETSTANDARD2_0
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(json, _options);
#else
            var json = await response.Content.ReadAsStringAsync();
            return await response.Content.ReadFromJsonAsync<T>(_options);
#endif
        }

        /// <summary>
        /// Get default options for JSON serialization.
        /// </summary>
        /// <returns></returns>
        internal JsonSerializerOptions DefaultJsonSerializerOptions()
        {
            var options = new JsonSerializerOptions(JsonSerializerDefaults.Web)
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                NumberHandling = JsonNumberHandling.AllowReadingFromString,
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true,
                //WriteIndented = true,
            };
            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseUpper));

            return options;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="credentialsFile"></param>
        /// <returns></returns>
        private Credentials? GetCredentialsFromFile(string credentialsFile)
        {
            Credentials? credentials = null;
            if (File.Exists(credentialsFile))
            {
                var options = DefaultJsonSerializerOptions();
                options.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
                using (var stream = new FileStream(credentialsFile, FileMode.Open, FileAccess.Read))
                {
                    credentials = JsonSerializer.Deserialize<Credentials>(stream, options);
                }
            }

            return credentials;
        }
 
        /// <summary>
        /// This method uses the gcloud command-line tool to retrieve an access token from the Application Default Credentials (ADC).
        /// It is specific to Google Cloud Platform and allows easy authentication with the Gemini API on Google Cloud.
        /// Reference: https://cloud.google.com/docs/authentication 
        /// </summary>
        /// <returns>The access token.</returns>
        protected string GetAccessTokenFromAdc()
        {
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
            {
                return RunExternalExe("cmd.exe", "/c gcloud auth application-default print-access-token").TrimEnd();
            }
            else
            {
                return RunExternalExe("gcloud", "auth application-default print-access-token").TrimEnd();
            }
        }
        
        /// <summary>
        /// Run an external application as process in the underlying operating system, if possible.
        /// </summary>
        /// <param name="filename">The command or application to run.</param>
        /// <param name="arguments">Optional arguments given to the application to run.</param>
        /// <returns>Output from the application.</returns>
        /// <exception cref="Exception"></exception>
        private string RunExternalExe(string filename, string? arguments = null)
        {
            var process = new Process();

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
            var stdOutput = new StringBuilder();
            process.OutputDataReceived += (sender, args) => stdOutput.AppendLine(args.Data); // Use AppendLine rather than Append since args.Data is one line of output, not including the newline character.

            string stdError = null;
            try
            {
                process.Start();
                process.BeginOutputReadLine();
                stdError = process.StandardError.ReadToEnd();
                process.WaitForExit();
            }
            catch (Exception e)
            {
                throw new Exception("OS error while executing " + Format(filename, arguments)+ ": " + e.Message, e);
            }

            if (process.ExitCode == 0)
            {
                return stdOutput.ToString();
            }
            else
            {
                var message = new StringBuilder();

                if (!string.IsNullOrEmpty(stdError))
                {
                    message.AppendLine(stdError);
                }

                if (stdOutput.Length != 0)
                {
                    message.AppendLine("Std output:");
                    message.AppendLine(stdOutput.ToString());
                }

                throw new Exception(Format(filename, arguments) + " finished with exit code = " + process.ExitCode + ": " + message);
            }
        }

        /// <summary>
        /// Formatting string for logging purpose.
        /// </summary>
        /// <param name="filename">The command or application to run.</param>
        /// <param name="arguments">Optional arguments given to the application to run.</param>
        /// <returns>Formatted string containing parameter values.</returns>
        private string Format(string filename, string? arguments)
        {
            return "'" + filename + 
                ((string.IsNullOrEmpty(arguments)) ? string.Empty : " " + arguments) +
                "'";
        }
   }
}