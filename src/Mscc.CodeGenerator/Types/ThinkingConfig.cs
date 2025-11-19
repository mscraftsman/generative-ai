namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Config for thinking features.
	/// </summary>
	public partial class ThinkingConfig
	{
		/// <summary>
		/// Indicates whether to include thoughts in the response. If true, thoughts are returned only when available.
		/// </summary>
		public bool? IncludeThoughts { get; set; }
		/// <summary>
		/// The number of thoughts tokens that the model should generate.
		/// </summary>
		public int? ThinkingBudget { get; set; }
		/// <summary>
		/// Optional. The level of thoughts tokens that the model should generate.
		/// </summary>
		public ThinkingLevel? ThinkingLevel { get; set; }
    }
}
