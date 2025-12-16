using System;

namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// Request for image generation.
    /// </summary>
    public partial class GenerateImagesRequest : ImageGenerationRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenerateImagesRequest"/> class.
        /// </summary>
        /// <param name="prompt">The text prompt guides what images the model generates.</param>
        /// <param name="sampleCount">The number of generated images.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="prompt"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the <paramref name="sampleCount"/> is less than 1 or greater than 8.</exception>
        public GenerateImagesRequest(string prompt, int? sampleCount = 4) : base(prompt, sampleCount) { }
    }
}