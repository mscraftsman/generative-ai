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
        private const string UrlVertexAi =
            "https://{region}-aiplatform.googleapis.com/{version}/projects/{projectId}/locations/{region}/publishers/{publisher}/models/{model}:{method}";

        private string Url => UrlVertexAi;

        private string Method => GenerativeAI.Method.Predict;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageTextModel"/> class.
        /// The default constructor attempts to read <c>.env</c> file and environment variables.
        /// Sets default values, if available.
        /// </summary>
        public ImageTextModel(ILogger? logger = null) : base(logger) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageTextModel"/> class with access to Vertex AI Gemini API.
        /// </summary>
        /// <param name="projectId">Identifier of the Google Cloud project</param>
        /// <param name="region">Region to use</param>
        /// <param name="model">Model to use</param>
        /// <param name="logger">Optional. Logger instance used for logging</param>
        public ImageTextModel(string? projectId = null, string? region = null,
            string? model = null, ILogger? logger = null) : base(projectId, region, model, logger)
        {
        }

        /// <summary>
        /// Generates images from the specified <see cref="ImageTextRequest"/>.
        /// </summary>
        /// <param name="request">Required. The request to send to the API.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Response from the model for generated images.</returns>
        public async Task<ImageTextResponse> GetCaptions(ImageTextRequest request,
            CancellationToken cancellationToken = default)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var url = ParseUrl(Url, Method);
            string json = Serialize(request);
            var payload = new StringContent(json, Encoding.UTF8, Constants.MediaType);
            var response = await Client.PostAsync(url, payload, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await Deserialize<ImageTextResponse>(response);
        }

        /// <summary>
        /// Generates a response from the model given an input prompt and other parameters.
        /// </summary>
        /// <param name="base64Image">Required. The base64 encoded image to process.</param>
        /// <param name="numberOfResults">Optional. Number of results to return. Default is 1.</param>
        /// <param name="language">Optional. Language to use. Default is en.</param>
        /// <param name="storageUri">Optional. Cloud Storage uri where to store the generated predictions.</param>
        /// <returns>Response from the model for generated content.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="base64Image"/> is <see langword="null"/>.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        /// <exception cref="NotSupportedException">Thrown when the <paramref name="language"/> is not supported by the API.</exception>
        public async Task<ImageTextResponse> GetCaptions(string base64Image,
            int? numberOfResults = null,
            string? language = null,
            string? storageUri = null)
        {
            if (base64Image == null) throw new ArgumentNullException(nameof(base64Image));

            var request = new ImageTextRequest(base64Image, null, numberOfResults, language, storageUri);
            return await GetCaptions(request);
        }

        /// <summary>
        /// Generates a response from the model given an input prompt and other parameters.
        /// </summary>
        /// <param name="base64Image">Required. The base64 encoded image to process.</param>
        /// <param name="question">Required. The question to ask about the image.</param>
        /// <param name="numberOfResults">Optional. Number of results to return. Default is 1.</param>
        /// <param name="language">Optional. Language to use. Default is en.</param>
        /// <returns>Response from the model for generated content.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="base64Image"/> is <see langword="null"/>.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        /// <exception cref="NotSupportedException">Thrown when the <paramref name="language"/> is not supported by the API.</exception>
        public async Task<ImageTextResponse> AskQuestion(string base64Image,
            string question,
            int? numberOfResults = null,
            string? language = null)
        {
            if (base64Image == null) throw new ArgumentNullException(nameof(base64Image));
            if (question == null) throw new ArgumentNullException(nameof(question));

            var request = new ImageTextRequest(base64Image, question, numberOfResults, language);
            return await GetCaptions(request);
        }
    }
}