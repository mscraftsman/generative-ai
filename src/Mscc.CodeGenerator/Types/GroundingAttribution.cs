namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Attribution for a source that contributed to an answer.
	/// </summary>
	public partial class GroundingAttribution
	{
		/// <summary>
		/// Grounding source content that makes up this attribution.
		/// </summary>
		public Content? Content { get; set; }
		/// <summary>
		/// Output only. Identifier for the source contributing to this attribution.
		/// </summary>
		public AttributionSourceId? SourceId { get; set; }
    }
}
