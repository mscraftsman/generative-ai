#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Collections.Generic;
#endif
using System.Diagnostics;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Response from ListModels method containing a paginated list of Models.
    /// </summary>
    internal class ListModelsResponse
    {
        /// <summary>
        /// The list of Models.
        /// </summary>
        public List<ModelResponse>? Models { get; set; }
        /// <summary>
        /// A token, which can be sent as pageToken to retrieve the next page.
        /// If this field is omitted, there are no more pages.
        /// </summary>
        public string? NextPageToken { get; set; }
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
        /// The version number of the model (Google AI).
        /// </summary>
        public string? Version { get; set; } = default;
        /// <summary>
        /// The version Id of the model (Vertex AI).
        /// </summary>
        public string? VersionId
        {
            get => Version;
            set => Version = value;
        }
        /// <summary>
        /// User provided version aliases so that a model version can be referenced via
        /// alias (i.e. projects/{project}/locations/{location}/models/{model_id}@{version_alias}
        /// instead of auto-generated version id (i.e. projects/{project}/locations/{location}/models/{model_id}@{version_id}).
        /// </summary>
        /// <remarks>
        /// The format is a-z{0,126}[a-z0-9] to distinguish from version_id.
        /// A default version alias will be created for the first version of the model,
        /// and there must be exactly one default version alias for a model.
        /// </remarks>
        public List<string>? VersionAliases { get; set; }
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
        /// The maximum temperature this model can use.
        /// </summary>
        public float? MaxTemperature { get; set; } = default;
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
        /// Output only. The state of the tuned model.
        /// </summary>
        public State? State { get; set; }
        /// <summary>
        /// Output only. The timestamp when this model was created.
        /// A timestamp in RFC3339 UTC "Zulu" format, with nanosecond resolution and up to nine fractional digits. Examples: "2014-10-02T15:01:23Z" and "2014-10-02T15:01:23.045123456Z".
        /// </summary>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// Output only. The timestamp when this model was updated.
        /// A timestamp in RFC3339 UTC "Zulu" format, with nanosecond resolution and up to nine fractional digits. Examples: "2014-10-02T15:01:23Z" and "2014-10-02T15:01:23.045123456Z".
        /// </summary>
        public DateTime? UpdateTime { get; set; }
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
        /// <summary>
        /// Output only. The timestamp when this model was created.
        /// A timestamp in RFC3339 UTC "Zulu" format, with nanosecond resolution and up to nine fractional digits. Examples: "2014-10-02T15:01:23Z" and "2014-10-02T15:01:23.045123456Z".
        /// </summary>
        public DateTime? VersionCreateTime { get; set; }
        /// <summary>
        /// Output only. The timestamp when this model was updated.
        /// A timestamp in RFC3339 UTC "Zulu" format, with nanosecond resolution and up to nine fractional digits. Examples: "2014-10-02T15:01:23Z" and "2014-10-02T15:01:23.045123456Z".
        /// </summary>
        public DateTime? VersionUpdateTime { get; set; }
        /// <summary>
        /// "sourceType": "GENIE"
        /// </summary>
        public dynamic? ModelSourceInfo { get; set; }
        /// <summary>
        /// "genieSource": {}
        /// </summary>
        public dynamic? BaseModelSource { get; set; }
    }
    
    /// <summary>
    /// Tuned model as a source for training a new model.
    /// </summary>
    public class TunedModelSource
    {
        /// <summary>
        /// Immutable. The name of the TunedModel to use as the starting point for training the new model. Example: tunedModels/my-tuned-model
        /// </summary>
        public string TunedModel { get; set; }
        /// <summary>
        /// Output only. The name of the base Model this TunedModel was tuned from. Example: models/text-bison-001
        /// </summary>
        public string BaseModel { get; set; }
    }
}