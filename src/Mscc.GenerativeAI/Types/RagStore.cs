using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Response from `ListRagStores` containing a paginated list of `RagStores`.
    /// The results are sorted by ascending `rag_store.create_time`.
    /// </summary>
    public class ListRagStoresResponse
    {
        /// <summary>
        /// The returned rag_stores.
        /// </summary>
        public List<RagStore> RagStores { get; set; }
        /// <summary>
        /// A token, which can be sent as `page_token` to retrieve the next page. If this field is omitted, there are no more pages.
        /// </summary>
        public string? NextPageToken { get; set; }
    }
    
    /// <summary>
    /// A `RagStore` is a collection of `Document`s.
    /// </summary>
    [DebuggerDisplay("{DisplayName} ({Name})")]
    public class RagStore
    {
        /// <summary>
        /// Output only. Immutable. Identifier. The `RagStore` resource name.
        /// </summary>
        /// <remarks>
        /// It is an ID (name excluding the "ragStores/" prefix) that can contain up to 40 characters that are
        /// lowercase alphanumeric or dashes (-). It is output only. The unique name will be derived from
        /// `display_name` along with a 12 character random suffix. Example: `ragStores/my-awesome-rag-store-123a456b789c`
        /// If `display_name` is not provided, the name will be randomly generated.
        /// </remarks>
        public string Name { get; set; }
        /// <summary>
        /// Optional. The human-readable display name for the `RagStore`. The display name must be no more than 512 characters in length, including spaces. Example: "Docs on Semantic Retriever"
        /// </summary>
        public string? DisplayName { get; set; }
        /// <summary>
        /// Output only. The number of documents in the Ragstore that are active and ready for retrieval.
        /// </summary>
        public int? ActiveDocumentsCount { get; set; }
        /// <summary>
        /// Output only. The number of documents in the Ragstore that have failed processing.
        /// </summary>
        public int? FailedDocumentsCount { get; set; }
        /// <summary>
        /// Output only. The number of documents in the Ragstore that are being processed.
        /// </summary>
        public int? PendingDocumentsCount { get; set; }
        /// <summary>
        /// Output only. The size in bytes of the Ragstore. This is the total size of all the documents in the Ragstore.
        /// </summary>
        public int? SizeBytes { get; set; }
        /// <summary>
        /// Output only. The Timestamp of when the `RagStore` was created.
        /// </summary>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// Output only. The Timestamp of when the `RagStore` was last updated.
        /// </summary>
        public DateTime? UpdateTime { get; set; }
   }
}