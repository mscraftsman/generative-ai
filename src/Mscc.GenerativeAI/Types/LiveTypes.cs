using System.Collections.Generic;

namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// Represents content sent by the client.
    /// </summary>
    public class LiveClientContent
    {
        /// <summary>
        /// The content turns to be added to the conversation.
        /// </summary>
        public List<Content>? Turns { get; set; }
        /// <summary>
        /// Indicates if the current turn is complete.
        /// </summary>
        public bool? TurnComplete { get; set; }
    }

    /// <summary>
    /// Represents responses to tool calls from the model.
    /// </summary>
    public class LiveClientToolResponse
    {
        /// <summary>
        /// The responses to the function calls.
        /// </summary>
        public List<FunctionResponse>? FunctionResponses { get; set; }
    }

    /// <summary>
    /// Parameters for sending client content.
    /// </summary>
    public class LiveSendClientContentParameters
    {
        /// <summary>
        /// The content turns to be added to the conversation.
        /// </summary>
        public List<Content>? Turns { get; set; }
        /// <summary>
        /// Indicates if the current turn is complete.
        /// </summary>
        public bool? TurnComplete { get; set; }
    }

    /// <summary>
    /// Parameters for sending realtime input.
    /// </summary>
    public class LiveSendRealtimeInputParameters
    {
        /// <summary>
        /// The realtime input data (e.g., audio, video).
        /// </summary>
        public List<Blob>? MediaChunks { get; set; }
    }

    /// <summary>
    /// Parameters for sending tool responses.
    /// </summary>
    public class LiveSendToolResponseParameters
    {
        /// <summary>
        /// The responses to the function calls.
        /// </summary>
        public List<FunctionResponse>? FunctionResponses { get; set; }
    }

    /// <summary>
    /// Parameters for establishing a connection to the live model.
    /// </summary>
    public class LiveConnectParameters
    {
        /// <summary>
        /// The name of the model.
        /// </summary>
        public string? Model { get; set; }
        /// <summary>
        /// The session configuration.
        /// </summary>
        public LiveConnectConfig? Config { get; set; }
    }
}
