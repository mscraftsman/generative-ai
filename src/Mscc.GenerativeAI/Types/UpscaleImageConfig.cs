namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// Configuration for upscaling an image.
    /// </summary>
    public partial class UpscaleImageConfig : BaseConfig
    {
        /// <summary>
        /// The level of compression if the output_mime_type is image/jpeg.
        /// </summary>
        public int? OutputCompressionQuality { get; set; }
        /// <summary>
        /// The image format that the output should be saved as.
        /// </summary>
        public string? OutputMimeType { get; set; }
    }
}