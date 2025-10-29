using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// The responses to the requests in the batch.
    /// </summary>
    public class InlinedResponses
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("inlinedResponses")]
        public List<InlinedResponse> Responses { get; set; }
    }
    
    /// <summary>
    /// The response to a single request in the batch.
    /// </summary>
    public class InlinedResponse
    {
        /// <summary>
        /// Output only. The response to the request. 
        /// </summary>
        public GenerateContentResponse Response { get; set; }
        /// <summary>
        /// Output only. The error encountered while processing the request.
        /// </summary>
        public Status? Error { get; set; }
        /// <summary>
        /// Output only. The metadata associated with the request.
        /// </summary>
        public object? Metadata { get; set; }
    }
}