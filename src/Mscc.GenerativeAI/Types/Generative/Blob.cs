using System.Diagnostics;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Raw media bytes. Text should not be sent as raw bytes, use the 'text' field.
    /// </summary>
    [DebuggerDisplay("{MimeType}")]
    public class Blob
    {
        /// <summary>
        /// Raw bytes for media formats.
        /// </summary>
        public byte Data { get; set; }
        /// <summary>
        /// The IANA standard MIME type of the source data.
        /// </summary>
        /// <remarks>
        /// Examples: - image/png - image/jpeg If an unsupported MIME type is provided, an error will be returned.
        /// For a complete list of supported types, see [Supported file formats](https://ai.google.dev/gemini-api/docs/prompting_with_media#supported_file_formats).
        /// </remarks>
        public string MimeType { get; set; }
    }
}