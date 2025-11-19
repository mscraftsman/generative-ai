namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Configures the input to the batch request.
    /// </summary>
    public class InputConfig
    {
        /// <summary>
        /// The name of the `File` containing the input requests.
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// The requests to be processed in the batch.
        /// </summary>
        public InlinedRequests Requests { get; set; }
    }
}