#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Collections.Generic;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Response for image generation.
    /// </summary>
    public class GenerateImagesResponse : ImageGenerationResponse
    {
        /// <summary>
        /// Output only. A list of the generated images.
        /// </summary>
        public List<Image> Images => Predictions;
        /// <summary>
        /// Output only. Model used to generate the images.
        /// </summary>
        public string Model { get; set; }
        /// <summary>
        /// Output only. Always \"image\", required by the SDK.
        /// </summary>
        public string Object { get; set; }
    }
}