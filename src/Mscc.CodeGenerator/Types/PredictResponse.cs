namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Response message for [PredictionService.Predict].
	/// </summary>
	public partial class PredictResponse
	{
		/// <summary>
		/// The outputs of the prediction call.
		/// </summary>
		public List<object>? Predictions { get; set; }
    }
}
