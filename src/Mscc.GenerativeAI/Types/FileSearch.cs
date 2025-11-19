using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// The FileSearch tool that retrieves knowledge from Semantic Retrieval corpora.
    /// Files are imported to Semantic Retrieval corpora using the ImportFile API.
    /// </summary>
    public class FileSearch : ITool
    {
        /// <summary>
        /// Optional. The configuration for the retrieval.
        /// </summary>
        public FileSearchRetrievalConfig? RetrievalConfig { get; set; }
        /// <summary>
        /// Required. Semantic retrieval resources to retrieve from. Currently only supports one corpus.
        /// In the future we may open up multiple corpora support.
        /// </summary>
        public List<RetrievalResource> RetrievalResources { get; set; }
        /// <summary>
        /// Required. The names of the file_search_stores to retrieve from.
        /// Example: `fileSearchStores/my-file-search-store-123`
        /// </summary>
        public List<string>? FileSearchStoreNames { get; set; }
        /// <summary>
        /// Optional. Metadata filter to apply to the semantic retrieval documents and chunks.
        /// </summary>
        public string? MetadataFilter { get; set; }
        // /// <summary>
        // /// Optional. The number of semantic retrieval chunks to retrieve.
        // /// </summary>
        // public float? TopK { get; set; } = default;

        /// <summary>
        /// Convenience property.
        /// </summary>
        [JsonIgnore]
        public List<string>? Stores
        {
            get => FileSearchStoreNames;
            set => FileSearchStoreNames = value;
        }
        /// <summary>
        /// Convenience property.
        /// </summary>
        [JsonIgnore]
        public string? Filter
        {
            get => MetadataFilter;
            set => MetadataFilter = value;
        }
    }
}