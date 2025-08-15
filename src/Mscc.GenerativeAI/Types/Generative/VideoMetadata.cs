using System.Diagnostics;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Metadata describes the input video content.
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
        /// If not specified, the default value will be 1.0. The fps range is (0.0, 24.0].
        /// </summary>
        public double? Fps { get; set; }
    }
}