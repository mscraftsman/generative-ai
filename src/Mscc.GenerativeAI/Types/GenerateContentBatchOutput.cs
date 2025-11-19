namespace Mscc.GenerativeAI
{
    /// <summary>
    /// The output of a batch request. This is returned in the BatchGenerateContentResponse or the GenerateContentBatch.output field.
    /// </summary>
    public class GenerateContentBatchOutput
    {
        /// <summary>
        /// Output only. The responses to the requests in the batch.
        /// </summary>
        /// <remarks>
        /// Returned when the batch was built using inlined requests.
        /// The responses will be in the same order as the input requests.
        /// </remarks>
        public InlinedResponses InlinedResponses { get; set; }
        /// <summary>
        /// Output only. The file ID of the file containing the responses.
        /// </summary>
        /// <remarks>
        /// The file will be a JSONL file with a single response per line.
        /// The responses will be GenerateContentResponse messages formatted as JSON. The responses will be written in the same order as the input requests.
        /// </remarks>
        public string ResponsesFile { get; set; }
    }
}