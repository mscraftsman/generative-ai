using System.Collections.Generic;

namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// Message sent from the client to the server in a BidiGenerateContent session.
    /// </summary>
    public class LiveClientMessage
    {
        /// <summary>
        /// Initial setup configuration.
        /// </summary>
        public BidiGenerateContentSetup? Setup { get; set; }
        /// <summary>
        /// Non-realtime, turn-based content.
        /// </summary>
        public LiveClientContent? ClientContent { get; set; }
        /// <summary>
        /// Realtime input parameters.
        /// </summary>
        public LiveSendRealtimeInputParameters? RealtimeInput { get; set; }
        /// <summary>
        /// Response to a tool call.
        /// </summary>
        public LiveClientToolResponse? ToolResponse { get; set; }
    }
}
