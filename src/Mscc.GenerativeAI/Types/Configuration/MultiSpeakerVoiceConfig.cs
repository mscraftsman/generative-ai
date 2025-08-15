#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Collections.Generic;
#endif

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