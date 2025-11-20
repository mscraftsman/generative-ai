using System.Collections.Generic;
using System.Diagnostics;

namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// Response from `ListRagEngineCorpora` containing a paginated list of `RagEngineCorpora`.
    /// </summary>
    public class ListRagCorporaResponse
    {
        /// <summary>
        /// The returned corpora.
        /// </summary>
        public List<RagCorpus> Corpora { get; set; }
        /// <summary>
        /// A token, which can be sent as `page_token` to retrieve the next page.
        /// If this field is omitted, there are no more pages.
        /// </summary>
        public string? NextPageToken { get; set; }
    }
    
    /// <summary>
    /// 
    /// </summary>
    [DebuggerDisplay("{DisplayName} ({Description})")]
    public class RagCorpus
    {
        /// <summary>
        /// 
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// The display name of the RAG corpus.
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// Optional. The description of the RAG corpus.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Optional. The configuration for the Vector DBs.
        /// </summary>
        public RagVectorDbConfig? VectorDbConfig { get; set; }  // BackendConfig
        /// <summary>
        /// Optional. The configuration for the Vertex AI Search.
        /// </summary>
        public VertexAiSearchConfig? VertexAiSearchConfig { get; set; }
    }
}