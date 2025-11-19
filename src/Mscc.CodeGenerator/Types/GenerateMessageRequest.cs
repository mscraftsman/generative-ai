namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Request to generate a message response from the model.
	/// </summary>
	public partial class GenerateMessageRequest
	{
		/// <summary>
		/// Optional. The number of generated response messages to return. This value must be between <see cref="[1, 8]"/>, inclusive. If unset, this will default to <see cref="1"/>.
		/// </summary>
		public int? CandidateCount { get; set; }
		/// <summary>
		/// Required. The structured textual input given to the model as a prompt. Given a prompt, the model will return what it predicts is the next message in the discussion.
		/// </summary>
		public MessagePrompt? Prompt { get; set; }
		/// <summary>
		/// Optional. Controls the randomness of the output. Values can range over <see cref="[0.0,1.0]"/>, inclusive. A value closer to <see cref="1.0"/> will produce responses that are more varied, while a value closer to <see cref="0.0"/> will typically result in less surprising responses from the model.
		/// </summary>
		public double? Temperature { get; set; }
		/// <summary>
		/// Optional. The maximum number of tokens to consider when sampling. The model uses combined Top-k and nucleus sampling. Top-k sampling considers the set of <see cref="top_k"/> most probable tokens.
		/// </summary>
		public int? TopK { get; set; }
		/// <summary>
		/// Optional. The maximum cumulative probability of tokens to consider when sampling. The model uses combined Top-k and nucleus sampling. Nucleus sampling considers the smallest set of tokens whose probability sum is at least <see cref="top_p"/>.
		/// </summary>
		public double? TopP { get; set; }
    }
}
