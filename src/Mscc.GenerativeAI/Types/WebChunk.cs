namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// A `Web` chunk is a piece of evidence that comes from a web page. It contains the URI of the
    /// web page, the title of the page, and the domain of the page. This is used to provide the user
    /// with a link to the source of the information.
    /// </summary>
    public sealed class WebChunk
    {
        /// <summary>
        /// The domain of the web page that contains the evidence. This can be used to filter out
        /// low-quality sources. This field is not supported in Gemini API.
        /// </summary>
        public string? Domain { get; set; }
        /// <summary>
        /// The URI of the web page that contains the evidence.
        /// </summary>
        public string? Uri { get; set; }
        /// <summary>
        /// The title of the web page that contains the evidence.
        /// </summary>
        public string? Title { get; set; }
    }
}