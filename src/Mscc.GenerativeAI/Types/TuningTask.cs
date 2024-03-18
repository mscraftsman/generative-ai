#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Collections.Generic;
#endif

namespace Mscc.GenerativeAI
{
    public class TuningTask
    {
        public DateTime? StartTime { get; set; }
        public DateTime? CompleteTime { get; set; }
        public List<Snapshot>? Snapshots { get; set; }
        public HyperParameters? Hyperparameters { get; set; }
        public TrainingData? TrainingData { get; set; }
    }
}