namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Describes the options to customize dynamic retrieval.
    /// </summary>
    public class DynamicRetrievalConfig
    {
        /// <summary>
        /// The mode of the predictor to be used in dynamic retrieval.
        /// </summary>
        public DynamicRetrievalConfigMode? Mode { get; set; }
        /// <summary>
        /// The threshold to be used in dynamic retrieval. If not set, a system default value is used.
        /// </summary>
        public float? DynamicThreshold { get; set; }
    }
}