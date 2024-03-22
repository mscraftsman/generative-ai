#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
#endif
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Text;

namespace Mscc.GenerativeAI
{
    public class GenerativeModel
    {
        private const string EndpointGoogleAi = "generativelanguage.googleapis.com";
        private const string UrlGoogleAi = "https://{endpointGoogleAI}/{version}/{model}:{method}";
        private const string UrlParameterKey = "?key={apiKey}"; // Or in the x-goog-api-key header
        private const string UrlVertexAi = "https://{region}-aiplatform.googleapis.com/{version}/projects/{projectId}/locations/{region}/publishers/{publisher}/models/{model}:{method}";
        private const string MediaType = "application/json";

        private readonly bool _useVertexAi;
        private readonly string _region = "us-central1";
        private readonly string _publisher = "google";
        private readonly JsonSerializerOptions _options;
        private readonly Credentials? _credentials;

        private string _model;
        private bool _useHeaderApiKey;
        private string? _apiKey;
        private string? _accessToken;
        private bool _useHeaderProjectId;
        private string? _projectId;
        private List<SafetySetting>? _safetySettings;
        private GenerationConfig? _generationConfig;
        private List<Tool>? _tools;

#if NET472_OR_GREATER || NETSTANDARD2_0
        private static readonly Version _httpVersion = HttpVersion.Version11;
        private static readonly HttpClient Client = new HttpClient();
#else
        private static readonly Version _httpVersion = HttpVersion.Version30;
        private static readonly HttpClient Client = new HttpClient(new SocketsHttpHandler
        {
            PooledConnectionLifetime = TimeSpan.FromMinutes(30),
            EnableMultipleHttp2Connections = true,
        })
        {
            DefaultRequestVersion = _httpVersion
        };
#endif

        private string Url
        {
            get
            {
                var url = UrlGoogleAi;
                if (!string.IsNullOrEmpty(_apiKey) && !_useHeaderApiKey)
                {
                    url += UrlParameterKey;
                }

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

        /// <summary>
        /// Returns the name of the model. 
        /// </summary>
        /// <returns>Name of the model.</returns>
        public string Name => _model;

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
        public string? ApiKey
        {
            set
            {
                _apiKey = value;
                if (!string.IsNullOrEmpty(_apiKey))
                {
                    _useHeaderApiKey = Client.DefaultRequestHeaders.Contains("x-goog-api-key");
                    if (!_useHeaderApiKey)
                    {
                        Client.DefaultRequestHeaders.Add("x-goog-api-key", _apiKey);
                    }
                    _useHeaderApiKey = Client.DefaultRequestHeaders.Contains("x-goog-api-key");
                }
            }
        }
        
        public string? AccessToken
        {
            set
            {
                _accessToken = value;
                if (value != null)
                    Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
            }
        }
        
        public string? ProjectId
        {
            set
            {
                _projectId = value;
                if (!string.IsNullOrEmpty(_projectId))
                {
                    _useHeaderProjectId = Client.DefaultRequestHeaders.Contains("x-goog-user-project");
                    if (!_useHeaderProjectId)
                    {
                        Client.DefaultRequestHeaders.Add("x-goog-user-project", _projectId);
                    }
                    _useHeaderProjectId = Client.DefaultRequestHeaders.Contains("x-goog-user-project");
                }
            }
        }

        /// <summary>
        /// Default constructor attempts to read environment variables and
        /// sets default values, if available
        /// </summary>
        public GenerativeModel()
        {
            _options = DefaultJsonSerializerOptions();
            GenerativeModelExtensions.ReadDotEnv();
            var credentialsFile = 
                Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS") ?? 
                Environment.GetEnvironmentVariable("GOOGLE_WEB_CREDENTIALS") ?? 
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "gcloud",
                    "application_default_credentials.json");
            _credentials = GetCredentialsFromFile(credentialsFile);
            
            ApiKey = Environment.GetEnvironmentVariable("GOOGLE_API_KEY");
            AccessToken = Environment.GetEnvironmentVariable("GOOGLE_ACCESS_TOKEN"); // ?? GetAccessTokenFromAdc();
            Model = Environment.GetEnvironmentVariable("GOOGLE_AI_MODEL") ?? 
                     GenerativeAI.Model.Gemini10Pro;
            _region = Environment.GetEnvironmentVariable("GOOGLE_REGION") ?? _region;
        }

        /// <summary>
        /// Constructor to initialize access to Google AI Gemini API.
        /// </summary>
        /// <param name="apiKey">API key provided by Google AI Studio</param>
        /// <param name="model">Model to use (default: "gemini-pro")</param>
        /// <param name="generationConfig">Optional. Configuration options for model generation and outputs.</param>
        /// <param name="safetySettings">Optional. A list of unique SafetySetting instances for blocking unsafe content.</param>
        public GenerativeModel(string? apiKey = null, 
            string? model = null, 
            GenerationConfig? generationConfig = null, 
            List<SafetySetting>? safetySettings = null) : this()
        {
            ApiKey = apiKey ?? _apiKey;
            Model = model ?? _model;
            _generationConfig ??= generationConfig;
            _safetySettings ??= safetySettings;
        }

        /// <summary>
        /// Constructor to initialize access to Vertex AI Gemini API.
        /// </summary>
        /// <param name="projectId">Identifier of the Google Cloud project</param>
        /// <param name="region">Region to use</param>
        /// <param name="model">Model to use</param>
        /// <param name="generationConfig">Optional. Configuration options for model generation and outputs.</param>
        /// <param name="safetySettings">Optional. A list of unique SafetySetting instances for blocking unsafe content.</param>
        internal GenerativeModel(string? projectId = null, string? region = null, 
            string? model = null, 
            GenerationConfig? generationConfig = null, 
            List<SafetySetting>? safetySettings = null) : this()
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
        }

        /// <summary>
        /// Lists models available through the API.
        /// </summary>
        /// <returns>List of available models.</returns>
        /// <param name="tuned">Flag, whether models or tuned models shall be returned.</param>
        /// <param name="pageSize">The maximum number of Models to return (per page).</param>
        /// <param name="pageToken">A page token, received from a previous models.list call. Provide the pageToken returned by one request as an argument to the next request to retrieve the next page.</param>
        /// <param name="filter">Optional. A filter is a full text search over the tuned model's description and display name. By default, results will not include tuned models shared with everyone. Additional operators: - owner:me - writers:me - readers:me - readers:everyone</param>
        /// <exception cref="NotSupportedException"></exception>
        public async Task<List<ModelResponse>> ListModels(bool tuned = false, 
            int? pageSize = null, 
            string? pageToken = null, 
            string? filter = null)
        {
            if (tuned)
            {
                return await ListTunedModels(pageSize, pageToken, filter);
            }
            
            if (_useVertexAi)
            {
                throw new NotSupportedException();
            }

            var url = "https://{endpointGoogleAI}/{Version}/models";
            var queryStringParams = new Dictionary<string, string?>()
            {
                [nameof(pageSize)] = Convert.ToString(pageSize), 
                [nameof(pageToken)] = pageToken
            };
            if (!string.IsNullOrEmpty(_apiKey) && !_useHeaderApiKey)
            {
                queryStringParams.Add("key", _apiKey);
            }

            url = ParseUrl(url).AddQueryString(queryStringParams);
            var response = await Client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var models = await Deserialize<ListModelsResponse>(response);
            return models?.Models!;
        }

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

            var url = "https://{endpointGoogleAI}/{Version}/tunedModels";   // v1beta3
            var queryStringParams = new Dictionary<string, string?>()
            {
                [nameof(pageSize)] = Convert.ToString(pageSize), 
                [nameof(pageToken)] = pageToken,
                [nameof(filter)] = filter
            };
            if (!string.IsNullOrEmpty(_apiKey) && !_useHeaderApiKey)
            {
                queryStringParams.Add("key", _apiKey);
            }

            url = ParseUrl(url).AddQueryString(queryStringParams);
            var response = await Client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var models = await Deserialize<ListTunedModelResponse>(response);
            return models?.TunedModels!;
        }

        /// <summary>
        /// Creates a tuned model.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public async Task<CreateTunedModelResponse> CreateTunedModel(CreateTunedModelRequest request)
        {
            if (!(_model.Equals($"models/{GenerativeAI.Model.BisonText001}", StringComparison.InvariantCultureIgnoreCase) ||
                _model.Equals($"models/{GenerativeAI.Model.Gemini10Pro001}", StringComparison.InvariantCultureIgnoreCase)))
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
            var url = "https://{endpointGoogleAI}/{Version}/{method}";   // v1beta3
            url = ParseUrl(url, method);
            string json = Serialize(request);
            var payload = new StringContent(json, Encoding.UTF8, MediaType);
            var response = await Client.PostAsync(url, payload);
            response.EnsureSuccessStatusCode();
            return await Deserialize<CreateTunedModelResponse>(response);
        }

        /// <summary>
        /// Deletes a tuned model.
        /// </summary>
        /// <param name="model">Required. The resource name of the model. Format: tunedModels/my-model-id</param>
        /// <returns>If successful, the response body is empty.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        public async Task<string> DeleteTunedModel(string model)
        {
            if (string.IsNullOrEmpty(model))
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (_useVertexAi)
            {
                throw new NotSupportedException();
            }

            model = model.SanitizeModelName();
            if (!string.IsNullOrEmpty(_apiKey) && model.StartsWith("tunedModel", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new NotSupportedException("Accessing tuned models via API key is not provided. Setup OAuth for your project.");
            }

            var url = $"https://{EndpointGoogleAi}/{Version}/{model}";   // v1beta3
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
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        public async Task<ModelResponse> PatchTunedModel(string model, ModelResponse tunedModel, string? updateMask = null)
        {
            if (string.IsNullOrEmpty(model))
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (_useVertexAi)
            {
                throw new NotSupportedException();
            }

#if NET472_OR_GREATER || NETSTANDARD2_0
            throw new NotSupportedException();
#else
            model = model.SanitizeModelName();
            if (!string.IsNullOrEmpty(_apiKey) && model.StartsWith("tunedModel", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new NotSupportedException("Accessing tuned models via API key is not provided. Setup OAuth for your project.");
            }

            var url = $"https://{EndpointGoogleAi}/{Version}/{model}";   // v1beta3
            var queryStringParams = new Dictionary<string, string?>()
            {
                [nameof(updateMask)] = updateMask
            };
            if (!string.IsNullOrEmpty(_apiKey) && !_useHeaderApiKey)
            {
                queryStringParams.Add("key", _apiKey);
            }

            url = ParseUrl(url).AddQueryString(queryStringParams);
            string json = Serialize(tunedModel);
            var payload = new StringContent(json, Encoding.UTF8, MediaType);
            var response = await Client.PatchAsync(url, payload);
            response.EnsureSuccessStatusCode();
            return await Deserialize<ModelResponse>(response);
#endif
        }

        /// <summary>
        /// Transfers ownership of the tuned model. This is the only way to change ownership of the tuned model. The current owner will be downgraded to writer role.
        /// </summary>
        /// <param name="model">Required. The resource name of the tuned model to transfer ownership. Format: tunedModels/my-model-id</param>
        /// <param name="emailAddress">Required. The email address of the user to whom the tuned model is being transferred to.</param>
        /// <returns>If successful, the response body is empty.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        public async Task<string> TransferOwnership(string model, string emailAddress)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (emailAddress == null) throw new ArgumentNullException(nameof(emailAddress));

            if (_useVertexAi)
            {
                throw new NotSupportedException();
            }

            model = model.SanitizeModelName();
            if (!string.IsNullOrEmpty(_apiKey) && model.StartsWith("tunedModel", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new NotSupportedException("Accessing tuned models via API key is not provided. Setup OAuth for your project.");
            }

            var method = GenerativeAI.Method.TransferOwnership;
            var url = ParseUrl(Url, method);
            string json = Serialize(new { EmailAddress = emailAddress });
            var payload = new StringContent(json, Encoding.UTF8, MediaType);
            var response = await Client.PostAsync(url, payload);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Gets information about a specific Model.
        /// </summary>
        /// <param name="model">Required. The resource name of the model. This name should match a model name returned by the models.list method. Format: models/model-id or tunedModels/my-model-id</param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public async Task<ModelResponse> GetModel(string model = GenerativeAI.Model.GeminiPro)
        {
            if (_useVertexAi)
            {
                throw new NotSupportedException();
            }

            model = model.SanitizeModelName();
            if (!string.IsNullOrEmpty(_apiKey) && model.StartsWith("tunedModel", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new NotSupportedException("Accessing tuned models via API key is not provided. Setup OAuth for your project.");
            }

            var url = $"https://{EndpointGoogleAi}/{Version}/{model}";
            if (!string.IsNullOrEmpty(_apiKey) && !_useHeaderApiKey)
            {
                url += UrlParameterKey;
            }

            url = ParseUrl(url);
            var response = await Client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await Deserialize<ModelResponse>(response);
        }

        /// <summary>
        /// Generates a response from the model given an input GenerateContentRequest.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<GenerateContentResponse> GenerateContent(GenerateContentRequest? request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var url = ParseUrl(Url, Method);
            string json = Serialize(request);
            var payload = new StringContent(json, Encoding.UTF8, MediaType);
            var response = await Client.PostAsync(url, payload);
            response.EnsureSuccessStatusCode();

            if (_useVertexAi)
            {
                var fullText = new StringBuilder();
                var contentResponseVertex = await Deserialize<List<GenerateContentResponse>>(response);
                foreach (var item in contentResponseVertex)
                {
                    switch (item.Candidates[0].FinishReason)
                    {
                        case FinishReason.Safety:
                            return item;
                            break;
                        case FinishReason.Unspecified:
                        default:
                            fullText.Append(item.Text);
                            break;
                    }
                }
                var result = contentResponseVertex.LastOrDefault();
                result.Candidates[0].Content.Parts[0].Text = fullText.ToString();
                return result;
            }
            return await Deserialize<GenerateContentResponse>(response);
        }

        /// <remarks/>
        public async Task<GenerateContentResponse> GenerateContent(string? prompt,
            GenerationConfig? generationConfig = null,
            List<SafetySetting>? safetySettings = null,
            List<Tool>? tools = null)
        {
            if (prompt == null) throw new ArgumentNullException(nameof(prompt));

            var request = new GenerateContentRequest(prompt, 
                generationConfig ?? _generationConfig, 
                safetySettings ?? _safetySettings, 
                tools ?? _tools);
            return await GenerateContent(request);
        }

        /// <remarks/>
        public async Task<GenerateContentResponse> GenerateContent(List<IPart>? parts,
            GenerationConfig? generationConfig = null,
            List<SafetySetting>? safetySettings = null,
            List<Tool>? tools = null)
        {
            if (parts == null) throw new ArgumentNullException(nameof(parts));

            var request = new GenerateContentRequest(parts, 
                generationConfig ?? _generationConfig, 
                safetySettings ?? _safetySettings, 
                tools ?? _tools);
            request.Contents[0].Role = Role.User;
            return await GenerateContent(request);
        }

        /// <summary>
        /// Generates a streamed response from the model given an input GenerateContentRequest.
        /// This method uses a MemoryStream and StreamContent to send a streaming request to the API.
        /// It runs asynchronously sending and receiving chunks to and from the API endpoint, which allows non-blocking code execution.
        /// </summary>
        /// <param name="request">The request to send to the API.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Stream of GenerateContentResponse with chunks asynchronously.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async IAsyncEnumerable<GenerateContentResponse> GenerateContentStream(GenerateContentRequest? request, 
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var method = "streamGenerateContent";
            var url = ParseUrl(Url, method);

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
            message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaType));

            using (var payload = new StreamContent(ms))
            {
                message.Content = payload;
                payload.Headers.ContentType = new MediaTypeHeaderValue(MediaType);

                using (var response = await Client.SendAsync(message, HttpCompletionOption.ResponseHeadersRead, cancellationToken))
                {
                    response.EnsureSuccessStatusCode();
                    if (response.Content is not null)
                    {
                        using var stream = await response.Content.ReadAsStreamAsync();
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
            List<Tool>? tools = null)
        {
            if (prompt == null) throw new ArgumentNullException(nameof(prompt));

            var request = new GenerateContentRequest(prompt, 
                generationConfig ?? _generationConfig, 
                safetySettings ?? _safetySettings, 
                tools ?? _tools);
            return GenerateContentStream(request);
        }

        /// <remarks/>
        public IAsyncEnumerable<GenerateContentResponse> GenerateContentStream(List<IPart>? parts,
            GenerationConfig? generationConfig = null,
            List<SafetySetting>? safetySettings = null,
            List<Tool>? tools = null)
        {
            if (parts == null) throw new ArgumentNullException(nameof(parts));

            var request = new GenerateContentRequest(parts, 
                generationConfig ?? _generationConfig, 
                safetySettings ?? _safetySettings, 
                tools ?? _tools);
            request.Contents[0].Role = Role.User;
            return GenerateContentStream(request);
        }

        /// <summary>
        /// Generates a grounded answer from the model given an input GenerateAnswerRequest.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Response from the model for a grounded answer.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        public async Task<GenerateAnswerResponse> GenerateAnswer(GenerateAnswerRequest? request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (!_model.Equals($"models/{GenerativeAI.Model.AttributedQuestionAnswering}", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new NotSupportedException();
            }

            var url = ParseUrl(Url, Method);
            string json = Serialize(request);
            var payload = new StringContent(json, Encoding.UTF8, MediaType);
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

        /// <summary>
        /// Generates an embedding from the model given an input Content.
        /// </summary>
        /// <param name="request">Required. EmbedContentRequest to process. The content to embed. Only the parts.text fields will be counted.</param>
        /// <param name="taskType">Optional. Optional task type for which the embeddings will be used. Can only be set for models/embedding-001.</param>
        /// <param name="title">Optional. An optional title for the text. Only applicable when TaskType is RETRIEVAL_DOCUMENT. Note: Specifying a title for RETRIEVAL_DOCUMENT provides better quality embeddings for retrieval.</param>
        /// <returns>Embeddings of the content as a list of floating numbers.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        public async Task<EmbedContentResponse> EmbedContent(EmbedContentRequest request, TaskType? taskType = null, string? title = null)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (!_model.Equals($"models/{GenerativeAI.Model.Embedding}", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new NotSupportedException();
            }

            var url = ParseUrl(Url, Method);
            string json = Serialize(request);
            var payload = new StringContent(json, Encoding.UTF8, MediaType);
            var response = await Client.PostAsync(url, payload);
            response.EnsureSuccessStatusCode();
            return await Deserialize<EmbedContentResponse>(response);
        }

        /// <summary>
        /// Generates an embedding from the model given an input Content.
        /// </summary>
        /// <param name="prompt">Required. String to process. The content to embed. Only the parts.text fields will be counted.</param>
        /// <param name="taskType">Optional. Optional task type for which the embeddings will be used. Can only be set for models/embedding-001.</param>
        /// <param name="title">Optional. An optional title for the text. Only applicable when TaskType is RETRIEVAL_DOCUMENT. Note: Specifying a title for RETRIEVAL_DOCUMENT provides better quality embeddings for retrieval.</param>
        /// <returns>Embeddings of the content as a list of floating numbers.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        public async Task<EmbedContentResponse> EmbedContent(string? prompt, TaskType? taskType = null, string? title = null)
        {
            if (prompt == null) throw new ArgumentNullException(nameof(prompt));
            if (!_model.Equals($"models/{GenerativeAI.Model.Embedding}", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new NotSupportedException();
            }

            var request = new EmbedContentRequest(prompt)
            {
                TaskType = taskType,
                Title = title
            };
            return await EmbedContent(request);
        }

        /// <summary>
        /// Generates multiple embeddings from the model given input text in a synchronous call.
        /// </summary>
        /// <param name="requests">Required. Embed requests for the batch. The model in each of these requests must match the model specified BatchEmbedContentsRequest.model.</param>
        /// <returns>List of Embeddings of the content as a list of floating numbers.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        public async Task<EmbedContentResponse> BatchEmbedContent(List<EmbedContentRequest> requests)
        {
            if (requests == null) throw new ArgumentNullException(nameof(requests));
            if (!_model.Equals($"models/{GenerativeAI.Model.Embedding}", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new NotSupportedException();
            }

            var method = GenerativeAI.Method.BatchEmbedContent;
            var url = ParseUrl(Url, method);
            string json = Serialize(requests);
            var payload = new StringContent(json, Encoding.UTF8, MediaType);
            var response = await Client.PostAsync(url, payload);
            return await Deserialize<EmbedContentResponse>(response);
        }

        /// <summary>
        /// Counts the number of tokens in the content. 
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Number of tokens.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<CountTokensResponse> CountTokens(GenerateContentRequest? request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var method = GenerativeAI.Method.CountTokens;
            var url = ParseUrl(Url, method);
            string json = Serialize(request);
            var payload = new StringContent(json, Encoding.UTF8, MediaType);
            var response = await Client.PostAsync(url, payload);
            response.EnsureSuccessStatusCode();
            return await Deserialize<CountTokensResponse>(response);
        }

        /// <remarks/>
        public async Task<CountTokensResponse> CountTokens(string? prompt)
        {
            if (prompt == null) throw new ArgumentNullException(nameof(prompt));

            var request = new GenerateContentRequest(prompt, _generationConfig, _safetySettings, _tools);
            return await CountTokens(request);
        }

        /// <remarks/>
        public async Task<CountTokensResponse> CountTokens(List<IPart>? parts)
        {
            if (parts == null) throw new ArgumentNullException(nameof(parts));

            var request = new GenerateContentRequest(parts, _generationConfig, _safetySettings, _tools);
            return await CountTokens(request);
        }

        // Todo: Implementation missing
        /// <summary>
        /// Starts a chat session. 
        /// </summary>
        /// <param name="history"></param>
        /// <param name="generationConfig">Optional. Configuration options for model generation and outputs.</param>
        /// <param name="safetySettings">Optional. A list of unique SafetySetting instances for blocking unsafe content.</param>
        /// <param name="tools">Optional. A list of Tools the model may use to generate the next response.</param>
        /// <returns></returns>
        public ChatSession StartChat(List<ContentResponse>? history = null, 
            GenerationConfig? generationConfig = null,
            List<SafetySetting>? safetySettings = null, 
            List<Tool>? tools = null)
        {
            var config = generationConfig ?? _generationConfig;
            var safety = safetySettings ?? _safetySettings;
            var tool = tools ?? _tools;
            return new ChatSession(this, history, config, safety, tool);
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
        private string RunExternalExe(string filename, string arguments = null)
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
        private string Format(string filename, string arguments)
        {
            return "'" + filename + 
                ((string.IsNullOrEmpty(arguments)) ? string.Empty : " " + arguments) +
                "'";
        }
    }
}
