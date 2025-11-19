namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Content filtering metadata associated with processing a single request. ContentFilter contains a reason and an optional supporting string. The reason may be unspecified.
	/// </summary>
	public partial class ContentFilter
	{
		/// <summary>
		/// A string that describes the filtering behavior in more detail.
		/// </summary>
		public string? Message { get; set; }
		/// <summary>
		/// The reason content was blocked during request processing.
		/// </summary>
		public Reason? Reason { get; set; }
    }
}
