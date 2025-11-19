namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Record for a single tuning step.
	/// </summary>
	public partial class TuningSnapshot
	{
		/// <summary>
		/// Output only. The timestamp when this metric was computed.
		/// </summary>
		public DateTime? ComputeTime { get; set; }
		/// <summary>
		/// Output only. The epoch this step was part of.
		/// </summary>
		public int? Epoch { get; set; }
		/// <summary>
		/// Output only. The mean loss of the training examples for this step.
		/// </summary>
		public double? MeanLoss { get; set; }
		/// <summary>
		/// Output only. The tuning step.
		/// </summary>
		public int? Step { get; set; }
    }
}
