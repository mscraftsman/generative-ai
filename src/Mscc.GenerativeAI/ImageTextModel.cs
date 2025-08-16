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
    /// Name of the model that supports image captioning.
    /// <see cref="ImageTextModel"/> generates a caption from an image you provide based on the language that you specify. The model supports the following languages: English (en), German (de), French (fr), Spanish (es) and Italian (it).
    /// </summary>
    public sealed class ImageTextModel : BaseModel
    {
        private const string UrlVertexAi = "{BaseUrlVertexAi}/publishers/{publisher}/{model}:{method}";

        internal override string Version => ApiVersion.V1;
        private string Url => UrlVertexAi;
        private string Method => GenerativeAI.Method.Predict;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageTextModel"/> class.
        /// </summary>
        public ImageTextModel() : this(httpClientFactory: null, logger: null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageTextModel"/> class.
        /// The default constructor attempts to read <c>.env</c> file and environment variables.
        /// Sets default values, if available.
        /// </summary>
        /// <param name="httpClientFactory">Optional. The <see cref="IHttpClientFactory"/> to use for creating HttpClient instances.</param>
        /// <param name="logger">Optional. Logger instance used for logging</param>
        public ImageTextModel(IHttpClientFactory? httpClientFactory = null, ILogger? logger = null) : base(httpClientFactory, logger) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageTextModel"/> class with access to Vertex AI Gemini API.
        /// </summary>
        /// <param name="projectId">Identifier of the Google Cloud project</param>
        /// <param name="region">Region to use</param>
        /// <param name="model">Model to use</param>
        /// <param name="httpClientFactory">Optional. The <see cref="IHttpClientFactory"/> to use for creating HttpClient instances.</param>
        /// <param name="logger">Optional. Logger instance used for logging</param>
        public ImageTextModel(string? projectId = null, string? region = null,
            string? model = null,
            IHttpClientFactory? httpClientFactory = null,
            ILogger? logger = null) : base(projectId, region, model, httpClientFactory, logger) { }

        /// <summary>
        /// Generates images from the specified <see cref="ImageTextRequest"/>.
        /// </summary>
        /// <param name="request">Required. The request to send to the API.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Response from the model for generated images.</returns>
        public async Task<ImageTextResponse> GetCaptions(ImageTextRequest request,
            RequestOptions? requestOptions = null, 
            CancellationToken cancellationToken = default)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var url = ParseUrl(Url, Method);
            return await PostAsync<ImageTextRequest, ImageTextResponse>(request, url, Method, requestOptions, HttpCompletionOption.ResponseContentRead, cancellationToken);
        }

        /// <summary>
        /// Generates a response from the model given an input prompt and other parameters.
        /// </summary>
        /// <param name="base64Image">Required. The base64 encoded image to process.</param>
        /// <param name="numberOfResults">Optional. Number of results to return. Default is 1.</param>
        /// <param name="language">Optional. Language to use. Default is en.</param>
        /// <param name="storageUri">Optional. Cloud Storage uri where to store the generated predictions.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Response from the model for generated content.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="base64Image"/> is <see langword="null"/>.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        /// <exception cref="NotSupportedException">Thrown when the <paramref name="language"/> is not supported by the API.</exception>
        public async Task<ImageTextResponse> GetCaptions(string base64Image,
            int? numberOfResults = null,
            string? language = null,
            string? storageUri = null,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            if (base64Image == null) throw new ArgumentNullException(nameof(base64Image));

            var request = new ImageTextRequest(base64Image, null, numberOfResults, language, storageUri);
            return await GetCaptions(request, requestOptions, cancellationToken);
        }

        /// <summary>
        /// Generates a response from the model given an input prompt and other parameters.
        /// </summary>
        /// <param name="base64Image">Required. The base64 encoded image to process.</param>
        /// <param name="question">Required. The question to ask about the image.</param>
        /// <param name="numberOfResults">Optional. Number of results to return. Default is 1.</param>
        /// <param name="language">Optional. Language to use. Default is en.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Response from the model for generated content.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="base64Image"/> is <see langword="null"/>.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        /// <exception cref="NotSupportedException">Thrown when the <paramref name="language"/> is not supported by the API.</exception>
        public async Task<ImageTextResponse> AskQuestion(string base64Image,
            string question,
            int? numberOfResults = null,
            string? language = null,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            if (base64Image == null) throw new ArgumentNullException(nameof(base64Image));
            if (question == null) throw new ArgumentNullException(nameof(question));

            var request = new ImageTextRequest(base64Image, question, numberOfResults, language);
            return await GetCaptions(request, requestOptions, cancellationToken);
        }
    }
}