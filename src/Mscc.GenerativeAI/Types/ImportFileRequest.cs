using System.Collections.Generic;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Request for <see cref="ImportFile"/> to import a File API file with a <see cref="FileSearchStore"/>. LINT.IfChange(ImportFileRequest)
    /// </summary>
    public class ImportFileRequest
    {
        /// <summary>
        /// Required. The name of the `File` to import. Example: `files/abc-123`
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// Optional. Config for telling the service how to chunk the file.
        /// If not provided, the service will use default parameters.
        /// </summary>
        public ChunkingConfig? ChunkingConfig { get; set; }
        /// <summary>
        /// Custom metadata to be associated with the file.
        /// </summary>
        public List<CustomMetadata>? CustomMetadata { get; set; }
    }
}