namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Configuration for specifying function calling behavior.
	/// </summary>
	public partial class FunctionCallingConfig
	{
		/// <summary>
		/// Optional. A set of function names that, when provided, limits the functions the model will call. This should only be set when the Mode is ANY or VALIDATED. Function names should match [FunctionDeclaration.name]. When set, model will predict a function call from only allowed function names.
		/// </summary>
		public List<string>? AllowedFunctionNames { get; set; }
		/// <summary>
		/// Optional. Specifies the mode in which function calling should execute. If unspecified, the default value will be set to AUTO.
		/// </summary>
		public Mode? Mode { get; set; }
    }
}
