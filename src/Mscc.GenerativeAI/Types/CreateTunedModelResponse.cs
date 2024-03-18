using System.Diagnostics;

namespace Mscc.GenerativeAI
{
    public class CreateTunedModelResponse
    {
        public string Name { get; set; }
        public CreateTunedModelMetadata Metadata { get; set; }
    }

    [DebuggerDisplay("{TunedModel})")]
    public class CreateTunedModelMetadata
    {
        public string Type { get; set; }
        public int TotalSteps { get; set; }
        public string TunedModel { get; set; }
    }
}