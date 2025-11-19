namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Passage included inline with a grounding configuration.
	/// </summary>
	public partial class GroundingPassage
	{
		/// <summary>
		/// Content of the passage.
		/// </summary>
		public Content? Content { get; set; }
		/// <summary>
		/// Identifier for the passage for attributing this passage in grounded answers.
		/// </summary>
		public string? Id { get; set; }
    }
}
