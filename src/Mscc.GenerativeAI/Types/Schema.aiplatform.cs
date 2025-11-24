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

// *** AUTO-GENERATED FILE - DO NOT EDIT MANUALLY *** //

namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Schema is used to define the format of input/output data. Represents a select subset of an [OpenAPI 3.0 schema object](https://spec.openapis.org/oas/v3.0.3#schema-object). More fields may be added in the future as needed.
	/// </summary>
	public partial class Schema
	{

		/// <summary>
		/// Optional. Can either be a boolean or an object; controls the presence of additional properties.
		/// </summary>
		public object? AdditionalProperties { get; set; }
		/// <summary>
		/// Optional. A map of definitions for use by <c>ref</c> Only allowed at the root of the schema.
		/// </summary>
		public object? Defs { get; set; }
		/// <summary>
		/// Optional. Allows indirect references between schema nodes. The value should be a valid reference to a child of the root <c>defs</c>. For example, the following schema defines a reference to a schema node named &quot;Pet&quot;: type: object properties: pet: ref: #/defs/Pet defs: Pet: type: object properties: name: type: string The value of the &quot;pet&quot; property is a reference to the schema node named &quot;Pet&quot;. See details in https://json-schema.org/understanding-json-schema/structuring
		/// </summary>
		public string? Ref { get; set; }
    }
}