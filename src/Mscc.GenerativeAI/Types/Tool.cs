#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Collections.Generic;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Tool details that the model may use to generate response.
    /// Ref: https://ai.google.dev/api/rest/v1beta/Tool
    /// </summary>
    public class Tool
    {
        /// <summary>
        /// Optional. One or more function declarations to be passed to the model along
        /// with the current user query. Model may decide to call a subset of these
        /// functions by populating [FunctionCall][content.part.function_call] in the
        /// response. User should provide a
        /// [FunctionResponse][content.part.function_response] for each function call
        /// in the next turn. Based on the function responses, Model will generate the
        /// final response back to the user. Maximum 64 function declarations can be
        /// provided.
        /// </summary>
        public List<FunctionDeclaration>? FunctionDeclarations { get; set; }
    }
}
