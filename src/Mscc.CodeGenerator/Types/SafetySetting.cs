namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Safety setting, affecting the safety-blocking behavior. Passing a safety setting for a category changes the allowed probability that content is blocked.
	/// </summary>
	public partial class SafetySetting
	{
		/// <summary>
		/// Required. The category for this setting.
		/// </summary>
		public Category? Category { get; set; }
		/// <summary>
		/// Required. Controls the probability threshold at which harm is blocked.
		/// </summary>
		public Threshold? Threshold { get; set; }
    }
}
