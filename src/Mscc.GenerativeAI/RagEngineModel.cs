#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
#endif
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Text;
using ArgumentNullException = System.ArgumentNullException;

namespace Mscc.GenerativeAI
{
    public sealed class RagEngineModel : BaseModel
    {
        private string Url => "{BaseUrlVertexAi}/ragCorpora";

        /// <summary>
        /// Initializes a new instance of the <see cref="RagEngineModel"/> class.
        /// </summary>
        public RagEngineModel() : this(httpClientFactory: null, logger: null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RagEngineModel"/> class.
        /// </summary>
        /// <param name="httpClientFactory">Optional. The <see cref="IHttpClientFactory"/> to use for creating HttpClient instances.</param>
        /// <param name="logger">Optional. Logger instance used for logging</param>
        public RagEngineModel(IHttpClientFactory? httpClientFactory = null, ILogger? logger = null) : base(httpClientFactory, logger)
        {
            Version = ApiVersion.V1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RagEngineModel"/> class.
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="region"></param>
        /// <param name="model"></param>
        /// <param name="generationConfig"></param>
        /// <param name="safetySettings"></param>
        /// <param name="tools"></param>
        /// <param name="systemInstruction"></param>
        /// <param name="toolConfig"></param>
        /// <param name="httpClientFactory">Optional. The <see cref="IHttpClientFactory"/> to use for creating HttpClient instances.</param>
        /// <param name="logger">Optional. Logger instance used for logging</param>
        internal RagEngineModel(string? projectId = null, string? region = null,
            string? model = null,
            GenerationConfig? generationConfig = null,
            List<SafetySetting>? safetySettings = null,
            List<Tool>? tools = null,
            Content? systemInstruction = null,
            ToolConfig? toolConfig = null,
            IHttpClientFactory? httpClientFactory = null, 
            ILogger? logger = null) : base(projectId, region, model, httpClientFactory, logger)
        {
        }

        /// <summary>
        /// Creates an empty `RAG Corpus`.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        public async Task<RagCorpus> Create(RagCorpus request,
            RequestOptions? requestOptions = null, 
            CancellationToken cancellationToken = default)
        {
            var url = ParseUrl(Url);
            return await PostAsync<RagCorpus, RagCorpus>(request, url, string.Empty, requestOptions, HttpCompletionOption.ResponseContentRead, cancellationToken);
        }

        /// <summary>
        /// Updates a `RAG Corpus`.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="corpus"></param>
        /// <param name="updateMask"></param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        public async Task<RagCorpus> Update(string name,
            RagCorpus corpus,
            string? updateMask = null,
            RequestOptions? requestOptions = null, 
            CancellationToken cancellationToken = default)
        {
            var url = $"{Url}/{name}"; // v1beta1
            var queryStringParams = new Dictionary<string, string?>() { [nameof(updateMask)] = updateMask };

            url = ParseUrl(url).AddQueryString(queryStringParams);
            var json = Serialize(corpus);
            var payload = new StringContent(json, Encoding.UTF8, Constants.MediaType);
            using var httpRequest = new HttpRequestMessage();
#if NET472_OR_GREATER || NETSTANDARD2_0
            httpRequest.Method = new HttpMethod("PATCH");
#else
            httpRequest.Method = HttpMethod.Patch;
#endif
            httpRequest.RequestUri = new Uri(url);
            httpRequest.Version = _httpVersion;
            httpRequest.Content = payload;
            var response = await SendAsync(httpRequest, requestOptions, cancellationToken);
            await response.EnsureSuccessAsync();
            return await Deserialize<RagCorpus>(response);
        }

        /// <summary>
        /// Lists all `RAG Corpora` owned by the user.
        /// </summary>
        /// <param name="pageSize">The maximum number of Models to return (per page).</param>
        /// <param name="pageToken">A page token, received from a previous ListModels call. Provide the pageToken returned by one request as an argument to the next request to retrieve the next page.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        public async Task<List<RagCorpus>> List(int? pageSize = 50,
            string? pageToken = null,
            RequestOptions? requestOptions = null, 
            CancellationToken cancellationToken = default)
        {
            var queryStringParams = new Dictionary<string, string?>()
            {
                [nameof(pageSize)] = Convert.ToString(pageSize), [nameof(pageToken)] = pageToken
            };

            var url = ParseUrl(Url).AddQueryString(queryStringParams);
            using var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await SendAsync(httpRequest, requestOptions, cancellationToken);
            await response.EnsureSuccessAsync();
            var corpora = await Deserialize<ListRagCorporaResponse>(response);
            return corpora?.Corpora!;
        }

        /// <summary>
        /// Gets information about a specific `RAG Corpus`.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        public async Task<RagCorpus> Get(string name,
            RequestOptions? requestOptions = null, 
            CancellationToken cancellationToken = default)
        {
            var url = ParseUrl($"{Url}/{name}");
            using var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await SendAsync(httpRequest, requestOptions, cancellationToken);
            await response.EnsureSuccessAsync();
            return await Deserialize<RagCorpus>(response);
        }

        /// <summary>
        /// Deletes a `RAG Corpus`.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="force"></param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>DeleteOperationMetadata</returns>
        public async Task<string> Delete(string name,
            bool force,
            RequestOptions? requestOptions = null, 
            CancellationToken cancellationToken = default)
        {
            var url = $"{Url}/{name}";
            var queryStringParams = new Dictionary<string, string?>() { [nameof(force)] = Convert.ToString(force) };

            url = ParseUrl(url).AddQueryString(queryStringParams);
            using var httpRequest = new HttpRequestMessage(HttpMethod.Delete, url);
            var response = await SendAsync(httpRequest, requestOptions, cancellationToken);
            await response.EnsureSuccessAsync();
#if NET472_OR_GREATER || NETSTANDARD2_0
            return await response.Content.ReadAsStringAsync();
#else
            return await response.Content.ReadAsStringAsync(cancellationToken);
#endif
        }

        /// <summary>
        /// Upload a local file to the corpus.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="uri">URI or path to the file to upload.</param>
        /// <param name="displayName">A name displayed for the uploaded file.</param>
        /// <param name="description"></param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        public async Task<RagFile> UploadFile(string name,
            string uri,
            string displayName,
            string description,
            RequestOptions? requestOptions = null, 
            CancellationToken cancellationToken = default)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (name == uri) throw new ArgumentNullException(nameof(uri));
            if (!File.Exists(uri)) throw new FileNotFoundException(nameof(uri));
            if (string.IsNullOrEmpty(displayName)) throw new ArgumentException(nameof(displayName));

            var mimeType = GenerativeAIExtensions.GetMimeType(uri);
            using var fs = new FileStream(uri, FileMode.Open);
            return await UploadFile(name, fs, mimeType, displayName, description, requestOptions, cancellationToken);
        }
        
        /// <summary>
        /// Upload a stream to the corpus.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="stream">Stream to upload.</param>
        /// <param name="mimeType">The MIME type of the stream content.</param>
        /// <param name="displayName">A name displayed for the uploaded file.</param>
        /// <param name="description"></param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public async Task<RagFile> UploadFile(string name,
            Stream stream,
            string mimeType,
            string displayName,
            string description,
            RequestOptions? requestOptions = null, 
            CancellationToken cancellationToken = default)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (string.IsNullOrEmpty(mimeType)) throw new ArgumentException(nameof(mimeType));
            if (string.IsNullOrEmpty(displayName)) throw new ArgumentException(nameof(displayName));
            
            // "multipart" Upload
            // metadata = StringContent("metadata", Serialize(ragFile)
            // StreamContent("file"
            var totalBytes = stream.Length;
            var ragFile = new RagFile() { DisplayName = displayName, Description = description };
            var request = new UploadMediaRequest()
            {
                File = new FileRequest()
                {
                    DisplayName = displayName
                }
            };

            var url = $"{Url}/{name}/ragFiles:upload";
            url = ParseUrl(url).AddQueryString(new Dictionary<string, string?>()
            {
                ["alt"] = "json", 
                ["uploadType"] = "multipart"
            });
            var json = Serialize(request);

            var multipartContent = new MultipartContent("related");
            multipartContent.Add(new StringContent(json, Encoding.UTF8, Constants.MediaType));
            multipartContent.Add(new StreamContent(stream, (int)Constants.ChunkSize)
            {
                Headers = { 
                    ContentType = new MediaTypeHeaderValue(mimeType), 
                    ContentLength = totalBytes 
                }
            });
            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);
            httpRequest.Content = multipartContent;
            var response = await SendAsync(httpRequest, requestOptions, cancellationToken);
            await response.EnsureSuccessAsync();
            return await Deserialize<RagFile>(response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="uris"></param>
        /// <param name="transformationConfig"></param>
        /// <param name="maxEmbeddingRequestsPerMin"></param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>ImportRagFilesOperationMetadata</returns>
        public async Task<string> ImportFiles(string name,
            string[] uris,
            TransformationConfig? transformationConfig,
            int? maxEmbeddingRequestsPerMin,
            CancellationToken cancellationToken = default)
        {
            var url = $"{Url}/{name}/ragFiles:import";

            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pageSize">The maximum number of Models to return (per page).</param>
        /// <param name="pageToken">A page token, received from a previous ListModels call. Provide the pageToken returned by one request as an argument to the next request to retrieve the next page.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        public async Task<List<RagFile>> ListFiles(string name,
            int? pageSize = 50,
            string? pageToken = null,
            RequestOptions? requestOptions = null, 
            CancellationToken cancellationToken = default)
        {
            var queryStringParams = new Dictionary<string, string?>()
            {
                [nameof(pageSize)] = Convert.ToString(pageSize), [nameof(pageToken)] = pageToken
            };

            var url = ParseUrl($"{Url}/{name}/ragFiles").AddQueryString(queryStringParams);
            using var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await SendAsync(httpRequest, requestOptions, cancellationToken);
            await response.EnsureSuccessAsync();
            var corpora = await Deserialize<ListRagFilesResponse>(response);
            return corpora?.Files!;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fileName"></param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        public async Task<RagFile> GetFile(string name,
            string fileName,
            RequestOptions? requestOptions = null, 
            CancellationToken cancellationToken = default)
        {
            var url = ParseUrl($"{Url}/{name}/ragFiles/{fileName}");
            using var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await SendAsync(httpRequest, requestOptions, cancellationToken);
            await response.EnsureSuccessAsync();
            return await Deserialize<RagFile>(response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fileName"></param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<string> DeleteFile(string name,
            string fileName,
            RequestOptions? requestOptions = null, 
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrEmpty(fileName)) throw new ArgumentNullException(nameof(fileName));

            var url = ParseUrl($"{Url}/{name}/ragFiles/{fileName}");
            url = ParseUrl(url);
            using var httpRequest = new HttpRequestMessage(HttpMethod.Delete, url);
            var response = await SendAsync(httpRequest, requestOptions, cancellationToken);
            await response.EnsureSuccessAsync();
#if NET472_OR_GREATER || NETSTANDARD2_0
            return await response.Content.ReadAsStringAsync();
#else
            return await response.Content.ReadAsStringAsync(cancellationToken);
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        public async Task<RagQueryResponse> RetrievalQuery(RagRetrievalQueryRequest request,
            RequestOptions? requestOptions = null, 
            CancellationToken cancellationToken = default)
        {
            var method = GenerativeAI.Method.RetrieveContexts;
            var url = "{BaseUrlVertexAi}:{method}";
            return await PostAsync<RagRetrievalQueryRequest, RagQueryResponse>(request, url, method, requestOptions, HttpCompletionOption.ResponseContentRead, cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ragResources"></param>
        /// <param name="text"></param>
        /// <param name="ragRetrievalConfig"></param>
        /// <param name="vectorDistanceThreshold"></param>
        /// <param name="similarityTopK"></param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        public async Task<RagQueryResponse> RetrievalQuery(RagResource[] ragResources,
            string text,
            RagRetrievalConfig? ragRetrievalConfig,
            float? vectorDistanceThreshold,
            float? similarityTopK,
            RequestOptions? requestOptions = null, 
            CancellationToken cancellationToken = default)
        {
            var request = new RagRetrievalQueryRequest()
            {
                VertexRagStore =
                    new()
                    {
                        RagResources = ragResources, 
                        VectorDistanceThreshold = vectorDistanceThreshold
                    },
                Query = new()
                {
                    Text = text, 
                    RagRetrievalConfig = ragRetrievalConfig, 
                    SimilarityTopK = similarityTopK
                }
            };

            return await RetrievalQuery(request, requestOptions, cancellationToken);
        }
    }

    public class RagRetrievalQueryResponse
    {
        public List<RagFile> Files { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class RagRetrievalQueryRequest
    {
        /// <summary>
        /// Required. Single RAG retrieve query.
        /// </summary>
        public RagQuery Query { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public VertexRagStore VertexRagStore { get; set; }
    }

    public class RagQueryRequest
    {
        /// <summary>
        /// Required. Single RAG retrieve query.
        /// </summary>
        public RagQuery Query { get; set; }
        /// <summary>
        /// The data source for Vertex RagStore.
        /// </summary>
        public VertexRagStore DataSource { get; set; }
    }

    public class RagQueryResponse
    {
        public RagContexts Contexts { get; set; }
    }

    /// <summary>
    /// Relevant contexts for one query.
    /// </summary>
    public class RagContexts
    {
        public List<Context> Contexts { get; set; }
    }

    /// <summary>
    /// A context of the query.
    /// </summary>
    public class Context
    {
        /// <summary>
        /// If the file is imported from Cloud Storage or Google Drive,
        /// sourceUri will be original file URI in Cloud Storage or Google Drive;
        /// if file is uploaded, sourceUri will be file display name.
        /// </summary>
        public string SourceUri { get; set; }
        /// <summary>
        /// The file display name.
        /// </summary>
        public string SourceDisplayName { get; set; }
        /// <summary>
        /// The text chunk.
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// According to the underlying Vector DB and the selected metric type,
        /// the score can be either the distance or the similarity between the query
        /// and the context and its range depends on the metric type.
        /// </summary>
        /// <remarks>
        /// For example, if the metric type is COSINE_DISTANCE, it represents the distance
        /// between the query and the context. The larger the distance, the less relevant
        /// the context is to the query.
        /// The range is [0, 2], while 0 means the most relevant and 2 means the least relevant.
        /// </remarks>
        public float Score { get; set; }
    }
}
