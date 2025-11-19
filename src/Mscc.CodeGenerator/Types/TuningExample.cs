namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// A single example for tuning.
	/// </summary>
	public partial class TuningExample
	{
		/// <summary>
		/// Required. The expected model output.
		/// </summary>
		public string? Output { get; set; }
		/// <summary>
		/// Optional. Text model input.
		/// </summary>
		public string? TextInput { get; set; }
    }
}
