using System;

namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// Hyperparameters controlling the tuning process.
    /// Read more at https://ai.google.dev/docs/model_tuning_guidance
    /// </summary>
    public partial class Hyperparameters
    {
        /// <summary>
        /// Optional: The Adapter size to use for the tuning job.
        /// </summary>
        /// <remarks>
        /// The adapter size influences the number of trainable parameters for the tuning job.
        /// A larger adapter size implies that the model can learn more complex tasks,
        /// but it requires a larger training dataset and longer training times.
        /// </remarks>
        public AdapterSize? AdapterSize { get; set; }
    }

    [Obsolete("Kindly use the official type 'Hyperparameters' instead of 'HyperParameters'.")]
    public class HyperParameters : Hyperparameters { }
}