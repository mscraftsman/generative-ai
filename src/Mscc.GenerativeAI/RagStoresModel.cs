#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
#endif
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Text;

namespace Mscc.GenerativeAI
{
    public class RagStoresModel : BaseModel
    {
        internal override string Version => ApiVersion.V1Beta;

        /// <summary>
        /// Initializes a new instance of the <see cref="RagStoresModel"/> class.
        /// </summary>
        public RagStoresModel() : this(httpClientFactory: null, logger: null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RagStoresModel"/> class.
        /// </summary>
        /// <param name="httpClientFactory">Optional. The <see cref="IHttpClientFactory"/> to use for creating HttpClient instances.</param>
        /// <param name="logger">Optional. Logger instance used for logging</param>
        public RagStoresModel(IHttpClientFactory? httpClientFactory = null, ILogger? logger = null) : base(httpClientFactory, logger) { }

        /// <summary>
        /// Creates an empty <see cref="Document"/>.
        /// </summary>
        /// <param name="request">Required. The `Document`.</param>
        /// <param name="ragStoresId">Required. The name of the `RagStore` where this `Document` will be created. Example: `ragStores/my-rag-store-123`</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="ragStoresId"/> is <see langword="null"/> or empty.</exception>
        /// <exception cref="NotSupportedException">Thrown when the functionality is not supported by the model.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        public async Task<Document> Create(Document request,
            string ragStoresId,
            RequestOptions requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(ragStoresId)) throw new ArgumentException("Value cannot be null or empty.", nameof(ragStoresId));
            // this.GuardSupported();

            var url = "{BaseUrlGoogleAi}/ragStores/{ragStoresId}/documents";
            return await PostAsync<Document, Document>(request, url, string.Empty, requestOptions, HttpCompletionOption.ResponseContentRead, cancellationToken);
        }
        
        /// <summary>
        /// Deletes a <see cref="Document"/>.
        /// </summary>
        /// <param name="ragStoresId">Required. The name of the `RagStore` where this `Document` exists. Example: `ragStores/my-rag-store-123`</param>
        /// <param name="documentsId">Required. The name of the `Document`.</param>
        /// <param name="force">Optional. If set to true, any `Chunk`s and objects related to this `Document` will also be deleted. If false (the default), a `FAILED_PRECONDITION` error will be returned if `Document` contains any `Chunk`s.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="ragStoresId"/> is <see langword="null"/> or empty.</exception>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="documentsId"/> is <see langword="null"/> or empty.</exception>
        /// <exception cref="NotSupportedException">Thrown when the functionality is not supported by the model.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        public async Task<string> Delete(string ragStoresId,
            string documentsId,
            bool force,
            RequestOptions requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(ragStoresId)) throw new ArgumentException("Value cannot be null or empty.", nameof(ragStoresId));
            if (string.IsNullOrEmpty(documentsId)) throw new ArgumentException("Value cannot be null or empty.", nameof(documentsId));
            // this.GuardSupported();

            var url = $"{BaseUrlGoogleAi}/ragStores/{ragStoresId}/documents/{documentsId}";
            var queryStringParams = new Dictionary<string, string?>() { [nameof(force)] = Convert.ToString(force) };

            url = ParseUrl(url).AddQueryString(queryStringParams);
            using var httpRequest = new HttpRequestMessage(HttpMethod.Delete, url);
            var response = await SendAsync(httpRequest, requestOptions, cancellationToken);
            await response.EnsureSuccessAsync(cancellationToken);
#if NET472_OR_GREATER || NETSTANDARD2_0
            return await response.Content.ReadAsStringAsync();
#else
            return await response.Content.ReadAsStringAsync(cancellationToken);
#endif
        }
        
        /// <summary>
        /// Gets information about a specific <see cref="Document"/>.
        /// </summary>
        /// <param name="ragStoresId">Required. The name of the `RagStore` where this `Document` exists. Example: `ragStores/my-rag-store-123`</param>
        /// <param name="documentsId">Required. The name of the `Document`.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="ragStoresId"/> is <see langword="null"/> or empty.</exception>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="documentsId"/> is <see langword="null"/> or empty.</exception>
        /// <exception cref="NotSupportedException">Thrown when the functionality is not supported by the model.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        public async Task<Document> Get(string ragStoresId,
            string documentsId,
            RequestOptions requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(ragStoresId)) throw new ArgumentException("Value cannot be null or empty.", nameof(ragStoresId));
            if (string.IsNullOrEmpty(documentsId)) throw new ArgumentException("Value cannot be null or empty.", nameof(documentsId));
            // this.GuardSupported();

            var url = $"{BaseUrlGoogleAi}/ragStores/{ragStoresId}/documents/{documentsId}";
            url = ParseUrl(url);
            using var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await SendAsync(httpRequest, requestOptions, cancellationToken);
            await response.EnsureSuccessAsync(cancellationToken);
            return await Deserialize<Document>(response);
        }
        
        /// <summary>
        /// Lists all <see cref="Document"/>s in a `RagStores`.
        /// </summary>
        /// <param name="ragStoresId">Required. The name of the `RagStore` where the `Document`s exist. Example: `ragStores/my-rag-store-123`</param>
        /// <param name="pageSize"></param>
        /// <param name="pageToken"></param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="ragStoresId"/> is <see langword="null"/> or empty.</exception>
        /// <exception cref="NotSupportedException">Thrown when the functionality is not supported by the model.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        public async Task<List<Document>> List(string ragStoresId,
            int? pageSize = 50,
            string? pageToken = null,
            RequestOptions requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(ragStoresId)) throw new ArgumentException("Value cannot be null or empty.", nameof(ragStoresId));
            // this.GuardSupported();

            var url = "{BaseUrlGoogleAi}/ragStores/{ragStoresId}/documents";
            var queryStringParams = new Dictionary<string, string?>()
            {
                [nameof(pageSize)] = Convert.ToString(pageSize, CultureInfo.InvariantCulture),
                [nameof(pageToken)] = pageToken
            };

            url = ParseUrl(url).AddQueryString(queryStringParams);
            using var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await SendAsync(httpRequest, requestOptions, cancellationToken);
            await response.EnsureSuccessAsync(cancellationToken);
            var documents = await Deserialize<ListDocumentsResponse>(response);
            return documents.Documents!;
        }

        /// <summary>
        /// Updates a `RagStore`.
        /// </summary>
        /// <param name="ragStoresId">Required. The name of the `RagStore` where the `Document`s exist. Example: `ragStores/my-rag-store-123`</param>
        /// <param name="ragStore"></param>
        /// <param name="updateMask">Optional. The list of fields to update.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="ragStoresId"/> is <see langword="null"/> or empty.</exception>
        public async Task<RagStore> Patch(string ragStoresId,
            RagStore ragStore,
            string? updateMask = null,
            RequestOptions requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(ragStoresId)) throw new ArgumentException("Value cannot be null or empty.", nameof(ragStoresId));

            var url = $"{BaseUrlGoogleAi}/ragStores/{ragStoresId}";
            var queryStringParams = new Dictionary<string, string?>()
            {
                [nameof(updateMask)] = updateMask
            };
            
            url = ParseUrl(url).AddQueryString(queryStringParams);
            var json = Serialize(ragStore);
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
            await response.EnsureSuccessAsync(cancellationToken);
            return await Deserialize<RagStore>(response);
        }

        /// <summary>
        /// Performs semantic search over a <see cref="Document"/>.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="ragStoresId">Required. The name of the `RagStore` where this `Document` exists. Example: `ragStores/my-rag-store-123`</param>
        /// <param name="documentsId">Required. The name of the `Document`.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="ragStoresId"/> is <see langword="null"/> or empty.</exception>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="documentsId"/> is <see langword="null"/> or empty.</exception>
        /// <exception cref="NotSupportedException">Thrown when the functionality is not supported by the model.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        public async Task<QueryDocumentResponse> Query(QueryDocumentRequest request,
            string ragStoresId,
            string documentsId,
            RequestOptions requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(ragStoresId)) throw new ArgumentException("Value cannot be null or empty.", nameof(ragStoresId));
            if (string.IsNullOrEmpty(documentsId)) throw new ArgumentException("Value cannot be null or empty.", nameof(documentsId));
            // this.GuardSupported();

            var method = Method.Query;
            var url = "{BaseUrlGoogleAi}/ragStores/{ragStoresId}/documents/{documentsId}:{method}";
            return await PostAsync<QueryDocumentRequest, QueryDocumentResponse>(request, url, method, requestOptions, HttpCompletionOption.ResponseContentRead, cancellationToken);
        }
        
        /// <summary>
        /// Gets the latest state of a long-running operation. Clients can use this method to poll the operation result at intervals as recommended by the API service.
        /// </summary>
        /// <param name="ragStoresId">Required. The name of the `RagStore` where this `Operation` exists. Example: `ragStores/my-rag-store-123`</param>
        /// <param name="operationsId">Required. The name of the `Operation`.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Metadata for the given file.</returns>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="ragStoresId"/> is <see langword="null"/> or empty.</exception>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="operationsId"/> is <see langword="null"/> or empty.</exception>
        /// <exception cref="NotSupportedException">Thrown when the functionality is not supported by the model.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        public async Task<Operation> GetOperation(string ragStoresId,
            string operationsId,
            RequestOptions? requestOptions = null, 
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(ragStoresId)) throw new ArgumentException("Value cannot be null or empty.", nameof(ragStoresId));
            if (string.IsNullOrEmpty(operationsId)) throw new ArgumentException("Value cannot be null or empty.", nameof(operationsId));
            // this.GuardSupported();

            var url = $"{BaseUrlGoogleAi}/ragStores/{ragStoresId}/operations/{operationsId}";
            url = ParseUrl(url);
            using var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await SendAsync(httpRequest, requestOptions, cancellationToken);
            await response.EnsureSuccessAsync(cancellationToken);
            return await Deserialize<Operation>(response);
        }

        /// <summary>
        /// Gets the latest state of a long-running operation. Clients can use this method to poll the operation result at intervals as recommended by the API service.
        /// </summary>
        /// <param name="ragStoresId">Required. The name of the `RagStore` where this `Operation` exists. Example: `ragStores/my-rag-store-123`</param>
        /// <param name="operationsId">Required. The name of the `Operation`.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Metadata for the given file.</returns>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="ragStoresId"/> is <see langword="null"/> or empty.</exception>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="operationsId"/> is <see langword="null"/> or empty.</exception>
        /// <exception cref="NotSupportedException">Thrown when the functionality is not supported by the model.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        public async Task<Operation> GetUpload(string ragStoresId,
            string operationsId,
            RequestOptions? requestOptions = null, 
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(ragStoresId)) throw new ArgumentException("Value cannot be null or empty.", nameof(ragStoresId));
            if (string.IsNullOrEmpty(operationsId)) throw new ArgumentException("Value cannot be null or empty.", nameof(operationsId));
            // this.GuardSupported();

            var url = $"{BaseUrlGoogleAi}/ragStores/{ragStoresId}/upload/operations/{operationsId}";
            url = ParseUrl(url);
            using var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await SendAsync(httpRequest, requestOptions, cancellationToken);
            await response.EnsureSuccessAsync(cancellationToken);
            return await Deserialize<Operation>(response);
        }
    }
}