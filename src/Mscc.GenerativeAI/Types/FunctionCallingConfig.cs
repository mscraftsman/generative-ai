#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Collections.Generic;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class FunctionCallingConfig
    {
        /// <summary>
        /// Mode of function calling to define the execution behavior for function calling.
        /// </summary>
        public FunctionCallingMode Mode { get; set; }
        /// <summary>
        /// Optional. List of allowed function names.
        /// </summary>
        public List<string>? AllowedFunctionNames { get; set; }
    }
}