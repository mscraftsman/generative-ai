#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Collections.Generic;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// The responses to the requests in the batch.
    /// </summary>
    public class InlinedEmbedContentResponses
    {
        /// <summary>
        /// Output only. The responses to the requests in the batch.
        /// </summary>
        public List<InlinedEmbedContentResponse> InlineResponses { get; set; }
    }
    
    /// <summary>
    /// The response to a single request in the batch.
    /// </summary>
    public class InlinedEmbedContentResponse
    {
        /// <summary>
        /// Output only. The response to the request.
        /// </summary>
        public EmbedContentResponse Response { get; set; }
        /// <summary>
        /// Output only. The error encountered while processing the request.
        /// </summary>
        public Status Error { get; set; }
        /// <summary>
        /// Optional. The metadata associated with the request.
        /// </summary>
        public object? Metadata { get; set; }
    }
}