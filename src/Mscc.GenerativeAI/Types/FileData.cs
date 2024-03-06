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
        /// The IANA standard MIME type of the source data.
        /// </summary>
        [JsonPropertyName("mime_type")]
        public string MimeType { get; set; } = string.Empty;
        /// <summary>
        /// URI of the file.
        /// </summary>
        [JsonPropertyName("file_uri")]
        public string FileUri { get; set; } = string.Empty;
    }
}
