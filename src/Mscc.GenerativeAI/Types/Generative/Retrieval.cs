namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Defines a retrieval tool that model can call to access external knowledge.
    /// </summary>
    public class Retrieval
    {
        /// <summary>
        /// Optional. Disable using the result from this tool in detecting grounding attribution.
        /// </summary>
        /// <remarks>This does not affect how the result is given to the model for generation.</remarks>
        public bool? DisableAttribution { get; set; }
        /// <summary>
        /// Optional. Set to use data source powered by Vertex AI Search.
        /// </summary>
        public VertexAISearch VertexAiSearch { get; set; }
    }
}