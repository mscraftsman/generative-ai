#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
#endif
using System.Diagnostics;

namespace Mscc.GenerativeAI
{
    [DebuggerDisplay("{Step}: ({Epoch,nq} - {ComputeTime,nq}")]
    public class Snapshot
    {
        public int Step { get; set; }
        public float? MeanLoss { get; set; }
        public DateTime ComputeTime { get; set; }
        public int? Epoch { get; set; }
    }
}