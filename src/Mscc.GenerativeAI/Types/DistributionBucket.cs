using System.Diagnostics;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// 
    /// </summary>
    [DebuggerDisplay("Count: {Count,nq} (Left: {Left,nq}, Right: {Right,nq})")]
    public class DistributionBucket
    {
        /// <summary>
        /// 
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Left { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Right { get; set; }
    }
}