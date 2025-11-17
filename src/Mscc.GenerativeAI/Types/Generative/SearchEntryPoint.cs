namespace Mscc.GenerativeAI
{
    /// <summary>
    /// An entry point for displaying Google Search results. A `SearchEntryPoint` is populated when
    /// the grounding source for a model's response is Google Search. It provides information that you
    /// can use to display the search results in your application.
    /// </summary>
    public class SearchEntryPoint
    {
        /// <summary>
        /// Optional. An HTML snippet that can be embedded in a web page or an application's webview.
        /// This snippet displays a search result, including the title, URL, and a brief description of
        /// the search result.
        /// </summary>
        public string? RenderedContent { get; set; }
        /// <summary>
        /// Optional. A base64-encoded JSON object that contains a list of search queries and their
        /// corresponding search URLs. This information can be used to build a custom search UI.
        /// </summary>
        public byte[]? SdkBlob { get; set; }
    }
}