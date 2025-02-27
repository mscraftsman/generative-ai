#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Text.Json.Serialization;
#endif
using System.Diagnostics;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Structured representation of a function declaration as defined by the OpenAPI 3.03 specification. Included in this declaration are the function name and parameters. This FunctionDeclaration is a representation of a block of code that can be used as a Tool by the model and executed by the client.
    /// </summary>
    [DebuggerDisplay("{Name,nq} ({Description,nq})")]
    public sealed class FunctionDeclaration
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
        public string? Description { get; set; }

        /// <summary>
        /// Optional. Describes the parameters to this function.
        /// </summary>
        /// <remarks>
        /// Reflects the Open API 3.03 Parameter Object string Key: the name of the parameter.
        /// Parameter names are case-sensitive. Schema Value: the Schema defining the type used for the parameter.
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
        [JsonConverter(typeof(ResponseSchemaJsonConverter))]
        public object? Response { get; set; }

        public Delegate? Callback { get; set; }

        public FunctionDeclaration() { }

        public FunctionDeclaration(string name, string? description)
        {
            Name = name;
            Description = description;
            Parameters = null;
        }

        public FunctionDeclaration(Delegate callback)
        {
            Name = callback.GetNormalizedName();
            Callback = callback;
        }

        public FunctionDeclaration(string name, Delegate callback)
        {
            Name = name;
            Callback = callback;
        }

        public FunctionDeclaration(string name, string? description, Delegate callback)
        {
            Name = name;
            Description = description;
            Callback = callback;
        }
    }
}