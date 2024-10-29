using System.Diagnostics;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Optional. For video input, the start and end offset of the video in Duration format.
    /// </summary>
    /// <remarks>
    /// For example, to specify a 10 second clip starting at 1:00,
    /// set "start_offset": { "seconds": 60 } and "end_offset": { "seconds": 70 }.
    /// </remarks>
    [DebuggerDisplay("{StartOffset.Seconds} - {EndOffset.Seconds}")]
    public class VideoMetadata : IPart
    {
        /// <summary>
        /// Duration of the video.
        /// </summary>
        /// <remarks>
        /// A duration in seconds with up to nine fractional digits, ending with 's'. Example: "3.5s".
        /// </remarks>
        public string? VideoDuration { get; set; }
        /// <summary>
        /// Starting offset of a video.
        /// </summary>
        public Duration StartOffset { get; set; }
        /// <summary>
        /// Ending offset of a video. Should be larger than the <see cref="StartOffset"/>.
        /// </summary>
        public Duration EndOffset { get; set; }
    }
}