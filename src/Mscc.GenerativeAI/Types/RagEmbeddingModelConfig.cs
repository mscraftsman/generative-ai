namespace Mscc.GenerativeAI.Types
{
    public partial class RagEmbeddingModelConfig
    {
        public string PublisherModel { get; set; }

        public RagEmbeddingModelConfig(string publisherModel)
        {
            PublisherModel = publisherModel;
        }
    }
}