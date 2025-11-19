namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Filter condition applicable to a single key.
	/// </summary>
	public partial class Condition
	{
		/// <summary>
		/// The numeric value to filter the metadata on.
		/// </summary>
		public double? NumericValue { get; set; }
		/// <summary>
		/// Required. Operator applied to the given key-value pair to trigger the condition.
		/// </summary>
		public Operation? Operation { get; set; }
		/// <summary>
		/// The string value to filter the metadata on.
		/// </summary>
		public string? StringValue { get; set; }
    }
}
