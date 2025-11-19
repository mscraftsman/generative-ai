namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Result of executing the <see cref="ExecutableCode"/>. Only generated when using the <see cref="CodeExecution"/>, and always follows a <see cref="part"/> containing the <see cref="ExecutableCode"/>.
	/// </summary>
	public partial class CodeExecutionResult
	{
		/// <summary>
		/// Required. Outcome of the code execution.
		/// </summary>
		public Outcome? Outcome { get; set; }
		/// <summary>
		/// Optional. Contains stdout when code execution is successful, stderr or other description otherwise.
		/// </summary>
		public string? Output { get; set; }
    }
}
