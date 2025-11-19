namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// The response message for Operations.ListOperations.
	/// </summary>
	public partial class ListOperationsResponse
	{
		/// <summary>
		/// The standard List next-page token.
		/// </summary>
		public string? NextPageToken { get; set; }
		/// <summary>
		/// A list of operations that matches the specified filter in the request.
		/// </summary>
		public List<Operation>? Operations { get; set; }
		/// <summary>
		/// Unordered list. Unreachable resources. Populated when the request sets <see cref="ListOperationsRequest.return_partial_success"/> and reads across collections. For example, when attempting to list all resources across all supported locations.
		/// </summary>
		public List<string>? Unreachable { get; set; }
    }
}
