#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Collections.Generic;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Tuning tasks that create tuned models.
    /// </summary>
    public class TuningTask
    {
        /// <summary>
        /// Output only. The timestamp when tuning this model started.
        /// A timestamp in RFC3339 UTC "Zulu" format, with nanosecond resolution and up to nine fractional digits. Examples: "2014-10-02T15:01:23Z" and "2014-10-02T15:01:23.045123456Z".
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// Output only. The timestamp when tuning this model completed.
        /// A timestamp in RFC3339 UTC "Zulu" format, with nanosecond resolution and up to nine fractional digits. Examples: "2014-10-02T15:01:23Z" and "2014-10-02T15:01:23.045123456Z".
        /// </summary>
        public DateTime? CompleteTime { get; set; }
        /// <summary>
        /// Output only. Metrics collected during tuning.
        /// </summary>
        public List<TuningSnapshot>? Snapshots { get; set; }
        /// <summary>
        /// Required. Input only. Immutable. The model training data.
        /// </summary>
        public TrainingData? TrainingData { get; set; }
        /// <summary>
        /// Immutable. Hyperparameters controlling the tuning process. If not provided, default values will be used.
        /// </summary>
        public HyperParameters? Hyperparameters { get; set; }
    }
}