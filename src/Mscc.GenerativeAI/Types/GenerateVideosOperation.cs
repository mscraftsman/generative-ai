namespace Mscc.GenerativeAI
{
    /// <summary>
    /// A video generation operation.\n\nUse the following code to refresh the operation:\n\n```\noperation = client.operations.get(operation)\n```
    /// </summary>
    public class GenerateVideosOperation
    {
        /// <summary>
        /// The server-assigned name, which is only unique within the same service that originally returns it. If you use the default HTTP mapping, the `name` should be a resource name ending with `operations/{unique_id}`.
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// The generated videos.
        /// </summary>
        public GenerateVideosResponse? Result { get; set; }
        /// <summary>
        /// Service-specific metadata associated with the operation. It typically contains progress information and common metadata such as create time. Some services might not provide such metadata.  Any method that returns a long-running operation should document the metadata type, if any.
        /// </summary>
        public object? Metadata { get; set; }
        /// <summary>
        /// If the value is `false`, it means the operation is still in progress. If `true`, the operation is completed, and either `error` or `response` is available.
        /// </summary>
        public bool? Done { get; set; }
        /// <summary>
        /// The error result of the operation in case of failure or cancellation.
        /// </summary>
        public object? Error { get; set; }
        /// <summary>
        /// The normal response of the operation in case of success.
        /// </summary>
        public object? Response { get; set; }
    }
}