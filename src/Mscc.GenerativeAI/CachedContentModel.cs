using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Mscc.GenerativeAI.Types;
using System.Text;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Content that has been preprocessed and can be used in subsequent request to GenerativeService.
    /// Cached content can be only used with model it was created for.
    /// </summary>
    public sealed class CachedContentModel : BaseModel
    {
        internal override string Version => ApiVersion.V1Beta;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="CachedContentModel"/> class.
        /// </summary>
        public CachedContentModel() : this(logger: null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CachedContentModel"/> class.
        /// </summary>
        /// <param name="httpClientFactory">Optional. The IHttpClientFactory to use for creating HttpClient instances.</param>
        /// <param name="logger">Optional. Logger instance used for logging</param>
        public CachedContentModel(IHttpClientFactory? httpClientFactory = null, ILogger? logger = null) : base(httpClientFactory, logger) { }

        /// <summary>
        /// Creates CachedContent resource.
        /// </summary>
        /// <param name="request">The cached content resource to create.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The cached content resource created</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="request"/> is <see langword="null"/>.</exception>
        public async Task<CachedContent> Create(CachedContent request,
            RequestOptions? requestOptions = null, 
            CancellationToken cancellationToken = default)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var url = $"{BaseUrlGoogleAi}/cachedContents";
            return await PostAsync<CachedContent, CachedContent>(request, url, string.Empty, requestOptions, HttpCompletionOption.ResponseContentRead, cancellationToken);
        }

        /// <summary>
        /// Creates CachedContent resource.
        /// </summary>
        /// <remarks>The minimum input token count for context caching is 32,768, and the maximum is the same as the maximum for the given model.</remarks>
        /// <param name="model">Required. The name of the `Model` to use for cached content Format: `models/{model}`</param>
        /// <param name="displayName">Optional. The user-generated meaningful display name of the cached content. Maximum 128 Unicode characters.</param>
        /// <param name="systemInstruction">Optional. Input only. Developer set system instruction. Currently, text only.</param>
        /// <param name="contents">Optional. Input only. The content to cache.</param>
        /// <param name="history">Optional. A chat history to initialize the session with.</param>
        /// <param name="ttl">Optional. Input only. New TTL for this resource, input only. A duration in seconds with up to nine fractional digits, ending with 's'</param>
        /// <param name="expireTime">Optional. Timestamp in UTC of when this resource is considered expired. This is always provided on output, regardless of what was sent on input.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The created cached content resource.</returns>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="model"/> is <see langword="null"/> or empty.</exception>
        public async Task<CachedContent> Create(string model,
            string? displayName = null, 
            Content? systemInstruction = null,
            List<Content>? contents = null,
            List<ContentResponse>? history = null,
            TimeSpan? ttl = null,
            DateTime? expireTime = null,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(model)) throw new ArgumentException("Value cannot be null or empty.", nameof(model));

            var request = new CachedContent()
            {
                Model = model,
                DisplayName = displayName,
                SystemInstruction = systemInstruction,
                Contents = contents ?? history?.Select(x =>
                    new Content { Role = x.Role, PartTypes = x.Parts }
                ).ToList(),
                Ttl = ttl ?? TimeSpan.FromMinutes(5),
                ExpireTime = expireTime
            };
            return await Create(request, requestOptions, cancellationToken);
        }

        /// <summary>
        /// Lists CachedContents resources.
        /// </summary>
        /// <param name="pageSize">Optional. The maximum number of cached contents to return. The service may return fewer than this value. If unspecified, some default (under maximum) number of items will be returned. The maximum value is 1000; values above 1000 will be coerced to 1000.</param>
        /// <param name="pageToken">Optional. A page token, received from a previous `ListCachedContents` call. Provide this to retrieve the subsequent page. When paginating, all other parameters provided to `ListCachedContents` must match the call that provided the page token.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        public async Task<List<CachedContent>> List(int? pageSize = 50, 
            string? pageToken = null,
            RequestOptions? requestOptions = null, 
            CancellationToken cancellationToken = default)
        {
            var url = $"{BaseUrlGoogleAi}/cachedContents";
            var queryStringParams = new Dictionary<string, string?>()
            {
                [nameof(pageSize)] = Convert.ToString(pageSize), 
                [nameof(pageToken)] = pageToken
            };

            url = ParseUrl(url).AddQueryString(queryStringParams);
            using var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await SendAsync(httpRequest, requestOptions, cancellationToken);
            await response.EnsureSuccessAsync(cancellationToken);
            var cachedContents = await Deserialize<ListCachedContentsResponse>(response);
            return cachedContents.CachedContents;
        }
        
        /// <summary>
        /// Reads CachedContent resource.
        /// </summary>
        /// <param name="cachedContentName">Required. The resource name referring to the content cache entry. Format: `cachedContents/{id}`</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The cached content resource.</returns>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="cachedContentName"/> is <see langword="null"/> or empty.</exception>
        public async Task<CachedContent> Get(string cachedContentName,
            RequestOptions? requestOptions = null, 
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(cachedContentName)) throw new ArgumentException("Value cannot be null or empty.", nameof(cachedContentName));

            cachedContentName = cachedContentName.SanitizeCachedContentName();

            var url = $"{BaseUrlGoogleAi}/{cachedContentName}";
            url = ParseUrl(url);
            using var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await SendAsync(httpRequest, requestOptions, cancellationToken);
            await response.EnsureSuccessAsync(cancellationToken);
            return await Deserialize<CachedContent>(response);
        }

        /// <summary>
        /// Updates CachedContent resource (only expiration is updatable).
        /// </summary>
        /// <param name="request">The cached content resource to update.</param>
        /// <param name="ttl">Optional. Input only. New TTL for this resource, input only. A duration in seconds with up to nine fractional digits, ending with 's'</param>
        /// <param name="updateMask">Optional. The list of fields to update.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The updated cached content resource.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="request"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="request.Name"/> is <see langword="null"/> or empty.</exception>
        public async Task<CachedContent> Update(CachedContent request, 
            TimeSpan ttl, 
            string? updateMask = null,
            RequestOptions? requestOptions = null, 
            CancellationToken cancellationToken = default)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (string.IsNullOrEmpty(request.Name)) throw new ArgumentException("Value cannot be null or empty.", nameof(request.Name));

            var url = $"{BaseUrlGoogleAi}/{request.Name}";
            var queryStringParams = new Dictionary<string, string?>()
            {
                [nameof(updateMask)] = updateMask
            };
            
            url = ParseUrl(url).AddQueryString(queryStringParams);
            var json = Serialize(request);
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
            return await Deserialize<CachedContent>(response);
        }

        /// <summary>
        /// Deletes CachedContent resource.
        /// </summary>
        /// <param name="cachedContentName">Required. The resource name referring to the content cache entry. Format: `cachedContents/{id}`</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>If successful, the response body is empty.</returns>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="cachedContentName"/> is <see langword="null"/> or empty.</exception>
        public async Task<string> Delete(string cachedContentName,
            RequestOptions? requestOptions = null, 
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(cachedContentName)) throw new ArgumentException("Value cannot be null or empty.", nameof(cachedContentName));

            cachedContentName = cachedContentName.SanitizeCachedContentName();

            var url = $"{BaseUrlGoogleAi}/{cachedContentName}";
            url = ParseUrl(url);
            using var httpRequest = new HttpRequestMessage(HttpMethod.Delete, url);
            var response = await SendAsync(httpRequest, requestOptions, cancellationToken);
            await response.EnsureSuccessAsync(cancellationToken);
#if NET472_OR_GREATER || NETSTANDARD2_0
            return await response.Content.ReadAsStringAsync();
#else
            return await response.Content.ReadAsStringAsync(cancellationToken);
#endif
        }
    }
}