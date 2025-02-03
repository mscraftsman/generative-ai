#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
#endif
using Microsoft.Extensions.Logging;
using System.Text;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Generates an image from the model given an input.
    /// </summary>
    public sealed class ImagesModel : BaseModel
    {
        internal override string Version => ApiVersion.V1Beta;
                
        private string Method => GenerativeAI.Method.Generate;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImagesModel"/> class.
        /// </summary>
        public ImagesModel() : this(logger: null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImagesModel"/> class.
        /// </summary>
        /// <param name="logger">Optional. Logger instance used for logging</param>
        public ImagesModel(ILogger? logger) : base(logger) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<GenerateImagesResponse> Images(GenerateImagesRequest request,
            CancellationToken cancellationToken = default)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var url = $"{BaseUrlGoogleAi}/images";
            url = ParseUrl(url, Method);
            var json = Serialize(request);
            var payload = new StringContent(json, Encoding.UTF8, Constants.MediaType);
            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);
            httpRequest.Content = payload;
            var response = await SendAsync(httpRequest, cancellationToken);
            await response.EnsureSuccessAsync();
            return await Deserialize<GenerateImagesResponse>(response);
        }
    }
}