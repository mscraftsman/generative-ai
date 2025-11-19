namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Counts the number of tokens in the <see cref="prompt"/> sent to a model. Models may tokenize text differently, so each model may return a different <see cref="token_count"/>.
	/// </summary>
	public partial class CountMessageTokensRequest
	{
		/// <summary>
		/// Required. The prompt, whose token count is to be returned.
		/// </summary>
		public MessagePrompt? Prompt { get; set; }
    }
}
