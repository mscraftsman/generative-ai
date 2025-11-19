namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Describes the options to customize dynamic retrieval.
	/// </summary>
	public partial class DynamicRetrievalConfig
	{
		/// <summary>
		/// The threshold to be used in dynamic retrieval. If not set, a system default value is used.
		/// </summary>
		public double? DynamicThreshold { get; set; }
		/// <summary>
		/// The mode of the predictor to be used in dynamic retrieval.
		/// </summary>
		public Mode? Mode { get; set; }
    }
}
