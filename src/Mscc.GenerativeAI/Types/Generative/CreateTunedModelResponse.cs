using System.Diagnostics;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Response of a newly created tuned model.
    /// </summary>
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
        /// <summary>
        /// A fine-tuned model created using ModelService.CreateTunedModel.
        /// </summary>
        public string TunedModel { get; set; }
    }
}