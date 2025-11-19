namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Backend response for a Diff get checksums response. For details on the Scotty Diff protocol, visit http://go/scotty-diff-protocol.
    /// </summary>
    public class DiffChecksumsResponse
    {
        /// <summary>
        /// The object version of the object the checksums are being returned for.
        /// </summary>
        public string ObjectVersion { get; set; }
        /// <summary>
        /// The total size of the server object.
        /// </summary>
        public int ObjectSizeBytes { get; set; }
        /// <summary>
        /// The chunk size of checksums. Must be a multiple of 256KB.
        /// </summary>
        public int ChunkSizeBytes { get; set; }
        /// <summary>
        /// If set, calculate the checksums based on the contents and return them to the caller.
        /// </summary>
        public CompositeMedia ObjectLocation { get; set; }
        /// <summary>
        /// Exactly one of these fields must be populated. If checksums_location is filled, the server will return the corresponding contents to the user. If object_location is filled, the server will calculate the checksums based on the content there and return that to the user. For details on the format of the checksums, see http://go/scotty-diff-protocol.
        /// </summary>
        public CompositeMedia ChecksumsLocation { get; set; }
    }
}