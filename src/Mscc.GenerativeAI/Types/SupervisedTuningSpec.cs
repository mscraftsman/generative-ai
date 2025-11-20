using Mscc.GenerativeAI.Types;

namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// 
    /// </summary>
    public class SupervisedTuningSpec
    {
        /// <summary>
        /// Cloud Storage URI of your training dataset.
        /// </summary>
        /// <remarks>
        /// The dataset must be formatted as a JSONL file. For best results,
        /// provide at least 100 to 500 examples.
        /// </remarks>
        public string TrainingDatasetUri { get; set; }
        /// <summary>
        /// Optional: The Cloud Storage URI of your validation dataset file.
        /// </summary>
        public string? ValidationDatasetUri { get; set; }
        /// <summary>
        /// Immutable. Hyperparameters controlling the tuning process. If not provided, default values will be used.
        /// </summary>
        public Hyperparameters? HyperParameters { get; set; }
    }
}