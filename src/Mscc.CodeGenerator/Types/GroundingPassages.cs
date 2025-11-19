namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// A repeated list of passages.
	/// </summary>
	public partial class GroundingPassages
	{
		/// <summary>
		/// List of passages.
		/// </summary>
		public List<GroundingPassage>? Passages { get; set; }
    }
}
