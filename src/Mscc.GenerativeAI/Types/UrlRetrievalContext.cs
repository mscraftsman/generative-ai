namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// Context of the a single url retrieval.
    /// </summary>
    public partial class UrlRetrievalContext
    {
        /// <summary>
        /// Retrieved url by the tool.
        /// </summary>
        public string RetrievedUrl { get; set; }
    }
}