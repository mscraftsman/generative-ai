namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// A predicted <see cref="FunctionCall"/> returned from the model that contains a string representing the <see cref="FunctionDeclaration.name"/> with the arguments and their values.
	/// </summary>
	public partial class FunctionCall
	{
		/// <summary>
		/// Optional. The function parameters and values in JSON object format.
		/// </summary>
		public object? Args { get; set; }
		/// <summary>
		/// Optional. The unique id of the function call. If populated, the client to execute the <see cref="function_call"/> and return the response with the matching <see cref="id"/>.
		/// </summary>
		public string? Id { get; set; }
		/// <summary>
		/// Required. The name of the function to call. Must be a-z, A-Z, 0-9, or contain underscores and dashes, with a maximum length of 64.
		/// </summary>
		public string? Name { get; set; }
    }
}
