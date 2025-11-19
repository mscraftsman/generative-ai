namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// A list of floats representing an embedding.
	/// </summary>
	public partial class ContentEmbedding
	{
		/// <summary>
		/// The embedding values.
		/// </summary>
		public List<double>? Values { get; set; }
    }
}
