namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// A file generated on behalf of a user.
	/// </summary>
	public partial class GeneratedFile
	{
		/// <summary>
		/// Error details if the GeneratedFile ends up in the STATE_FAILED state.
		/// </summary>
		public Status? Error { get; set; }
		/// <summary>
		/// MIME type of the generatedFile.
		/// </summary>
		public string? MimeType { get; set; }
		/// <summary>
		/// Identifier. The name of the generated file. Example: <see cref="generatedFiles/abc-123"/>
		/// </summary>
		public string? Name { get; set; }
		/// <summary>
		/// Output only. The state of the GeneratedFile.
		/// </summary>
		public State? State { get; set; }
    }
}
