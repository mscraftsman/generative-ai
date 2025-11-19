namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Grounding chunk.
	/// </summary>
	public partial class GroundingChunk
	{
		/// <summary>
		/// Optional. Grounding chunk from Google Maps.
		/// </summary>
		public Maps? Maps { get; set; }
		/// <summary>
		/// Optional. Grounding chunk from context retrieved by the file search tool.
		/// </summary>
		public RetrievedContext? RetrievedContext { get; set; }
		/// <summary>
		/// Grounding chunk from the web.
		/// </summary>
		public Web? Web { get; set; }
    }
}
