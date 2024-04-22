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
        /// Optional. Pseudo random seed for reproducible generated outcome; setting the seed lets you generate deterministic output.
        /// </summary>
        /// <remarks>Version 006 model only: To use the seed field you must also set "addWatermark": false in the list of parameters.</remarks>
        public int? Seed { get; set; }
        
        /// <summary>
        /// Optional. The text prompt for guiding the response.
        /// </summary>
        /// <remarks>en (default), de, fr, it, es</remarks>
        public string? Language { get; set; }
    }
}