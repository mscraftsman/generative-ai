#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Collections.Generic;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Request to create a tuned model.
    /// </summary>
    public class CreateTunedModelRequest
    {
        /// <summary>
        /// The name to display for this model in user interfaces. The display name must be up to 40 characters including spaces.
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// The name of the Model to tune. Example: models/text-bison-001
        /// </summary>
        public string BaseModel { get; set; }
        /// <summary>
        /// Tuning tasks that create tuned models.
        /// </summary>
        public TuningTask TuningTask { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public CreateTunedModelRequest()
        {
            TuningTask = new TuningTask
            {
                TrainingData = new()
                {
                    Examples = new()
                    {
                        Examples = new() { }
                    }
                }
            };
        }
        
        /// <summary>
        /// Creates a request for a tuned model.
        /// </summary>
        /// <param name="model">Model to use.</param>
        /// <param name="name">Name of the tuned model.</param>
        /// <param name="dataset">Dataset for training or validation.</param>
        /// <param name="parameters">Immutable. Hyperparameters controlling the tuning process. If not provided, default values will be used.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public CreateTunedModelRequest(string model, string name, 
            List<TuningExample>? dataset = null,
            HyperParameters? parameters = null) : this()
        {
            if (model is null) throw new ArgumentNullException(nameof(model));
            if (name is null) throw new ArgumentNullException(nameof(name));

            BaseModel = model.SanitizeModelName();
            DisplayName = name.Trim();
            TuningTask.Hyperparameters = parameters;
            TuningTask.TrainingData.Examples.Examples = dataset ?? new();
        }
    }
}
