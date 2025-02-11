namespace Mscc.GenerativeAI
{
    /// <summary>
    /// An output image.
    /// </summary>
    public class GeneratedImage
    {
        /// <summary>
        /// The output image data.
        /// </summary>
        public Image? Image { get; set; }
        /// <summary>
        /// Responsible AI filter reason if the image is filtered out of the response.
        /// </summary>
        public string? RaiFilteredReason { get; set; }
        /// <summary>
        /// The rewritten prompt used for the image generation if the prompt enhancer is enabled.
        /// </summary>
        public string? EnhancedPrompt { get; set; }
    }
}