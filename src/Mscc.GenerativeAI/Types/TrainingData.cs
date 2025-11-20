namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// Dataset for training or validation.
    /// </summary>
    // public class Dataset 
    public class TrainingData
    {
        /// <summary>
        /// Optional. Inline examples.
        /// </summary>
        public TuningExamples? Examples { get; set; }
    }
}