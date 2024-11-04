#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Collections.Generic;
#endif

namespace Mscc.GenerativeAI
{
    public class Distribution
    {
        public string Sum { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
        public double Mean { get; set; }
        public int Median { get; set; }
        public int P5 { get; set; }
        public int P95 { get; set; }
        public List<DistributionBucket> Buckets { get; set; }
    }
}