namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Represents token counting info for a single modality.
	/// </summary>
	public partial class ModalityTokenCount
	{
		/// <summary>
		/// The modality associated with this token count.
		/// </summary>
		public Modality? Modality { get; set; }
		/// <summary>
		/// Number of tokens.
		/// </summary>
		public int? TokenCount { get; set; }
    }
}
