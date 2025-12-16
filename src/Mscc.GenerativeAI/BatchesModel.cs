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
    /// <summary>
    /// 
    /// </summary>
    public sealed class BatchesModel : BaseModel
    {
        internal override string Version => ApiVersion.V1Beta;

        /// <summary>
        /// Initializes a new instance of the <see cref="BatchesModel"/> class.
        /// </summary>
        public BatchesModel() : this(logger: null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BatchesModel"/> class.
        /// </summary>
        /// <param name="httpClientFactory">Optional. The IHttpClientFactory to use for creating HttpClient instances.</param>
        /// <param name="logger">Optional. Logger instance used for logging</param>
        public BatchesModel(IHttpClientFactory? httpClientFactory = null, ILogger? logger = null) : base(httpClientFactory, logger) { }

        /// <summary>
        /// Lists operations that match the specified filter in the request.
        /// If the server doesn't support this method, it returns `UNIMPLEMENTED`.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="filter">The standard list filter.</param>
        /// <param name="pageSize">Optional. The maximum number of cached contents to return. The service may return fewer than this value. If unspecified, some default (under maximum) number of items will be returned. The maximum value is 1000; values above 1000 will be coerced to 1000.</param>
        /// <param name="pageToken">Optional. A page token, received from a previous `ListCachedContents` call. Provide this to retrieve the subsequent page. When paginating, all other parameters provided to `ListCachedContents` must match the call that provided the page token.</param>
        /// <param name="returnPartialSuccess">When set to `true`, operations that are reachable are returned as normal, and those that are unreachable are returned in the [ListOperationsResponse.unreachable] field. This can only be `true` when reading across collections e.g. when `parent` is set to `"projects/example/locations/-"`. This field is not by default supported and will result in an `UNIMPLEMENTED` error if set unless explicitly documented otherwise in service or product specific documentation.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        public async Task<List<Operation>> List(string name, 
            string filter, 
            int? pageSize = 50, 
            string? pageToken = null,
            bool? returnPartialSuccess = false,
            RequestOptions? requestOptions = null, 
            CancellationToken cancellationToken = default)
        {
            var url = $"{BaseUrlGoogleAi}/batches/{name}";
            var queryStringParams = new Dictionary<string, string?>()
            {
                [nameof(filter)] = filter, 
                [nameof(pageSize)] = Convert.ToString(pageSize, CultureInfo.InvariantCulture), 
                [nameof(pageToken)] = pageToken,
                [nameof(returnPartialSuccess)] = Convert.ToString(returnPartialSuccess, CultureInfo.InvariantCulture)
            };

            url = ParseUrl(url).AddQueryString(queryStringParams);
            using var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await SendAsync(httpRequest, requestOptions, cancellationToken);
            await response.EnsureSuccessAsync(cancellationToken);
            var operations = await Deserialize<ListOperationsResponse>(response);
            return operations.Operations;
        }
        
        /// <summary>
        /// Gets the latest state of a long-running operation.
        /// Clients can use this method to poll the operation result at intervals as recommended by the API service.
        /// </summary>
        /// <param name="batchesName">Required. The name of the operation resource. Format: `batches/{id}`</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The long-running operation resource.</returns>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="batchesName"/> is <see langword="null"/> or empty.</exception>
        public async Task<Operation> Get(string batchesName,
            RequestOptions? requestOptions = null, 
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(batchesName)) throw new ArgumentException("Value cannot be null or empty.", nameof(batchesName));

            batchesName = batchesName.SanitizeBatchesName();

            var url = $"{BaseUrlGoogleAi}/{batchesName}";
            url = ParseUrl(url);
            using var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await SendAsync(httpRequest, requestOptions, cancellationToken);
            await response.EnsureSuccessAsync(cancellationToken);
            return await Deserialize<Operation>(response);
        }
        
        /// <summary>
        /// Gets the latest state of a long-running operation.
        /// Clients can use this method to poll the operation result at intervals as recommended by the API service.
        /// </summary>
        /// <param name="batchesName">Required. The name of the operation resource. Format: `batches/{id}`</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The long-running operation resource.</returns>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="batchesName"/> is <see langword="null"/> or empty.</exception>
        public async Task<Operation<TResponse>> Get<TResponse>(string batchesName,
            RequestOptions? requestOptions = null, 
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(batchesName)) throw new ArgumentException("Value cannot be null or empty.", nameof(batchesName));

            batchesName = batchesName.SanitizeBatchesName();

            var url = $"{BaseUrlGoogleAi}/{batchesName}";
            url = ParseUrl(url);
            using var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await SendAsync(httpRequest, requestOptions, cancellationToken);
            await response.EnsureSuccessAsync(cancellationToken);
            return await Deserialize<Operation<TResponse>>(response);
        }
        
        /// <summary>
        /// Starts asynchronous cancellation on a long-running operation.
        /// The server makes a best effort to cancel the operation, but success is not guaranteed.
        /// If the server doesn't support this method, it returns `google.rpc.Code.UNIMPLEMENTED`.
        /// Clients can use Operations.GetOperation or other methods to check whether the cancellation succeeded
        /// or whether the operation completed despite cancellation. On successful cancellation, the operation is
        /// not deleted; instead, it becomes an operation with an Operation.error value with a google.rpc.Status.code of `1`,
        /// corresponding to `Code.CANCELLED`.
        /// </summary>
        /// <param name="batchesName">Required. The name of the operation resource. Format: `batches/{id}`</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>If successful, the response body is empty.</returns>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="batchesName"/> is <see langword="null"/> or empty.</exception>
        public async Task<string> Cancel(string batchesName,
            RequestOptions? requestOptions = null, 
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(batchesName)) throw new ArgumentException("Value cannot be null or empty.", nameof(batchesName));

            batchesName = batchesName.SanitizeBatchesName();

            var url = $"{BaseUrlGoogleAi}/{batchesName}:cancel";
            url = ParseUrl(url);
            // var json = Serialize(request);
            // var payload = new StringContent(json, Encoding.UTF8, Constants.MediaType);
            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);
            // httpRequest.Content = payload;
            var response = await SendAsync(httpRequest, requestOptions, cancellationToken);
            await response.EnsureSuccessAsync(cancellationToken);
#if NET472_OR_GREATER || NETSTANDARD2_0
            return await response.Content.ReadAsStringAsync();
#else
            return await response.Content.ReadAsStringAsync(cancellationToken);
#endif
        }

        /// <summary>
        /// Deletes a long-running operation.
        /// This method indicates that the client is no longer interested in the operation result. It does not cancel the operation. If the server doesn't support this method, it returns `google.rpc.Code.UNIMPLEMENTED`.
        /// </summary>
        /// <param name="batchesName">Required. The name of the operation resource to be deleted. Format: `batches/{id}`</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>If successful, the response body is empty.</returns>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="batchesName"/> is <see langword="null"/> or empty.</exception>
        public async Task<string> Delete(string batchesName,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(batchesName)) throw new ArgumentException("Value cannot be null or empty.", nameof(batchesName));

            batchesName = batchesName.SanitizeBatchesName();

            var url = $"{BaseUrlGoogleAi}/{batchesName}";
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

        /// <summary>
        /// Updates a batch of EmbedContent requests for batch processing.
        /// </summary>
        /// <param name="request">The batch resources to update</param>
        /// <param name="batchesName">Required. The name of the operation resource to be deleted. Format: `batches/{id}`</param>
        /// <param name="updateMask">Optional. The list of fields to update.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="batchesName"/> is <see langword="null"/> or empty.</exception>
        public async Task<EmbedContentBatch> UpdateEmbedContentBatch(EmbedContentBatch request,
            string batchesName,
            string? updateMask = null,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(batchesName)) throw new ArgumentException("Value cannot be null or empty.", nameof(batchesName));

            batchesName = batchesName.SanitizeBatchesName();

            var url = $"{BaseUrlGoogleAi}/{batchesName}:updateEmbedContentBatch";
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
            return await Deserialize<EmbedContentBatch>(response);
        }

        /// <summary>
        /// Updates a batch of GenerateContent requests for batch processing.
        /// </summary>
        /// <param name="request">The batch resources to update</param>
        /// <param name="batchesName">Required. The name of the operation resource to be deleted. Format: `batches/{id}`</param>
        /// <param name="updateMask">Optional. The list of fields to update.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="batchesName"/> is <see langword="null"/> or empty.</exception>
        public async Task<GenerateContentBatch> UpdateGenerateContentBatch(GenerateContentBatch request,
            string batchesName,
            string? updateMask = null,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(batchesName)) throw new ArgumentException("Value cannot be null or empty.", nameof(batchesName));

            batchesName = batchesName.SanitizeBatchesName();

            var url = $"{BaseUrlGoogleAi}/{batchesName}:updateGenerateContentBatch";
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
            return await Deserialize<GenerateContentBatch>(response);
        }
    }
}
