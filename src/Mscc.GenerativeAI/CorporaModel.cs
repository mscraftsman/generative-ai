using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Mscc.GenerativeAI.Types;
using System.Text;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// The `CorporaModel` class provides methods for interacting with a corpus of documents.
    /// </summary>
    public class CorporaModel : BaseModel
    {
        internal override string Version => ApiVersion.V1Beta;

        /// <summary>
        /// Initializes a new instance of the <see cref="CorporaModel"/> class.
        /// </summary>
        public CorporaModel() : this(httpClientFactory: null, logger: null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CorporaModel"/> class.
        /// </summary>
        /// <param name="httpClientFactory">Optional. The <see cref="IHttpClientFactory"/> to use for creating HttpClient instances.</param>
        /// <param name="logger">Optional. Logger instance used for logging</param>
        public CorporaModel(IHttpClientFactory? httpClientFactory = null, ILogger? logger = null) : base(httpClientFactory, logger) { }

        /// <summary>
        /// Creates an empty `Corpus`.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        public async Task<Corpus> Create(Corpus request,
            RequestOptions? requestOptions = null, 
            CancellationToken cancellationToken = default)
        {
            var url = "{BaseUrlGoogleAi}/corpora";
            return await PostAsync<Corpus, Corpus>(request, url, string.Empty, requestOptions, HttpCompletionOption.ResponseContentRead, cancellationToken);
        }

        /// <summary>
        /// Updates a `Corpus`.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="corpus"></param>
        /// <param name="updateMask"></param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        public async Task<Corpus> Update(string name,
            Corpus corpus,
            string? updateMask = null,
            RequestOptions? requestOptions = null, 
            CancellationToken cancellationToken = default)
        {
            var url = $"{BaseUrlGoogleAi}/corpora/{name}"; // v1beta3
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
            await response.EnsureSuccessAsync(cancellationToken);
            return await Deserialize<Corpus>(response);
        }

        /// <summary>
        /// Lists all `Corpora` owned by the user.
        /// </summary>
        /// <param name="pageSize">The maximum number of Corpora to return (per page).</param>
        /// <param name="pageToken">A page token, received from a previous List call. Provide the pageToken returned by one request as an argument to the next request to retrieve the next page.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        public async Task<List<Corpus>> List(int? pageSize = 50,
            string? pageToken = null,
            RequestOptions? requestOptions = null, 
            CancellationToken cancellationToken = default)
        {
            var url = "{BaseUrlGoogleAi}/corpora";
            var queryStringParams = new Dictionary<string, string?>()
            {
                [nameof(pageSize)] = Convert.ToString(pageSize),
                [nameof(pageToken)] = pageToken
            };

            url = ParseUrl(url).AddQueryString(queryStringParams);
            using var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await SendAsync(httpRequest, requestOptions, cancellationToken);
            await response.EnsureSuccessAsync(cancellationToken);
            var corpora = await Deserialize<ListCorporaResponse>(response);
            return corpora.Corpora!;
        }

        /// <summary>
        /// Gets information about a specific `Corpus`.
        /// </summary>
        /// <param name="name">Name od the corpus.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        public async Task<Corpus> Get(string name,
            RequestOptions? requestOptions = null, 
            CancellationToken cancellationToken = default)
        {
            var url = $"{BaseUrlGoogleAi}/corpora/{name}";
            url = ParseUrl(url);
            using var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await SendAsync(httpRequest, requestOptions, cancellationToken);
            await response.EnsureSuccessAsync(cancellationToken);
            return await Deserialize<Corpus>(response);
        }

        /// <summary>
        /// Deletes a `Corpus`.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="force"></param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        public async Task<string> Delete(string name,
            bool force,
            RequestOptions? requestOptions = null, 
            CancellationToken cancellationToken = default)
        {
            var url = $"{BaseUrlGoogleAi}/corpora/{name}";
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
        /// Performs semantic search over a `Corpus`.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        public async Task<CorpusQueryResponse> Query(CorpusQueryRequest request,
            RequestOptions? requestOptions = null, 
            CancellationToken cancellationToken = default)
        {
            var method = Method.Query;
            var url = "{BaseUrlGoogleAi}/corpora/{name}:{method}";
            return await PostAsync<CorpusQueryRequest, CorpusQueryResponse>(request, url, method, requestOptions, HttpCompletionOption.ResponseContentRead, cancellationToken);
        }
    }
}