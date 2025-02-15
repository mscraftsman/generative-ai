#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Collections.Generic;
using System.Text.Json.Serialization;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Defines a tool that model can call to access external knowledge.
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
        [JsonConverter(typeof(FunctionDeclarationsJsonConverter))]
        public List<FunctionDeclaration>? FunctionDeclarations { get; set; }
        /// <summary>
        /// Optional. Enables the model to execute code as part of generation.
        /// </summary>
        public CodeExecution? CodeExecution { get; set; }
        /// <summary>
        /// Optional. Retrieval tool type. System will always execute the provided retrieval tool(s)
        /// to get external knowledge to answer the prompt. Retrieval results are presented
        /// to the model for generation.
        /// </summary>
        public Retrieval? Retrieval { get; set; }
        /// <summary>
        /// Optional. GoogleSearchRetrieval tool type. Specialized retrieval tool that is powered by Google search.
        /// </summary>
        public GoogleSearchRetrieval? GoogleSearchRetrieval { get; set; }
        /// <summary>
        /// Optional. GoogleSearch tool type. Tool to support Google Search in Model. Powered by Google.
        /// </summary>
        public GoogleSearch? GoogleSearch { get; set; }
    }
}