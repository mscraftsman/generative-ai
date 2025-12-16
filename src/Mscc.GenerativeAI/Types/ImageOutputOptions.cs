namespace Mscc.GenerativeAI.Types
{
    public partial class ImageOutputOptions
    {
        /// <summary>
        /// Optional. The compression quality of the output image.
        /// </summary>
        public int? CompressionQuality { get; set; }
        /// <summary>
        /// Optional. The image format that the output should be saved as.
        /// </summary>
        public string? MimeType { get; set; }
    }
}