#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
#endif
using System.Diagnostics;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// A file resource of the File API.
    /// </summary>
    [DebuggerDisplay("{Name} ({MimeType})")]
    public class FileResource
    {
        /// <summary>
        /// Immutable. Identifier. The File resource name.
        /// The ID (name excluding the "files/" prefix) can contain up to 40 characters that are
        /// lowercase alphanumeric or dashes (-). The ID cannot start or end with a dash.
        /// If the name is empty on create, a unique name will be generated. Example: files/123-456
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Optional. The human-readable display name for the File. The display name must be no more than 512 characters in length, including spaces. Example: "Welcome Image"
        /// </summary>
        public string? DisplayName { get; set; }
        /// <summary>
        /// Output only. MIME type of the file.
        /// </summary>
        public string MimeType { get; set; }
        /// <summary>
        /// Output only. Size of the file in bytes.
        /// </summary>
        public long SizeBytes { get; set; }
        /// <summary>
        /// Output only. The timestamp of when the File was created.
        /// A timestamp in RFC3339 UTC "Zulu" format, with nanosecond resolution and up to nine fractional digits. Examples: "2014-10-02T15:01:23Z" and "2014-10-02T15:01:23.045123456Z".
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// Output only. The timestamp of when the File was last updated.
        /// A timestamp in RFC3339 UTC "Zulu" format, with nanosecond resolution and up to nine fractional digits. Examples: "2014-10-02T15:01:23Z" and "2014-10-02T15:01:23.045123456Z".
        /// </summary>
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// Output only. The timestamp of when the File will be deleted. Only set if the File is scheduled to expire.
        /// A timestamp in RFC3339 UTC "Zulu" format, with nanosecond resolution and up to nine fractional digits. Examples: "2014-10-02T15:01:23Z" and "2014-10-02T15:01:23.045123456Z".
        /// </summary>
        public DateTime ExpirationTime { get; set; }
        /// <summary>
        /// Output only. SHA-256 hash of the uploaded bytes. A base64-encoded string.
        /// </summary>
        public string Sha256Hash { get; set; }
        /// <summary>
        /// Output only. The uri of the File.
        /// </summary>
        public string Uri { get; set; }
        /// <summary>
        /// Output only. Processing state of the File.
        /// </summary>
        public FileState? State { get; set; }
    }
}