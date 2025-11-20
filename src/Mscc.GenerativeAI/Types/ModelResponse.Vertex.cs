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
}