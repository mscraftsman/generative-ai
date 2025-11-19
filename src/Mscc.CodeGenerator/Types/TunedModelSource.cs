namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Tuned model as a source for training a new model.
	/// </summary>
	public partial class TunedModelSource
	{
		/// <summary>
		/// Output only. The name of the base <see cref="Model"/> this <see cref="TunedModel"/> was tuned from. Example: <see cref="models/gemini-1.5-flash-001"/>
		/// </summary>
		public string? BaseModel { get; set; }
		/// <summary>
		/// Immutable. The name of the <see cref="TunedModel"/> to use as the starting point for training the new model. Example: <see cref="tunedModels/my-tuned-model"/>
		/// </summary>
		public string? TunedModel { get; set; }
    }
}
