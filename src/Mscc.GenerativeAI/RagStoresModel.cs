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
        /// Gets the latest state of a long-running operation. Clients can use this method to poll the operation result at intervals as recommended by the API service.
        /// </summary>
        /// <param name="ragStoresId"></param>
        /// <param name="operationsId"></param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Metadata for the given file.</returns>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="ragStoresId"/> is <see langword="null"/> or empty.</exception>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="operationsId"/> is <see langword="null"/> or empty.</exception>
        /// <exception cref="NotSupportedException">Thrown when the functionality is not supported by the model.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        public async Task<Operation> Get(string ragStoresId,
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
            await response.EnsureSuccessAsync();
            return await Deserialize<Operation>(response);
        }

        /// <summary>
        /// Gets the latest state of a long-running operation. Clients can use this method to poll the operation result at intervals as recommended by the API service.
        /// </summary>
        /// <param name="ragStoresId"></param>
        /// <param name="operationsId"></param>
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
            await response.EnsureSuccessAsync();
            return await Deserialize<Operation>(response);
        }
    }
}