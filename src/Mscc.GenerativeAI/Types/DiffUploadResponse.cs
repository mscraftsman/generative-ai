namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// Backend response for a Diff upload request. For details on the Scotty Diff protocol, visit http://go/scotty-diff-protocol.
    /// </summary>
    public partial class DiffUploadResponse
    {
        /// <summary>
        /// The object version of the object at the server.
        /// Must be included in the end notification response.
        /// The version in the end notification response must correspond
        /// to the new version of the object that is now stored at the server, after the upload.
        /// </summary>
        public string ObjectVersion { get; set; }
        /// <summary>
        /// The location of the original file for a diff upload request. Must be filled in if responding to an upload start notification.
        /// </summary>
        public CompositeMedia OriginalObject { get; set; }
    }
}