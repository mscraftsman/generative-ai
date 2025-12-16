namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// Request for image upscaling.
    /// </summary>
    public partial class UpscaleImageRequest : ImageGenerationRequest
    {
        /// <summary>
        /// Configuration for image upscaling.
        /// </summary>
        public UpscaleImageConfig? Config { get; set; }
    }
}