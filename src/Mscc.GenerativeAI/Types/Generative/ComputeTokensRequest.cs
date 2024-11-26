#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Collections.Generic;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Request message for ComputeTokens RPC call.
    /// </summary>
    public class ComputeTokensRequest
    {
        /// <summary>
        /// Optional. The name of the publisher model requested to serve the prediction.
        /// Format: models/{model}.
        /// </summary>
        public string? Model { get; set; }
        /// <summary>
        /// Required. The content of the current conversation with the model.
        /// For single-turn queries, this is a single instance. For multi-turn queries, this is a repeated field that contains conversation history + latest request.
        /// </summary>
        public List<Content> Contents { get; set; }
        /// <summary>
        /// Optional. The instances that are the input to token computing API call.
        /// </summary>
        /// <remarks>
        /// Schema is identical to the prediction schema of the text model, even for the non-text models, like chat models, or Codey models.
        /// </remarks>
        public IEnumerable<object>? Instances { get; set; }
    }
}