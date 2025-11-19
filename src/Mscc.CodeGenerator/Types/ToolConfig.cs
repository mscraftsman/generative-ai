namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// The Tool configuration containing parameters for specifying <see cref="Tool"/> use in the request.
	/// </summary>
	public partial class ToolConfig
	{
		/// <summary>
		/// Optional. Function calling config.
		/// </summary>
		public FunctionCallingConfig? FunctionCallingConfig { get; set; }
		/// <summary>
		/// Optional. Retrieval config.
		/// </summary>
		public RetrievalConfig? RetrievalConfig { get; set; }
    }
}
