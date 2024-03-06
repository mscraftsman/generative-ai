using System.Diagnostics;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// A predicted FunctionCall returned from the model that contains a string 
    /// representating the FunctionDeclaration.name with the parameters and their values.
    /// </summary>
    [DebuggerDisplay("{Name}")]
    public class FunctionCall : IPart
    {
        /// <summary>
        /// Required. The name of the function to call.
        /// Matches [FunctionDeclaration.name].
        /// </summary>
        public string Name { get; set; } = string.Empty;
        // Optional. Required. The function parameters and values in JSON object
        // format. See [FunctionDeclaration.parameters] for parameter details.
        // Args map[string] any
        public object? Args { get; set; }
    }
}
