namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Response from the model for a grounded answer.
	/// </summary>
	public partial class GenerateAnswerResponse
	{
		/// <summary>
		/// Candidate answer from the model. Note: The model *always* attempts to provide a grounded answer, even when the answer is unlikely to be answerable from the given passages. In that case, a low-quality or ungrounded answer may be provided, along with a low <see cref="answerable_probability"/>.
		/// </summary>
		public Candidate? Answer { get; set; }
		/// <summary>
		/// Output only. The model's estimate of the probability that its answer is correct and grounded in the input passages. A low <see cref="answerable_probability"/> indicates that the answer might not be grounded in the sources. When <see cref="answerable_probability"/> is low, you may want to: * Display a message to the effect of "We couldn’t answer that question" to the user. * Fall back to a general-purpose LLM that answers the question from world knowledge. The threshold and nature of such fallbacks will depend on individual use cases. <see cref="0.5"/> is a good starting threshold.
		/// </summary>
		public double? AnswerableProbability { get; set; }
		/// <summary>
		/// Output only. Feedback related to the input data used to answer the question, as opposed to the model-generated response to the question. The input data can be one or more of the following: - Question specified by the last entry in <see cref="GenerateAnswerRequest.content"/> - Conversation history specified by the other entries in <see cref="GenerateAnswerRequest.content"/> - Grounding sources (<see cref="GenerateAnswerRequest.semantic_retriever"/> or <see cref="GenerateAnswerRequest.inline_passages"/>)
		/// </summary>
		public InputFeedback? InputFeedback { get; set; }
    }
}
