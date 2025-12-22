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
	/// Defines the schema of input and output data. This is a subset of the [OpenAPI 3.0 Schema Object](https://spec.openapis.org/oas/v3.0.3#schema-object).
	/// </summary>
	public partial class Schema
	{

		/// <summary>
		/// Optional. If <c>type</c> is <c>OBJECT</c>, specifies how to handle properties not defined in <c>properties</c>. If it is a boolean <c>false</c>, no additional properties are allowed. If it is a schema, additional properties are allowed if they conform to the schema.
		/// </summary>
		public object? AdditionalProperties { get; set; }
		/// <summary>
		/// Optional. <c>defs</c> provides a map of schema definitions that can be reused by <c>ref</c> elsewhere in the schema. Only allowed at root level of the schema.
		/// </summary>
		public object? Defs { get; set; }
		/// <summary>
		/// Optional. Allows referencing another schema definition to use in place of this schema. The value must be a valid reference to a schema in <c>defs</c>. For example, the following schema defines a reference to a schema node named &quot;Pet&quot;: type: object properties: pet: ref: #/defs/Pet defs: Pet: type: object properties: name: type: string The value of the &quot;pet&quot; property is a reference to the schema node named &quot;Pet&quot;. See details in https://json-schema.org/understanding-json-schema/structuring
		/// </summary>
		public string? Ref { get; set; }
    }
}