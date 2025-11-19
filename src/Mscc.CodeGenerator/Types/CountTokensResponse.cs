namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// A response from <see cref="CountTokens"/>. It returns the model's <see cref="token_count"/> for the <see cref="prompt"/>.
	/// </summary>
	public partial class CountTokensResponse
	{
		/// <summary>
		/// Output only. List of modalities that were processed in the cached content.
		/// </summary>
		public List<ModalityTokenCount>? CacheTokensDetails { get; set; }
		/// <summary>
		/// Number of tokens in the cached part of the prompt (the cached content).
		/// </summary>
		public int? CachedContentTokenCount { get; set; }
		/// <summary>
		/// Output only. List of modalities that were processed in the request input.
		/// </summary>
		public List<ModalityTokenCount>? PromptTokensDetails { get; set; }
		/// <summary>
		/// The number of tokens that the <see cref="Model"/> tokenizes the <see cref="prompt"/> into. Always non-negative.
		/// </summary>
		public int? TotalTokens { get; set; }
    }
}
