namespace Mscc.GenerativeAI
{
    /// <summary>
    /// 
    /// </summary>
    public class ImageTextParameters
    {
        /// <summary>
        /// The number of generated images.
        /// </summary>
        /// <remarks>Accepted integer values: 1-3</remarks>
        public int? SampleCount { get; set; }

        /// <summary>
        /// Optional. Cloud Storage uri where to store the generated images.
        /// </summary>
        public string? StorageUri { get; set; }

        /// <summary>
        /// Optional. The seed for random number generator (RNG). If RNG seed is the same for requests with the inputs, the prediction results will be the same.
        /// </summary>
        public int? Seed { get; set; }
        
        /// <summary>
        /// Optional. The text prompt for guiding the response.
        /// </summary>
        /// <remarks>en (default), de, fr, it, es</remarks>
        public string? Language { get; set; }
    }
}