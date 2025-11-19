namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Configuration for recontextualizing an image.
    /// </summary>
    public class RecontextImageConfig : ImageGenerationParameters
    {
        /// <summary>
        /// The number of sampling steps. A higher value has better image quality, while a lower value
        /// has better latency.
        /// </summary>
        public int? BaseSteps { get; set; }
    }
}