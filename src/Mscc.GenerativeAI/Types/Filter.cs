namespace Mscc.GenerativeAI.Types
{
    public partial class Filter
    {
        /// <summary>
        /// Optional. String for metadata filtering.
        /// </summary>
        public string MetadataFilter { get; set; }
        /// <summary>
        /// Optional. Only returns contexts with vector distance smaller than the threshold.
        /// </summary>
        public float? VectorDistanceThreshold { get; set; }
        /// <summary>
        /// Optional. Only returns contexts with vector similarity larger than the threshold.
        /// </summary>
        public float? VectorSimilarityThreshold { get; set; }
    }
}