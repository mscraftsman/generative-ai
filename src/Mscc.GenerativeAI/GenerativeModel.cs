#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
#endif
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Text;

namespace Mscc.GenerativeAI
{
    public class GenerativeModel
    {
        private const string _endpointGoogleAi = "generativelanguage.googleapis.com";
        private const string _urlGoogleAi = "https://{endpointGoogleAI}/{version}/models/{model}:{method}";
        private const string _urlParameterKey = "?key={apiKey}"; // Or in the x-goog-api-key header
        private const string _urlVertexAi = "https://{region}-aiplatform.googleapis.com/{version}/projects/{projectId}/locations/{region}/publishers/{publisher}/models/{model}:{method}";
        private const string _mediaType = "application/json";

        private readonly bool _useVertexAi = false;
        private readonly bool _useApiKeyHeader = false;
        private readonly string _model;
        private readonly string _apiKey = default;
        private readonly string _projectId = default;
        private readonly string _region = default;
        private readonly string _publisher = "google";
        private readonly JsonSerializerOptions _options;
        private List<SafetySetting>? _safetySettings;
        private GenerationConfig? _generationConfig;
        private List<Tool>? _tools;

        private static readonly HttpClient Client = new HttpClient();

        private string Url
        {
            get
            {
                var url = _urlGoogleAi;
                if (!string.IsNullOrEmpty(_apiKey) && !_useApiKeyHeader)
                {
                    url += _urlParameterKey;
                }

                if (_useVertexAi)
                {
                    url = _urlVertexAi;
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
                switch (_model)
                {
                    case Model.BisonChat:
                        return "generateMessage";
                    case Model.BisonText:
                        return "generateText";
                    case Model.GeckoEmbedding:
                        return "embedText";
                    case Model.Embedding:
                        return "embedContent";
                    case Model.AttributedQuestionAnswering:
                        return "generateAnswer";
                    default:
                        break;
                }
                if (_useVertexAi)
                {
                    return "streamGenerateContent";
                }

                return "generateContent";
            }
        }

        // Todo: Remove after ADC has been added.
        private string accessToken;

        public string AccessToken
        {
            get { return accessToken; }
            set
            {
                accessToken = value;
                Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }
        }

        // Todo: Integrate Google.Apis.Auth to retrieve Access_Token on demand. 
        // Todo: Integrate Application Default Credentials as an alternative.
        // Reference: https://cloud.google.com/docs/authentication 
        public GenerativeModel()
        {
            _options = DefaultJsonSerializerOptions();
            // GOOGLE_APPLICATION_CREDENTIALS
            // Linux, macOS: $HOME /.config / gcloud / application_default_credentials.json
            // Windows: % APPDATA %\gcloud\application_default_credentials.json
            //var credentials = GoogleCredential.FromFile(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "gcloud", "application_default_credentials.json"))
            //  .CreateScoped();
        }

        // Todo: Add parameters for GenerationConfig, SafetySettings, Transport? and Tools
        /// <summary>
        /// Constructor to initialize access to Google AI Gemini API.
        /// </summary>
        /// <param name="apiKey">API key provided by Google AI Studio</param>
        /// <param name="model">Model to use (default: "gemini-pro")</param>
        /// <param name="generationConfig"></param>
        /// <param name="safetySettings"></param>
        public GenerativeModel(string apiKey = "", 
            string model = Model.GeminiPro, 
            GenerationConfig? generationConfig = null, 
            List<SafetySetting>? safetySettings = null) : this()
        {
            _apiKey = apiKey;
            _model = model.Sanitize();
            _generationConfig = generationConfig;
            _safetySettings = safetySettings;

            if (!string.IsNullOrEmpty(apiKey))
            {
                _useApiKeyHeader = Client.DefaultRequestHeaders.Contains("x-goog-api-key");
                if (!_useApiKeyHeader)
                {
                    Client.DefaultRequestHeaders.Add("x-goog-api-key", _apiKey);
                }
                _useApiKeyHeader = Client.DefaultRequestHeaders.Contains("x-goog-api-key");
            }
        }

        /// <summary>
        /// Constructor to initialize access to Vertex AI Gemini API.
        /// </summary>
        /// <param name="projectId">Identifier of the Google Cloud project</param>
        /// <param name="region">Region to use</param>
        /// <param name="model">Model to use</param>
        /// <param name="generationConfig"></param>
        /// <param name="safetySettings"></param>
        internal GenerativeModel(string projectId, string region, 
            string model = Model.Gemini10Pro, 
            GenerationConfig? generationConfig = null, 
            List<SafetySetting>? safetySettings = null) : this()
        {
            _useVertexAi = true;
            _projectId = projectId;
            _region = region;
            _model = model.Sanitize();
            _generationConfig = generationConfig;
            _safetySettings = safetySettings;
        }

        /// <summary>
        /// Get a list of available models and description.
        /// </summary>
        /// <returns></returns>
        public async Task<List<ModelResponse>> ListModels()
        {
            if (_useVertexAi)
            {
                throw new NotSupportedException();
            }

            var url = "https://{endpointGoogleAI}/{Version}/models";
            if (!string.IsNullOrEmpty(_apiKey) && !_useApiKeyHeader)
            {
                url += _urlParameterKey;
            }

            url = ParseUrl(url);
            var response = await Client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var models = await Deserialize<ListModelsResponse>(response);
            return models?.Models!;
        }

        /// <summary>
        /// Get information about the model, including default values.
        /// </summary>
        /// <param name="model">The model to query</param>
        /// <returns></returns>
        public async Task<ModelResponse> GetModel(string model = Model.GeminiPro)
        {
            if (_useVertexAi)
            {
                throw new NotSupportedException();
            }

            var url = $"https://{_endpointGoogleAi}/{Version}/models/{model}";
            if (!string.IsNullOrEmpty(_apiKey) && !_useApiKeyHeader)
            {
                url += _urlParameterKey;
            }

            url = ParseUrl(url);
            var response = await Client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await Deserialize<ModelResponse>(response);
        }

        /// <summary>
        /// Produces a single request and response.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<GenerateContentResponse> GenerateContent(GenerateContentRequest? request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var url = ParseUrl(Url, Method);
            string json = Serialize(request);
            var payload = new StringContent(json, Encoding.UTF8, _mediaType);
            var response = await Client.PostAsync(url, payload);
            response.EnsureSuccessStatusCode();

            if (_useVertexAi)
            {
                StringBuilder fullText = new();
                var contentResponseVertex = await Deserialize<List<GenerateContentResponse>>(response);
                foreach (var item in contentResponseVertex)
                {
                    fullText.Append(item.Text);
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

            var config = generationConfig ?? _generationConfig;
            var safety = safetySettings ?? _safetySettings;
            var tool = tools ?? _tools;
            var request = new GenerateContentRequest(prompt, config, safety, tool);
            request.Contents[0].Role = Role.User;
            return await GenerateContent(request);
        }

        /// <remarks/>
        public async Task<GenerateContentResponse> GenerateContent(List<IPart>? parts,
            GenerationConfig? generationConfig = null,
            List<SafetySetting>? safetySettings = null,
            List<Tool>? tools = null)
        {
            if (parts == null) throw new ArgumentNullException(nameof(parts));

            var config = generationConfig ?? _generationConfig;
            var safety = safetySettings ?? _safetySettings;
            var tool = tools ?? _tools;
            var request = new GenerateContentRequest(parts, config, safety, tool);
            request.Contents[0].Role = Role.User;
            return await GenerateContent(request);
        }

        /// <summary>
        /// Returns a list of responses to iterate over. 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        // public async Task<List<GenerateContentResponse>> GenerateContentStream(GenerateContentRequest? request)
        public async Task<Stream> GenerateContentStream(GenerateContentRequest? request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var method = "streamGenerateContent";
            var url = ParseUrl(Url, method);

            // Ref: https://code-maze.com/using-streams-with-httpclient-to-improve-performance-and-memory-usage/
            // Ref: https://www.stevejgordon.co.uk/using-httpcompletionoption-responseheadersread-to-improve-httpclient-performance-dotnet
            var ms = new MemoryStream();
            await JsonSerializer.SerializeAsync(ms, request, _options);
            ms.Seek(0, SeekOrigin.Begin);
            
            var message = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(url),
            };
            message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(_mediaType));

            using (var payload = new StreamContent(ms))
            {
                message.Content = payload;
                payload.Headers.ContentType = new MediaTypeHeaderValue(_mediaType);

                using (var response = await Client.SendAsync(message, HttpCompletionOption.ResponseHeadersRead))
                {
                    response.EnsureSuccessStatusCode();
                    if (response.Content is object)
                    {
                        return await response.Content.ReadAsStreamAsync();
                        // var stream = await response.Content.ReadAsStreamAsync();
                        // return await JsonSerializer.DeserializeAsync<List<GenerateContentResponse>>(stream, options);
                    }
                }
            }

            return null;
        }

        /// <remarks/>
        public async Task<Stream> GenerateContentStream(string? prompt)
        {
            if (prompt == null) throw new ArgumentNullException(nameof(prompt));

            var request = new GenerateContentRequest(prompt, _generationConfig, _safetySettings, _tools);
            request.Contents[0].Role = Role.User;
            return await GenerateContentStream(request);
        }

        /// <remarks/>
        public async Task<Stream> GenerateContentStream(List<IPart>? parts)
        {
            if (parts == null) throw new ArgumentNullException(nameof(parts));

            var request = new GenerateContentRequest(parts, _generationConfig, _safetySettings, _tools);
            request.Contents[0].Role = Role.User;
            return await GenerateContentStream(request);
        }

        /// <remarks/>
        public async Task<GenerateContentResponse> GenerateMessage(GenerateContentRequest? request)
        {
            // ToDo: implement
            throw new NotImplementedException();
        }

        /// <remarks/>
        public async Task<GenerateContentResponse> GenerateText(GenerateContentRequest? request)
        {
            // ToDo: implement
            throw new NotImplementedException();
        }

        /// <remarks/>
        public async Task<GenerateContentResponse> GenerateAnswer(GenerateContentRequest? request)
        {
            // ToDo: implement
            throw new NotImplementedException();
        }

        /// <remarks/>
        public async Task<GenerateContentResponse> EmbedText(GenerateContentRequest? request)
        {
            // ToDo: implement
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get embedding information of the content.
        /// </summary>
        /// <param name="prompt">String to process.</param>
        /// <returns>Embeddings of the content as a list of floating numbers.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        public async Task<EmbedContentResponse> EmbedContent(string? prompt)
        {
            if (prompt == null) throw new ArgumentNullException(nameof(prompt));
            if (_model != (string)Model.Embedding)
            {
                throw new NotSupportedException();
            }

            var request = new EmbedContentRequest(prompt, _generationConfig, _safetySettings, _tools);
            var method = "embedContent";
            var url = ParseUrl(Url, method);
            string json = Serialize(request);
            var payload = new StringContent(json, Encoding.UTF8, _mediaType);
            var response = await Client.PostAsync(url, payload);
            response.EnsureSuccessStatusCode();
            return await Deserialize<EmbedContentResponse>(response);
        }

        /// <summary>
        /// Counts the number of tokens in the content. 
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Number of tokens.</returns>
        public async Task<CountTokensResponse> CountTokens(GenerateContentRequest? request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var method = "countTokens";
            var url = ParseUrl(Url, method);
            string json = Serialize(request);
            var payload = new StringContent(json, Encoding.UTF8, _mediaType);
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

        /// <summary>
        /// Returns the name of the model. 
        /// </summary>
        /// <returns>Name of the model.</returns>
        public string Name()
        {
            return _model;
        }

        // Todo: Implementation missing
        /// <summary>
        /// Starts a chat session. 
        /// </summary>
        /// <param name="history"></param>
        /// <param name="tools"></param>
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
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
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
                    { "endpointGoogleAI", _endpointGoogleAi },
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
    }
}
