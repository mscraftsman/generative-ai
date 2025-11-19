namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// A <see cref="FileSearchStore"/> is a collection of <see cref="Document"/>s.
	/// </summary>
	public partial class FileSearchStore
	{
		/// <summary>
		/// Output only. The number of documents in the <see cref="FileSearchStore"/> that are active and ready for retrieval.
		/// </summary>
		public long? ActiveDocumentsCount { get; set; }
		/// <summary>
		/// Output only. The Timestamp of when the <see cref="FileSearchStore"/> was created.
		/// </summary>
		public DateTime? CreateTime { get; set; }
		/// <summary>
		/// Optional. The human-readable display name for the <see cref="FileSearchStore"/>. The display name must be no more than 512 characters in length, including spaces. Example: "Docs on Semantic Retriever"
		/// </summary>
		public string? DisplayName { get; set; }
		/// <summary>
		/// Output only. The number of documents in the <see cref="FileSearchStore"/> that have failed processing.
		/// </summary>
		public long? FailedDocumentsCount { get; set; }
		/// <summary>
		/// Output only. Immutable. Identifier. The <see cref="FileSearchStore"/> resource name. It is an ID (name excluding the "fileSearchStores/" prefix) that can contain up to 40 characters that are lowercase alphanumeric or dashes (-). It is output only. The unique name will be derived from <see cref="display_name"/> along with a 12 character random suffix. Example: <see cref="fileSearchStores/my-awesome-file-search-store-123a456b789c"/> If <see cref="display_name"/> is not provided, the name will be randomly generated.
		/// </summary>
		public string? Name { get; set; }
		/// <summary>
		/// Output only. The number of documents in the <see cref="FileSearchStore"/> that are being processed.
		/// </summary>
		public long? PendingDocumentsCount { get; set; }
		/// <summary>
		/// Output only. The size of raw bytes ingested into the <see cref="FileSearchStore"/>. This is the total size of all the documents in the <see cref="FileSearchStore"/>.
		/// </summary>
		public long? SizeBytes { get; set; }
		/// <summary>
		/// Output only. The Timestamp of when the <see cref="FileSearchStore"/> was last updated.
		/// </summary>
		public DateTime? UpdateTime { get; set; }
    }
}
