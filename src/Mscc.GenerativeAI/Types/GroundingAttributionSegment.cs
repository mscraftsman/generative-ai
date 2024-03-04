namespace Mscc.GenerativeAI
{
    public class GroundingAttributionSegment
    {
        /// <summary>
        /// Output only. Start index into the content.
        /// </summary>
        public int StartIndex { get; internal set; } = default;
        /// <summary>
        /// Output only. End index into the content.
        /// </summary>
        public int EndIndex { get; internal set; } = default;
        /// <summary>
        /// Output only. Part index into the content.
        /// </summary>
        public int PartIndex { get; internal set; } = default;
    }
}