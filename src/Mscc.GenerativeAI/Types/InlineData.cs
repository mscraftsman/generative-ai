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
        /// Data must be base64 string.
        /// </summary>
        public string Data { get; set; } = default;
        /// <summary>
        /// The IANA standard MIME type of the source data.
        /// The only accepted values: "image/png" or "image/jpeg".
        /// </summary>
        [JsonPropertyName("mime_type")]
        public string MimeType { get; set; } = default;
    }
}
