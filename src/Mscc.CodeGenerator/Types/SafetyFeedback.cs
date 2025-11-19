namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Safety feedback for an entire request. This field is populated if content in the input and/or response is blocked due to safety settings. SafetyFeedback may not exist for every HarmCategory. Each SafetyFeedback will return the safety settings used by the request as well as the lowest HarmProbability that should be allowed in order to return a result.
	/// </summary>
	public partial class SafetyFeedback
	{
		/// <summary>
		/// Safety rating evaluated from content.
		/// </summary>
		public SafetyRating? Rating { get; set; }
		/// <summary>
		/// Safety settings applied to the request.
		/// </summary>
		public SafetySetting? Setting { get; set; }
    }
}
