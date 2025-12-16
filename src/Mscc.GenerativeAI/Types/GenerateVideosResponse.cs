using System.Collections.Generic;

namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// Response with generated videos.
    /// </summary>
    public partial class GenerateVideosResponse
    {
        /// <summary>
        /// List of the generated videos
        /// </summary>
        public List<GeneratedVideo>? GeneratedVideos { get; set; }
        /// <summary>
        /// Returns if any videos were filtered due to RAI policies.
        /// </summary>
        public int? RaiMediaFilteredCount { get; set; }
        /// <summary>
        /// Returns rai failure reasons if any.
        /// </summary>
        public List<string>? RaiMediaFilteredReasons { get; set; }
    }
}