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
                switch (_model)
                {
                    case GenerativeAI.Model.BisonChat:
                        return "generateMessage";
                    case GenerativeAI.Model.BisonText:
                        return "generateText";
                    case GenerativeAI.Model.GeckoEmbedding:
                        return "embedText";
                    case GenerativeAI.Model.Embedding:
                        return "embedContent";
                    case GenerativeAI.Model.AttributedQuestionAnswering:
                        return "generateAnswer";
                }
                if (_useVertexAi)
                {
                    return "streamGenerateContent";
                }

                return "generateContent";
            }
        }

        /// <summary>
        /// Returns the name of the model. 
        /// </summary>
        /// <returns>Name of the model.</returns>
        public string Name => _model;

        public string Model
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
        /// <param name="generationConfig"></param>
        /// <param name="safetySettings"></param>
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
        /// <param name="generationConfig"></param>
        /// <param name="safetySettings"></param>
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
        /// Get a list of available models and description.
        /// </summary>
        /// <returns>List of available models.</returns>
        public async Task<List<ModelResponse>> ListModels()
        {
            if (_useVertexAi)
            {
                throw new NotSupportedException();
            }

            var url = "https://{endpointGoogleAI}/{Version}/models";
            if (!string.IsNullOrEmpty(_apiKey) && !_useHeaderApiKey)
            {
                url += UrlParameterKey;
            }

            url = ParseUrl(url);
            var response = await Client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var models = await Deserialize<ListModelsResponse>(response);
            return models?.Models!;
        }

        /// <summary>
        /// Get a list of available tuned models and description.
        /// </summary>
        /// <returns>List of available tuned models.</returns>
        public async Task<List<ModelResponse>> ListTunedModels()
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
            url = ParseUrl(url);
            var response = await Client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var models = await Deserialize<ListTunedModelResponse>(response);
            return models?.TunedModels!;
        }

        /// <summary>
        /// Create a tuned model.
        /// </summary>
        /// <returns></returns>
        public async Task<CreateTunedModelResponse> CreateTunedModel(CreateTunedModelRequest request)
        {
            if (!(_model is (string)GenerativeAI.Model.BisonText001 ||
                _model is (string)GenerativeAI.Model.Gemini10Pro001))
            {
                throw new NotSupportedException();
            }

            if (!string.IsNullOrEmpty(_apiKey))
            {
                throw new NotSupportedException("Accessing tuned models via API key is not provided. Setup OAuth for your project.");
            }

            var method = "tunedModels";
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
        /// Get information about the model, including default values.
        /// </summary>
        /// <param name="model">The model to query</param>
        /// <returns></returns>
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
        /// Produces a single request and response.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
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

            var request = new GenerateContentRequest(parts, 
                generationConfig ?? _generationConfig, 
                safetySettings ?? _safetySettings, 
                tools ?? _tools);
            request.Contents[0].Role = Role.User;
            return await GenerateContent(request);
        }

        /// <summary>
        /// Returns a list of responses to iterate over. 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        // public async Task<List<GenerateContentResponse>> GenerateContentStream(GenerateContentRequest? request)
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
            request.Contents[0].Role = Role.User;
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

        /// <remarks/>
        public async Task<GenerateContentResponse> Predict(GenerateContentRequest? request)
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
        public async Task<EmbedContentResponse> EmbedContent(string? prompt, TaskType? taskType = null, string? title = null)
        {
            if (prompt == null) throw new ArgumentNullException(nameof(prompt));
            if (_model != (string)GenerativeAI.Model.Embedding)
            {
                throw new NotSupportedException();
            }

            var request = new EmbedContentRequest(prompt)
            {
                TaskType = taskType,
                Title = title
            };
            var method = "embedContent";
            var url = ParseUrl(Url, method);
            string json = Serialize(request);
            var payload = new StringContent(json, Encoding.UTF8, MediaType);
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
        /// Retrieve access token from Application Default Credentials (ADC) 
        /// </summary>
        /// <returns>The access token.</returns>
        // Reference: https://cloud.google.com/docs/authentication 
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

        private string Format(string filename, string arguments)
        {
            return "'" + filename + 
                ((string.IsNullOrEmpty(arguments)) ? string.Empty : " " + arguments) +
                "'";
        }
    }
}
