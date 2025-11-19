namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Response for <see cref="ListFiles"/>.
	/// </summary>
	public partial class ListFilesResponse
	{
		/// <summary>
		/// The list of <see cref="File"/>s.
		/// </summary>
		public List<File>? Files { get; set; }
		/// <summary>
		/// A token that can be sent as a <see cref="page_token"/> into a subsequent <see cref="ListFiles"/> call.
		/// </summary>
		public string? NextPageToken { get; set; }
    }
}
