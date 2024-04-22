namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Hyperparameters controlling the tuning process.
    /// Read more at https://ai.google.dev/docs/model_tuning_guidance
    /// </summary>
    public class HyperParameters
    {
        /// <summary>
        /// Immutable. The batch size hyperparameter for tuning. If not set, a default of 4 or 16 will be used based on the number of training examples.
        /// </summary>
        public int BatchSize { get; set; }
        /// <summary>
        /// Optional. Immutable. The learning rate hyperparameter for tuning. If not set, a default of 0.001 or 0.0002 will be calculated based on the number of training examples.
        /// </summary>
        public float? LearningRate { get; set; }
        /// <summary>
        /// Optional. Immutable. The learning rate multiplier is used to calculate a final learningRate based on the default (recommended) value. Actual learning rate := learningRateMultiplier * default learning rate Default learning rate is dependent on base model and dataset size. If not set, a default of 1.0 will be used.
        /// </summary>
        public float? LearningRateMultiplier { get; set; }
        /// <summary>
        /// Immutable. The number of training epochs. An epoch is one pass through the training data. If not set, a default of 5 will be used.
        /// </summary>
        public int EpochCount { get; set; }
    }
}