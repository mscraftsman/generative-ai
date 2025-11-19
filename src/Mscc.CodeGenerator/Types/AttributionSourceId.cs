namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Identifier for the source contributing to this attribution.
	/// </summary>
	public partial class AttributionSourceId
	{
		/// <summary>
		/// Identifier for an inline passage.
		/// </summary>
		public GroundingPassageId? GroundingPassage { get; set; }
		/// <summary>
		/// Identifier for a <see cref="Chunk"/> fetched via Semantic Retriever.
		/// </summary>
		public SemanticRetrieverChunk? SemanticRetrieverChunk { get; set; }
    }
}
