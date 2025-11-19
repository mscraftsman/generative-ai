namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Request for embedding generation.
    /// </summary>
    public class GenerateEmbeddingsRequest
    {
        private string _model;

        /// <summary>
        /// Required. Model to generate the embeddings for.
        /// </summary>
        public string Model
        {
            get => _model;
            set => _model = value.SanitizeModelName();
        }
        /// <summary>
        /// Required. The input to generate embeddings for.
        /// Can be a string, or a list of strings.
        /// The SDK supports a list of numbers and list of list of numbers, but this is not yet implemented.
        /// </summary>
        public object Input { get; set; }
        /// <summary>
        /// Optional. The format of the encoding.
        /// Must be either \"float\" or \"base64\".
        /// </summary>
        public string? EncodingFormat { get; set; }
        /// <summary>
        /// Optional. Dimensional size of the generated embeddings.
        /// </summary>
        public int? Dimensions { get; set; }    // = 768;
    }
}