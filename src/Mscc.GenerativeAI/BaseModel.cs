using Microsoft.Extensions.Logging;
using Mscc.GenerativeAI.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Authentication;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// The base class for all models, providing common functionality for API communication.
    /// </summary>
    public abstract class BaseModel : BaseLogger, IDisposable, IAsyncDisposable
    {
        /// <summary>
        /// The base URL for the Google AI API.
        /// </summary>
        protected const string BaseUrlGoogleAi = "https://generativelanguage.googleapis.com/{version}";
        /// <summary>
        /// The base URL for the Vertex AI API.
        /// </summary>
        protected const string BaseUrlVertexAi = "https://{region}-aiplatform.googleapis.com/{version}/projects/{projectId}/locations/{region}";
        /// <summary>
        /// The base URL for the Vertex AI API for global locations.
        /// </summary>
        protected const string BaseUrlVertexAiGlobal = "https://aiplatform.googleapis.com/{version}/projects/{projectId}/locations/global";

        protected readonly string _publisher = "google";
        protected static readonly JsonSerializerOptions ReadOptions = ReadJsonSerializerOptions();
        protected static readonly JsonSerializerOptions WriteOptions = WriteJsonSerializerOptions();

        protected string _model;
        protected string? _apiKey;
        protected string _apiVersion;
        protected string? _accessToken;
        protected string? _projectId;
        protected string _region = "us-central1";
        protected string? _endpointId;

        protected readonly Version _httpVersion = HttpVersion.Version11;
        internal readonly IHttpClientFactory? _httpClientFactory;
        private HttpClient? _httpClient;
        private TimeSpan? _httpTimeout;
        private RequestOptions? _requestOptions;

        /// <summary>
        /// Gets the <see cref="HttpClient"/> instance used for making API requests.
        /// </summary>
        protected HttpClient Client =>
            _httpClient ??= _httpClientFactory?.CreateClient(nameof(BaseModel)) ?? CreateDefaultHttpClient();

        private HttpClient CreateDefaultHttpClient()
        {
#if NET472_OR_GREATER || NETSTANDARD2_0
            // On older .NET frameworks, explicitly set Tls12 for security.
            var handler = new HttpClientHandler
            {
                SslProtocols = SslProtocols.Tls12
            };
            if (_requestOptions?.Proxy != null)
            {
                handler.Proxy = _requestOptions?.Proxy;
                handler.UseProxy = true;
            }
            var client = new HttpClient(new HttpRequestTimeoutHandler(Logger)
            {
                InnerHandler = handler
            });
#else
            // On modern .NET, use SocketsHttpHandler for better performance and connection pooling.
            var handler = new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(30),
                EnableMultipleHttp2Connections = true
            };
            if (_requestOptions?.Proxy != null)
            {
                handler.Proxy = _requestOptions?.Proxy;
                handler.UseProxy = true;
            }
            var client = new HttpClient(new HttpRequestTimeoutHandler(Logger)
            {
                InnerHandler = handler
            })
            {
                DefaultRequestVersion = _httpVersion,
                DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrHigher
            };
#endif
            client.Timeout = System.Threading.Timeout.InfiniteTimeSpan;

            return client;
        }

        /// <summary>
        /// Gets or sets the API version to use.
        /// </summary>
        internal virtual string Version
        {
            get => _apiVersion;
            set => _apiVersion = value;
        }

        /// <summary>
        /// Gets or sets the request options for API calls.
        /// </summary>
        internal RequestOptions? RequestOptions
        {
            get => _requestOptions;
            set => _requestOptions = value;
        }

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
        public string? ApiKey { set => _apiKey = value.GuardApiKey(); }

        /// <summary>
        /// Specify API key in HTTP header
        /// </summary>
        /// <seealso href="https://cloud.google.com/docs/authentication/api-keys-use#using-with-rest">Using an API key with REST</seealso>
        /// <param name="request"><see cref="HttpRequestMessage"/> to send to the API.</param>
        protected virtual void AddApiKeyHeader(HttpRequestMessage request)
        {
            request.AddApiKeyHeader(_apiKey);
        }

        /// <summary>
        /// Sets the access token to use for the request.
        /// </summary>
        public string? AccessToken { set => _accessToken = value; }

        /// <summary>
        /// Sets the project ID to use for the request.
        /// </summary>
        /// <remarks>
        /// The value can only be set or modified before the first request is made.
        /// </remarks>
        public string? ProjectId { set => _projectId = value; }

        /// <summary>
        /// Gets or sets the region to use for the request.
        /// This is used in Vertex AI requests.
        /// The default value is "us-central1".
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
            get => _httpTimeout ?? Client.Timeout;
            set => _httpTimeout = value;
        }

        /// <summary>
        /// A hook to verify if a specific request is supported by the current model configuration.
        /// Throws a <see cref="NotSupportedException"/> if the functionality is not supported.
        /// </summary>
        /// <typeparam name="T">The type of the request object.</typeparam>
        /// <param name="request">The request to verify.</param>
        protected virtual void ThrowIfUnsupportedRequest<T>(T request) { }

        // Instance fields for default headers
        private readonly ProductInfoHeaderValue _defaultUserAgent;
        private readonly KeyValuePair<string, string> _defaultApiClientHeader;

        private int _disposed;        
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseModel"/> class, primarily for Google AI.
        /// It configures the model using an API key and environment variables.
        /// </summary>
        /// <param name="httpClientFactory">Optional. The <see cref="IHttpClientFactory"/> to use for creating HttpClient instances.</param>
        /// <param name="logger">Optional. Logger instance used for logging</param>
        /// <param name="requestOptions">Optional. Provides options for API requests.</param>
        public BaseModel(IHttpClientFactory? httpClientFactory = null,
            ILogger? logger = null,
            RequestOptions? requestOptions = null) : base(logger)
        {
            _httpClientFactory = httpClientFactory;
            _requestOptions = requestOptions;

            // Initialize the default headers in constructor
            var productHeaderValue = new ProductHeaderValue(
                name: Assembly.GetExecutingAssembly().GetName().Name ?? "Mscc.GenerativeAI",
                version: Assembly.GetExecutingAssembly().GetName().Version?.ToString());
            _defaultUserAgent = new ProductInfoHeaderValue(productHeaderValue);
            _defaultApiClientHeader = new KeyValuePair<string, string>(
                "x-goog-api-client",
                _defaultUserAgent.ToString());

            GenerativeAIExtensions.ReadDotEnv();
            ApiKey = Environment.GetEnvironmentVariable("GOOGLE_API_KEY") ??
                     Environment.GetEnvironmentVariable("GEMINI_API_KEY");
            AccessToken = Environment.GetEnvironmentVariable("GOOGLE_ACCESS_TOKEN"); // ?? GetAccessTokenFromAdc();
            Model = Environment.GetEnvironmentVariable("GOOGLE_AI_MODEL") ??
                    GenerativeAI.Types.Model.Gemini25Pro;
            _projectId = Environment.GetEnvironmentVariable("GOOGLE_PROJECT_ID") ??
                         Environment.GetEnvironmentVariable("GOOGLE_CLOUD_PROJECT");
            _region = Environment.GetEnvironmentVariable("GOOGLE_REGION") ??
                      Environment.GetEnvironmentVariable("GOOGLE_CLOUD_LOCATION") ?? _region;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseModel"/> class, primarily for Vertex AI.
        /// It configures the model using a project ID, region, and access token.
        /// </summary>
        /// <param name="projectId">The Google Cloud project ID.</param>
        /// <param name="region">The Google Cloud region.</param>
        /// <param name="model">The model name to use.</param>
        /// <param name="accessToken">Optional. The access token for authentication.</param>
        /// <param name="httpClientFactory">Optional. The IHttpClientFactory to use for creating HttpClient instances.</param>
        /// <param name="logger">Optional. Logger instance used for logging</param>
        /// <param name="requestOptions">Options for the request.</param>
        public BaseModel(string? projectId = null,
            string? region = null,
            string? model = null,
            string? accessToken = null,
            IHttpClientFactory? httpClientFactory = null,
            ILogger? logger = null,
            RequestOptions? requestOptions = null) : this(httpClientFactory, logger, requestOptions)
        {
            var credentialsFile =
                Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS") ??
                Environment.GetEnvironmentVariable("GOOGLE_WEB_CREDENTIALS") ??
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "gcloud",
                    "application_default_credentials.json");
            var credentials = GetCredentialsFromFile(credentialsFile);
            AccessToken = accessToken ??
                          Environment.GetEnvironmentVariable("GOOGLE_ACCESS_TOKEN") ??
                          _accessToken ??
                          GetAccessTokenFromAdc();
            ProjectId = projectId ??
                        credentials?.ProjectId ??
                        _projectId;
            _region = region ?? _region;
            Model = model ?? _model;
        }

        /// <summary>
        /// Internal constructor for testing purposes, allows injecting a custom HttpMessageHandler.
        /// </summary>
        internal BaseModel(HttpMessageHandler handler, ILogger? logger = null,
            RequestOptions? requestOptions = null) : base(logger)
        {
            _httpClient = new HttpClient(handler);
            _requestOptions = requestOptions;
            // Initialize the default headers in constructor
            var productHeaderValue = new ProductHeaderValue(
                name: Assembly.GetExecutingAssembly().GetName().Name ?? "Mscc.GenerativeAI",
                version: Assembly.GetExecutingAssembly().GetName().Version?.ToString());
            _defaultUserAgent = new ProductInfoHeaderValue(productHeaderValue);
            _defaultApiClientHeader = new KeyValuePair<string, string>(
                "x-goog-api-client",
                _defaultUserAgent.ToString());

            // Basic initialization, specific API key/model/project details would be set by subclass or test
            GenerativeAIExtensions.ReadDotEnv();
            Model = Environment.GetEnvironmentVariable("GOOGLE_AI_MODEL") ?? GenerativeAI.Types.Model.Gemini25Pro;
            ApiKey = Environment.GetEnvironmentVariable("GOOGLE_API_KEY") ??
                     Environment.GetEnvironmentVariable("GEMINI_API_KEY");
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
            if (string.Equals(_region, "global", StringComparison.OrdinalIgnoreCase))
            {
                replacements["BaseUrlVertexAi"] = BaseUrlVertexAiGlobal;
            }

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
                    { "modelsId", _model },
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
            var json = JsonSerializer.Serialize(request, WriteOptions);

            Logger.LogJsonRequest(json);

            return json;
        }

        /// <summary>
        /// Return deserialized object from JSON response.
        /// </summary>
        /// <typeparam name="T">Type to deserialize response into.</typeparam>
        /// <param name="response">Response from an API call in JSON format.</param>
        /// <returns>An instance of type T.</returns>
        protected async Task<T> Deserialize<T>(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();

            Logger.LogJsonResponse(json);

#if NET472_OR_GREATER || NETSTANDARD2_0
            return JsonSerializer.Deserialize<T>(json, ReadOptions);
#else
            return await response.Content.ReadFromJsonAsync<T>(ReadOptions);
#endif
        }

        /// <summary>
        /// Get default read options for JSON deserialization.
        /// </summary>
        /// <returns>default options for JSON deserialization.</returns>
        private static JsonSerializerOptions ReadJsonSerializerOptions()
        {
            var options = new JsonSerializerOptions(JsonSerializerDefaults.Web)
            {
#if DEBUG
                WriteIndented = true,
#endif
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                NumberHandling = JsonNumberHandling.AllowReadingFromString,
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true,
                UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip,
#if NET9_0_OR_GREATER
                RespectNullableAnnotations = true
#endif
            };
            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseUpper));
            options.Converters.Add(new DateTimeFormatJsonConverter());

            return options;
        }

        /// <summary>
        /// Get default write options for JSON serialization.
        /// </summary>
        /// <returns>default options for JSON serialization.</returns>
        private static JsonSerializerOptions WriteJsonSerializerOptions()
        {
            var options = new JsonSerializerOptions(JsonSerializerDefaults.Web)
            {
#if DEBUG
                WriteIndented = true,
#endif
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                NumberHandling = JsonNumberHandling.AllowReadingFromString,
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true,
                UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip,
#if NET9_0_OR_GREATER
                RespectNullableAnnotations = true
#endif
            };
            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseUpper));
            options.Converters.Add(new DateTimeFormatJsonConverter());

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
                var options = WriteJsonSerializerOptions();
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
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
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
                Logger.LogRunExternalExe("OS error while executing " + Format(filename, arguments) + ": " + e.Message);
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

                throw new Exception(Format(filename, arguments) + " finished with exit code = " + process.ExitCode +
                                    ": " + message);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="completionOption"></param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="GeminiApiTimeoutException">The HTTP response timed out.</exception>
        protected async Task<TResponse> PostAsync<TRequest, TResponse>(TRequest request,
            string url, string method,
            RequestOptions? requestOptions = null,
            HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead,
            CancellationToken cancellationToken = default)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var requestUri = ParseUrl(url, method);
            var json = Serialize(request);
            var payload = new StringContent(json, Encoding.UTF8, Constants.MediaType);
            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, requestUri);
            httpRequest.Content = payload;
            var response = await SendAsync(httpRequest, requestOptions, cancellationToken, completionOption);
            await response.EnsureSuccessAsync(cancellationToken);
            return await Deserialize<TResponse>(response);
        }

        /// <summary>
        /// Sends an HTTP request and returns the response.
        /// It handles adding authentication headers, user-agent, and custom headers.
        /// It also implements a retry mechanism for transient failures.
        /// </summary>
        /// <param name="request">The <see cref="HttpRequestMessage"/> to send.</param>
        /// <param name="requestOptions">Optional. Options for the request, including timeout and retry settings.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <param name="completionOption">Defines when the operation should complete.</param>
        /// <returns>The <see cref="HttpResponseMessage"/> from the API.</returns>
        protected async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default,
            HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead)
        {
            requestOptions ??= _requestOptions;

            var timeout = requestOptions?.Timeout ?? Timeout;
            request.SetTimeout(timeout);

            // Add auth headers specific to this request
            request.AddApiKeyHeader(_apiKey);
            request.AddAccessTokenHeader(_accessToken);
            request.AddProjectIdHeader(_projectId);

            // Add instance default headers
            request.Headers.UserAgent.Add(_defaultUserAgent);
            request.Headers.Add(_defaultApiClientHeader.Key, _defaultApiClientHeader.Value);

            // Add headers given in the request options
            request.AddRequestHeaders(requestOptions?.Headers);

            Logger.LogParsedRequest(
                request.Method,
                request.RequestUri,
                $"{request.Headers.ToFormattedString()}{(request.Content is null ? string.Empty : request.Content.Headers.ToFormattedString())}",
                request.Content is null ? string.Empty : await request.Content.ReadAsStringAsync()
            );

            var retry = requestOptions?.Retry ?? new Retry();
            var statusCodes = retry.StatusCodes ?? Constants.RetryStatusCodes;
            var delay = retry.Initial;
            var stopwatch = Stopwatch.StartNew();
            HttpResponseMessage lastResponse = null;

            for (var index = 1; index <= retry.Maximum; index++)
            {
                if (retry.Timeout.HasValue && stopwatch.Elapsed > retry.Timeout.Value)
                {
                    if (lastResponse != null)
                    {
                        var message = await GetResponseMessageAsync(lastResponse, cancellationToken);
                        throw new GeminiApiTimeoutException(
                            $"The request retry logic has timed out. Last API response:\n{message}", lastResponse,
                            new TimeoutException());
                    }

                    throw new GeminiApiTimeoutException("The request retry logic has timed out.",
                        new TimeoutException());
                }

                try
                {
	                using var requestMessage = await CloneHttpRequestMessageAsync(request, cancellationToken);
	                lastResponse = await Client.SendAsync(requestMessage, completionOption, cancellationToken);
	                if (!statusCodes.Contains((int)lastResponse.StatusCode))
	                {
		                return lastResponse;
	                }

	                var message = await GetResponseMessageAsync(lastResponse, cancellationToken);
	                Logger.LogRequestNotSuccessful(index, message);

	                if ((int)lastResponse.StatusCode == 429) // HttpStatusCode.TooManyRequests
	                {
		                var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(message, ReadOptions);
		                if (errorResponse.Error.TryGetDetail<RetryInfo>(out var retryInfo))
		                {
			                delay = (int)retryInfo.RetryDelay.TotalSeconds + 1;
		                }
	                }
                }
                catch (HttpRequestException e)
                {
	                Logger.LogRequestNotSuccessful(index);
	                if (index == retry.Maximum)
	                {
		                if (lastResponse != null)
		                {
			                var message = GetResponseMessageAsync(lastResponse, cancellationToken);
			                throw new GeminiApiException($"The request was not successful. {message}", lastResponse, e);
		                }

		                throw new GeminiApiException("The request was not successful.", e);
	                }
                }
                catch (Exception e)	// catch all, log and rethrow.
                {
	                Logger.LogRequestException(index, e.Message);
		            throw new GeminiApiException("The request was not successful.", e);
                }

                await Task.Delay(TimeSpan.FromSeconds(delay), cancellationToken);
                delay *= retry.Multiplies;
                if (delay > retry.Maximum)
                {
                    delay = retry.Maximum;
                }
            }

            return await lastResponse!.EnsureSuccessAsync(cancellationToken);
        }

        /// <summary>
        /// Asynchronously reads the content of an <see cref="HttpResponseMessage"/> as a string.
        /// </summary>
        /// <param name="response">The HTTP response.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        private static async Task<string> GetResponseMessageAsync(HttpResponseMessage response,
            CancellationToken cancellationToken)
        {
#if NET472_OR_GREATER || NETSTANDARD2_0
            return await response.Content.ReadAsStringAsync();
#else
            return await response.Content.ReadAsStringAsync(cancellationToken);
#endif
        }

        /// <summary>
        /// Creates a deep clone of an <see cref="HttpRequestMessage"/>.
        /// This is necessary for retrying requests, as a request message can only be sent once.
        /// </summary>
        /// <param name="request">The request to clone.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        private static async Task<HttpRequestMessage> CloneHttpRequestMessageAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var clonedRequest = new HttpRequestMessage(request.Method, request.RequestUri) { Version = request.Version, };

            if (request.Content != null)
            {
                clonedRequest.Content = request.Content switch
                {
                    StringContent stringContent => await stringContent.Clone(),
                    StreamContent streamContent => await streamContent.Clone(),
                    MultipartContent multipartContent => await multipartContent.Clone(),
                    _ => clonedRequest.Content
                };

                foreach (var header in request.Content.Headers)
                {
	                if (header.Key.Equals("Content-Type", StringComparison.OrdinalIgnoreCase)
	                    || header.Key.Equals("Content-Length", StringComparison.OrdinalIgnoreCase)
	                    || header.Key.Equals("Content-Disposition", StringComparison.OrdinalIgnoreCase))
	                {
		                continue;
	                }
	                clonedRequest.Content!.Headers.Add(header.Key, header.Value);
                }
            }
	        request.CopyHeadersPropertiesOptions(clonedRequest);

            return clonedRequest;
        }

        /// <summary>
        /// Disposes <see cref="BaseModel"/> and its underlying resources.
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (Interlocked.CompareExchange(ref _disposed, 1, 0) != 0)
            {
                return;
            }

            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
                Client?.Dispose();
            }
        }

        // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~BaseModel()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        /// <summary>
        /// Asynchronously disposes <see cref="BaseModel"/> and its underlying resources.
        /// </summary>
        /// <returns>A <see cref="ValueTask"/> that represents the asynchronous dispose operation.</returns>
        public virtual ValueTask DisposeAsync()
        {
            Dispose();
#if NET472_OR_GREATER || NETSTANDARD2_0
            return new ValueTask(Task.CompletedTask);
#else
            return ValueTask.CompletedTask;
#endif
        }
    }
}