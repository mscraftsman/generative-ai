namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Computer Use tool type.
	/// </summary>
	public partial class ComputerUse
	{
		/// <summary>
		/// Required. The environment being operated.
		/// </summary>
		public Environment? Environment { get; set; }
		/// <summary>
		/// Optional. By default, predefined functions are included in the final model call. Some of them can be explicitly excluded from being automatically included. This can serve two purposes: 1. Using a more restricted / different action space. 2. Improving the definitions / instructions of predefined functions.
		/// </summary>
		public List<string>? ExcludedPredefinedFunctions { get; set; }
    }
}
