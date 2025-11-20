/*
 * Copyright 2024-2025 Jochen Kirstätter
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

namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Configuration options for model generation and outputs. Not all parameters are configurable for every model.
	/// </summary>
	public partial class GenerationConfig : BaseConfig
	{
		/// <summary>
		/// Optional. If enabled, the model will detect emotions and adapt its responses accordingly.
		/// For example, if the model detects that the user is frustrated, it may provide a more
		/// empathetic response. This field is not supported in Gemini API.
		/// </summary>
		public bool? EnableAffectiveDialog { get; set; }
		/// <summary>
		/// Optional. Config for model selection.
		/// </summary>
		public ModelSelectionConfig? ModelSelectionConfig { get; set; }
		/// <summary>
		/// Optional. The number of candidate responses to generate. A higher `candidate_count` can
		/// provide more options to choose from, but it also consumes more resources. This can be useful
		/// for generating a variety of responses and selecting the best one.
		/// </summary>
		public bool? AudioTimestamp { get; set; }
		/// <summary>
		/// Optional. Output schema of the generated response.
		/// This is an alternative to `response_schema` that accepts [JSON Schema](https://json-schema.org/).
		/// If set, `response_schema` must be omitted, but `response_mime_type` is required.
		/// While the full JSON Schema may be sent, not all features are supported.
		/// Specifically, only the following properties are supported:
		/// - `$id`
		/// - `$defs`
		/// - `$ref`
		/// - `$anchor`
		/// - `type`
		/// - `format`
		/// - `title`
		/// - `description`
		/// - `enum` (for strings and numbers)
		/// - `items`
		/// - `prefixItems`
		/// - `minItems`
		/// - `maxItems`
		/// - `minimum`
		/// - `maximum`
		/// - `anyOf`
		/// - `oneOf` (interpreted the same as `anyOf`)
		/// - `properties`
		/// - `additionalProperties`
		/// - `required`
		/// The non-standard `propertyOrdering` property may also be set.
		/// Cyclic references are unrolled to a limited degree and, as such, may only be used within non-required
		/// properties. (Nullable properties are not sufficient.) If `$ref` is set on a sub-schema, no other properties,
		/// except for than those starting as a `$`, may be set.
		/// </summary>
		public object? ResponseJsonSchema { get; set; }
		/// <summary>
		/// Optional. An internal detail. Use `responseJsonSchema` rather than this field.
		/// </summary>
		public object? ResponseJsonSchemaOrdered { get; set; }
		/// <summary>
		/// Optional. Routing configuration. This field is not supported in Gemini API.
		/// </summary>
		public RoutingConfig? RoutingConfig { get; set; }
    }
}