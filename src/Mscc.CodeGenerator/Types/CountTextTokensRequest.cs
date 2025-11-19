namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Counts the number of tokens in the <see cref="prompt"/> sent to a model. Models may tokenize text differently, so each model may return a different <see cref="token_count"/>.
	/// </summary>
	public partial class CountTextTokensRequest
	{
		/// <summary>
		/// Required. The free-form input text given to the model as a prompt.
		/// </summary>
		public TextPrompt? Prompt { get; set; }
    }
}
