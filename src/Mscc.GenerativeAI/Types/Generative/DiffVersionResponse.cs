namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Backend response for a Diff get version response. For details on the Scotty Diff protocol, visit http://go/scotty-diff-protocol.
    /// </summary>
    public class DiffVersionResponse
    {
        /// <summary>
        /// The object version of the object the checksums are being returned for.
        /// </summary>
        public string ObjectVersion { get; set; }
        /// <summary>
        /// The total size of the server object.
        /// </summary>
        public int ObjectSizeBytes { get; set; }
    }
}