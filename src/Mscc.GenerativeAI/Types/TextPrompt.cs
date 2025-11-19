namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Text given to the model as a prompt. The Model will use this TextPrompt to Generate a text completion.
	/// </summary>
	public partial class TextPrompt
	{
		/// <summary>
		/// Required. The prompt text.
		/// </summary>
		public string? Text { get; set; }
    }
}
