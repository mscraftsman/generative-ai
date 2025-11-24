namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// A Diff upload request. For details on the Scotty Diff protocol, visit http://go/scotty-diff-protocol.
    /// </summary>
    public partial class DiffUploadRequest
    {
        /// <summary>
        /// The object version of the object that is the base version the
        /// incoming diff script will be applied to. This field will always be filled in.
        /// </summary>
        public string? ObjectVersion { get; set; }
        /// <summary>
        /// The location of the new object.
        /// Agents must clone the object located here, as the upload server
        /// will delete the contents once a response is received.
        /// </summary>
        public CompositeMedia? ObjectInfo { get; set; }
        /// <summary>
        /// The location of the checksums for the new object.
        /// Agents must clone the object located here, as the upload server
        /// will delete the contents once a response is received.
        /// For details on the format of the checksums, see http://go/scotty-diff-protocol.
        /// </summary>
        public CompositeMedia? ChecksumsInfo { get; set; }
    }
}