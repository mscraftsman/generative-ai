using System.Collections.Generic;

namespace Mscc.GenerativeAI
{
    public class GroundingMetadata
    {
        public List<GroundingAttribution>? GroundingAttributions { get; set; }
        public List<string>? WebSearchQueries { get; set; }
    }
}