namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Request containing the <see cref="Content"/> for the model to embed.
	/// </summary>
	public partial class EmbedContentRequest
	{
		/// <summary>
		/// Required. The content to embed. Only the <see cref="parts.text"/> fields will be counted.
		/// </summary>
		public Content? Content { get; set; }
		/// <summary>
		/// Required. The model's resource name. This serves as an ID for the Model to use. This name should match a model name returned by the <see cref="ListModels"/> method. Format: <see cref="models/{model}"/>
		/// </summary>
		public string? Model { get; set; }
		/// <summary>
		/// Optional. Optional reduced dimension for the output embedding. If set, excessive values in the output embedding are truncated from the end. Supported by newer models since 2024 only. You cannot set this value if using the earlier model (<see cref="models/embedding-001"/>).
		/// </summary>
		public int? OutputDimensionality { get; set; }
		/// <summary>
		/// Optional. Optional task type for which the embeddings will be used. Not supported on earlier models (<see cref="models/embedding-001"/>).
		/// </summary>
		public TaskType? TaskType { get; set; }
		/// <summary>
		/// Optional. An optional title for the text. Only applicable when TaskType is <see cref="RETRIEVAL_DOCUMENT"/>. Note: Specifying a <see cref="title"/> for <see cref="RETRIEVAL_DOCUMENT"/> provides better quality embeddings for retrieval.
		/// </summary>
		public string? Title { get; set; }
    }
}
