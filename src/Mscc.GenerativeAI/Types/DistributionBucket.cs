using System.Diagnostics;

namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// 
    /// </summary>
    [DebuggerDisplay("Count: {Count,nq} (Left: {Left,nq}, Right: {Right,nq})")]
    public partial class DistributionBucket
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