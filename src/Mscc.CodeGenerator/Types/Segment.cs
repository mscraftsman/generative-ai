namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Segment of the content.
	/// </summary>
	public partial class Segment
	{
		/// <summary>
		/// Output only. End index in the given Part, measured in bytes. Offset from the start of the Part, exclusive, starting at zero.
		/// </summary>
		public int? EndIndex { get; set; }
		/// <summary>
		/// Output only. The index of a Part object within its parent Content object.
		/// </summary>
		public int? PartIndex { get; set; }
		/// <summary>
		/// Output only. Start index in the given Part, measured in bytes. Offset from the start of the Part, inclusive, starting at zero.
		/// </summary>
		public int? StartIndex { get; set; }
		/// <summary>
		/// Output only. The text corresponding to the segment from the response.
		/// </summary>
		public string? Text { get; set; }
    }
}
