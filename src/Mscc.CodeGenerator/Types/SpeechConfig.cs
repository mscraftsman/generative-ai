namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// The speech generation config.
	/// </summary>
	public partial class SpeechConfig
	{
		/// <summary>
		/// Optional. Language code (in BCP 47 format, e.g. "en-US") for speech synthesis. Valid values are: de-DE, en-AU, en-GB, en-IN, en-US, es-US, fr-FR, hi-IN, pt-BR, ar-XA, es-ES, fr-CA, id-ID, it-IT, ja-JP, tr-TR, vi-VN, bn-IN, gu-IN, kn-IN, ml-IN, mr-IN, ta-IN, te-IN, nl-NL, ko-KR, cmn-CN, pl-PL, ru-RU, and th-TH.
		/// </summary>
		public string? LanguageCode { get; set; }
		/// <summary>
		/// Optional. The configuration for the multi-speaker setup. It is mutually exclusive with the voice_config field.
		/// </summary>
		public MultiSpeakerVoiceConfig? MultiSpeakerVoiceConfig { get; set; }
		/// <summary>
		/// The configuration in case of single-voice output.
		/// </summary>
		public VoiceConfig? VoiceConfig { get; set; }
    }
}
