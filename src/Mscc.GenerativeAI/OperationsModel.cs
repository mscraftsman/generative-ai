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
    public class OperationsModel : BaseModel
    {
        internal override string Version => ApiVersion.V1Beta;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationsModel"/> class.
        /// </summary>
        public OperationsModel() : this(httpClientFactory: null, logger: null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationsModel"/> class.
        /// </summary>
        /// <param name="httpClientFactory">Optional. The <see cref="IHttpClientFactory"/> to use for creating HttpClient instances.</param>
        /// <param name="logger">Optional. Logger instance used for logging</param>
        public OperationsModel(IHttpClientFactory? httpClientFactory = null, ILogger? logger = null) : base(httpClientFactory, logger) { }

        /// <summary>
        /// Gets the latest state of a long-running operation.
        /// Clients can use this method to poll the operation result at intervals as recommended by the API service.
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="token"></param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<TResponse> Get<TRequest, TResponse>(TRequest operation,
            RequestOptions requestOptions = null,
            CancellationToken cancellationToken = default)
            where TRequest : IOperation
            where TResponse : IOperation
        {
            if (operation == null) throw new ArgumentNullException(nameof(operation));
            var url = $"{BaseUrlGoogleAi}/{operation.Name}";
            url = ParseUrl(url);
            using var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await SendAsync(httpRequest, requestOptions, cancellationToken);
            await response.EnsureSuccessAsync(cancellationToken);
            return await Deserialize<TResponse>(response);
        }
    }
}