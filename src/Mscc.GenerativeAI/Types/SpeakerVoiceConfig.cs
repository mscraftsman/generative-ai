namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Configuration for a single speaker in a multi-speaker setup.
    /// </summary>
    public class SpeakerVoiceConfig
    {
        /// <summary>
        /// The name of the speaker. This should be the same as the speaker name used in the prompt.
        /// </summary>
        public string? Speaker { get; set; }
        /// <summary>
        /// The configuration for the voice of this speaker.
        /// </summary>
        public VoiceConfig? VoiceConfig { get; set; }
    }
}