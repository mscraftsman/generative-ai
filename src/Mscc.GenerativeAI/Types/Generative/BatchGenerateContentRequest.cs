namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Request for a BatchGenerateContent operation.
    /// </summary>
    public class BatchGenerateContentRequest
    {
        /// <summary>
        /// Required. The batch to create.
        /// </summary>
        public GenerateContentBatch Batch { get; set; }
    }
}