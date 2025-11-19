namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Context retrieved from a data source to ground the model's response. This is used when a
    /// retrieval tool fetches information from a user-provided corpus or a public dataset. This data
    /// type is not supported in Gemini API.
    /// </summary>
    public class RetrievedContext
    {
        /// <summary>
        /// The content of the retrieved data source.
        /// </summary>
        public string? Text { get; set; }
        /// <summary>
        /// The title of the retrieved data source.
        /// </summary>
        public string? Title { get; set; }
        /// <summary>
        /// The URI of the retrieved data source.
        /// </summary>
        public string? Uri { get; set; }
        /// <summary>
        /// Optional. Name of the <see cref="FileSearchStore"/> containing the document. Example: `fileSearchStores/123`
        /// </summary>
        public string? FileSearchStore { get; set; }
    }
}