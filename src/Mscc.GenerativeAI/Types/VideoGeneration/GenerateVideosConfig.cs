#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Text.Json.Serialization;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// 
    /// </summary>
    public class GenerateVideosConfig
    {
        /// <summary>
        /// Number of output videos.
        /// </summary>
        public int? NumberOfVideos { get; set; } = 1;
        /// <summary>
        /// Optional. The aspect ratio for the generated video. 16:9 (landscape) and 9:16 (portrait) are supported.
        /// </summary>
        /// <remarks>Value: 9:16, or 16:9</remarks>
        public string? AspectRatio { get; set; }
        /// <summary>
        /// Duration of the clip for video generation in seconds.
        /// </summary>
        public int? DurationSeconds { get; set; }
        /// <summary>
        /// Whether to use the prompt rewriting logic.
        /// </summary>
        public bool? EnhancePrompt { get; set; }
        /// <summary>
        /// Frames per second for video generation.
        /// </summary>
        public int? Fps { get; set; }
        /// <summary>
        /// Used to override HTTP request options.
        /// </summary>
        public HttpOptions HttpOptions { get; set; }
        /// <summary>
        /// Optional field in addition to the text content. Negative prompts can be explicitly stated here to help generate the video.
        /// </summary>
        //[Obsolete("Setting negativePrompt is no longer supported.")]
        public string? NegativePrompt { get; set; }
        /// <summary>
        /// The GCS bucket where to save the generated videos.
        /// </summary>
        public string? OutputGcsUri { get; set; }
        /// <summary>
        /// Whether allow to generate person videos, and restrict to specific ages. Supported values are: dont_allow, allow_adult.
        /// </summary>
        /// <remarks>"personGeneration": "allow_all" is not available in Imagen 2 Editing and is only available to approved users‡ in Imagen 2 Generation.
        /// Values:
        /// allow_all: Allow generation of people of all ages.
        /// allow_adult (default): Allow generation of adults only.
        /// dont_allow: Disables the inclusion of people or faces in images.
        /// </remarks>
        public PersonGeneration? PersonGeneration { get; set; }
        //public string? PersonGeneration { get; set; }
        /// <summary>
        /// The PubSub topic where to publish the video generation progress.
        /// </summary>
        public string? PubsubTopic { get; set; }
        /// <summary>
        /// The resolution for the generated video. 1280x720, 1920x1080 are supported.
        /// </summary>
        /// <remarks>Value: 1280x720, or 1920x1080</remarks>
        //public ResolutionType? Resolution { get; set; }
        public string? Resolution { get; set; }
        /// <summary>
        /// The RNG seed. If RNG seed is exactly same for each request with unchanged inputs, the prediction results will be consistent. Otherwise, a random RNG seed will be used each time to produce a different result.
        /// </summary>
        public int? Seed { get; set; }
    }
}