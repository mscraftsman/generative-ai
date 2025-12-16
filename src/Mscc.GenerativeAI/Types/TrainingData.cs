namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// Dataset for training or validation.
    /// </summary>
    // public partial class Dataset 
    public partial class TrainingData
    {
        /// <summary>
        /// Optional. Inline examples.
        /// </summary>
        public TuningExamples? Examples { get; set; }
    }
}