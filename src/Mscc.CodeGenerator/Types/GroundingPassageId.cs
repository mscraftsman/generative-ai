namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Identifier for a part within a <see cref="GroundingPassage"/>.
	/// </summary>
	public partial class GroundingPassageId
	{
		/// <summary>
		/// Output only. Index of the part within the <see cref="GenerateAnswerRequest"/>'s <see cref="GroundingPassage.content"/>.
		/// </summary>
		public int? PartIndex { get; set; }
		/// <summary>
		/// Output only. ID of the passage matching the <see cref="GenerateAnswerRequest"/>'s <see cref="GroundingPassage.id"/>.
		/// </summary>
		public string? PassageId { get; set; }
    }
}
