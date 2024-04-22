namespace Mscc.GenerativeAI
{
    /// <summary>
    /// 
    /// </summary>
    public class Image
    {
        /// <summary>
        /// A base64 encoded string of one (generated) image. (20 MB)
        /// </summary>
        public string? BytesBase64Encoded { get; set; }

        /// <summary>
        /// The IANA standard MIME type of the image.
        /// </summary>
        public string? MimeType { get; set; }

        /// <summary>
        /// Exists if storageUri is provided. The Cloud Storage uri where the generated images are stored.
        /// </summary>
        public string? GcsUri { get; set; }
    }
}