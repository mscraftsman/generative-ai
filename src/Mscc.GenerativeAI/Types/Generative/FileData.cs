using System.Diagnostics;
using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// URI based data.
    /// </summary>
    [DebuggerDisplay("{MimeType} - {FileUri}")]
    public class FileData : IPart
    {
        /// <summary>
        /// The URI of the file in Google Cloud Storage.
        /// URI of the file of the image or video to include in the prompt.
        /// </summary>
        /// <remarks>
        /// The bucket that stores the file must be in the same Google Cloud project that's sending the request. You must also specify MIMETYPE.
        /// Size limit: 20MB
        /// </remarks>
        // [JsonPropertyName("file_uri")]
        public string? FileUri { get; set; }
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
        // [JsonPropertyName("mime_type")]
        public string? MimeType { get; set; }
    }
}
