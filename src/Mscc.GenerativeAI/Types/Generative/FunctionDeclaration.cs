#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Text.Json.Serialization;
using System.Threading;
#endif
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

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
        public Schema? Response { get; set; }

        [JsonIgnore]
        public Delegate? Callback { get; set; }

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

        public FunctionDeclaration() { }

        public FunctionDeclaration(string name, string? description)
        {
            Name = SanitizeFunctionName(name);
            Description = description;
            Parameters = null;
        }

        public FunctionDeclaration(Delegate callback)
        {
            Name = GenerateNameForCallback(callback);
            Callback = callback;
            Parameters = Schema.BuildParametersSchemaFromDelegate(callback);
            Response = Schema.BuildResponseSchemaFromDelegate(callback);
        }

        public FunctionDeclaration(string name, Delegate callback)
        {
            Name = SanitizeFunctionName(name);
            Callback = callback;
            Parameters = Schema.BuildParametersSchemaFromDelegate(callback);
            Response = Schema.BuildResponseSchemaFromDelegate(callback);
        }

        public FunctionDeclaration(string name, string? description, Delegate callback)
        {
            Name = SanitizeFunctionName(name);
            Description = description;
            Callback = callback;
            Parameters = Schema.BuildParametersSchemaFromDelegate(callback);
            Response = Schema.BuildResponseSchemaFromDelegate(callback);
        }

        private static string GenerateNameForCallback(Delegate callback)
        {
            MethodInfo? method = callback.Method;

            if (!IsLambda(method))
            {
                // Keep prior behavior for normal methods
                return SanitizeFunctionName(callback.Method.Name);
            }

            // Try to derive a friendly base name
            string baseName = "lambda";
            string methodName = method.Name;
            int start = methodName.IndexOf('<');
            int end = methodName.IndexOf('>');
            if (start >= 0 && end > start)
            {
                string inner = methodName.Substring(start + 1, end - start - 1);
                if (!string.IsNullOrWhiteSpace(inner))
                {
                    baseName = inner;
                }
            }

            // Add a short param summary
            StringBuilder parts = new();
            foreach (ParameterInfo p in method.GetParameters())
            {
                // Skip framework-only params if present
                if (p.ParameterType == typeof(CancellationToken))
                    continue;
                if (parts.Length > 0) parts.Append('_');
                parts.Append(p.Name ?? "arg");
            }

            string candidate = parts.Length > 0 ? $"{baseName}_{parts}" : baseName;

            // Add a small stable suffix to reduce collisions across different lambdas
            try
            {
                int token = method.MetadataToken;
                candidate = $"{candidate}_{(token & 0xFFF):x}"; // 3-hex suffix
            }
            catch
            {
                // ignore
            }

            return SanitizeFunctionName(candidate);
        }

        private static bool IsLambda(MethodInfo method)
        {
            // Lambdas/anonymous/closures are typically compiler-generated and/or have angle-bracket names
            if (method.IsDefined(typeof(CompilerGeneratedAttribute), inherit: true))
                return true;
            if (method.Name.StartsWith("<") && method.Name.Contains(">"))
                return true;
            Type? declaringType = method.DeclaringType;
            if (declaringType != null && declaringType.IsDefined(typeof(CompilerGeneratedAttribute), inherit: true))
                return true;
            return false;
        }

        private static string SanitizeFunctionName(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return "_function";
            }

            // Allowed: letters, digits, underscore, dash. Must start with letter or underscore. Max 63.
            StringBuilder sb = new(name.Length);
            foreach (char ch in name)
            {
                if (char.IsLetterOrDigit(ch) || ch == '_' || ch == '-')
                {
                    sb.Append(ch);
                }
                else
                {
                    sb.Append('_');
                }
            }

            string sanitized = sb.ToString();

            if (sanitized.Length == 0)
                sanitized = "_function";

            if (!char.IsLetter(sanitized[0]) && sanitized[0] != '_')
            {
                sanitized = "_" + sanitized;
            }

            if (sanitized.Length > 63)
            {
                sanitized = sanitized.Substring(0, 63);
            }

            return sanitized;
        }
    }
}