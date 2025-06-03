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
        /// Optional. Describes the parameters to this function.
        /// </summary>
        /// <remarks>
        /// Reflects the Open API 3.03 Parameter Object string Key: the name of the parameter.
        /// Parameter names are case sensitive. Schema Value: the Schema defining the type used for the parameter.
        /// For function with no parameters, this can be left unset. Example with 1 required and 1 optional parameter:
        /// type: OBJECT
        /// properties:
        ///   param1:
        ///     type: STRING
        ///   param2:
        ///     type: INTEGER
        ///   required: - 
        /// </remarks>
        public Schema? Parameters { get; set; }
        //public Dictionary<string, object> ParametersPython { get; set; }
        /// <summary>
        /// Optional. Describes the output from this function in JSON Schema format.
        /// </summary>
        /// <remarks>
        /// Reflects the Open API 3.03 Response Object. The Schema defines the type used for the response value of the function.
        /// </remarks>
        public Schema? Response { get; set; }
        /// <summary>
        /// Optional. Specifies the function Behavior. Currently only supported by the BidiGenerateContent method.
        /// </summary>
        public BehaviorType? Behavior { get; set; }
        /// <summary>
        /// Optional. Describes the parameters to the function in JSON Schema format.
        /// The schema must describe an object where the properties are the parameters to the function.
        /// For example: ``` { "type": "object", "properties": { "name": { "type": "string" }, "age": { "type": "integer" } }, "additionalProperties": false, "required": ["name", "age"], "propertyOrdering": ["name", "age"] } ```
        /// This field is mutually exclusive with `parameters`.
        /// </summary>
        public string? ParametersJsonSchema { get; set; }
        /// <summary>
        /// Optional. Describes the output from this function in JSON Schema format.
        /// The value specified by the schema is the response value of the function.
        /// This field is mutually exclusive with `response`.
        /// </summary>
        public string? ResponseJsonSchema { get; set; }
    }
}
