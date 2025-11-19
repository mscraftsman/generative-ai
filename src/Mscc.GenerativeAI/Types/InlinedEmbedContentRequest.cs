using System.Collections.Generic;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// The requests to be processed in the batch if provided as part of the batch creation request.
    /// </summary>
    /// <remarks>
    /// This data type is not supported in Vertex AI.
    /// </remarks>
    public class InlinedEmbedContentRequests
    {
        /// <summary>
        /// Required. The requests to be processed in the batch.
        /// </summary>
        public List<InlinedEmbedContentRequest>? Requests { get; set; }
    }
    
    /// <summary>
    /// The request to be processed in the batch.
    /// </summary>
    /// <remarks>
    /// This data type is not supported in Vertex AI.
    /// </remarks>
    public class InlinedEmbedContentRequest
    {
        /// <summary>
        /// Required. The request to be processed in the batch.
        /// </summary>
        public EmbedContentRequest? Request { get; set; }
        /// <summary>
        /// Optional. The metadata to be associated with the request.
        /// </summary>
        public Dictionary<string, object>? Metadata { get; set; }
    }
}