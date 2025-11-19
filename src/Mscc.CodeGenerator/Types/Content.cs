namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// The base structured datatype containing multi-part content of a message. A <see cref="Content"/> includes a <see cref="role"/> field designating the producer of the <see cref="Content"/> and a <see cref="parts"/> field containing multi-part data that contains the content of the message turn.
	/// </summary>
	public partial class Content
	{
		/// <summary>
		/// Ordered <see cref="Parts"/> that constitute a single message. Parts may have different MIME types.
		/// </summary>
		public List<Part>? Parts { get; set; }
		/// <summary>
		/// Optional. The producer of the content. Must be either 'user' or 'model'. Useful to set for multi-turn conversations, otherwise can be left blank or unset.
		/// </summary>
		public string? Role { get; set; }
    }
}
