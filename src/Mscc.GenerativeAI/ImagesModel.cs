using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Mscc.GenerativeAI.Types;
using System.Text;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Generates an image from the model given an input.
    /// </summary>
    public sealed class ImagesModel : BaseModel
    {
        internal override string Version => ApiVersion.V1Beta;
                
        private string Method => GenerativeAI.Types.Method.Generate;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImagesModel"/> class.
        /// </summary>
        public ImagesModel() : this(httpClientFactory: null, logger: null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImagesModel"/> class.
        /// </summary>
        /// <param name="httpClientFactory">Optional. The <see cref="IHttpClientFactory"/> to use for creating HttpClient instances.</param>
        /// <param name="logger">Optional. Logger instance used for logging</param>
        public ImagesModel(IHttpClientFactory? httpClientFactory = null, ILogger? logger = null) : base(httpClientFactory, logger) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<ImagesGenerationsResponse> Images(ImagesGenerationsRequest request,
            RequestOptions? requestOptions = null, 
            CancellationToken cancellationToken = default)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var url = $"{BaseUrlGoogleAi}/images/generations";
            return await PostAsync<ImagesGenerationsRequest, ImagesGenerationsResponse>(request, url, Method, requestOptions, HttpCompletionOption.ResponseContentRead, cancellationToken);
        }
    }
}