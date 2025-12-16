using System.Collections.Generic;

namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// Request message for ComputeTokens RPC call.
    /// </summary>
    public partial class ComputeTokensRequest
    {
        /// <summary>
        /// Optional parameters for the request.
        /// </summary>
        public ComputeTokensConfig? Config { get; set; }
    }
}