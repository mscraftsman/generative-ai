namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Retrieval config.
	/// </summary>
	public partial class RetrievalConfig
	{
		/// <summary>
		/// Optional. The language code of the user. Language code for content. Use language tags defined by [BCP47](https://www.rfc-editor.org/rfc/bcp/bcp47.txt).
		/// </summary>
		public string? LanguageCode { get; set; }
		/// <summary>
		/// Optional. The location of the user.
		/// </summary>
		public LatLng? LatLng { get; set; }
    }
}
