namespace Mscc.GenerativeAI
{
    /// <summary>
    /// The configuration for the Vertex AI Search.
    /// </summary>
    public class VertexAiSearchConfig
    {
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// Format: projects/{project}/locations/{location}/collections/{collection}/engines/{engine}/servingConfigs/{serving_config}
        ///      or projects/{project}/locations/{location}/collections/{collection}/dataStores/{data_store}/servingConfigs/{serving_config}
        /// </remarks>
        public string ServingConfig { get; set; }
    }
}