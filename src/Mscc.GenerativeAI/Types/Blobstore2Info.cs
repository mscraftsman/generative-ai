namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// Information to read/write to blobstore2.
    /// </summary>
    public partial class Blobstore2Info
    {
        /// <summary>
        /// The blob id, e.g., /blobstore/prod/playground/scotty
        /// </summary>
        public string BlobId { get; set; }
        /// <summary>
        /// The blob read token.
        /// Needed to read blobs that have not been replicated. Might not be available until the final call.
        /// </summary>
        public string ReadToken { get; set; }
        /// <summary>
        /// The blob generation id.
        /// </summary>
        public int BlobGeneration { get; set; }
        /// <summary>
        /// Metadata passed from Blobstore -> Scotty for a new GCS upload. This is a signed, serialized blobstore2.BlobMetadataContainer proto which must never be consumed outside of Bigstore, and is not applicable to non-GCS media uploads.
        /// </summary>
        public byte UploadMetadataContainer { get; set; }
        /// <summary>
        /// Read handle passed from Bigstore -> Scotty for a GCS download. This is a signed, serialized blobstore2.ReadHandle proto which must never be set outside of Bigstore, and is not applicable to non-GCS media downloads.
        /// </summary>
        public byte DownloadReadHandle { get; set; }
    }
}