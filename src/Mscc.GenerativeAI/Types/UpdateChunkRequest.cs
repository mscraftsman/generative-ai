namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Request to update a <see cref="Chunk"/>.
	/// </summary>
	public partial class UpdateChunkRequest
	{
		/// <summary>
		/// Required. The <see cref="Chunk"/> to update.
		/// </summary>
		public Chunk? Chunk { get; set; }
		/// <summary>
		/// Required. The list of fields to update. Currently, this only supports updating <see cref="custom_metadata"/> and <see cref="data"/>.
		/// </summary>
		public string? UpdateMask { get; set; }
    }
}
