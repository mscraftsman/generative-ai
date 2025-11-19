namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Request for a <see cref="BatchGenerateContent"/> operation.
	/// </summary>
	public partial class BatchGenerateContentRequest
	{
		/// <summary>
		/// Required. The batch to create.
		/// </summary>
		public GenerateContentBatch? Batch { get; set; }
    }
}
