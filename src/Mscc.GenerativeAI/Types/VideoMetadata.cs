using System.Diagnostics;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Provides metadata for a video, including the start and end offsets for clipping and the frame
    /// rate.
    /// </summary>
    [DebuggerDisplay("{StartOffset} - {EndOffset} (FPS: {Fps})")]
    public class VideoMetadata : IPart
    {
        /// <summary>
        /// Optional. The start offset of the video.
        /// </summary>
        public string? StartOffset { get; set; }
        /// <summary>
        /// Optional. The end offset of the video.
        /// </summary>
        public string? EndOffset { get; set; }
        /// <summary>
        /// Optional. The frame rate of the video sent to the model.
        /// If not specified, the default value is 1.0. The valid range is (0.0, 24.0].
        /// </summary>
        public double? Fps { get; set; }
    }
}