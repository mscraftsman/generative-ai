namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Counts the number of tokens in the <see cref="prompt"/> sent to a model. Models may tokenize text differently, so each model may return a different <see cref="token_count"/>.
	/// </summary>
	public partial class CountTokensRequest
	{
		/// <summary>
		/// Optional. The input given to the model as a prompt. This field is ignored when <see cref="generate_content_request"/> is set.
		/// </summary>
		public List<Content>? Contents { get; set; }
		/// <summary>
		/// Optional. The overall input given to the <see cref="Model"/>. This includes the prompt as well as other model steering information like [system instructions](https://ai.google.dev/gemini-api/docs/system-instructions), and/or function declarations for [function calling](https://ai.google.dev/gemini-api/docs/function-calling). <see cref="Model"/>s/<see cref="Content"/>s and <see cref="generate_content_request"/>s are mutually exclusive. You can either send <see cref="Model"/> + <see cref="Content"/>s or a <see cref="generate_content_request"/>, but never both.
		/// </summary>
		public GenerateContentRequest? GenerateContentRequest { get; set; }
    }
}
