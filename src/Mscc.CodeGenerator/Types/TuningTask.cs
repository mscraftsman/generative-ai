namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Tuning tasks that create tuned models.
	/// </summary>
	public partial class TuningTask
	{
		/// <summary>
		/// Output only. The timestamp when tuning this model completed.
		/// </summary>
		public DateTime? CompleteTime { get; set; }
		/// <summary>
		/// Immutable. Hyperparameters controlling the tuning process. If not provided, default values will be used.
		/// </summary>
		public Hyperparameters? Hyperparameters { get; set; }
		/// <summary>
		/// Output only. Metrics collected during tuning.
		/// </summary>
		public List<TuningSnapshot>? Snapshots { get; set; }
		/// <summary>
		/// Output only. The timestamp when tuning this model started.
		/// </summary>
		public DateTime? StartTime { get; set; }
		/// <summary>
		/// Required. Input only. Immutable. The model training data.
		/// </summary>
		public Dataset? TrainingData { get; set; }
    }
}
