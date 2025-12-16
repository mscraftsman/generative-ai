namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// A sequence of media data references representing composite data. Introduced to support Bigstore composite objects. For details, visit http://go/bigstore-composites.
    /// </summary>
    public partial class CompositeMedia
    {
        /// <summary>
        /// Media data, set if reference_type is INLINE
        /// </summary>
        public byte[] Inline { get; set; }
        /// <summary>
        /// Path to the data, set if reference_type is PATH
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// Describes what the field reference contains.
        /// </summary>
        public ReferenceType ReferenceType { get; set; }
        /// <summary>
        /// Scotty-provided MD5 hash for an upload.
        /// </summary>
        public string Md5Hash { get; set; }
        /// <summary>
        /// Scotty-provided SHA1 hash for an upload.
        /// </summary>
        public byte Sha1Hash { get; set; }
        /// <summary>
        /// Scotty-provided SHA256 hash for an upload.
        /// </summary>
        public byte Sha256Hash { get; set; }
        /// <summary>
        /// For Scotty Uploads: Scotty-provided hashes for uploads For Scotty Downloads: (WARNING: DO NOT USE WITHOUT PERMISSION FROM THE SCOTTY TEAM.) A Hash provided by the agent to be used to verify the data being downloaded. Currently only supported for inline payloads. Further, only crc32c_hash is currently supported.
        /// </summary>
        public int Crc32cHash { get; set; }
        /// <summary>
        /// Blobstore v1 reference, set if reference_type is BLOBSTORE_REF This should be the byte representation of a blobstore.BlobRef. Since Blobstore is deprecating v1, use blobstore2_info instead. For now, any v2 blob will also be represented in this field as v1 BlobRef.
        /// </summary>
        public byte BlobRef { get; set; }
        /// <summary>
        /// Size of the data, in bytes
        /// </summary>
        public int Length { get; set; }
        /// <summary>
        /// Reference to a TI Blob, set if reference_type is BIGSTORE_REF.
        /// </summary>
        public ObjectId ObjectId { get; set; }
        /// <summary>
        /// A binary data reference for a media download. Serves as a technology-agnostic binary reference in some Google infrastructure. This value is a serialized storage_cosmo.BinaryReference proto. Storing it as bytes is a hack to get around the fact that the cosmo proto (as well as others it includes) doesn't support JavaScript. This prevents us from including the actual type of this field.
        /// </summary>
        public byte CosmoBinaryReference { get; set; }
        /// <summary>
        /// Blobstore v2 info, set if reference_type is BLOBSTORE_REF and it refers to a v2 blob.
        /// </summary>
        public Blobstore2Info Blobstore2Info { get; set; }
    }
}