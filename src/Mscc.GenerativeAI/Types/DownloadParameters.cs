namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// Parameters specific to media downloads.
    /// </summary>
    public partial class DownloadParameters
    {
        /// <summary>
        /// A boolean to be returned in the response to Scotty. Allows/disallows gzip encoding of the payload content when the server thinks it's advantageous (hence, does not guarantee compression) which allows Scotty to GZip the response to the client.
        /// </summary>
        public bool AllowGzipCompression { get; set; }
        /// <summary>
        /// Determining whether or not Apiary should skip the inclusion of any Content-Range header on its response to Scotty.
        /// </summary>
        public bool IgnoreRange { get; set; }
    }
}