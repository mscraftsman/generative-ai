using System.Diagnostics;

namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// A reference to data stored on the filesystem, on GFS or in blobstore.
    /// </summary>
    [DebuggerDisplay("{Filename})")]
    public partial class Media
    {
        /// <summary>
        /// Original file name.
        /// </summary>
        public string Filename { get; set; }
        /// <summary>
        /// Media data, set if reference_type is INLINE
        /// </summary>
        public byte[] Inline { get; set; }
        /// <summary>
        /// A composite media composed of one or more media objects, set if reference_type is COMPOSITE_MEDIA.
        /// The media length field must be set to the sum of the lengths of all composite media objects.
        /// Note: All composite media must have length specified.
        /// </summary>
        public CompositeMedia CompositeMedia { get; set; }
        /// <summary>
        /// Parameters for a media download.
        /// </summary>
        public DownloadParameters DownloadParameters { get; set; }
        /// <summary>
        /// A unique fingerprint/version id for the media data.
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// Extended content type information provided for Scotty uploads.
        /// </summary>
        public ContentTypeInfo ContentTypeInfo { get; set; }
        /// <summary>
        /// Scotty-provided SHA1 hash for an upload.
        /// </summary>
        public byte Sha1Hash { get; set; }
        /// <summary>
        /// Scotty-provided SHA256 hash for an upload.
        /// </summary>
        public byte Sha256Hash { get; set; }
        /// <summary>
        /// Scotty-provided MD5 hash for an upload.
        /// </summary>
        public string Md5Hash { get; set; }
        /// <summary>
        /// For Scotty uploads only.
        /// If a user sends a hash code and the backend has requested that Scotty verify the upload against the client hash, Scotty will perform the check on behalf of the backend and will reject it if the hashes don't match. This is set to true if Scotty performed this verification.
        /// </summary>
        public bool HashVerified { get; set; }
        /// <summary>
        /// MIME type of the data.
        /// </summary>
        public string ContentType { get; set; }
        /// <summary>
        /// Set if reference_type is DIFF_UPLOAD_REQUEST.
        /// </summary>
        public DiffUploadRequest DiffUploadRequest { get; set; }
        /// <summary>
        /// Set if reference_type is DIFF_UPLOAD_RESPONSE.
        /// </summary>
        public DiffUploadResponse DiffUploadResponse { get; set; }
        /// <summary>
        /// Set if reference_type is DIFF_CHECKSUMS_RESPONSE.
        /// </summary>
        public DiffChecksumsResponse DiffChecksumsResponse { get; set; }
        /// <summary>
        /// Set if reference_type is DIFF_VERSION_RESPONSE.
        /// </summary>
        public DiffVersionResponse DiffVersionResponse { get; set; }
        /// <summary>
        /// Set if reference_type is DIFF_DOWNLOAD_RESPONSE.
        /// </summary>
        public DiffDownloadResponse DiffDownloadResponse { get; set; }
        /// <summary>
        /// Deprecated, use one of explicit hash type fields instead.
        /// Algorithm used for calculating the hash.
        /// As of 2011/01/21, \"MD5\" is the only possible value for this field.
        /// New values may be added at any time.
        /// </summary>
        public string Algorithm { get; set; }
        /// <summary>
        /// Describes what the field reference contains.
        /// </summary>
        public ReferenceType ReferenceType { get; set; }
        /// <summary>
        /// Use object_id instead.
        /// </summary>
        public byte BigstoreObjectRef { get; set; }
        /// <summary>
        /// Time at which the media data was last updated, in milliseconds since UNIX epoch
        /// </summary>
        public int Timestamp { get; set; }
        /// <summary>
        /// Path to the data, set if reference_type is PATH
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// Blobstore v2 info, set if reference_type is BLOBSTORE_REF, and it refers to a v2 blob.
        /// </summary>
        public Blobstore2Info Blobstore2Info { get; set; }
        /// <summary>
        /// Deprecated, use one of explicit hash type fields instead. These two hash related fields will only be populated on Scotty based media uploads and will contain the content of the hash group in the NotificationRequest: Hex encoded hash value of the uploaded media.
        /// </summary>
        public string Hash { get; set; }
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
        /// |is_potential_retry| is set false only when Scotty is certain that it has not sent the request before. When a client resumes an upload, this field must be set true in agent calls, because Scotty cannot be certain that it has never sent the request before due to potential failure in the session state persistence.
        /// </summary>
        public bool IsPotentialRetry { get; set; }
        /// <summary>
        /// For Scotty Uploads: Scotty-provided hashes for uploads For Scotty Downloads: (WARNING: DO NOT USE WITHOUT PERMISSION FROM THE SCOTTY TEAM.) A Hash provided by the agent to be used to verify the data being downloaded. Currently only supported for inline payloads. Further, only crc32c_hash is currently supported.
        /// </summary>
        public int Crc32CHash { get; set; }
        /// <summary>
        /// Media id to forward to the operation GetMedia. Can be set if reference_type is GET_MEDIA.
        /// </summary>
        public byte MediaId { get; set; }
        /// <summary>
        /// A binary data reference for a media download. Serves as a technology-agnostic binary reference in some Google infrastructure. This value is a serialized storage_cosmo.BinaryReference proto. Storing it as bytes is a hack to get around the fact that the cosmo proto (as well as others it includes) doesn't support JavaScript. This prevents us from including the actual type of this field.
        /// </summary>
        public byte CosmoBinaryReference { get; set; }
    }
}