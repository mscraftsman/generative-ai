namespace Mscc.GenerativeAI
{
    /// <summary>
    /// 
    /// </summary>
    public class OutputOptions
    {
        /// <summary>
        /// Optional. The IANA standard MIME type of the image.
        /// </summary>
        /// <remarks>Values:
        /// image/jpeg
        /// image/png
        /// </remarks>
        public string? MimeType { get; set; }

        /// <summary>
        /// Optional. The compression quality of the output image if encoding in image/jpeg.
        /// </summary>
        public int? CompressionQuality { get; set; }
    }
}