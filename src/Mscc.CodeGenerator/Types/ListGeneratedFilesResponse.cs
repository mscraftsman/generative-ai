namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Response for <see cref="ListGeneratedFiles"/>.
	/// </summary>
	public partial class ListGeneratedFilesResponse
	{
		/// <summary>
		/// The list of <see cref="GeneratedFile"/>s.
		/// </summary>
		public List<GeneratedFile>? GeneratedFiles { get; set; }
		/// <summary>
		/// A token that can be sent as a <see cref="page_token"/> into a subsequent <see cref="ListGeneratedFiles"/> call.
		/// </summary>
		public string? NextPageToken { get; set; }
    }
}
