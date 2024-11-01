namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Google search entry point.
    /// </summary>
    public class SearchEntryPoint
    {
        /// <summary>
        /// Optional. Web content snippet that can be embedded in a web page or an app webview.
        /// </summary>
        public string? RenderedContent { get; set; }
        /// <summary>
        /// Optional. Base64 encoded JSON representing array of tuple.
        /// </summary>
        public byte[]? SdkBlob { get; set; }
    }
}