using System.Collections.Generic;

namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Information about a Generative Language Model.
	/// </summary>
	public partial class Model
	{
		/// <summary>
		/// Required. The name of the base model, pass this to the generation request. Examples: * <see cref="gemini-1.5-flash"/>
		/// </summary>
		public string? BaseModelId { get; set; }
		/// <summary>
		/// A short description of the model.
		/// </summary>
		public string? Description { get; set; }
		/// <summary>
		/// The human-readable name of the model. E.g. "Gemini 1.5 Flash". The name can be up to 128 characters long and can consist of any UTF-8 characters.
		/// </summary>
		public string? DisplayName { get; set; }
		/// <summary>
		/// Maximum number of input tokens allowed for this model.
		/// </summary>
		public int? InputTokenLimit { get; set; }
		/// <summary>
		/// The maximum temperature this model can use.
		/// </summary>
		public double? MaxTemperature { get; set; }
		/// <summary>
		/// Required. The resource name of the <see cref="Model"/>. Refer to [Model variants](https://ai.google.dev/gemini-api/docs/models/gemini#model-variations) for all allowed values. Format: <see cref="models/{model}"/> with a <see cref="{model}"/> naming convention of: * "{base_model_id}-{version}" Examples: * <see cref="models/gemini-1.5-flash-001"/>
		/// </summary>
		public string? Name { get; set; }
		/// <summary>
		/// Maximum number of output tokens available for this model.
		/// </summary>
		public int? OutputTokenLimit { get; set; }
		/// <summary>
		/// The model's supported generation methods. The corresponding API method names are defined as Pascal case strings, such as <see cref="generateMessage"/> and <see cref="generateContent"/>.
		/// </summary>
		public List<string>? SupportedGenerationMethods { get; set; }
		/// <summary>
		/// Controls the randomness of the output. Values can range over <see cref="[0.0,max_temperature]"/>, inclusive. A higher value will produce responses that are more varied, while a value closer to <see cref="0.0"/> will typically result in less surprising responses from the model. This value specifies default to be used by the backend while making the call to the model.
		/// </summary>
		public double? Temperature { get; set; }
		/// <summary>
		/// Whether the model supports thinking.
		/// </summary>
		public bool? Thinking { get; set; }
		/// <summary>
		/// For Top-k sampling. Top-k sampling considers the set of <see cref="top_k"/> most probable tokens. This value specifies default to be used by the backend while making the call to the model. If empty, indicates the model doesn't use top-k sampling, and <see cref="top_k"/> isn't allowed as a generation parameter.
		/// </summary>
		public int? TopK { get; set; }
		/// <summary>
		/// For [Nucleus sampling](https://ai.google.dev/gemini-api/docs/prompting-strategies#top-p). Nucleus sampling considers the smallest set of tokens whose probability sum is at least <see cref="top_p"/>. This value specifies default to be used by the backend while making the call to the model.
		/// </summary>
		public double? TopP { get; set; }
		/// <summary>
		/// Required. The version number of the model. This represents the major version (<see cref="1.0"/> or <see cref="1.5"/>)
		/// </summary>
		public string? Version { get; set; }
    }
}
