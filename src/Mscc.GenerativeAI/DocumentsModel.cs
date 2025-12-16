using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Mscc.GenerativeAI.Types;
using System.Globalization;
using System.Text;

namespace Mscc.GenerativeAI
{
    public class DocumentsModel : BaseModel
    {
        internal override string Version => ApiVersion.V1Beta;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentsModel"/> class.
        /// </summary>
        public DocumentsModel() : this(httpClientFactory: null, logger: null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentsModel"/> class.
        /// </summary>
        /// <param name="httpClientFactory">Optional. The <see cref="IHttpClientFactory"/> to use for creating HttpClient instances.</param>
        /// <param name="logger">Optional. Logger instance used for logging</param>
        public DocumentsModel(IHttpClientFactory? httpClientFactory = null, ILogger? logger = null) : base(httpClientFactory, logger) { }

        /// <summary>
        /// Creates an empty <see cref="Document"/>.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="parent">Required. The name of the <see cref="FileSearchStore"/> where this <see cref="Document"/> exists. Example: `ragStores/my-rag-store-123`</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="parent"/> is <see langword="null"/> or empty.</exception>
        public async Task<Document> Create(Document? request,
            string parent,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            request ??= new Document();
            if (string.IsNullOrEmpty(parent)) throw new ArgumentException("Value cannot be null or empty.", nameof(parent));
            parent = parent.SanitizeFileSearchStoreName();
            
            var url = $"{BaseUrlGoogleAi}/{parent}/documents";
            return await PostAsync<Document, Document>(request, url, string.Empty, requestOptions, HttpCompletionOption.ResponseContentRead, cancellationToken);
        }

        /// <summary>
        /// Deletes a <see cref="Document"/>.
        /// </summary>
        /// <param name="name">Required. The name of the <see cref="Document"/>.</param>
        /// <param name="parent">Required. The name of the <see cref="FileSearchStore"/> where this <see cref="Document"/> exists. Example: `ragStores/my-rag-store-123`</param>
        /// <param name="force"></param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="parent"/> is <see langword="null"/> or empty.</exception>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="name"/> is <see langword="null"/> or empty.</exception>
        public async Task<string> Delete(string name, 
            string parent,
            bool force = false,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Value cannot be null or empty.", nameof(name));
            if (string.IsNullOrEmpty(parent)) throw new ArgumentException("Value cannot be null or empty.", nameof(parent));
            name = name.SanitizeDocumentName();
            parent = parent.SanitizeFileSearchStoreName();
            
            var url = $"{BaseUrlGoogleAi}/{parent}/{name}";
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
        /// <param name="name">Required. The name of the <see cref="Document"/>.</param>
        /// <param name="parent">Required. The name of the <see cref="FileSearchStore"/> where this <see cref="Document"/> exists. Example: `ragStores/my-rag-store-123`</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="parent"/> is <see langword="null"/> or empty.</exception>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="name"/> is <see langword="null"/> or empty.</exception>
        public async Task<Document> Get(string name, 
            string parent,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Value cannot be null or empty.", nameof(name));
            if (string.IsNullOrEmpty(parent)) throw new ArgumentException("Value cannot be null or empty.", nameof(parent));
            name = name.SanitizeDocumentName();
            parent = parent.SanitizeFileSearchStoreName();

            var url = $"{BaseUrlGoogleAi}/{parent}/{name}";
            url = ParseUrl(url);
            using var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await SendAsync(httpRequest, requestOptions, cancellationToken);
            await response.EnsureSuccessAsync(cancellationToken);
            return await Deserialize<Document>(response);
        }

        /// <summary>
        /// Lists all <see cref="Document"/>s in a <see cref="Corpus"/>.
        /// </summary>
        /// <param name="parent">Required. The name of the <see cref="FileSearchStore"/> where this <see cref="Document"/> exists. Example: `ragStores/my-rag-store-123`</param>
        /// <param name="pageSize">The maximum number of items to return (per page).</param>
        /// <param name="pageToken">A page token, received from a previous List call. Provide the pageToken returned by one request as an argument to the next request to retrieve the next page.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="parent"/> is <see langword="null"/> or empty.</exception>
        public async Task<ListDocumentsResponse> List(string parent, 
            int? pageSize = 10,
            string? pageToken = null,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(parent)) throw new ArgumentException("Value cannot be null or empty.", nameof(parent));

            parent = parent.SanitizeFileSearchStoreName();
            var url = $"{BaseUrlGoogleAi}/{parent}/documents";
            var queryStringParams = new Dictionary<string, string?>()
            {
                [nameof(pageSize)] = Convert.ToString(pageSize, CultureInfo.InvariantCulture),
                [nameof(pageToken)] = pageToken
            };

            url = ParseUrl(url).AddQueryString(queryStringParams);
            using var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await SendAsync(httpRequest, requestOptions, cancellationToken);
            await response.EnsureSuccessAsync(cancellationToken);
            return await Deserialize<ListDocumentsResponse>(response);
        }

        /// <summary>
        /// Performs semantic search over a <see cref="Document"/>.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="name">Required. The name of the <see cref="Document"/>.</param>
        /// <param name="parent">Required. The name of the <see cref="FileSearchStore"/> where this <see cref="Document"/> exists. Example: `ragStores/my-rag-store-123`</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="parent"/> is <see langword="null"/> or empty.</exception>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="name"/> is <see langword="null"/> or empty.</exception>
        /// <exception cref="NotSupportedException">Thrown when the functionality is not supported by the model.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        public async Task<QueryDocumentResponse> Query(QueryDocumentRequest request,
            string name,
            string parent,
            RequestOptions requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Value cannot be null or empty.", nameof(name));
            if (string.IsNullOrEmpty(parent)) throw new ArgumentException("Value cannot be null or empty.", nameof(parent));
            name = name.SanitizeDocumentName();
            parent = parent.SanitizeFileSearchStoreName();
            // this.GuardSupported();

            var method = Method.Query;
            var url = $"{BaseUrlGoogleAi}/{parent}/{name}:{method}";
            return await PostAsync<QueryDocumentRequest, QueryDocumentResponse>(request, url, method, requestOptions, HttpCompletionOption.ResponseContentRead, cancellationToken);
        }

        /// <summary>
        /// Updates a <see cref="Document"/>.
        /// </summary>
        /// <param name="corpus"></param>
        /// <param name="name">Required. The name of the <see cref="Document"/>.</param>
        /// <param name="parent">Required. The name of the <see cref="Corpus"/> where this <see cref="Document"/> exists. Example: `ragStores/my-rag-store-123`</param>
        /// <param name="updateMask"></param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        public async Task<Document> Patch(Document corpus,
            string name,
            string parent,
            string? updateMask = null,
            RequestOptions? requestOptions = null, 
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Value cannot be null or empty.", nameof(name));
            if (string.IsNullOrEmpty(parent)) throw new ArgumentException("Value cannot be null or empty.", nameof(parent));
            name = name.SanitizeDocumentName();
            parent = parent.SanitizeCorporaName();

            var url = $"{BaseUrlGoogleAi}/{parent}/{name}"; // v1beta3
            var queryStringParams = new Dictionary<string, string?>() { [nameof(updateMask)] = updateMask };

            url = ParseUrl(url).AddQueryString(queryStringParams);
            var json = Serialize(corpus);
            var payload = new StringContent(json, Encoding.UTF8, Constants.MediaType);
#if NET472_OR_GREATER || NETSTANDARD2_0
            var method = new HttpMethod("PATCH");
#else
            var method = HttpMethod.Patch;
#endif
            using var httpRequest = new HttpRequestMessage(method, url);
            httpRequest.Version = _httpVersion;
            httpRequest.Content = payload;
            var response = await SendAsync(httpRequest, requestOptions, cancellationToken);
            await response.EnsureSuccessAsync(cancellationToken);
            return await Deserialize<Document>(response);
        }
    }
}