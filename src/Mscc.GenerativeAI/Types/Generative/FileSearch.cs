#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Collections.Generic;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// The FileSearch tool that retrieves knowledge from Semantic Retrieval corpora.
    /// Files are imported to Semantic Retrieval corpora using the ImportFile API.
    /// </summary>
    public class FileSearch
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
    }
}