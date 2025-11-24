/*
 * Copyleft 2024-2025 Jochen Kirstätter and contributors
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      https://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System.Collections.Generic;

// *** AUTO-GENERATED FILE - DO NOT EDIT MANUALLY *** //

namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Information about a Generative Language Model.
	/// </summary>
	public partial class ModelResponse
	{
		/// <summary>
		/// Required. The name of the base model, pass this to the generation request. Examples: * <c>gemini-1.5-flash</c>
		/// </summary>
		public string? BaseModelId { get; set; }
		/// <summary>
		/// A short description of the model.
		/// </summary>
		public string? Description { get; set; }
		/// <summary>
		/// The human-readable name of the model. E.g. &quot;Gemini 1.5 Flash&quot;. The name can be up to 128 characters long and can consist of any UTF-8 characters.
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
		/// Required. The resource name of the <c>Model</c>. Refer to [Model variants](https://ai.google.dev/gemini-api/docs/models/gemini#model-variations) for all allowed values. Format: <c>models/{model}</c> with a <c>{model}</c> naming convention of: * &quot;{base_model_id}-{version}&quot; Examples: * <c>models/gemini-1.5-flash-001</c>
		/// </summary>
		public string? Name { get; set; }
		/// <summary>
		/// Maximum number of output tokens available for this model.
		/// </summary>
		public int? OutputTokenLimit { get; set; }
		/// <summary>
		/// The model&apos;s supported generation methods. The corresponding API method names are defined as Pascal case strings, such as <c>generateMessage</c> and <c>generateContent</c>.
		/// </summary>
		public List<string>? SupportedGenerationMethods { get; set; }
		/// <summary>
		/// Controls the randomness of the output. Values can range over <c>[0.0,max_temperature]</c>, inclusive. A higher value will produce responses that are more varied, while a value closer to <c>0.0</c> will typically result in less surprising responses from the model. This value specifies default to be used by the backend while making the call to the model.
		/// </summary>
		public double? Temperature { get; set; }
		/// <summary>
		/// Whether the model supports thinking.
		/// </summary>
		public bool? Thinking { get; set; }
		/// <summary>
		/// For Top-k sampling. Top-k sampling considers the set of <c>top_k</c> most probable tokens. This value specifies default to be used by the backend while making the call to the model. If empty, indicates the model doesn&apos;t use top-k sampling, and <c>top_k</c> isn&apos;t allowed as a generation parameter.
		/// </summary>
		public int? TopK { get; set; }
		/// <summary>
		/// For [Nucleus sampling](https://ai.google.dev/gemini-api/docs/prompting-strategies#top-p). Nucleus sampling considers the smallest set of tokens whose probability sum is at least <c>top_p</c>. This value specifies default to be used by the backend while making the call to the model.
		/// </summary>
		public double? TopP { get; set; }
		/// <summary>
		/// Required. The version number of the model. This represents the major version (<c>1.0</c> or <c>1.5</c>)
		/// </summary>
		public string? Version { get; set; }
    }
}