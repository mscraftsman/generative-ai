namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Request for <see cref="CreateFile"/>.
	/// </summary>
	public partial class CreateFileRequest
	{
		/// <summary>
		/// Optional. Metadata for the file to create.
		/// </summary>
		public File? File { get; set; }
    }
}
