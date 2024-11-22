#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateTuningJobRequest
    {
        /// <summary>
        /// Optional. Name of the foundation model to tune.
        /// </summary>
        /// <remarks>
        /// Supported values: gemini-1.5-pro-002, gemini-1.5-flash-002, and gemini-1.0-pro-002.
        /// </remarks>
        public string BaseModel { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public SupervisedTuningSpec SupervisedTuningSpec { get; set; }
        /// <summary>
        /// Optional. A display name for the tuned model. If not set, a random name is generated.
        /// </summary>
        public string? TunedModelDisplayName { get; set; }

        /// <summary>
        /// Creates an instance of <see cref="SupervisedTuningSpec"/>.
        /// </summary>
        public CreateTuningJobRequest()
        {
            SupervisedTuningSpec = new();
        }

        /// <summary>
        /// Creates a request for tuning a model.
        /// </summary>
        /// <param name="model">Model to use.</param>
        /// <param name="datasetUri">URI of dataset for training.</param>
        /// <param name="validationUri">URI of dataset for validation.</param>
        /// <param name="displayName"></param>
        /// <param name="parameters">Immutable. Hyperparameters controlling the tuning process. If not provided, default values will be used.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="model"/> is empty or null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="datasetUri"/> is empty or null.</exception>
        public CreateTuningJobRequest(string model, 
            string datasetUri,
            string? validationUri = null, 
            string? displayName = null,
            HyperParameters? parameters = null) : this()
        {
            if (string.IsNullOrEmpty(model)) throw new ArgumentException("Value cannot be null or empty.", nameof(model));
            if (string.IsNullOrEmpty(datasetUri)) throw new ArgumentException("Value cannot be null or empty.", nameof(datasetUri));

            BaseModel = model;
            TunedModelDisplayName = displayName;
            SupervisedTuningSpec.TrainingDatasetUri = datasetUri;
            SupervisedTuningSpec.ValidationDatasetUri = validationUri;
            SupervisedTuningSpec.HyperParameters = parameters;
        }
    }
}