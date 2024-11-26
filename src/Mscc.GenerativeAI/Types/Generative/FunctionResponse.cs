#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Collections.Generic;
#endif
using System.Diagnostics;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// The result output of a FunctionCall that contains a string 
    /// representing the FunctionDeclaration.name and a structured 
    /// JSON object containing any output from the function call. 
    /// It is used as context to the model.
    /// </summary>
    [DebuggerDisplay("{Name}")]
    public class FunctionResponse : IPart
    {
        /// <summary>
        /// Required. The name of the function to call.
        /// Matches [FunctionDeclaration.name] and [FunctionCall.name].
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Required. The function response in JSON object format.
        /// </summary>
        /// <remarks>
        /// Use "output" key to specify function output and "error" key to specify error details (if any).
        /// If "output" and "error" keys are not specified, then whole "response" is treated as function output.
        /// </remarks>
        //Response map[string] any
        public dynamic? Response { get; set; }
        //public virtual IDictionary<string, object> Response { get; set; }
    }
}
