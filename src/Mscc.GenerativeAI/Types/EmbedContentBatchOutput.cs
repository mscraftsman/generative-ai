namespace Mscc.GenerativeAI
{
    /// <summary>
    /// The output of a batch request.
    /// This is returned in the `AsyncBatchEmbedContentResponse` or the `EmbedContentBatch.output` field.
    /// </summary>
    public class EmbedContentBatchOutput
    {
        /// <summary>
        /// Output only. The responses to the requests in the batch. Returned when the batch was built using inlined requests. The responses will be in the same order as the input requests.
        /// </summary>
        public InlinedEmbedContentResponses? InlinedResponses { get; set; }
        /// <summary>
        /// Output only. The file ID of the file containing the responses.
        /// The file will be a JSONL file with a single response per line.
        /// The responses will be `EmbedContentResponse` messages formatted as JSON.
        /// The responses will be written in the same order as the input requests.
        /// </summary>
        public string? ResponsesFile { get; set; }
    }
}