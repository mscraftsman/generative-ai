namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Extracted data that represents the <see cref="Chunk"/> content.
	/// </summary>
	public partial class ChunkData
	{
		/// <summary>
		/// The <see cref="Chunk"/> content as a string. The maximum number of tokens per chunk is 2043.
		/// </summary>
		public string? StringValue { get; set; }
    }
}
