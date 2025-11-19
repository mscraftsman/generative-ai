namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Tool to retrieve public web data for grounding, powered by Google.
	/// </summary>
	public partial class GoogleSearchRetrieval
	{
		/// <summary>
		/// Specifies the dynamic retrieval configuration for the given source.
		/// </summary>
		public DynamicRetrievalConfig? DynamicRetrievalConfig { get; set; }
    }
}
