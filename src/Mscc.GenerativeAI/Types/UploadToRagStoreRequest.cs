using System.Collections.Generic;

namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// Request for `UploadToRagStore`.
    /// </summary>
    public partial class UploadToRagStoreRequest
    {
        /// <summary>
        /// Optional. Config for telling the service how to chunk the data.
        /// If not provided, the service will use default parameters.
        /// </summary>
        public ChunkingConfig? ChunkingConfig { get; set; }
        /// <summary>
        /// Custom metadata to be associated with the data.
        /// </summary>
        public List<CustomMetadata>? CustomMetadata { get; set; }
        /// <summary>
        /// Optional. Display name of the created document.
        /// </summary>
        public string? DisplayName { get; set; }
        /// <summary>
        /// Optional. MIME type of the data.
        /// If not provided, it will be inferred from the uploaded content.
        /// </summary>
        public string? MimeType { get; set; }
    }
}