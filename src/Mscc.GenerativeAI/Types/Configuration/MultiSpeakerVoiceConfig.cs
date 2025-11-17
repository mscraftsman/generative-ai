using System.Collections.Generic;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Configuration for a multi-speaker text-to-speech request.
    /// </summary>
    public class MultiSpeakerVoiceConfig
    {
        /// <summary>
        /// Required. A list of configurations for the voices of the speakers. Exactly two speaker voice
        /// configurations must be provided.
        /// </summary>
        public List<SpeakerVoiceConfig> SpeakerVoiceConfigs { get; set; }
    }
}