namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// The response from the model. This includes candidate messages and conversation history in the form of chronologically-ordered messages.
	/// </summary>
	public partial class GenerateMessageResponse
	{
		/// <summary>
		/// Candidate response messages from the model.
		/// </summary>
		public List<Message>? Candidates { get; set; }
		/// <summary>
		/// A set of content filtering metadata for the prompt and response text. This indicates which <see cref="SafetyCategory"/>(s) blocked a candidate from this response, the lowest <see cref="HarmProbability"/> that triggered a block, and the HarmThreshold setting for that category.
		/// </summary>
		public List<ContentFilter>? Filters { get; set; }
		/// <summary>
		/// The conversation history used by the model.
		/// </summary>
		public List<Message>? Messages { get; set; }
    }
}
