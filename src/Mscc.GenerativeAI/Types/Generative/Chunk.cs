#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Collections.Generic;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// A `Chunk` is a subpart of a `Document` that is treated as an independent unit for the purposes of vector representation and storage.
    /// </summary>
    public class Chunk
    {
        /// <summary>
        /// Immutable. Identifier. The `Chunk` resource name. The ID (name excluding the \"corpora/*/documents/*/chunks/\" prefix) can contain up to 40 characters that are lowercase alphanumeric or dashes (-). The ID cannot start or end with a dash. If the name is empty on create, a random 12-character unique ID will be generated. Example: `corpora/{corpus_id}/documents/{document_id}/chunks/123a456b789c`
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Required. The content for the `Chunk`, such as the text string. The maximum number of tokens per chunk is 2043.
        /// </summary>
        public ChunkData Data { get; set; }
        /// <summary>
        /// Output only. Current state of the `Chunk`.
        /// </summary>
        public StateChunk? State { get; set; }
        /// <summary>
        /// Output only. The Timestamp of when the `Chunk` was created.
        /// </summary>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// Output only. The Timestamp of when the `Chunk` was last updated.
        /// </summary>
        public DateTime? UpdateTime { get; set; }
        /// <summary>
        /// Optional. User provided custom metadata stored as key-value pairs. The maximum number of `CustomMetadata` per chunk is 20.
        /// </summary>
        public List<CustomMetadata>? CustomMetadata { get; set; }
    }
}