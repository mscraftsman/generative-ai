using System.Collections.Generic;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Configuration for specifying function calling behavior.
    /// </summary>
    public sealed class FunctionCallingConfig
    {
        /// <summary>
        /// Optional. Specifies the mode in which function calling should execute. If unspecified, the default value will be set to AUTO.
        /// </summary>
        public FunctionCallingConfigMode Mode { get; set; }
        /// <summary>
        /// Optional. A set of function names that, when provided, limits the functions the model will call. This should only be set when the Mode is ANY. Function names should match [FunctionDeclaration.name]. With mode set to ANY, model will predict a function call from the set of function names provided.
        /// </summary>
        public List<string>? AllowedFunctionNames { get; set; }
        /// <summary>
        /// Optional. When set to true, arguments of a single function call will be streamed out in
        /// multiple parts/contents/responses. Partial parameter results will be returned in the
        /// [FunctionCall.partial_args] field.
        /// </summary>
        /// <remarks>
        /// This field is not supported in Gemini API.
        /// </remarks>
        public bool? StreamFunctionCallArguments {get; set;}
    }
}