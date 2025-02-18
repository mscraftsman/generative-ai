namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Identifier for a `Chunk` retrieved via Semantic Retriever specified in the `GenerateAnswerRequest` using `SemanticRetrieverConfig`.
    /// </summary>
    public class SemanticRetrieverChunk
    {
        /// <summary>
        /// Output only. Name of the `Chunk` containing the attributed text. Example: `corpora/123/documents/abc/chunks/xyz`
        /// </summary>
        public string Chunk { get; set; }
        /// <summary>
        /// Output only. Name of the source matching the request's `SemanticRetrieverConfig.source`. Example: `corpora/123` or `corpora/123/documents/abc`
        /// </summary>
        public string Source { get; set; }
    }
}