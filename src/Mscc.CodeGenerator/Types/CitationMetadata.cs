namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// A collection of source attributions for a piece of content.
	/// </summary>
	public partial class CitationMetadata
	{
		/// <summary>
		/// Citations to sources for a specific response.
		/// </summary>
		public List<CitationSource>? CitationSources { get; set; }
    }
}
