namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// Configures the realtime input behavior in `BidiGenerateContent`.
    /// </summary>
    public partial class RealtimeInputConfig
    {
        /// <summary>
        /// Optional. Defines what effect activity has.
        /// </summary>
        public ActivityHandling? ActivityHandling { get; set; }
        /// <summary>
        /// Optional. If not set, automatic activity detection is enabled by default. If automatic voice detection is disabled, the client must send activity signals.
        /// </summary>
        public AutomaticActivityDetection? AutomaticActivityDetection { get; set; }
        /// <summary>
        /// Optional. Defines which input is included in the user's turn.
        /// </summary>
        public TurnCoverage? TurnCoverage { get; set; }
    }
}