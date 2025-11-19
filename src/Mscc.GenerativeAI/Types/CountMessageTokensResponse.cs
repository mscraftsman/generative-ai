namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// A response from <see cref="CountMessageTokens"/>. It returns the model's <see cref="token_count"/> for the <see cref="prompt"/>.
	/// </summary>
	public partial class CountMessageTokensResponse
	{
		/// <summary>
		/// The number of tokens that the <see cref="model"/> tokenizes the <see cref="prompt"/> into. Always non-negative.
		/// </summary>
		public int? TokenCount { get; set; }
    }
}
