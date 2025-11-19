namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Metadata returned to client when grounding is enabled.
	/// </summary>
	public partial class GroundingMetadata
	{
		/// <summary>
		/// Optional. Resource name of the Google Maps widget context token that can be used with the PlacesContextElement widget in order to render contextual data. Only populated in the case that grounding with Google Maps is enabled.
		/// </summary>
		public string? GoogleMapsWidgetContextToken { get; set; }
		/// <summary>
		/// List of supporting references retrieved from specified grounding source.
		/// </summary>
		public List<GroundingChunk>? GroundingChunks { get; set; }
		/// <summary>
		/// List of grounding support.
		/// </summary>
		public List<GroundingSupport>? GroundingSupports { get; set; }
		/// <summary>
		/// Metadata related to retrieval in the grounding flow.
		/// </summary>
		public RetrievalMetadata? RetrievalMetadata { get; set; }
		/// <summary>
		/// Optional. Google search entry for the following-up web searches.
		/// </summary>
		public SearchEntryPoint? SearchEntryPoint { get; set; }
		/// <summary>
		/// Web search queries for the following-up web search.
		/// </summary>
		public List<string>? WebSearchQueries { get; set; }
    }
}
