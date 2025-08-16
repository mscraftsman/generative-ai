#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
#endif
using Microsoft.Extensions.Logging;
using System.Text;

namespace Mscc.GenerativeAI
{
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
        /// <returns></returns>
        public async Task<Corpus> Create(Corpus request,
            CancellationToken cancellationToken = default)
        {
            var url = "{BaseUrlGoogleAi}/corpora";
            return await PostAsync<Corpus, Corpus>(request, url, string.Empty, null, HttpCompletionOption.ResponseContentRead, cancellationToken);
        }

        /// <summary>
        /// Updates a `Corpus`.
        /// </summary>
        /// <returns></returns>
        public async Task<Corpus> Update(string name,
            Corpus corpus,
            string? updateMask = null,
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
            var response = await SendAsync(httpRequest, null, cancellationToken);
            await response.EnsureSuccessAsync();
            return await Deserialize<Corpus>(response);
        }

        /// <summary>
        /// Lists all `Corpora` owned by the user.
        /// </summary>
        /// <returns></returns>
        public async Task<List<Corpus>> List(int? pageSize = 50,
            string? pageToken = null,
            CancellationToken cancellationToken = default)
        {
            var url = "{BaseUrlGoogleAi}/corpora";
            var queryStringParams = new Dictionary<string, string?>()
            {
                [nameof(pageSize)] = Convert.ToString(pageSize), [nameof(pageToken)] = pageToken
            };

            url = ParseUrl(url).AddQueryString(queryStringParams);
            using var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await SendAsync(httpRequest, null, cancellationToken);
            await response.EnsureSuccessAsync();
            var corpora = await Deserialize<ListCorporaResponse>(response);
            return corpora?.Corpora!;
        }

        /// <summary>
        /// Gets information about a specific `Corpus`.
        /// </summary>
        /// <returns></returns>
        public async Task<Corpus> Get(string name,
            CancellationToken cancellationToken = default)
        {
            var url = $"{BaseUrlGoogleAi}/corpora/{name}";
            url = ParseUrl(url);
            using var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await SendAsync(httpRequest, null, cancellationToken);
            await response.EnsureSuccessAsync();
            return await Deserialize<Corpus>(response);
        }

        /// <summary>
        /// Deletes a `Corpus`.
        /// </summary>
        /// <returns></returns>
        public async Task<string> Delete(string name,
            bool force,
            CancellationToken cancellationToken = default)
        {
            var url = $"{BaseUrlGoogleAi}/corpora/{name}";
            var queryStringParams = new Dictionary<string, string?>() { [nameof(force)] = Convert.ToString(force) };

            url = ParseUrl(url).AddQueryString(queryStringParams);
            using var httpRequest = new HttpRequestMessage(HttpMethod.Delete, url);
            var response = await SendAsync(httpRequest, null, cancellationToken);
            await response.EnsureSuccessAsync();
#if NET472_OR_GREATER || NETSTANDARD2_0
            return await response.Content.ReadAsStringAsync();
#else
            return await response.Content.ReadAsStringAsync(cancellationToken);
#endif
        }

        /// <summary>
        /// Performs semantic search over a `Corpus`.
        /// </summary>
        /// <returns></returns>
        public async Task<CorpusQueryResponse> Query(CorpusQueryRequest request,
            CancellationToken cancellationToken = default)
        {
            var method = Method.Query;
            var url = "{BaseUrlGoogleAi}/corpora/{name}:{method}";
            return await PostAsync<CorpusQueryRequest, CorpusQueryResponse>(request, url, method, null, HttpCompletionOption.ResponseContentRead, cancellationToken);
        }
    }
}