#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Collections.Generic;
#endif
using System.Diagnostics;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// The result output of a FunctionCall that contains a string 
    /// representing the FunctionDeclaration.name and a structured 
    /// JSON object containing any output from the function call. 
    /// It is used as context to the model.
    /// </summary>
    [DebuggerDisplay("{Name}")]
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
        /// <remarks>
        /// Use "output" key to specify function output and "error" key to specify error details (if any).
        /// If "output" and "error" keys are not specified, then whole "response" is treated as function output.
        /// </remarks>
        //Response map[string] any
        public object? Response { get; set; }

        //public virtual IDictionary<string, object> Response { get; set; }
        /// <summary>
        /// Optional. The id of the function call this response is for.
        /// Populated by the client to match the corresponding function call `id`.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Optional. Specifies how the response should be scheduled in the conversation.
        /// </summary>
        /// <remarks>
        /// Only applicable to NON_BLOCKING function calls, is ignored otherwise. Defaults to WHEN_IDLE.
        /// </remarks>
        public SchedulingType? Scheduling { get; set; }
        /// <summary>
        /// Optional. Signals that function call continues, and more responses will be returned,
        /// turning the function call into a generator.
        /// </summary>
        /// <remarks>
        /// Is only applicable to NON_BLOCKING function calls, is ignored otherwise.
        /// If set to false, future responses will not be considered.
        /// It is allowed to return empty `response` with `will_continue=False` to signal that the function call is finished.
        /// This may still trigger the model generation. To avoid triggering the generation and finish the function
        /// call, additionally set `scheduling` to `SILENT`.
        /// </remarks>
        public bool? WillContinue { get; set; }
    }
}