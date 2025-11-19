namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// A file uploaded to the API. Next ID: 15
	/// </summary>
	public partial class File
	{
		/// <summary>
		/// Output only. The timestamp of when the <see cref="File"/> was created.
		/// </summary>
		public DateTime? CreateTime { get; set; }
		/// <summary>
		/// Optional. The human-readable display name for the <see cref="File"/>. The display name must be no more than 512 characters in length, including spaces. Example: "Welcome Image"
		/// </summary>
		public string? DisplayName { get; set; }
		/// <summary>
		/// Output only. The download uri of the <see cref="File"/>.
		/// </summary>
		public string? DownloadUri { get; set; }
		/// <summary>
		/// Output only. Error status if File processing failed.
		/// </summary>
		public Status? Error { get; set; }
		/// <summary>
		/// Output only. The timestamp of when the <see cref="File"/> will be deleted. Only set if the <see cref="File"/> is scheduled to expire.
		/// </summary>
		public DateTime? ExpirationTime { get; set; }
		/// <summary>
		/// Output only. MIME type of the file.
		/// </summary>
		public string? MimeType { get; set; }
		/// <summary>
		/// Immutable. Identifier. The <see cref="File"/> resource name. The ID (name excluding the "files/" prefix) can contain up to 40 characters that are lowercase alphanumeric or dashes (-). The ID cannot start or end with a dash. If the name is empty on create, a unique name will be generated. Example: <see cref="files/123-456"/>
		/// </summary>
		public string? Name { get; set; }
		/// <summary>
		/// Output only. SHA-256 hash of the uploaded bytes.
		/// </summary>
		public byte[]? Sha256Hash { get; set; }
		/// <summary>
		/// Output only. Size of the file in bytes.
		/// </summary>
		public long? SizeBytes { get; set; }
		/// <summary>
		/// Source of the File.
		/// </summary>
		public Source? Source { get; set; }
		/// <summary>
		/// Output only. Processing state of the File.
		/// </summary>
		public State? State { get; set; }
		/// <summary>
		/// Output only. The timestamp of when the <see cref="File"/> was last updated.
		/// </summary>
		public DateTime? UpdateTime { get; set; }
		/// <summary>
		/// Output only. The uri of the <see cref="File"/>.
		/// </summary>
		public string? Uri { get; set; }
		/// <summary>
		/// Output only. Metadata for a video.
		/// </summary>
		public VideoFileMetadata? VideoMetadata { get; set; }
    }
}
