namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Configuration for speech generation.
    /// </summary>
    public class SpeechConfig
    {
        /// <summary>
        /// The configuration for the voice to use.
        /// </summary>
        public VoiceConfig? VoiceConfig { get; set; }
        /// <summary>
        /// Optional. The language code (ISO 639-1) for the speech synthesis.
        /// </summary>
        /// <remarks>
        /// Valid values are: de-DE, en-AU, en-GB, en-IN, en-US, es-US, fr-FR, hi-IN, pt-BR, ar-XA, es-ES, fr-CA, id-ID, it-IT, ja-JP, tr-TR, vi-VN, bn-IN, gu-IN, kn-IN, ml-IN, mr-IN, ta-IN, te-IN, nl-NL, ko-KR, cmn-CN, pl-PL, ru-RU, and th-TH.
        /// </remarks>
        public string? LanguageCode { get; set; }
        /// <summary>
        /// The configuration for a multi-speaker text-to-speech request. This field is mutually
        /// exclusive with <see cref="VoiceConfig"/>.
        /// </summary>
        public MultiSpeakerVoiceConfig? MultiSpeakerVoiceConfig { get; set; }
    }
}