using System;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Defines a retrieval tool that model can call to access external knowledge.
    /// </summary>
    public class Retrieval : ITool
    {
        /// <summary>
        /// Optional. Disable using the result from this tool in detecting grounding attribution.
        /// </summary>
        /// <remarks>This does not affect how the result is given to the model for generation.</remarks>
        [Obsolete("This item is deprecated!")]
        public bool? DisableAttribution { get; set; }
        /// <summary>
        /// Optional. Set to use data source powered by Vertex AI Search.
        /// </summary>
        public VertexAISearch? VertexAiSearch { get; set; }
        /// <summary>
        /// Optional. Set to use a data source powered by Vertex AI RAG store.
        /// </summary>
        public VertexRagStore? VertexRagStore { get; set; }
        /// <summary>
        /// Use data source powered by external API for grounding.
        /// </summary>
        public ExternalApi? ExternalApi { get; set; }
    }
}