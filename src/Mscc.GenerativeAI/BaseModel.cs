#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Security.Authentication;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
#endif
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Text;

namespace Mscc.GenerativeAI
{
    public abstract class BaseModel : BaseLogger
    {
        protected const string BaseUrlGoogleAi = "https://generativelanguage.googleapis.com/{version}";
        protected const string BaseUrlVertexAi = "https://{region}-aiplatform.googleapis.com/{version}/projects/{projectId}/locations/{region}";

        protected readonly string _publisher = "google";
        protected readonly JsonSerializerOptions _options;

        protected string _model;
        protected string? _apiKey;
        protected string? _accessToken;
        protected string? _projectId;
        protected string _region = "us-central1";
        protected string? _endpointId;

#if NET472_OR_GREATER || NETSTANDARD2_0
        protected static readonly Version _httpVersion = HttpVersion.Version11;
        protected static readonly HttpClient Client = new HttpClient(new HttpClientHandler
        {
            SslProtocols = SslProtocols.Tls12
        });
#else
        protected static readonly Version _httpVersion = HttpVersion.Version11;
        protected static readonly HttpClient Client = new HttpClient(new SocketsHttpHandler
        {
            PooledConnectionLifetime = TimeSpan.FromMinutes(30),
            EnableMultipleHttp2Connections = true
        })
        {
            DefaultRequestVersion = _httpVersion,
            DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrHigher
        };
#endif

        protected virtual string Version => ApiVersion.V1;

        /// <summary>
        /// Gets or sets the name of the model to use.
        /// </summary>
        public string Model
        {
            get => _model;
            set => _model = value.SanitizeModelName();
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
        public virtual string? ApiKey
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
        /// Returns the region to use for the request.
        /// </summary>
        public string Region
        {
            get => _region;
            set => _region = value;
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
        /// Throws a <see cref="NotSupportedException"/>, if the functionality is not supported by combination of settings.
        /// </summary>
        protected virtual void ThrowIfUnsupportedRequest<T>(T request) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger">Optional. Logger instance used for logging</param>
        public BaseModel(ILogger? logger = null) : base(logger)
        {
            SetDefaultRequestHeaders();
            _options = DefaultJsonSerializerOptions();
            GenerativeAIExtensions.ReadDotEnv();
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
        public BaseModel(string? projectId = null, string? region = null, 
            string? model = null, ILogger? logger = null) : this(logger)
        {
            var credentialsFile = 
                Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS") ?? 
                Environment.GetEnvironmentVariable("GOOGLE_WEB_CREDENTIALS") ?? 
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "gcloud",
                    "application_default_credentials.json");
            var credentials = GetCredentialsFromFile(credentialsFile);
            AccessToken = _accessToken ?? 
                          GetAccessTokenFromAdc();
            ProjectId = projectId ??
                        Environment.GetEnvironmentVariable("GOOGLE_PROJECT_ID") ??
                        credentials?.ProjectId ?? 
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
        protected string ParseUrl(string url, string method = "")
        {
            var replacements = GetReplacements();
            replacements.Add("method", method);

            do
            {
                url = Regex.Replace(url, @"\{(?<name>.*?)\}",
                    match => replacements.TryGetValue(match.Groups["name"].Value, out var value) ? value : "");
            } while (url.Contains("{"));

            return url;

            Dictionary<string, string> GetReplacements()
            {
                return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    { "BaseUrlGoogleAi", BaseUrlGoogleAi },
                    { "BaseUrlVertexAi", BaseUrlVertexAi },
                    { "version", Version },
                    { "model", _model },
                    { "apikey", _apiKey ?? "" },
                    { "projectId", _projectId ?? "" },
                    { "region", _region },
                    { "location", _region },
                    { "publisher", _publisher },
                    { "endpointId", _endpointId }
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
        /// <typeparam name="T">Type to deserialize response into.</typeparam>
        /// <param name="response">Response from an API call in JSON format.</param>
        /// <returns>An instance of type T.</returns>
        protected async Task<T> Deserialize<T>(HttpResponseMessage response)
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
        /// Set default request headers of <see cref="HttpClient"/>.
        /// </summary>
        private void SetDefaultRequestHeaders()
        {
            // ('x-goog-api-client', 'genai-py/0.8.2 gl-python/3.12.3 grpc/1.68.0 gax/2.23.0')
            // ('x-goog-request-params', 'model=models/imagen-3.0-generate-001')
            var productHeaderValue = new ProductHeaderValue(
                name: Assembly.GetExecutingAssembly().GetName().Name ?? "Mscc.GenerativeAI", 
                version: Assembly.GetExecutingAssembly().GetName().Version?.ToString());
            var productInfoHeaderValue = new ProductInfoHeaderValue(productHeaderValue);
            Client.DefaultRequestHeaders.UserAgent.Add(productInfoHeaderValue);
            Client.DefaultRequestHeaders.Add(name: "x-goog-api-client", productInfoHeaderValue.ToString());
        }

        /// <summary>
        /// Get default options for JSON serialization.
        /// </summary>
        /// <returns>default options for JSON serialization.</returns>
        private JsonSerializerOptions DefaultJsonSerializerOptions()
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
        /// Get credentials from specified file.
        /// </summary>
        /// <remarks>This would usually be the secret.json file from Google Cloud Platform.</remarks>
        /// <param name="credentialsFile">File with credentials to read.</param>
        /// <returns>Credentials read from file.</returns>
        private Credentials? GetCredentialsFromFile(string credentialsFile)
        {
            Credentials? credentials = null;
            if (File.Exists(credentialsFile))
            {
                var options = DefaultJsonSerializerOptions();
                options.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
                using var stream = new FileStream(credentialsFile, FileMode.Open, FileAccess.Read);
                credentials = JsonSerializer.Deserialize<Credentials>(stream, options);
            }

            return credentials;
        }
 
        /// <summary>
        /// This method uses the gcloud command-line tool to retrieve an access token from the Application Default Credentials (ADC).
        /// It is specific to Google Cloud Platform and allows easy authentication with the Gemini API on Google Cloud.
        /// Reference: https://cloud.google.com/docs/authentication 
        /// </summary>
        /// <returns>The access token.</returns>
        private string GetAccessTokenFromAdc()
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
        private string RunExternalExe(string filename, string arguments)
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
            process.OutputDataReceived += (sender, args) => stdOutput.AppendLine(args.Data);
            process.ErrorDataReceived += (sender, args) => stdError.AppendLine(args.Data);

            try
            {
                process.Start();
                process.BeginOutputReadLine();
                process.WaitForExit();
            }
            catch (Exception e)
            {
                Logger.LogRunExternalExe("OS error while executing " + Format(filename, arguments)+ ": " + e.Message);
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