namespace Mscc.GenerativeAI
{
    public class GroundingAttribution
    {
        public int ConfidenceScore { get; set; } = default;
        public GroundingAttributionSegment? Segment { get; set; }
        public GroundingAttributionWeb? Web { get; set; }
    }
}