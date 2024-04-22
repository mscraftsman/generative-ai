#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
#endif
using System.Text;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Name of the model that supports image captioning.
    /// <see cref="ImageTextModel"/> generates a caption from an image you provide based on the language that you specify. The model supports the following languages: English (en), German (de), French (fr), Spanish (es) and Italian (it).
    /// </summary>
    public class ImageTextModel : BaseGeneration
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
        public ImageTextModel() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageTextModel"/> class with access to Vertex AI Gemini API.
        /// </summary>
        /// <param name="projectId">Identifier of the Google Cloud project</param>
        /// <param name="region">Region to use</param>
        /// <param name="model">Model to use</param>
        public ImageTextModel(string? projectId = null, string? region = null,
            string? model = null) : base(projectId, region, model)
        {
        }

        /// <summary>
        /// Generates images from the specified <see cref="ImageTextRequest"/>.
        /// </summary>
        /// <param name="request">Required. The request to send to the API.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Response from the model for generated images.</returns>
        public async Task<ImageTextResponse> Predict(ImageTextRequest request,
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
        /// <param name="prompt">Required. String to process.</param>
        /// <returns>Response from the model for generated content.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="prompt"/> is <see langword="null"/>.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        public async Task<ImageTextResponse> Predict(string prompt)
        {
            if (prompt == null) throw new ArgumentNullException(nameof(prompt));

            var request = new ImageTextRequest(prompt);
            return await Predict(request);
        }

        /// <summary>
        /// Generates a response from the model given an input prompt and other parameters.
        /// </summary>
        /// <param name="prompt">Required. String to process.</param>
        /// <returns>Response from the model for generated content.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="prompt"/> is <see langword="null"/>.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        public async Task<ImageTextResponse> GenerateContent(string prompt)
        {
            return await Predict(prompt);
        }
    }
}