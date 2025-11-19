namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Request message for [PredictionService.PredictLongRunning].
	/// </summary>
	public partial class PredictLongRunningRequest
	{
		/// <summary>
		/// Required. The instances that are the input to the prediction call.
		/// </summary>
		public List<object>? Instances { get; set; }
		/// <summary>
		/// Optional. The parameters that govern the prediction call.
		/// </summary>
		public object? Parameters { get; set; }
    }
}
