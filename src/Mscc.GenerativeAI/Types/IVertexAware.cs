namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// Interface for request objects that need to adapt their structure based on whether Vertex AI is being used.
    /// </summary>
    internal interface IVertexAware
    {
        /// <summary>
        /// Prepares the request object for serialization.
        /// </summary>
        /// <param name="useVertexAi">Flag indicating whether Vertex AI is being used.</param>
        void PrepareForSerialization(bool useVertexAi);
    }
}
