namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Session resumption configuration. This message is included in the session configuration as `BidiGenerateContentSetup.session_resumption`. If configured, the server will send `SessionResumptionUpdate` messages.
    /// </summary>
    public class SessionResumptionConfig
    {
        /// <summary>
        /// The handle of a previous session. If not present then a new session is created. Session handles come from `SessionResumptionUpdate.token` values in previous connections.
        /// </summary>
        public string Handle { get; set; }
    }
}