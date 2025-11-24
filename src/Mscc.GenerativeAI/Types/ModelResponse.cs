using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// Information about a Generative Language Model.
    /// Ref: https://ai.google.dev/api/rest/v1beta/models
    /// </summary>
    [DebuggerDisplay("{DisplayName} ({Name})")]
    public partial class ModelResponse
    {
        /// <summary>
        /// The version Id of the model (Vertex AI).
        /// </summary>
        public string? VersionId
        {
            get => Version;
            set => Version = value;
        }
        // Properties related to tunedModels.
        /// <summary>
        /// Output only. The state of the tuned model.
        /// </summary>
        public State? State { get; set; }
        /// <summary>
        /// Required. The tuning task that creates the tuned model.
        /// </summary>
        public TuningTask? TuningTask { get; set; }
        /// <summary>
        /// Optional. TunedModel to use as the starting point for training the new model.
        /// </summary>
        public TunedModelSource? TunedModelSource { get; set; }
        /// <summary>
        /// The name of the base model, pass this to the generation request.
        /// </summary>
        public string? BaseModel { get; set; }
        /// <summary>
        /// The ETag of the item.
        /// </summary>
        public virtual string? ETag { get; set; }
        /// <summary>
        /// Optional. The labels with user-defined metadata for the request.
        /// </summary>
        /// <remarks>
        /// It is used for billing and reporting only.
        /// Label keys and values can be no longer than 63 characters (Unicode codepoints) and
        /// can only contain lowercase letters, numeric characters, underscores, and dashes.
        /// International characters are allowed. Label values are optional. Label keys must start with a letter.
        /// </remarks>
        public virtual IDictionary<string, string>? Labels { get; set; }
    }
}