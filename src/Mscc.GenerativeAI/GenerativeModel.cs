#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
#endif
using System.Text.RegularExpressions;
using System.Text;

namespace Mscc.GenerativeAI
{
    public class GenerativeModel
    {
        private readonly bool useVertexAI = false;
        private readonly bool useApiKeyHeader = false;
        private readonly string endpointGoogleAI = "generativelanguage.googleapis.com";
        private readonly string urlGoogleAI = "https://{endpointGoogleAI}/{version}/models/{model}:{method}";
        // Or in the x-goog-api-key header
        private readonly string urlParameterKey = "?key={apiKey}";
        private readonly string urlVertexAI = "https://{region}-aiplatform.googleapis.com/{version}/projects/{projectId}/locations/{region}/publishers/{publisher}/models/{model}:{method}";
        private readonly string model;
        private readonly string apiKey = default;
        private readonly string projectId = default;
        private readonly string region = default;
        private readonly string publisher = "google";
        private List<SafetySetting>? safetySettings;
        private GenerationConfig? generationConfig;
        private List<Tool>? tools;

        private static readonly HttpClient Client = new HttpClient();

        private string Url
        {
            get
            {
                var url = urlGoogleAI;
                if (!string.IsNullOrEmpty(apiKey) && !useApiKeyHeader)
                {
                    url += urlParameterKey;
                }

                if (useVertexAI)
                {
                    url = urlVertexAI;
                }

                return url;
            }
        }

        private string Version
        {
            get
            {
                if (useVertexAI)
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
                switch (model)
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
                if (useVertexAI)
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
                Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            }
        }

        // Todo: Integrate Google.Apis.Auth to retrieve Access_Token on demand. 
        // Todo: Integrate Application Default Credentials as an alternative.
        // Reference: https://cloud.google.com/docs/authentication 
        public GenerativeModel()
        {
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
        public GenerativeModel(string apiKey = "", string model = Model.GeminiPro, GenerationConfig? generationConfig = null, List<SafetySetting>? safetySettings = null)
        {
            this.apiKey = apiKey;
            this.model = model;
            this.generationConfig = generationConfig;
            this.safetySettings = safetySettings;

            if (!string.IsNullOrEmpty(apiKey))
            {
                useApiKeyHeader = Client.DefaultRequestHeaders.Contains("x-goog-api-key");
                if (!useApiKeyHeader)
                {
                    Client.DefaultRequestHeaders.Add("x-goog-api-key", apiKey);
                }
                useApiKeyHeader = Client.DefaultRequestHeaders.Contains("x-goog-api-key");
            }
        }

        // Todo: Add parameters for GenerationConfig, SafetySettings, Transport? and Tools
        /// <summary>
        /// Constructor to initialize access to Vertex AI Gemini API.
        /// </summary>
        /// <param name="projectId">Identifier of the Google Cloud project</param>
        /// <param name="region">Region to use</param>
        /// <param name="model">Model to use</param>
        /// <param name="generationConfig"></param>
        /// <param name="safetySettings"></param>
        internal GenerativeModel(string projectId, string region, string model, GenerationConfig? generationConfig = null, List<SafetySetting>? safetySettings = null)
        {
            this.useVertexAI = true;
            this.projectId = projectId;
            this.region = region;
            this.model = model;
            this.generationConfig = generationConfig;
            this.safetySettings = safetySettings;
        }

        /// <summary>
        /// Get a list of available models and description.
        /// </summary>
        /// <returns></returns>
        public async Task<List<ModelResponse>> ListModels()
        {
            if (useVertexAI)
            {
                throw new NotSupportedException();
            }

            var url = "https://{endpointGoogleAI}/{Version}/models";
            if (!string.IsNullOrEmpty(apiKey) && !useApiKeyHeader)
            {
                url += urlParameterKey;
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
            if (useVertexAI)
            {
                throw new NotSupportedException();
            }

            var url = $"https://{endpointGoogleAI}/{Version}/models/{model}";
            if (!string.IsNullOrEmpty(apiKey) && !useApiKeyHeader)
            {
                url += urlParameterKey;
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
            var mediaType = "application/json";     // MediaTypeHeaderValue.Parse("application/json");
            var payload = new StringContent(json, System.Text.Encoding.UTF8, mediaType);
            var response = await Client.PostAsync(url, payload);
            response.EnsureSuccessStatusCode();

            if (useVertexAI)
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
        public async Task<GenerateContentResponse> GenerateContent(string? prompt)
        {
            if (prompt == null) throw new ArgumentNullException(nameof(prompt));

            var request = new GenerateContentRequest(prompt, generationConfig, safetySettings, tools);
            request.Contents[0].Role = "user";
            return await GenerateContent(request);
        }

        /// <remarks/>
        public async Task<GenerateContentResponse> GenerateContent(List<IPart>? parts)
        {
            if (parts == null) throw new ArgumentNullException(nameof(parts));

            var request = new GenerateContentRequest(parts, generationConfig, safetySettings, tools);
            request.Contents[0].Role = "user";
            return await GenerateContent(request);
        }

        /// <summary>
        /// Returns a list of responses to iterate over. 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<List<GenerateContentResponse>> GenerateContentStream(GenerateContentRequest? request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var method = "streamGenerateContent";
            var url = ParseUrl(Url, method);
            string json = Serialize(request);
            var mediaType = "application/json";     // MediaTypeHeaderValue.Parse("application/json");
            var payload = new StringContent(json, System.Text.Encoding.UTF8, mediaType);
            var response = await Client.PostAsync(url, payload);
            response.EnsureSuccessStatusCode();
            return await Deserialize<List<GenerateContentResponse>>(response);
        }

        /// <remarks/>
        public async Task<List<GenerateContentResponse>> GenerateContentStream(string? prompt)
        {
            if (prompt == null) throw new ArgumentNullException(nameof(prompt));

            var request = new GenerateContentRequest(prompt, generationConfig, safetySettings, tools);
            request.Contents[0].Role = "user";
            return await GenerateContentStream(request);
        }

        /// <remarks/>
        public async Task<List<GenerateContentResponse>> GenerateContentStream(List<IPart>? parts)
        {
            if (parts == null) throw new ArgumentNullException(nameof(parts));

            var request = new GenerateContentRequest(parts, generationConfig, safetySettings, tools);
            request.Contents[0].Role = "user";
            return await GenerateContentStream(request);
        }

        /// <remarks/>
        public async Task<GenerateContentResponse> GenerateMessage(GenerateContentRequest? request)
        {
            throw new NotImplementedException();
        }

        /// <remarks/>
        public async Task<GenerateContentResponse> GenerateText(GenerateContentRequest? request)
        {
            throw new NotImplementedException();
        }

        /// <remarks/>
        public async Task<GenerateContentResponse> GenerateAnswer(GenerateContentRequest? request)
        {
            throw new NotImplementedException();
        }

        /// <remarks/>
        public async Task<GenerateContentResponse> EmbedText(GenerateContentRequest? request)
        {
            throw new NotImplementedException();
        }

        /// <remarks/>
        public async Task<GenerateContentResponse> EmbedContent(GenerateContentRequest? request)
        {
            throw new NotImplementedException();
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
            var mediaType = "application/json";     // MediaTypeHeaderValue.Parse("application/json");
            var payload = new StringContent(json, System.Text.Encoding.UTF8, mediaType);
            var response = await Client.PostAsync(url, payload);
            response.EnsureSuccessStatusCode();
            return await Deserialize<CountTokensResponse>(response);
        }

        /// <remarks/>
        public async Task<CountTokensResponse> CountTokens(string? prompt)
        {
            if (prompt == null) throw new ArgumentNullException(nameof(prompt));

            var request = new GenerateContentRequest(prompt, generationConfig, safetySettings, tools);
            return await CountTokens(request);
        }

        /// <remarks/>
        public async Task<CountTokensResponse> CountTokens(List<IPart>? parts)
        {
            if (parts == null) throw new ArgumentNullException(nameof(parts));

            var request = new GenerateContentRequest(parts, generationConfig, safetySettings, tools);
            return await CountTokens(request);
        }

        /// <summary>
        /// Returns the name of the model. 
        /// </summary>
        /// <returns>Name of the model.</returns>
        public string Name()
        {
            return this.model;
        }

        // Todo: Implementation missing
        /// <summary>
        /// Starts a chat session. 
        /// </summary>
        /// <param name="history"></param>
        /// <param name="tools"></param>
        /// <returns></returns>
        public ChatSession StartChat(List<ContentResponse>? history = null, GenerationConfig? generationConfig = null, List<SafetySetting>? safetySettings = null, List<Tool>? tools = null)
        {
            //this.generationConfig = generationConfig ?? new GenerationConfig();
            //this.safetySettings = safetySettings ?? new List<SafetySetting>();
            //this.tools = tools ?? new List<Tool>();
            return new ChatSession(this, history);
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
                    { "endpointGoogleAI", endpointGoogleAI },
                    { "version", Version },
                    { "model", model },
                    { "apikey", apiKey },
                    { "projectid", projectId },
                    { "region", region },
                    { "location", region },
                    { "publisher", publisher }
                };
            }
        }

        /// <summary>
        /// Return serialized JSON string of request payload.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private string Serialize(GenerateContentRequest? request)
        {
            request.Synchronize();
            var options = DefaultJsonSerializerOptions();
            return JsonSerializer.Serialize(request, options);
        }

        /// <summary>
        /// Return deserialized object from JSON response.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response"></param>
        /// <returns></returns>
        private async Task<T> Deserialize<T>(HttpResponseMessage? response)
        {
            var options = DefaultJsonSerializerOptions();
#if NET472_OR_GREATER || NETSTANDARD2_0
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(json, options);
#else
            return await response.Content.ReadFromJsonAsync<T>(options);
#endif
        }

        /// <summary>
        /// Get default options for JSON serialization.
        /// </summary>
        /// <returns></returns>
        private static JsonSerializerOptions DefaultJsonSerializerOptions()
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
