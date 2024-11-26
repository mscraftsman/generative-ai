#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Collections.Generic;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Tokens info with a list of tokens and the corresponding list of token ids.
    /// </summary>
    public class TokensInfo
    {
        /// <summary>
        /// A list of token ids from the input.
        /// </summary>
        public virtual IList<long?> TokenIds { get; set; }
        /// <summary>
        /// A list of tokens from the input.
        /// </summary>
        public virtual IList<string> Tokens { get; set; }
        /// <summary>
        /// Optional. Optional fields for the role from the corresponding Content.
        /// </summary>
        public virtual string? Role { get; set; }
    }
}