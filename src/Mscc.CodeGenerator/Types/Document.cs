namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// A <see cref="Document"/> is a collection of <see cref="Chunk"/>s.
	/// </summary>
	public partial class Document
	{
		/// <summary>
		/// Output only. The Timestamp of when the <see cref="Document"/> was created.
		/// </summary>
		public DateTime? CreateTime { get; set; }
		/// <summary>
		/// Optional. User provided custom metadata stored as key-value pairs used for querying. A <see cref="Document"/> can have a maximum of 20 <see cref="CustomMetadata"/>.
		/// </summary>
		public List<CustomMetadata>? CustomMetadata { get; set; }
		/// <summary>
		/// Optional. The human-readable display name for the <see cref="Document"/>. The display name must be no more than 512 characters in length, including spaces. Example: "Semantic Retriever Documentation"
		/// </summary>
		public string? DisplayName { get; set; }
		/// <summary>
		/// Output only. The mime type of the Document.
		/// </summary>
		public string? MimeType { get; set; }
		/// <summary>
		/// Immutable. Identifier. The <see cref="Document"/> resource name. The ID (name excluding the "fileSearchStores/*/documents/" prefix) can contain up to 40 characters that are lowercase alphanumeric or dashes (-). The ID cannot start or end with a dash. If the name is empty on create, a unique name will be derived from <see cref="display_name"/> along with a 12 character random suffix. Example: <see cref="fileSearchStores/{file_search_store_id}/documents/my-awesome-doc-123a456b789c"/>
		/// </summary>
		public string? Name { get; set; }
		/// <summary>
		/// Output only. The size of raw bytes ingested into the Document.
		/// </summary>
		public long? SizeBytes { get; set; }
		/// <summary>
		/// Output only. Current state of the <see cref="Document"/>.
		/// </summary>
		public State? State { get; set; }
		/// <summary>
		/// Output only. The Timestamp of when the <see cref="Document"/> was last updated.
		/// </summary>
		public DateTime? UpdateTime { get; set; }
    }
}
