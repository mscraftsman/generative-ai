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
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Text;

namespace Mscc.GenerativeAI
{
    public class GenerativeModel
    {
        private const string EndpointGoogleAi = "https://generativelanguage.googleapis.com";
        private const string UrlGoogleAi = "{endpointGoogleAI}/{version}/{model}:{method}";
        private const string UrlVertexAi = "https://{region}-aiplatform.googleapis.com/{version}/projects/{projectId}/locations/{region}/publishers/{publisher}/{model}:{method}";

        private readonly bool _useVertexAi;
        private readonly string _region = "us-central1";
        private readonly string _publisher = "google";
        private readonly JsonSerializerOptions _options;
        private readonly Credentials? _credentials;

        private string _model;
        private string? _apiKey;
        private string? _accessToken;
        private string? _projectId;
        private List<SafetySetting>? _safetySettings;
        private GenerationConfig? _generationConfig;
        private List<Tool>? _tools;
        private ToolConfig? _toolConfig;
        private Content? _systemInstruction;

#if NET472_OR_GREATER || NETSTANDARD2_0
        private static readonly Version _httpVersion = HttpVersion.Version11;
        private static readonly HttpClient Client = new HttpClient(new HttpClientHandler
        {
            SslProtocols = SslProtocols.Tls12,
        });
#else
        private static readonly Version _httpVersion = HttpVersion.Version11;
        private static readonly HttpClient Client = new HttpClient(new SocketsHttpHandler
        {
            PooledConnectionLifetime = TimeSpan.FromMinutes(30),
            EnableMultipleHttp2Connections = true
        })
        {
            DefaultRequestVersion = _httpVersion,
            DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrHigher
        };
#endif

        private string Url
        {
            get
            {
                var url = UrlGoogleAi;
                if (_useVertexAi)
                {
                    url = UrlVertexAi;
                }

                return url;
            }
        }

        private string Version
        {
            get
            {
                if (_useVertexAi)
                {
                    return ApiVersion.V1;
                }

                return ApiVersion.V1Beta;
            }
        }

        private string Method
        {
            get
            {
                var model = _model.SanitizeModelName().Split(new[] { '/' })[1];
                switch (model)
                {
                    case GenerativeAI.Model.BisonChat:
                        return GenerativeAI.Method.GenerateMessage;
                    case GenerativeAI.Model.BisonText:
                        return GenerativeAI.Method.GenerateText;
                    case GenerativeAI.Model.GeckoEmbedding:
                        return GenerativeAI.Method.EmbedText;
                    case GenerativeAI.Model.Embedding:
                        return GenerativeAI.Method.EmbedContent;
                    case GenerativeAI.Model.AttributedQuestionAnswering:
                        return GenerativeAI.Method.GenerateAnswer;
                }
                if (_useVertexAi)
                {
                    return GenerativeAI.Method.StreamGenerateContent;
                }

                return GenerativeAI.Method.GenerateContent;
            }
        }
        
        internal bool IsVertexAI => _useVertexAi;

        private string Model
        {
            set
            {
                if (value != null)
                {
                    _model = value.SanitizeModelName();
                }
            }
        }

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
        /// Returns the name of the model. 
        /// </summary>
        /// <returns>Name of the model.</returns>
        public string Name => _model;
        
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
        /// You can enable Server Sent Events (SSE) for gemini-1.0-pro
        /// </summary>
        /// <remarks>
        /// See <a href="https://developer.mozilla.org/en-US/docs/Web/API/Server-sent_events">Server-sent Events</a>
        /// </remarks>
        public bool UseServerSentEventsFormat { get; set; } = false;

        /// <summary>
        /// Activate JSON Mode (default = no)
        /// </summary>
        public bool UseJsonMode { get; set; } = false;

        /// <summary>
        /// Gets or sets the timespan to wait before the request times out.
        /// </summary>
        public TimeSpan Timeout
        {
            get => Client.Timeout;
            set => Client.Timeout = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenerativeModel"/> class.
        /// The default constructor attempts to read <c>.env</c> file and environment variables.
        /// Sets default values, if available.
        /// </summary>
        public GenerativeModel()
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
            
            var productHeaderValue = new ProductHeaderValue(name:  "Mscc.GenerativeAI", version: Assembly.GetExecutingAssembly().GetName().Version.ToString());
            var productInfoHeaderValue = new ProductInfoHeaderValue(productHeaderValue);
            Client.DefaultRequestHeaders.UserAgent.Add(productInfoHeaderValue);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenerativeModel"/> class with access to Google AI Gemini API.
        /// </summary>
        /// <param name="apiKey">API key provided by Google AI Studio</param>
        /// <param name="model">Model to use</param>
        /// <param name="generationConfig">Optional. Configuration options for model generation and outputs.</param>
        /// <param name="safetySettings">Optional. A list of unique SafetySetting instances for blocking unsafe content.</param>
        /// <param name="tools">Optional. A list of Tools the model may use to generate the next response.</param>
        /// <param name="systemInstruction">Optional. </param>
        /// <param name="toolConfig">Optional. Configuration of tools.</param>
        internal GenerativeModel(string? apiKey = null, 
            string? model = null, 
            GenerationConfig? generationConfig = null, 
            List<SafetySetting>? safetySettings = null,
            List<Tool>? tools = null,
            Content? systemInstruction = null,
            ToolConfig? toolConfig = null) : this()
        {
            ApiKey = apiKey ?? _apiKey;
            Model = model ?? _model;
            _generationConfig ??= generationConfig;
            _safetySettings ??= safetySettings;
            _tools = tools;
            _toolConfig = toolConfig;
            _systemInstruction = systemInstruction;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenerativeModel"/> class with access to Vertex AI Gemini API.
        /// </summary>
        /// <param name="projectId">Identifier of the Google Cloud project</param>
        /// <param name="region">Region to use</param>
        /// <param name="model">Model to use</param>
        /// <param name="generationConfig">Optional. Configuration options for model generation and outputs.</param>
        /// <param name="safetySettings">Optional. A list of unique SafetySetting instances for blocking unsafe content.</param>
        /// <param name="tools">Optional. A list of Tools the model may use to generate the next response.</param>
        /// <param name="systemInstruction">Optional. </param>
        /// <param name="toolConfig">Optional. Configuration of tools.</param>
        internal GenerativeModel(string? projectId = null, string? region = null, 
            string? model = null, 
            GenerationConfig? generationConfig = null, 
            List<SafetySetting>? safetySettings = null,
            List<Tool>? tools = null,
            Content? systemInstruction = null,
            ToolConfig? toolConfig = null) : this()
        {
            _useVertexAi = true;
            AccessToken = Environment.GetEnvironmentVariable("GOOGLE_ACCESS_TOKEN") ?? 
                          GetAccessTokenFromAdc();
            ProjectId = projectId ??
                Environment.GetEnvironmentVariable("GOOGLE_PROJECT_ID") ??
                _credentials?.ProjectId ?? 
                _projectId;
            _region = region ?? _region;
            Model = model ?? _model;
            _generationConfig = generationConfig;
            _safetySettings = safetySettings;
            _tools = tools;
            _toolConfig = toolConfig;
            _systemInstruction = systemInstruction;
        }

        #region Undecided location of methods.Maybe IGenerativeAI might be better...

        /// <summary>
        /// Lists models available through the API.
        /// </summary>
        /// <returns>List of available models.</returns>
        /// <param name="tuned">Flag, whether models or tuned models shall be returned.</param>
        /// <param name="pageSize">The maximum number of Models to return (per page).</param>
        /// <param name="pageToken">A page token, received from a previous models.list call. Provide the pageToken returned by one request as an argument to the next request to retrieve the next page.</param>
        /// <param name="filter">Optional. A filter is a full text search over the tuned model's description and display name. By default, results will not include tuned models shared with everyone. Additional operators: - owner:me - writers:me - readers:me - readers:everyone</param>
        /// <exception cref="NotSupportedException">Thrown when the functionality is not supported by the model.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        public async Task<List<ModelResponse>> ListModels(bool tuned = false, 
            int? pageSize = 50, 
            string? pageToken = null, 
            string? filter = null)
        {
            this.GuardSupported();
            
            if (tuned)
            {
                return await ListTunedModels(pageSize, pageToken, filter);
            }

            var url = "{endpointGoogleAI}/{Version}/models";
            var queryStringParams = new Dictionary<string, string?>()
            {
                [nameof(pageSize)] = Convert.ToString(pageSize), 
                [nameof(pageToken)] = pageToken
            };

            url = ParseUrl(url).AddQueryString(queryStringParams);
            var response = await Client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var models = await Deserialize<ListModelsResponse>(response);
            return models?.Models!;
        }

        /// <summary>
        /// Gets information about a specific Model.
        /// </summary>
        /// <param name="model">Required. The resource name of the model. This name should match a model name returned by the models.list method. Format: models/model-id or tunedModels/my-model-id</param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException">Thrown when the functionality is not supported by the model.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        public async Task<ModelResponse> GetModel(string? model = null)
        {
            this.GuardSupported();

            model ??= _model;
            model = model.SanitizeModelName();
            if (!string.IsNullOrEmpty(_apiKey) && model.StartsWith("tunedModel", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new NotSupportedException("Accessing tuned models via API key is not provided. Setup OAuth for your project.");
            }

            var url = $"{EndpointGoogleAi}/{Version}/{model}";
            url = ParseUrl(url);
            var response = await Client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await Deserialize<ModelResponse>(response);
        }

        /// <summary>
        /// Creates a tuned model.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotSupportedException">Thrown when the functionality is not supported by the model.</exception>
        public async Task<CreateTunedModelResponse> CreateTunedModel(CreateTunedModelRequest request)
        {
            if (!(_model.Equals($"{GenerativeAI.Model.BisonText001.SanitizeModelName()}", StringComparison.InvariantCultureIgnoreCase) ||
                _model.Equals($"{GenerativeAI.Model.Gemini10Pro001.SanitizeModelName()}", StringComparison.InvariantCultureIgnoreCase)))
            {
                throw new NotSupportedException();
            }

            if (!string.IsNullOrEmpty(_apiKey))
            {
                throw new NotSupportedException("Accessing tuned models via API key is not provided. Setup OAuth for your project.");
            }

            var method = GenerativeAI.Method.TunedModels;
            // var method = "createTunedModel";
            // if (_model is (string)Model.BisonText001)
            //     method = "createTunedTextModel";
            var url = "{endpointGoogleAI}/{Version}/{method}";   // v1beta3
            url = ParseUrl(url, method);
            string json = Serialize(request);
            var payload = new StringContent(json, Encoding.UTF8, Constants.MediaType);
            var response = await Client.PostAsync(url, payload);
            response.EnsureSuccessStatusCode();
            return await Deserialize<CreateTunedModelResponse>(response);
        }

        /// <summary>
        /// Deletes a tuned model.
        /// </summary>
        /// <param name="model">Required. The resource name of the model. Format: tunedModels/my-model-id</param>
        /// <returns>If successful, the response body is empty.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="model"/> is null or empty.</exception>
        /// <exception cref="NotSupportedException">Thrown when the functionality is not supported by the model.</exception>
        public async Task<string> DeleteTunedModel(string model)
        {
            if (string.IsNullOrEmpty(model)) throw new ArgumentNullException(nameof(model));
            this.GuardSupported();

            model = model.SanitizeModelName();
            if (!string.IsNullOrEmpty(_apiKey) && model.StartsWith("tunedModel", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new NotSupportedException("Accessing tuned models via API key is not provided. Setup OAuth for your project.");
            }

            var url = $"{EndpointGoogleAi}/{Version}/{model}";   // v1beta3
            url = ParseUrl(url);
            var response = await Client.DeleteAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Updates a tuned model.
        /// </summary>
        /// <param name="model">Required. The resource name of the model. Format: tunedModels/my-model-id</param>
        /// <param name="tunedModel">The tuned model to update.</param>
        /// <param name="updateMask">Required. The list of fields to update. This is a comma-separated list of fully qualified names of fields. Example: "user.displayName,photo".</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="model"/> is null or empty.</exception>
        /// <exception cref="NotSupportedException">Thrown when the functionality is not supported by the model.</exception>
        public async Task<ModelResponse> UpdateTunedModel(string model, ModelResponse tunedModel, string? updateMask = null)
        {
            if (string.IsNullOrEmpty(model)) throw new ArgumentNullException(nameof(model));
            this.GuardSupported();

            model = model.SanitizeModelName();
            if (!string.IsNullOrEmpty(_apiKey) && model.StartsWith("tunedModel", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new NotSupportedException("Accessing tuned models via API key is not provided. Setup OAuth for your project.");
            }

            var url = $"{EndpointGoogleAi}/{Version}/{model}";   // v1beta3
            var queryStringParams = new Dictionary<string, string?>()
            {
                [nameof(updateMask)] = updateMask
            };
            
            url = ParseUrl(url).AddQueryString(queryStringParams);
            string json = Serialize(tunedModel);
            var payload = new StringContent(json, Encoding.UTF8, Constants.MediaType);
#if NET472_OR_GREATER || NETSTANDARD2_0
            var message = new HttpRequestMessage
            {
                Method = new HttpMethod("PATCH"),
                Content = payload,
                RequestUri = new Uri(url),
                Version = _httpVersion
            };
            var response = await Client.SendAsync(message);
#else
            var response = await Client.PatchAsync(url, payload);
#endif
            response.EnsureSuccessStatusCode();
            return await Deserialize<ModelResponse>(response);
        }

        /// <summary>
        /// Transfers ownership of the tuned model. This is the only way to change ownership of the tuned model. The current owner will be downgraded to writer role.
        /// </summary>
        /// <param name="model">Required. The resource name of the tuned model to transfer ownership. Format: tunedModels/my-model-id</param>
        /// <param name="emailAddress">Required. The email address of the user to whom the tuned model is being transferred to.</param>
        /// <returns>If successful, the response body is empty.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="model"/> or <paramref name="emailAddress"/> is null or empty.</exception>
        /// <exception cref="NotSupportedException">Thrown when the functionality is not supported by the model.</exception>
        public async Task<string> TransferOwnership(string model, string emailAddress)
        {
            if (string.IsNullOrEmpty(model)) throw new ArgumentNullException(nameof(model));
            if (string.IsNullOrEmpty(emailAddress)) throw new ArgumentNullException(nameof(emailAddress));
            this.GuardSupported();

            model = model.SanitizeModelName();
            if (!string.IsNullOrEmpty(_apiKey) && model.StartsWith("tunedModel", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new NotSupportedException("Accessing tuned models via API key is not provided. Setup OAuth for your project.");
            }

            var method = GenerativeAI.Method.TransferOwnership;
            var url = ParseUrl(Url, method);
            string json = Serialize(new { EmailAddress = emailAddress });   // TransferOwnershipRequest
            var payload = new StringContent(json, Encoding.UTF8, Constants.MediaType);
            var response = await Client.PostAsync(url, payload);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Uploads a file to the File API backend.
        /// </summary>
        /// <param name="uri">URI or path to the file to upload.</param>
        /// <param name="displayName">A name displazed for the uploaded file.</param>
        /// <param name="resumable">Flag indicating whether to use resumable upload.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the upload.</param>
        /// <returns>A URI of the uploaded file.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="uri"/> is null or empty.</exception>
        /// <exception cref="FileNotFoundException">Thrown when the file <paramref name="uri"/> is not found.</exception>
        /// <exception cref="MaxUploadFileSizeException">Thrown when the file size exceeds the maximum allowed size.</exception>
        /// <exception cref="UploadFileException">Thrown when the file upload fails.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        public async Task<UploadMediaResponse> UploadFile(string uri,
            string? displayName = null,
            bool resumable = false,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            if (uri == null) throw new ArgumentNullException(nameof(uri));
            if (!File.Exists(uri)) throw new FileNotFoundException(nameof(uri));
            var fileInfo = new FileInfo(uri);
            if (fileInfo.Length > Constants.MaxUploadFileSize) throw new MaxUploadFileSizeException(nameof(uri));

            var mimeType = GenerativeAIExtensions.GetMimeType(uri);
            var totalBytes = new FileInfo(uri).Length;
            var request = new UploadMediaRequest()
            {
                File = new FileRequest()
                {
                    DisplayName = displayName ?? Path.GetFileNameWithoutExtension(uri),
                }
            };

            var url = $"{EndpointGoogleAi}/upload/{Version}/files";   // v1beta3 // ?key={apiKey}
            if (resumable)
            { 
                url = $"{EndpointGoogleAi}/resumable/upload/{Version}/files";   // v1beta3 // ?key={apiKey}
            }
            url = ParseUrl(url).AddQueryString(new Dictionary<string, string?>()
            {
                ["alt"] = "json", 
                ["uploadType"] = "multipart"
            });
            string json = Serialize(request);
            var multipartContent = new MultipartContent("related");
            multipartContent.Add(new StringContent(json, Encoding.UTF8, Constants.MediaType));
            multipartContent.Add(new StreamContent(new FileStream(uri, FileMode.Open), (int)Constants.ChunkSize)
            {
                Headers = { 
                    ContentType = new MediaTypeHeaderValue(mimeType), 
                    ContentLength = totalBytes 
                }
            });

            var response = await Client.PostAsync(url, multipartContent, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await Deserialize<UploadMediaResponse>(response);
        }

        /// <summary>
        /// Lists the metadata for Files owned by the requesting project.
        /// </summary>
        /// <param name="pageSize">The maximum number of Models to return (per page).</param>
        /// <param name="pageToken">A page token, received from a previous files.list call. Provide the pageToken returned by one request as an argument to the next request to retrieve the next page.</param>
        /// <returns>List of files in File API.</returns>
        /// <exception cref="NotSupportedException">Thrown when the functionality is not supported by the model.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        public async Task<ListFilesResponse> ListFiles(int? pageSize = 100, 
            string? pageToken = null)
        {
            this.GuardSupported();
            
            var url = "{endpointGoogleAI}/{Version}/files";
            var queryStringParams = new Dictionary<string, string?>()
            {
                [nameof(pageSize)] = Convert.ToString(pageSize), 
                [nameof(pageToken)] = pageToken
            };

            url = ParseUrl(url).AddQueryString(queryStringParams);
            var response = await Client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await Deserialize<ListFilesResponse>(response);
        }

        /// <summary>
        /// Gets the metadata for the given File.
        /// </summary>
        /// <param name="file">Required. The resource name of the file to get. This name should match a file name returned by the files.list method. Format: files/file-id.</param>
        /// <returns>Metadata for the given file.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="file"/> is null or empty.</exception>
        /// <exception cref="NotSupportedException">Thrown when the functionality is not supported by the model.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        public async Task<FileResource> GetFile(string file)
        {
            if (string.IsNullOrEmpty(file)) throw new ArgumentNullException(nameof(file));
            this.GuardSupported();

            file = file.SanitizeFileName();

            var url = $"{EndpointGoogleAi}/{Version}/{file}";
            url = ParseUrl(url);
            var response = await Client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await Deserialize<FileResource>(response);
        }

        /// <summary>
        /// Deletes a file.
        /// </summary>
        /// <param name="file">Required. The resource name of the file to get. This name should match a file name returned by the files.list method. Format: files/file-id.</param>
        /// <returns>If successful, the response body is empty.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="file"/> is null or empty.</exception>
        /// <exception cref="NotSupportedException">Thrown when the functionality is not supported by the model.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        public async Task<string> DeleteFile(string file)
        {
            if (string.IsNullOrEmpty(file)) throw new ArgumentNullException(nameof(file));
            this.GuardSupported();

            file = file.SanitizeFileName();

            var url = $"{EndpointGoogleAi}/{Version}/{file}";   // v1beta3
            url = ParseUrl(url);
            var response = await Client.DeleteAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        
        #endregion

        /// <summary>
        /// Generates a response from the model given an input <see cref="GenerateContentRequest"/>.
        /// </summary>
        /// <param name="request">Required. The request to send to the API.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <returns>Response from the model for generated content.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="request"/> is <see langword="null"/>.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        public async Task<GenerateContentResponse> GenerateContent(GenerateContentRequest? request,
            RequestOptions? requestOptions = null)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            request.GenerationConfig ??= _generationConfig;
            request.SafetySettings ??= _safetySettings;
            request.Tools ??= _tools;
            request.ToolConfig ??= _toolConfig;
            request.SystemInstruction ??= _systemInstruction;
            
            var url = ParseUrl(Url, Method);
            if (UseJsonMode)
            {
                request.GenerationConfig ??= new GenerationConfig();
                request.GenerationConfig.ResponseMimeType = Constants.MediaType;
            }
            string json = Serialize(request);
            var payload = new StringContent(json, Encoding.UTF8, Constants.MediaType);

            if (requestOptions != null)
            {
                Client.Timeout = requestOptions.Timeout;
            }
            
            var response = await Client.PostAsync(url, payload);
            // ToDo: Handle payload exception like this
            // except google.api_core.exceptions.InvalidArgument as e:
            // if e.message.startswith("Request payload size exceeds the limit:"):
            // e.message += (
            //     " Please upload your files with the File API instead."
            // "`f = genai.upload_file(path); m.generate_content(['tell me about this file:', f])`"
            //     )
            response.EnsureSuccessStatusCode();

            if (_useVertexAi)
            {
                var fullText = new StringBuilder();
                GroundingMetadata groundingMetadata = null;
                var contents = await Deserialize<List<GenerateContentResponse>>(response);
                foreach (var content in contents)
                {
                    if (!(content.Candidates?[0].GroundingMetadata is null))
                    {
                        groundingMetadata = content.Candidates[0].GroundingMetadata;
                        continue;
                    }
                    
                    switch (content.Candidates?[0].FinishReason)
                    {
                        case FinishReason.Safety:
                            return content;
                            break;
                        case FinishReason.FinishReasonUnspecified:
                        default:
                            fullText.Append(content.Text);
                            break;
                    }
                }
                var result = contents.LastOrDefault();
                if (result.Candidates is null)
                {
                    result.Candidates = new()
                    {
                        new()
                        {
                            Content = new()
                            {
                                Parts = new()
                                {
                                    new()
                                }
                            }
                        }
                    };
                }
                result.Candidates[0].GroundingMetadata = groundingMetadata;
                result.Candidates[0].Content.Parts[0].Text = fullText.ToString();
                return result;
            }
            return await Deserialize<GenerateContentResponse>(response);
        }

        /// <summary>
        /// Generates a response from the model given an input prompt and other parameters.
        /// </summary>
        /// <param name="prompt">Required. String to process.</param>
        /// <param name="generationConfig">Optional. Configuration options for model generation and outputs.</param>
        /// <param name="safetySettings">Optional. A list of unique SafetySetting instances for blocking unsafe content.</param>
        /// <param name="tools">Optional. A list of Tools the model may use to generate the next response.</param>
        /// <param name="toolConfig">Optional. Configuration of tools.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <returns>Response from the model for generated content.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="prompt"/> is <see langword="null"/>.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        public async Task<GenerateContentResponse> GenerateContent(string? prompt,
            GenerationConfig? generationConfig = null,
            List<SafetySetting>? safetySettings = null,
            List<Tool>? tools = null,
            ToolConfig? toolConfig = null,
            RequestOptions? requestOptions = null)
        {
            if (prompt == null) throw new ArgumentNullException(nameof(prompt));

            var request = new GenerateContentRequest(prompt, 
                generationConfig ?? _generationConfig, 
                safetySettings ?? _safetySettings, 
                tools ?? _tools,
                toolConfig: toolConfig ?? _toolConfig);
            return await GenerateContent(request, requestOptions);
        }

        /// <remarks/>
        public async Task<GenerateContentResponse> GenerateContent(List<IPart>? parts,
            GenerationConfig? generationConfig = null,
            List<SafetySetting>? safetySettings = null,
            List<Tool>? tools = null,
            ToolConfig? toolConfig = null,
            RequestOptions? requestOptions = null)
        {
            if (parts == null) throw new ArgumentNullException(nameof(parts));

            var request = new GenerateContentRequest(parts, 
                generationConfig ?? _generationConfig, 
                safetySettings ?? _safetySettings, 
                tools ?? _tools,
                toolConfig: toolConfig ?? _toolConfig);
            request.Contents[0].Role = Role.User;
            return await GenerateContent(request, requestOptions);
        }

        /// <summary>
        /// Generates a streamed response from the model given an input GenerateContentRequest.
        /// This method uses a MemoryStream and StreamContent to send a streaming request to the API.
        /// It runs asynchronously sending and receiving chunks to and from the API endpoint, which allows non-blocking code execution.
        /// </summary>
        /// <param name="request">The request to send to the API.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Stream of GenerateContentResponse with chunks asynchronously.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="request"/> is <see langword="null"/>.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        public async IAsyncEnumerable<GenerateContentResponse> GenerateContentStream(GenerateContentRequest? request,
            RequestOptions? requestOptions = null, 
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            if (UseServerSentEventsFormat)
            {
                await foreach (var item in GenerateContentStreamSSE(request, requestOptions, cancellationToken))
                {
                    if (cancellationToken.IsCancellationRequested)
                        yield break;
                    yield return item;
                }
                yield break;
            }

            if (request == null) throw new ArgumentNullException(nameof(request));

            request.GenerationConfig ??= _generationConfig;
            request.SafetySettings ??= _safetySettings;
            request.Tools ??= _tools;
            request.ToolConfig ??= _toolConfig;
            request.SystemInstruction ??= _systemInstruction;

            var method = "streamGenerateContent";
            var url = ParseUrl(Url, method);
            if (UseJsonMode)
            {
                request.GenerationConfig ??= new GenerationConfig();
                request.GenerationConfig.ResponseMimeType = Constants.MediaType;
            }

            // Ref: https://code-maze.com/using-streams-with-httpclient-to-improve-performance-and-memory-usage/
            // Ref: https://www.stevejgordon.co.uk/using-httpcompletionoption-responseheadersread-to-improve-httpclient-performance-dotnet
            var ms = new MemoryStream();
            await JsonSerializer.SerializeAsync(ms, request, _options, cancellationToken);
            ms.Seek(0, SeekOrigin.Begin);
            
            var message = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(url),
                Version = _httpVersion
            };
            message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.MediaType));

            using (var payload = new StreamContent(ms))
            {
                message.Content = payload;
                payload.Headers.ContentType = new MediaTypeHeaderValue(Constants.MediaType);

                using (var response = await Client.SendAsync(message, HttpCompletionOption.ResponseHeadersRead, cancellationToken))
                {
                    response.EnsureSuccessStatusCode();
                    if (response.Content is not null)
                    {
                        using var stream = await response.Content.ReadAsStreamAsync();
                        // Ref: https://github.com/dotnet/runtime/issues/97128 - HttpIOException
                        // https://github.com/grpc/grpc-dotnet/issues/2361#issuecomment-1895805167 
                        await foreach (var item in JsonSerializer.DeserializeAsyncEnumerable<GenerateContentResponse>(
                                           stream, _options, cancellationToken))
                        {
                            if (cancellationToken.IsCancellationRequested)
                                yield break;
                            yield return item;
                        }
                    }
                }
            }
        }

        /// <remarks/>
        public IAsyncEnumerable<GenerateContentResponse> GenerateContentStream(string? prompt,
            GenerationConfig? generationConfig = null,
            List<SafetySetting>? safetySettings = null,
            List<Tool>? tools = null,
            ToolConfig? toolConfig = null,
            RequestOptions? requestOptions = null)
        {
            if (prompt == null) throw new ArgumentNullException(nameof(prompt));

            var request = new GenerateContentRequest(prompt, 
                generationConfig ?? _generationConfig, 
                safetySettings ?? _safetySettings, 
                tools ?? _tools, 
                toolConfig: toolConfig ?? _toolConfig);
            return GenerateContentStream(request, requestOptions);
        }

        /// <remarks/>
        public IAsyncEnumerable<GenerateContentResponse> GenerateContentStream(List<IPart>? parts,
            GenerationConfig? generationConfig = null,
            List<SafetySetting>? safetySettings = null,
            List<Tool>? tools = null,
            ToolConfig? toolConfig = null,
            RequestOptions? requestOptions = null)
        {
            if (parts == null) throw new ArgumentNullException(nameof(parts));

            var request = new GenerateContentRequest(parts, 
                generationConfig ?? _generationConfig, 
                safetySettings ?? _safetySettings, 
                tools ?? _tools,
                toolConfig: toolConfig ?? _toolConfig);
            request.Contents[0].Role = Role.User;
            return GenerateContentStream(request, requestOptions);
        }

        /// <summary>
        /// Generates a response from the model given an input GenerateContentRequest.
        /// </summary>
        /// <param name="request">Required. The request to send to the API.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Response from the model for generated content.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="request"/> is <see langword="null"/>.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        internal async IAsyncEnumerable<GenerateContentResponse> GenerateContentStreamSSE(GenerateContentRequest? request, 
            RequestOptions? requestOptions = null,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            request.GenerationConfig ??= _generationConfig;
            request.SafetySettings ??= _safetySettings;
            request.Tools ??= _tools;
            
            var method = "streamGenerateContent";
            var url = ParseUrl(Url, method).AddQueryString(new Dictionary<string, string?>() { ["alt"] = "sse" });
            string json = Serialize(request);
            var payload = new StringContent(json, Encoding.UTF8, Constants.MediaType);
            var message = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                Content = payload,
                RequestUri = new Uri(url),
                Version = _httpVersion
            };
            // message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/event-stream"));
            message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.MediaType));

            using (var response = await Client.SendAsync(message, HttpCompletionOption.ResponseHeadersRead, cancellationToken))
            {
                response.EnsureSuccessStatusCode();
                if (response.Content is not null)
                {
                    using (var sr = new StreamReader(await response.Content.ReadAsStreamAsync()))
                    {
                        while (!sr.EndOfStream)
                        {
                            var data = await sr.ReadLineAsync();
                            if (string.IsNullOrWhiteSpace(data)) 
                                continue;
                            
                            var item = JsonSerializer.Deserialize<GenerateContentResponse>(
                                data.Substring("data:".Length).Trim(), _options);
                            if (cancellationToken.IsCancellationRequested)
                                yield break;
                            yield return item;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Generates a grounded answer from the model given an input GenerateAnswerRequest.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Response from the model for a grounded answer.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="request"/> is <see langword="null"/>.</exception>
        /// <exception cref="NotSupportedException"></exception>
        public async Task<GenerateAnswerResponse> GenerateAnswer(GenerateAnswerRequest? request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (!_model.Equals($"{GenerativeAI.Model.AttributedQuestionAnswering.SanitizeModelName()}", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new NotSupportedException();
            }

            var url = ParseUrl(Url, Method);
            string json = Serialize(request);
            var payload = new StringContent(json, Encoding.UTF8, Constants.MediaType);
            var response = await Client.PostAsync(url, payload);
            response.EnsureSuccessStatusCode();

            // if (_useVertexAi)
            // {
            //     var fullText = new StringBuilder();
            //     var contentResponseVertex = await Deserialize<List<GenerateAnswerResponse>>(response);
            //     foreach (var item in contentResponseVertex)
            //     {
            //         switch (item.Candidates[0].FinishReason)
            //         {
            //             case FinishReason.Safety:
            //                 return item;
            //                 break;
            //             case FinishReason.Unspecified:
            //             default:
            //                 fullText.Append(item.Text);
            //                 break;
            //         }
            //     }
            //     var result = contentResponseVertex.LastOrDefault();
            //     result.Candidates[0].Content.Parts[0].Text = fullText.ToString();
            //     return result;
            // }
            return await Deserialize<GenerateAnswerResponse>(response);
        }

        /// <remarks/>
        public async Task<GenerateAnswerResponse> GenerateAnswer(string? prompt,
            AnswerStyle? answerStyle = null,
            List<SafetySetting>? safetySettings = null)
        {
            if (prompt == null) throw new ArgumentNullException(nameof(prompt));

            var request = new GenerateAnswerRequest(prompt, 
                answerStyle,
                safetySettings ?? _safetySettings);
            return await GenerateAnswer(request);
        }

        /// <summary>
        /// Generates an embedding from the model given an input Content.
        /// </summary>
        /// <param name="request">Required. EmbedContentRequest to process. The content to embed. Only the parts.text fields will be counted.</param>
        /// <param name="model">Optional. The model used to generate embeddings. Defaults to models/embedding-001.</param>
        /// <param name="taskType">Optional. Optional task type for which the embeddings will be used. Can only be set for models/embedding-001.</param>
        /// <param name="title">Optional. An optional title for the text. Only applicable when TaskType is RETRIEVAL_DOCUMENT. Note: Specifying a title for RETRIEVAL_DOCUMENT provides better quality embeddings for retrieval.</param>
        /// <returns>List containing the embedding (list of float values) for the input content.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="request"/> is <see langword="null"/>.</exception>
        /// <exception cref="NotSupportedException">Thrown when the functionality is not supported by the model.</exception>
        public async Task<EmbedContentResponse> EmbedContent(EmbedContentRequest request,
            string? model = null,
            TaskType? taskType = null,
            string? title = null)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (request.Content == null) throw new ArgumentNullException(nameof(request.Content));
            if (string.IsNullOrEmpty(request.Model))
            {
                request.Model = model ?? _model;
            }
            request.TaskType ??= taskType;
            request.Title ??= title;

            string[] allowedModels =
            [
                GenerativeAI.Model.Embedding.SanitizeModelName(), 
                GenerativeAI.Model.TextEmbedding.SanitizeModelName()
            ];
            if (!allowedModels.Contains(request.Model.SanitizeModelName())) throw new NotSupportedException();
            if (!string.IsNullOrEmpty(request.Title) && request.TaskType != TaskType.RetrievalDocument) throw new NotSupportedException("If a title is specified, the task must be a retrieval document type task.");
            
            var url = ParseUrl(Url, Method);
            string json = Serialize(request);
            var payload = new StringContent(json, Encoding.UTF8, Constants.MediaType);
            var response = await Client.PostAsync(url, payload);
            response.EnsureSuccessStatusCode();
            return await Deserialize<EmbedContentResponse>(response);
        }

        /// <summary>
        /// Generates multiple embeddings from the model given input text in a synchronous call.
        /// </summary>
        /// <param name="requests">Required. Embed requests for the batch. The model in each of these requests must match the model specified BatchEmbedContentsRequest.model.</param>
        /// <param name="model">Optional. The model used to generate embeddings. Defaults to models/embedding-001.</param>
        /// <param name="taskType">Optional. Optional task type for which the embeddings will be used. Can only be set for models/embedding-001.</param>
        /// <param name="title">Optional. An optional title for the text. Only applicable when TaskType is RETRIEVAL_DOCUMENT. Note: Specifying a title for RETRIEVAL_DOCUMENT provides better quality embeddings for retrieval.</param>
        /// <returns>List containing the embedding (list of float values) for the input content.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="requests"/> is <see langword="null"/>.</exception>
        /// <exception cref="NotSupportedException"></exception>
        public async Task<EmbedContentResponse> EmbedContent(List<EmbedContentRequest> requests,
            string? model = null,
            TaskType? taskType = null,
            string? title = null)
        {
            if (requests == null) throw new ArgumentNullException(nameof(requests));
            string[] allowedModels =
            [
                GenerativeAI.Model.Embedding.SanitizeModelName(), 
                GenerativeAI.Model.TextEmbedding.SanitizeModelName()
            ];
            if (!allowedModels.Contains(_model.SanitizeModelName())) throw new NotSupportedException();
            if (!string.IsNullOrEmpty(title) && taskType != TaskType.RetrievalDocument) throw new NotSupportedException("If a title is specified, the task must be a retrieval document type task.");

            var method = GenerativeAI.Method.BatchEmbedContent;
            var url = ParseUrl(Url, method);
            string json = Serialize(requests);
            var payload = new StringContent(json, Encoding.UTF8, Constants.MediaType);
            var response = await Client.PostAsync(url, payload);
            return await Deserialize<EmbedContentResponse>(response);
        }

        /// <summary>
        /// Generates an embedding from the model given an input Content.
        /// </summary>
        /// <param name="content">Required. String to process. The content to embed. Only the parts.text fields will be counted.</param>
        /// <param name="model">Optional. The model used to generate embeddings. Defaults to models/embedding-001.</param>
        /// <param name="taskType">Optional. Optional task type for which the embeddings will be used. Can only be set for models/embedding-001.</param>
        /// <param name="title">Optional. An optional title for the text. Only applicable when TaskType is RETRIEVAL_DOCUMENT. Note: Specifying a title for RETRIEVAL_DOCUMENT provides better quality embeddings for retrieval.</param>
        /// <returns>List containing the embedding (list of float values) for the input content.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="content"/> is <see langword="null"/>.</exception>
        /// <exception cref="NotSupportedException">Thrown when the functionality is not supported by the model.</exception>
        public async Task<EmbedContentResponse> EmbedContent(string content, string? model = null, TaskType? taskType = null, string? title = null)
        {
            if (content == null) throw new ArgumentNullException(nameof(content));

            var request = new EmbedContentRequest(content)
            {
                Model = model,
                TaskType = taskType,
                Title = title
            };
            return await EmbedContent(request);
        }

        // Todo: Capture Python SDK for JSON structures.
        /// <summary>
        /// Generates an embedding from the model given an input Content.
        /// </summary>
        /// <param name="content">Required. List of strings to process. The content to embed. Only the parts.text fields will be counted.</param>
        /// <param name="model">Optional. The model used to generate embeddings. Defaults to models/embedding-001.</param>
        /// <param name="taskType">Optional. Optional task type for which the embeddings will be used. Can only be set for models/embedding-001.</param>
        /// <param name="title">Optional. An optional title for the text. Only applicable when TaskType is RETRIEVAL_DOCUMENT. Note: Specifying a title for RETRIEVAL_DOCUMENT provides better quality embeddings for retrieval.</param>
        /// <returns>List containing the embedding (list of float values) for the input content.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="content"/> is <see langword="null"/>.</exception>
        /// <exception cref="NotSupportedException">Thrown when the functionality is not supported by the model.</exception>
        public async Task<EmbedContentResponse> EmbedContent(IEnumerable<string> content, string? model = null, TaskType? taskType = null, string? title = null)
        {
            if (content == null) throw new ArgumentNullException(nameof(content));

            // var requests = new List<EmbedContentRequest>();
            // foreach (string prompt in content)
            // {
            //     if (string.IsNullOrEmpty(prompt)) continue;
            //     var request = new EmbedContentRequest()
            //     {
            //         Model = model?.SanitizeModelName(),
            //         Content = new(prompt),
            //         TaskType = taskType,
            //         Title = title
            //     };
            //     requests.Add(request);
            // }
            // return await EmbedContent(requests);
            var request = new EmbedContentRequest()
            {
                Model = model?.SanitizeModelName(),
                Content = new(),
                TaskType = taskType,
                Title = title
            };
            foreach (string prompt in content)
            {
                if (string.IsNullOrEmpty(prompt)) continue;
                request.Content.Parts.Add(new() { Text = prompt });
            }
            return await EmbedContent(request);
        }

        /// <summary>
        /// Generates multiple embeddings from the model given input text in a synchronous call.
        /// </summary>
        /// <param name="content">Content to embed.</param>
        /// <param name="model">Optional. The model used to generate embeddings. Defaults to models/embedding-001.</param>
        /// <param name="taskType">Optional. Optional task type for which the embeddings will be used. Can only be set for models/embedding-001.</param>
        /// <param name="title">Optional. An optional title for the text. Only applicable when TaskType is RETRIEVAL_DOCUMENT. Note: Specifying a title for RETRIEVAL_DOCUMENT provides better quality embeddings for retrieval.</param>
        /// <returns>List containing the embedding (list of float values) for the input content.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="content"/> is <see langword="null"/>.</exception>
        /// <exception cref="NotSupportedException">Thrown when the functionality is not supported by the model.</exception>
        public async Task<EmbedContentResponse> EmbedContent(ContentResponse content, string? model = null, TaskType? taskType = null, string? title = null)
        {
            if (content == null) throw new ArgumentNullException(nameof(content));

            var request = new EmbedContentRequest()
            {
                Model = model,
                Content = content,
                TaskType = taskType,
                Title = title
            };
            return await EmbedContent(request);
        }

        /// <summary>
        /// Counts the number of tokens in the content. 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <returns>Number of tokens.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="request"/> is <see langword="null"/>.</exception>
        public async Task<CountTokensResponse> CountTokens(GenerateContentRequest? request,
            RequestOptions? requestOptions = null)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var method = GenerativeAI.Method.CountTokens;
            var url = ParseUrl(Url, method);
            string json = Serialize(request);
            var payload = new StringContent(json, Encoding.UTF8, Constants.MediaType);
            
            if (requestOptions != null)
            {
                Client.Timeout = requestOptions.Timeout;
            }
            
            var response = await Client.PostAsync(url, payload);
            response.EnsureSuccessStatusCode();
            return await Deserialize<CountTokensResponse>(response);
        }

        /// <remarks/>
        public async Task<CountTokensResponse> CountTokens(string? prompt,
            RequestOptions? requestOptions = null)
        {
            if (prompt == null) throw new ArgumentNullException(nameof(prompt));
            
            var model = _model.SanitizeModelName().Split(new[] { '/' })[1];
            switch (model)
            {
                case GenerativeAI.Model.BisonChat:
                    var chatRequest = new GenerateMessageRequest(prompt);
                    return await CountTokens(chatRequest, requestOptions);
                case GenerativeAI.Model.BisonText:
                    var textRequest = new GenerateTextRequest(prompt);
                    return await CountTokens(textRequest, requestOptions);
                case GenerativeAI.Model.GeckoEmbedding:
                    var embeddingRequest = new GenerateTextRequest(prompt);
                    return await CountTokens(embeddingRequest, requestOptions);
                default:
                    var request = new GenerateContentRequest(prompt, _generationConfig, _safetySettings, _tools);
                    return await CountTokens(request, requestOptions);
            }
        }

        /// <remarks/>
        public async Task<CountTokensResponse> CountTokens(List<IPart>? parts)
        {
            if (parts == null) throw new ArgumentNullException(nameof(parts));

            var request = new GenerateContentRequest(parts, _generationConfig, _safetySettings, _tools);
            return await CountTokens(request);
        }

        public async Task<CountTokensResponse> CountTokens(FileResource file)
        {
            if (file == null) throw new ArgumentNullException(nameof(file));

            var request = new GenerateContentRequest(file, _generationConfig, _safetySettings, _tools);
            return await CountTokens(request);
        }

        // Todo: Implementation missing
        /// <summary>
        /// Starts a chat session. 
        /// </summary>
        /// <param name="history">Optional. A collection of <see cref="ContentResponse"/> objects, or equivalents to initialize the session.</param>
        /// <param name="generationConfig">Optional. Configuration options for model generation and outputs.</param>
        /// <param name="safetySettings">Optional. A list of unique SafetySetting instances for blocking unsafe content.</param>
        /// <param name="tools">Optional. A list of Tools the model may use to generate the next response.</param>
        /// <param name="enableAutomaticFunctionCalling"></param>
        /// <returns>Returns a <see cref="ChatSession"/> attached to this model.</returns>
        public ChatSession StartChat(List<ContentResponse>? history = null, 
            GenerationConfig? generationConfig = null,
            List<SafetySetting>? safetySettings = null, 
            List<Tool>? tools = null,
            bool enableAutomaticFunctionCalling = false)
        {
            var config = generationConfig ?? _generationConfig;
            var safety = safetySettings ?? _safetySettings;
            var tool = tools ?? _tools;
            return new ChatSession(this, history, config, safety, tool, enableAutomaticFunctionCalling);
        }
        
        #region "PaLM 2" methods

        /// <summary>
        /// Generates a response from the model given an input message.
        /// </summary>
        /// <param name="request">The request to send to the API.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="request"/> is <see langword="null"/>.</exception>
        /// <exception cref="NotSupportedException"></exception>
        public async Task<GenerateTextResponse> GenerateText(GenerateTextRequest? request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (!_model.Equals($"{GenerativeAI.Model.BisonText.SanitizeModelName()}", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new NotSupportedException();
            }

            var url = ParseUrl(Url, Method);
            string json = Serialize(request);
            var payload = new StringContent(json, Encoding.UTF8, Constants.MediaType);
            var response = await Client.PostAsync(url, payload);
            response.EnsureSuccessStatusCode();
            return await Deserialize<GenerateTextResponse>(response);
        }

        /// <remarks/>
        public async Task<GenerateTextResponse> GenerateText(string prompt)
        {
            if (prompt == null) throw new ArgumentNullException(nameof(prompt));

            var request = new GenerateTextRequest(prompt);
            return await GenerateText(request);
        }
        
        /// <summary>
        /// Counts the number of tokens in the content. 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <returns>Number of tokens.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="request"/> is <see langword="null"/>.</exception>
        public async Task<CountTokensResponse> CountTokens(GenerateTextRequest? request,
            RequestOptions? requestOptions = null)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var method = GenerativeAI.Method.CountTextTokens;
            var url = ParseUrl(Url, method);
            string json = Serialize(request);
            var payload = new StringContent(json, Encoding.UTF8, Constants.MediaType);
            
            if (requestOptions != null)
            {
                Client.Timeout = requestOptions.Timeout;
            }
            
            var response = await Client.PostAsync(url, payload);
            response.EnsureSuccessStatusCode();
            return await Deserialize<CountTokensResponse>(response);
        }

        /// <summary>
        /// Generates a response from the model given an input prompt.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="request"/> is <see langword="null"/>.</exception>
        public async Task<GenerateMessageResponse> GenerateMessage(GenerateMessageRequest? request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (!_model.Equals($"{GenerativeAI.Model.BisonChat.SanitizeModelName()}", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new NotSupportedException();
            }

            var url = ParseUrl(Url, Method);
            string json = Serialize(request);
            var payload = new StringContent(json, Encoding.UTF8, Constants.MediaType);
            var response = await Client.PostAsync(url, payload);
            response.EnsureSuccessStatusCode();
            return await Deserialize<GenerateMessageResponse>(response);
        }

        /// <remarks/>
        public async Task<GenerateMessageResponse> GenerateMessage(string prompt)
        {
            if (prompt == null) throw new ArgumentNullException(nameof(prompt));

            var request = new GenerateMessageRequest(prompt);
            return await GenerateMessage(request);
        }

        /// <summary>
        /// Runs a model's tokenizer on a string and returns the token count.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <returns>Number of tokens.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="request"/> is <see langword="null"/>.</exception>
        public async Task<CountTokensResponse> CountTokens(GenerateMessageRequest request,
                RequestOptions? requestOptions = null)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var method = GenerativeAI.Method.CountMessageTokens;
            var url = ParseUrl(Url, method);
            string json = Serialize(request);
            var payload = new StringContent(json, Encoding.UTF8, Constants.MediaType);
        
            if (requestOptions != null)
            {
                Client.Timeout = requestOptions.Timeout;
            }
            
            var response = await Client.PostAsync(url, payload);
            response.EnsureSuccessStatusCode();
            return await Deserialize<CountTokensResponse>(response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="request"/> is <see langword="null"/>.</exception>
        /// <exception cref="NotSupportedException"></exception>
        public async Task<EmbedTextResponse> EmbedText(EmbedTextRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (!_model.Equals($"{GenerativeAI.Model.GeckoEmbedding.SanitizeModelName()}", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new NotSupportedException();
            }
            
            var url = ParseUrl(Url, Method);
            string json = Serialize(request);
            var payload = new StringContent(json, Encoding.UTF8, Constants.MediaType);
            var response = await Client.PostAsync(url, payload);
            response.EnsureSuccessStatusCode();
            return await Deserialize<EmbedTextResponse>(response);

        }
        
        /// <remarks/>
        public async Task<EmbedTextResponse> EmbedText(string prompt)
        {
            if (prompt == null) throw new ArgumentNullException(nameof(prompt));
            if (!_model.Equals($"{GenerativeAI.Model.GeckoEmbedding.SanitizeModelName()}", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new NotSupportedException();
            }
            
            var request = new EmbedTextRequest(prompt);
            return await EmbedText(request);
        }

        /// <summary>
        /// Counts the number of tokens in the content. 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <returns>Number of tokens.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="request"/> is <see langword="null"/>.</exception>
        public async Task<CountTokensResponse> CountTokens(EmbedTextRequest request,
            RequestOptions? requestOptions = null)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var method = GenerativeAI.Method.CountMessageTokens;
            var url = ParseUrl(Url, method);
            string json = Serialize(request);
            var payload = new StringContent(json, Encoding.UTF8, Constants.MediaType);
            
            if (requestOptions != null)
            {
                Client.Timeout = requestOptions.Timeout;
            }
            
            var response = await Client.PostAsync(url, payload);
            response.EnsureSuccessStatusCode();
            return await Deserialize<CountTokensResponse>(response);
        }

        /// <summary>
        /// Generates multiple embeddings from the model given input text in a synchronous call.
        /// </summary>
        /// <param name="request">Required. Embed requests for the batch. The model in each of these requests must match the model specified BatchEmbedContentsRequest.model.</param>
        /// <returns>List of Embeddings of the content as a list of floating numbers.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="request"/> is <see langword="null"/>.</exception>
        /// <exception cref="NotSupportedException"></exception>
        public async Task<EmbedTextResponse> BatchEmbedText(BatchEmbedTextRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (!_model.Equals($"{GenerativeAI.Model.GeckoEmbedding.SanitizeModelName()}", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new NotSupportedException();
            }

            var method = GenerativeAI.Method.BatchEmbedText;
            var url = ParseUrl(Url, method);
            string json = Serialize(request);
            var payload = new StringContent(json, Encoding.UTF8, Constants.MediaType);
            var response = await Client.PostAsync(url, payload);
            return await Deserialize<EmbedTextResponse>(response);
        }

        #endregion

        #region "Private methods"

        /// <summary>
        /// Get a list of available tuned models and description.
        /// </summary>
        /// <returns>List of available tuned models.</returns>
        /// <param name="pageSize">The maximum number of Models to return (per page).</param>
        /// <param name="pageToken">A page token, received from a previous models.list call. Provide the pageToken returned by one request as an argument to the next request to retrieve the next page.</param>
        /// <param name="filter">Optional. A filter is a full text search over the tuned model's description and display name. By default, results will not include tuned models shared with everyone. Additional operators: - owner:me - writers:me - readers:me - readers:everyone</param>
        /// <exception cref="NotSupportedException"></exception>
        private async Task<List<ModelResponse>> ListTunedModels(int? pageSize = null, 
            string? pageToken = null, 
            string? filter = null)
        {
            if (_useVertexAi)
            {
                throw new NotSupportedException();
            }

            if (!string.IsNullOrEmpty(_apiKey))
            {
                throw new NotSupportedException("Accessing tuned models via API key is not provided. Setup OAuth for your project.");
            }

            var url = "{endpointGoogleAI}/{Version}/tunedModels";   // v1beta3
            var queryStringParams = new Dictionary<string, string?>()
            {
                [nameof(pageSize)] = Convert.ToString(pageSize), 
                [nameof(pageToken)] = pageToken,
                [nameof(filter)] = filter
            };

            url = ParseUrl(url).AddQueryString(queryStringParams);
            var response = await Client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var models = await Deserialize<ListTunedModelResponse>(response);
            return models?.TunedModels!;
        }
        
        /// <summary>
        /// Parses the URL template and replaces the placeholder with current values.
        /// Given two API endpoints for Google AI Gemini and Vertex AI Gemini this
        /// method uses regular expressions to replace placeholders in a URL template with actual values.
        /// </summary>
        /// <param name="url">API endpoint to parse.</param>
        /// <param name="method">Method part of the URL to inject</param>
        /// <returns></returns>
        private string ParseUrl(string url, string? method = default)
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
                    { "endpointGoogleAI", EndpointGoogleAi },
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
        private string Serialize<T>(T request)
        {
            return JsonSerializer.Serialize(request, _options);
        }

        /// <summary>
        /// Return deserialized object from JSON response.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response"></param>
        /// <returns></returns>
        private async Task<T> Deserialize<T>(HttpResponseMessage? response)
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
        
        #endregion
    }
}
