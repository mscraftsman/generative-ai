namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// The configuration for the multi-speaker setup.
	/// </summary>
	public partial class MultiSpeakerVoiceConfig
	{
		/// <summary>
		/// Required. All the enabled speaker voices.
		/// </summary>
		public List<SpeakerVoiceConfig>? SpeakerVoiceConfigs { get; set; }
    }
}
