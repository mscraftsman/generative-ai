#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Collections.Generic;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Response message for ComputeTokens RPC call.
    /// </summary>
    public class ComputeTokensResponse
    {
        /// <summary>
        /// Lists of tokens info from the input.
        /// </summary>
        /// <remarks>
        /// A ComputeTokensRequest could have multiple instances with a prompt in each instance.
        /// We also need to return lists of tokens info for the request with multiple instances.
        /// </remarks>
        public virtual IList<TokensInfo> TokensInfo { get; set; }
    }
}