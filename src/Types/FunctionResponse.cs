namespace Mscc.GenerativeAI
{
    /// <summary>
    /// The result output of a FunctionCall that contains a string 
    /// representing the FunctionDeclaration.name and a structured 
    /// JSON object containing any output from the function call. 
    /// It is used as context to the model.
    /// </summary>
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
        //Response map[string] any
        public object? Response { get; set; }
    }
}
