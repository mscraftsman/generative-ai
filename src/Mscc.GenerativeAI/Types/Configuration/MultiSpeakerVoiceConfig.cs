using System.Collections.Generic;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// The configuration for the multi-speaker setup.
    /// </summary>
    public class MultiSpeakerVoiceConfig
    {
        /// <summary>
        /// Required. All the enabled speaker voices.
        /// </summary>
        public List<SpeakerVoiceConfig> SpeakerVoiceConfigs { get; set; }
    }
}