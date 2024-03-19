using System.Diagnostics;
using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Raw media bytes sent directly in the request. 
    /// Text should not be sent as raw bytes.
    /// </summary>
    [DebuggerDisplay("{MimeType}")]
    public class InlineData : IPart
    {
        /// <summary>
        /// Raw bytes for media formats.
        /// Data must be base64 string.
        /// </summary>
        public string Data { get; set; } = default;
        /// <summary>
        /// The IANA standard MIME type of the source data.
        /// Accepted types include: "image/png", "image/jpeg", "image/heic", "image/heif", "image/webp".
        /// </summary>
        [JsonPropertyName("mime_type")]
        public string MimeType { get; set; } = default;
    }
}
