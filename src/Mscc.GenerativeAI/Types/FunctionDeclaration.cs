namespace Mscc.GenerativeAI
{
    public class FunctionDeclaration
    {
        // Required. The name of the function to call.
        // Must start with a letter or an underscore.
        // Must be a-z, A-Z, 0-9, or contain underscores and dashes, with a maximum
        // length of 64.
        public string Name { get; set; } = string.Empty;
        // Optional. Description and purpose of the function.
        // Model uses it to decide how and whether to call the function.
        public string? Description { get; set; } = string.Empty;
        // Optional. Describes the parameters to this function in JSON Schema Object
        // format. Reflects the Open API 3.03 Parameter Object. string Key: the name
        // of the parameter. Parameter names are case sensitive. Schema Value: the
        // Schema defining the type used for the parameter. For function with no
        // parameters, this can be left unset. Example with 1 required and 1 optional
        // parameter: type: OBJECT properties:
        //  param1:
        //    type: STRING
        //  param2:
        //    type: INTEGER
        // required:
        //  - param1
        public Schema? Parameters { get; set; }
        //public Dictionary<string, object> ParametersPython { get; set; }
    }
}
