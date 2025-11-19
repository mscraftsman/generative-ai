namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// User provided metadata stored as key-value pairs.
	/// </summary>
	public partial class CustomMetadata
	{
		/// <summary>
		/// Required. The key of the metadata to store.
		/// </summary>
		public string? Key { get; set; }
		/// <summary>
		/// The numeric value of the metadata to store.
		/// </summary>
		public double? NumericValue { get; set; }
		/// <summary>
		/// The StringList value of the metadata to store.
		/// </summary>
		public StringList? StringListValue { get; set; }
		/// <summary>
		/// The string value of the metadata to store.
		/// </summary>
		public string? StringValue { get; set; }
    }
}
