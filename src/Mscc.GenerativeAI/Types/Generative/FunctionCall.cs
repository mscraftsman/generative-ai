#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Collections.Generic;
#endif
using System.Diagnostics;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// A predicted FunctionCall returned from the model that contains a string 
    /// representing the FunctionDeclaration.name with the parameters and their values.
    /// </summary>
    [DebuggerDisplay("{Name}")]
    public class FunctionCall : IPart
    {
        /// <summary>
        /// Required. The name of the function to call.
        /// Matches [FunctionDeclaration.name].
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Optional. The function parameters and values in JSON object format.
        /// See [FunctionDeclaration.parameters] for parameter details.
        /// </summary>
        public object? Args { get; set; }
        // public virtual IDictionary<string, object>? Args { get; set; }
        /// <summary>
        /// Optional. The unique id of the function call.
        /// If populated, the client to execute the `function_call` and return the response with the matching `id`.
        /// </summary>
        public string? Id { get; set; }
    }
}
