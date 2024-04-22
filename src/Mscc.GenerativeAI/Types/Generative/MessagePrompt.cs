#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Collections.Generic;
using System.Linq;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// All of the structured input text passed to the model as a prompt.
    /// A MessagePrompt contains a structured set of fields that provide context for the conversation,
    /// examples of user input/model output message pairs that prime the model to respond in different ways,
    /// and the conversation history or list of messages representing the alternating turns of the conversation
    /// between the user and the model.
    /// </summary>
    public class MessagePrompt
    {
        /// <summary>
        /// Optional. Text that should be provided to the model first to ground the response.
        /// If not empty, this context will be given to the model first before the examples and messages. When using a context be sure to provide it with every request to maintain continuity.
        /// </summary>
        public string? Context { get; set; }
        /// <summary>
        /// Optional. Examples of what the model should generate.
        /// This includes both user input and the response that the model should emulate.
        /// </summary>
        public List<Example>? Examples { get; set; }
        /// <summary>
        /// Required. A snapshot of the recent conversation history sorted chronologically.
        /// Turns alternate between two authors.
        /// </summary>
        public List<Message> Messages { get; set; }
    }
}