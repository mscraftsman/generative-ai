namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// The configuration for a single speaker in a multi speaker setup.
	/// </summary>
	public partial class SpeakerVoiceConfig
	{
		/// <summary>
		/// Required. The name of the speaker to use. Should be the same as in the prompt.
		/// </summary>
		public string? Speaker { get; set; }
		/// <summary>
		/// Required. The configuration for the voice to use.
		/// </summary>
		public VoiceConfig? VoiceConfig { get; set; }
    }
}
