namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// URI based data.
	/// </summary>
	public partial class FileData
	{
		/// <summary>
		/// Required. URI.
		/// </summary>
		public string? FileUri { get; set; }
		/// <summary>
		/// Optional. The IANA standard MIME type of the source data.
		/// </summary>
		public string? MimeType { get; set; }
    }
}
