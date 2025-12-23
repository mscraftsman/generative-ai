using System.Diagnostics;

namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// Raw media bytes sent directly in the request. 
    /// Text should not be sent as raw bytes.
    /// </summary>
    /// <remarks>
    /// Serialized bytes data of the image or video.
    /// You can specify at most 1 image with inlineData. To specify up to 16 images, use fileData.
    /// </remarks>
    [DebuggerDisplay("{MimeType}")]
    public partial class InlineData : IPart     // Blob
    {
        /// <summary>
        /// The base64 encoding of the image, PDF, or video to include inline in the prompt.
        /// </summary>
        /// <remarks>
        /// When including media inline, you must also specify MIMETYPE.
        /// Size limit: 20MB
        /// </remarks>
        public string Data { get; set; } = "";
        /// <summary>
        /// The IANA standard MIME type of the source data.
        /// </summary>
        /// <remarks>
        /// The media type of the image, PDF, or video specified in the data or fileUri fields.
        /// Acceptable values include the following: 
        /// "image/png", "image/jpeg", "image/heic", "image/heif", "image/webp".
        /// application/pdf
        /// video/mov
        /// video/mpeg
        /// video/mp4
        /// video/mpg
        /// video/avi
        /// video/wmv
        /// video/mpegps
        /// video/flv
        /// Maximum video length: 2 minutes. No limit on image resolution. 
        ///</remarks>
        public string? MimeType { get; set; } = "";
		/// <summary>
		/// 
		/// </summary>
        public string? DisplayName { get; set; }
    }
}
