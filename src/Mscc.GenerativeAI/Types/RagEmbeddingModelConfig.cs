namespace Mscc.GenerativeAI.Types
{
    public class RagEmbeddingModelConfig
    {
        public string PublisherModel { get; set; }
        public VertexPredictionEndpoint VertexPredictionEndpoint { get; set; }

        public RagEmbeddingModelConfig(string publisherModel)
        {
            PublisherModel = publisherModel;
        }
    }
}