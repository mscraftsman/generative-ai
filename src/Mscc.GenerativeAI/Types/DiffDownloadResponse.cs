namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Backend response for a Diff download response. For details on the Scotty Diff protocol, visit http://go/scotty-diff-protocol.
    /// </summary>
    public class DiffDownloadResponse
    {
        /// <summary>
        /// The original object location.
        /// </summary>
        public CompositeMedia ObjectLocation { get; set; }
    }
}