namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Request to get a text embedding from the model.
	/// </summary>
	public partial class EmbedTextRequest
	{
		/// <summary>
		/// Required. The model name to use with the format model=models/{model}.
		/// </summary>
		public string? Model { get; set; }
		/// <summary>
		/// Optional. The free-form input text that the model will turn into an embedding.
		/// </summary>
		public string? Text { get; set; }
    }
}
