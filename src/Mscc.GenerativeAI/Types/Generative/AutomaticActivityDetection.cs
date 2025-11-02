namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Configures automatic detection of activity.
    /// </summary>
    public class AutomaticActivityDetection
    {
        /// <summary>
        /// Optional. If enabled (the default), detected voice and text input count as activity. If disabled, the client must send activity signals.
        /// </summary>
        public bool? Disabled { get; set; }
        /// <summary>
        /// Optional. The required duration of detected speech before start-of-speech is committed. The lower this value, the more sensitive the start-of-speech detection is and shorter speech can be recognized. However, this also increases the probability of false positives.
        /// </summary>
        public int? PrefixPaddingMs { get; set; }
        /// <summary>
        /// Optional. The required duration of detected non-speech (e.g. silence) before end-of-speech is committed. The larger this value, the longer speech gaps can be without interrupting the user's activity but this will increase the model's latency.
        /// </summary>
        public int? SilenceDurationMs { get; set; }
        /// <summary>
        /// Optional. Determines how likely speech is to be detected.
        /// </summary>
        public StartOfSpeechSensitivity? StartOfSpeechSensitivity { get; set; }
        /// <summary>
        /// Optional. Determines how likely detected speech is ended.
        /// </summary>
        public EndOfSpeechSensitivity? EndOfSpeechSensitivity { get; set; }
    }
}