#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
#endif
using System.Diagnostics;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Record for a single tuning step.
    /// </summary>
    [DebuggerDisplay("{Step}: ({Epoch,nq} - {ComputeTime,nq}")]
    public class TuningSnapshot
    {
        /// <summary>
        /// Output only. The tuning step.
        /// </summary>
        public int Step { get; set; }
        /// <summary>
        /// Output only. The epoch this step was part of.
        /// </summary>
        public int? Epoch { get; set; }
        /// <summary>
        /// Output only. The mean loss of the training examples for this step.
        /// </summary>
        public float? MeanLoss { get; set; }
        /// <summary>
        /// Output only. The timestamp when this metric was computed.
        /// A timestamp in RFC3339 UTC "Zulu" format, with nanosecond resolution and up to nine fractional digits. Examples: "2014-10-02T15:01:23Z" and "2014-10-02T15:01:23.045123456Z".
        /// </summary>
        public DateTime ComputeTime { get; set; }
    }
}