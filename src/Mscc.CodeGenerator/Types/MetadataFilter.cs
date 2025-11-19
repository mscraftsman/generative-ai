namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// User provided filter to limit retrieval based on <see cref="Chunk"/> or <see cref="Document"/> level metadata values. Example (genre = drama OR genre = action): key = "document.custom_metadata.genre" conditions = [{string_value = "drama", operation = EQUAL}, {string_value = "action", operation = EQUAL}]
	/// </summary>
	public partial class MetadataFilter
	{
		/// <summary>
		/// Required. The <see cref="Condition"/>s for the given key that will trigger this filter. Multiple <see cref="Condition"/>s are joined by logical ORs.
		/// </summary>
		public List<Condition>? Conditions { get; set; }
		/// <summary>
		/// Required. The key of the metadata to filter on.
		/// </summary>
		public string? Key { get; set; }
    }
}
