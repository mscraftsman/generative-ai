namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// User provided string values assigned to a single metadata key.
	/// </summary>
	public partial class StringList
	{
		/// <summary>
		/// The string values of the metadata to store.
		/// </summary>
		public List<string>? Values { get; set; }
    }
}
