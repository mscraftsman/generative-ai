namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Request for image generation.
    /// </summary>
    public class GenerateImagesRequest : ImageGenerationRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenerateImagesRequest"/> class.
        /// </summary>
        /// <param name="prompt">The text prompt guides what images the model generates.</param>
        /// <param name="sampleCount">The number of generated images.</param>
        public GenerateImagesRequest(string prompt, int? sampleCount = 4) : base(prompt, sampleCount) { }
    }
}