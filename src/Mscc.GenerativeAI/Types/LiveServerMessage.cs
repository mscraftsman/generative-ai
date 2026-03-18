using System.Collections.Generic;
using System.Text.Json;

namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// Message received from the server in a BidiGenerateContent session.
    /// </summary>
    public class LiveServerMessage
    {
        /// <summary>
        /// Response to the setup message.
        /// </summary>
        public LiveSetupComplete? SetupComplete { get; set; }
        /// <summary>
        /// Model generated content.
        /// </summary>
        public GenerateContentResponse? ServerContent { get; set; }
        /// <summary>
        /// Model requested tool calls.
        /// </summary>
        public LiveToolCall? ToolCall { get; set; }
        /// <summary>
        /// Cancellation of a tool call.
        /// </summary>
        public LiveToolCallCancellation? ToolCallCancellation { get; set; }

        /// <summary>
        /// Deserializes a JSON string into a <see cref="LiveServerMessage"/>.
        /// </summary>
        public static LiveServerMessage? FromJson(string json)
        {
            return JsonSerializer.Deserialize<LiveServerMessage>(json, JsonConfig.LiveSerializerOptions);
        }
    }

    /// <summary>
    /// Confirmation that the session has been set up.
    /// </summary>
    public class LiveSetupComplete { }

    /// <summary>
    /// Represents a tool call requested by the model.
    /// </summary>
    public class LiveToolCall
    {
        public List<FunctionCall>? FunctionCalls { get; set; }
    }

    /// <summary>
    /// Represents a cancellation of a tool call.
    /// </summary>
    public class LiveToolCallCancellation
    {
        public List<string>? Ids { get; set; }
    }
}
