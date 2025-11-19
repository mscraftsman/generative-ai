namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// A resource representing a batch of <see cref="GenerateContent"/> requests.
	/// </summary>
	public partial class GenerateContentBatch
	{
		/// <summary>
		/// Output only. Stats about the batch.
		/// </summary>
		public BatchStats? BatchStats { get; set; }
		/// <summary>
		/// Output only. The time at which the batch was created.
		/// </summary>
		public DateTime? CreateTime { get; set; }
		/// <summary>
		/// Required. The user-defined name of this batch.
		/// </summary>
		public string? DisplayName { get; set; }
		/// <summary>
		/// Output only. The time at which the batch processing completed.
		/// </summary>
		public DateTime? EndTime { get; set; }
		/// <summary>
		/// Required. Input configuration of the instances on which batch processing are performed.
		/// </summary>
		public InputConfig? InputConfig { get; set; }
		/// <summary>
		/// Required. The name of the <see cref="Model"/> to use for generating the completion. Format: <see cref="models/{model}"/>.
		/// </summary>
		public string? Model { get; set; }
		/// <summary>
		/// Output only. Identifier. Resource name of the batch. Format: <see cref="batches/{batch_id}"/>.
		/// </summary>
		public string? Name { get; set; }
		/// <summary>
		/// Output only. The output of the batch request.
		/// </summary>
		public GenerateContentBatchOutput? Output { get; set; }
		/// <summary>
		/// Optional. The priority of the batch. Batches with a higher priority value will be processed before batches with a lower priority value. Negative values are allowed. Default is 0.
		/// </summary>
		public long? Priority { get; set; }
		/// <summary>
		/// Output only. The state of the batch.
		/// </summary>
		public State? State { get; set; }
		/// <summary>
		/// Output only. The time at which the batch was last updated.
		/// </summary>
		public DateTime? UpdateTime { get; set; }
    }
}
