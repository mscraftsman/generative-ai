namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Response for <see cref="CreateFile"/>.
	/// </summary>
	public partial class CreateFileResponse
	{
		/// <summary>
		/// Metadata for the created file.
		/// </summary>
		public File? File { get; set; }
    }
}
