#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Collections.Generic;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// The requests to be processed in the batch if provided as part of the batch creation request.
    /// </summary>
    public class InlinedEmbedContentRequests
    {
        /// <summary>
        /// Required. The requests to be processed in the batch.
        /// </summary>
        public List<InlinedEmbedContentRequest> Requests { get; set; }
    }
    
    /// <summary>
    /// The request to be processed in the batch.
    /// </summary>
    public class InlinedEmbedContentRequest
    {
        /// <summary>
        /// Required. The request to be processed in the batch.
        /// </summary>
        public EmbedContentRequest Request { get; set; }
        /// <summary>
        /// Optional. The metadata to be associated with the request.
        /// </summary>
        public object? Metadata { get; set; }
    }
}