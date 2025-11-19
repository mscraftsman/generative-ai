namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Dataset for training or validation.
	/// </summary>
	public partial class Dataset
	{
		/// <summary>
		/// Optional. Inline examples with simple input/output text.
		/// </summary>
		public TuningExamples? Examples { get; set; }
    }
}
