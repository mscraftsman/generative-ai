using System.Collections.Generic;

namespace Mscc.GenerativeAI.Types
{
    public class SupervisedTuningDataStats
    {
        public string TuninDatasetExampleCount { get; set; }
        public string TotalTuningCharacterCount { get; set; }
        public string TuningStepCount { get; set; }
        public string TotalBillableTokenCount { get; set; }
        public Distribution UserInputTokenDistribution { get; set; }
        public Distribution UserOutputTokenDistribution { get; set; }
        public Distribution UserMessagePerExampleDistribution { get; set; }
        public List<ContentResponse> UserDatasetExamples { get; set; }
    }
}