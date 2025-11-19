namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Configuration for retrieving grounding content from a <see cref="Corpus"/> or <see cref="Document"/> created using the Semantic Retriever API.
	/// </summary>
	public partial class SemanticRetrieverConfig
	{
		/// <summary>
		/// Optional. Maximum number of relevant <see cref="Chunk"/>s to retrieve.
		/// </summary>
		public int? MaxChunksCount { get; set; }
		/// <summary>
		/// Optional. Filters for selecting <see cref="Document"/>s and/or <see cref="Chunk"/>s from the resource.
		/// </summary>
		public List<MetadataFilter>? MetadataFilters { get; set; }
		/// <summary>
		/// Optional. Minimum relevance score for retrieved relevant <see cref="Chunk"/>s.
		/// </summary>
		public double? MinimumRelevanceScore { get; set; }
		/// <summary>
		/// Required. Query to use for matching <see cref="Chunk"/>s in the given resource by similarity.
		/// </summary>
		public Content? Query { get; set; }
		/// <summary>
		/// Required. Name of the resource for retrieval. Example: <see cref="corpora/123"/> or <see cref="corpora/123/documents/abc"/>.
		/// </summary>
		public string? Source { get; set; }
    }
}
