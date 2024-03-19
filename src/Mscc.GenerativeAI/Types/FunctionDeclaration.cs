namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Structured representation of a function declaration as defined by the OpenAPI 3.03 specification. Included in this declaration are the function name and parameters. This FunctionDeclaration is a representation of a block of code that can be used as a Tool by the model and executed by the client.
    /// </summary>
    public class FunctionDeclaration
    {
        /// <summary>
        /// Required. The name of the function to call.
        /// Must start with a letter or an underscore.
        /// Must be a-z, A-Z, 0-9, or contain underscores and dashes, with a maximum length of 63.
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Required. A brief description of the function.
        /// Description and purpose of the function.
        /// Model uses it to decide how and whether to call the function.
        /// </summary>
        public string? Description { get; set; } = string.Empty;
        /// <summary>
        /// Optional. Describes the parameters to this function. Reflects the Open API 3.03 Parameter Object string Key: the name of the parameter. Parameter names are case sensitive. Schema Value: the Schema defining the type used for the parameter.
        /// For function with no parameters, this can be left unset. Example with 1 required and 1 optional parameter:
        /// type: OBJECT
        /// properties:
        ///   param1:
        ///     type: STRING
        ///   param2:
        ///     type: INTEGER
        ///   required: - 
        /// </summary>
        public Schema? Parameters { get; set; }
        //public Dictionary<string, object> ParametersPython { get; set; }
    }
}
