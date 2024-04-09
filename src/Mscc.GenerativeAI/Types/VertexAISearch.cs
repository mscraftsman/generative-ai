namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Retrieve from Vertex AI Search datastore for grounding.
    /// </summary>
    public class VertexAISearch
    {
        /// <summary>
        /// Fully-qualified Vertex AI Search's datastore resource ID.
        /// </summary>
        /// <remarks>
        /// Format: projects/{project_id}/locations/{location}/collections/default_collection/dataStores/{data_store_id}
        /// See https://cloud.google.com/vertex-ai-search-and-conversation
        /// </remarks>
        public string Datastore { get; set; }
    }
}