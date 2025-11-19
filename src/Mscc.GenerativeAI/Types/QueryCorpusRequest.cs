namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Request for querying a <see cref="Corpus"/>.
	/// </summary>
	public partial class QueryCorpusRequest
	{
		/// <summary>
		/// Optional. Filter for <see cref="Chunk"/> and <see cref="Document"/> metadata. Each <see cref="MetadataFilter"/> object should correspond to a unique key. Multiple <see cref="MetadataFilter"/> objects are joined by logical "AND"s. Example query at document level: (year >= 2020 OR year < 2010) AND (genre = drama OR genre = action) <see cref="MetadataFilter"/> object list: metadata_filters = [ {key = "document.custom_metadata.year" conditions = [{int_value = 2020, operation = GREATER_EQUAL}, {int_value = 2010, operation = LESS}]}, {key = "document.custom_metadata.year" conditions = [{int_value = 2020, operation = GREATER_EQUAL}, {int_value = 2010, operation = LESS}]}, {key = "document.custom_metadata.genre" conditions = [{string_value = "drama", operation = EQUAL}, {string_value = "action", operation = EQUAL}]}] Example query at chunk level for a numeric range of values: (year > 2015 AND year <= 2020) <see cref="MetadataFilter"/> object list: metadata_filters = [ {key = "chunk.custom_metadata.year" conditions = [{int_value = 2015, operation = GREATER}]}, {key = "chunk.custom_metadata.year" conditions = [{int_value = 2020, operation = LESS_EQUAL}]}] Note: "AND"s for the same key are only supported for numeric values. String values only support "OR"s for the same key.
		/// </summary>
		public List<MetadataFilter>? MetadataFilters { get; set; }
		/// <summary>
		/// Required. Query string to perform semantic search.
		/// </summary>
		public string? Query { get; set; }
		/// <summary>
		/// Optional. The maximum number of <see cref="Chunk"/>s to return. The service may return fewer <see cref="Chunk"/>s. If unspecified, at most 10 <see cref="Chunk"/>s will be returned. The maximum specified result count is 100.
		/// </summary>
		public int? ResultsCount { get; set; }
    }
}
