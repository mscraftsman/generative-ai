namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Identifier for a <see cref="Chunk"/> retrieved via Semantic Retriever specified in the <see cref="GenerateAnswerRequest"/> using <see cref="SemanticRetrieverConfig"/>.
	/// </summary>
	public partial class SemanticRetrieverChunk
	{
		/// <summary>
		/// Output only. Name of the <see cref="Chunk"/> containing the attributed text. Example: <see cref="corpora/123/documents/abc/chunks/xyz"/>
		/// </summary>
		public string? Chunk { get; set; }
		/// <summary>
		/// Output only. Name of the source matching the request's <see cref="SemanticRetrieverConfig.source"/>. Example: <see cref="corpora/123"/> or <see cref="corpora/123/documents/abc"/>
		/// </summary>
		public string? Source { get; set; }
    }
}
