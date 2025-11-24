using System.Collections.Generic;

namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// Response for image generation.
    /// </summary>
    public partial class GenerateImagesResponse : ImageGenerationResponse
    {
        /// <summary>
        /// Output only. A list of the generated images.
        /// </summary>
        public List<Image> Images => Predictions;
        /// <summary>
        /// List of generated images.
        /// </summary>
        public List<GeneratedImage>? GeneratedImages { get; set; }
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