namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Request for an `AsyncBatchEmbedContent` operation.
    /// </summary>
    public class AsyncBatchEmbedContentRequest
    {
        /// <summary>
        /// Required. The batch to create.
        /// </summary>
        /// <returns></returns>
        public EmbedContentBatch Batch { get; set; }
    }
}