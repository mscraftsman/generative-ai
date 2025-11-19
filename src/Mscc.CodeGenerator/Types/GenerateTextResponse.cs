namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// The response from the model, including candidate completions.
	/// </summary>
	public partial class GenerateTextResponse
	{
		/// <summary>
		/// Candidate responses from the model.
		/// </summary>
		public List<TextCompletion>? Candidates { get; set; }
		/// <summary>
		/// A set of content filtering metadata for the prompt and response text. This indicates which <see cref="SafetyCategory"/>(s) blocked a candidate from this response, the lowest <see cref="HarmProbability"/> that triggered a block, and the HarmThreshold setting for that category. This indicates the smallest change to the <see cref="SafetySettings"/> that would be necessary to unblock at least 1 response. The blocking is configured by the <see cref="SafetySettings"/> in the request (or the default <see cref="SafetySettings"/> of the API).
		/// </summary>
		public List<ContentFilter>? Filters { get; set; }
		/// <summary>
		/// Returns any safety feedback related to content filtering.
		/// </summary>
		public List<SafetyFeedback>? SafetyFeedback { get; set; }
    }
}
