namespace Mscc.GenerativeAI.Types
{
    public partial class TransformationConfig
    {
        /// <summary>
        /// Optional. Config for telling the service how to chunk the data.
        /// If not provided, the service will use default parameters.
        /// </summary>
        public ChunkingConfig? ChunkingConfig { get; set; }
    }
}