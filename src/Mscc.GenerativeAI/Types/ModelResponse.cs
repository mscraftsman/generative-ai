#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Collections.Generic;
#endif
using System.Diagnostics;

namespace Mscc.GenerativeAI
{
    /// <remarks/>
    internal class ListModelsResponse
    {
        public List<ModelResponse>? Models { get; set; }
    }

    /// <summary>
    /// Information about a Generative Language Model.
    /// Ref: https://ai.google.dev/api/rest/v1beta/models
    /// </summary>
    [DebuggerDisplay("{DisplayName} ({Name})")]
    public class ModelResponse
    {
        /// <summary>
        /// Required. The resource name of the Model.
        /// </summary>
        public string? Name { get; set; } = default;
        /// <summary>
        /// The name of the base model, pass this to the generation request.
        /// </summary>
        public string? BaseModelId { get; set; } = default;
        /// <summary>
        /// The version number of the model.
        /// </summary>
        public string? Version { get; set; } = default;
        /// <summary>
        /// The human-readable name of the model. E.g. "Chat Bison".
        /// The name can be up to 128 characters long and can consist of any UTF-8 characters.
        /// </summary>
        public string? DisplayName { get; set; } = default;
        /// <summary>
        /// A short description of the model.
        /// </summary>
        public string? Description { get; set; } = default;
        /// <summary>
        /// Maximum number of input tokens allowed for this model.
        /// </summary>
        public int? InputTokenLimit { get; set; } = default;
        /// <summary>
        /// Maximum number of output tokens available for this model.
        /// </summary>
        public int? OutputTokenLimit { get; set; } = default;
        /// <summary>
        /// The model's supported generation methods.
        /// The method names are defined as Pascal case strings, such as generateMessage which correspond to API methods.
        /// </summary>
        public List<string>? SupportedGenerationMethods { get; set; }
        /// <summary>
        /// Controls the randomness of the output.
        /// Values can range over [0.0,1.0], inclusive. A value closer to 1.0 will produce responses that are more varied, while a value closer to 0.0 will typically result in less surprising responses from the model. This value specifies default to be used by the backend while making the call to the model.
        /// </summary>
        public float? Temperature { get; set; } = default;
        /// <summary>
        /// For Nucleus sampling.
        /// Nucleus sampling considers the smallest set of tokens whose probability sum is at least topP. This value specifies default to be used by the backend while making the call to the model.
        /// </summary>
        public float? TopP { get; set; } = default;
        /// <summary>
        /// For Top-k sampling.
        /// Top-k sampling considers the set of topK most probable tokens. This value specifies default to be used by the backend while making the call to the model.
        /// </summary>
        public int? TopK { get; set; } = default;
        
        // Properties related to tunedModels.
        /// <summary>
        /// The name of the base model, pass this to the generation request.
        /// </summary>
        public string? BaseModel { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? State { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? UpdateTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public TuningTask? TuningTask { get; set; }

    }
}