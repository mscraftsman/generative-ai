namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// A set of tuning examples. Can be training or validation data.
	/// </summary>
	public partial class TuningExamples
	{
		/// <summary>
		/// The examples. Example input can be for text or discuss, but all examples in a set must be of the same type.
		/// </summary>
		public List<TuningExample>? Examples { get; set; }
    }
}
