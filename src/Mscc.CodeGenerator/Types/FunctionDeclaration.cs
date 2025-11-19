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
	/// Structured representation of a function declaration as defined by the [OpenAPI 3.03 specification](https://spec.openapis.org/oas/v3.0.3). Included in this declaration are the function name and parameters. This FunctionDeclaration is a representation of a block of code that can be used as a <see cref="Tool"/> by the model and executed by the client.
	/// </summary>
	public partial class FunctionDeclaration
	{
		/// <summary>
		/// Optional. Specifies the function Behavior. Currently only supported by the BidiGenerateContent method.
		/// </summary>
		public Behavior? Behavior { get; set; }
		/// <summary>
		/// Required. A brief description of the function.
		/// </summary>
		public string? Description { get; set; }
		/// <summary>
		/// Required. The name of the function. Must be a-z, A-Z, 0-9, or contain underscores, colons, dots, and dashes, with a maximum length of 64.
		/// </summary>
		public string? Name { get; set; }
		/// <summary>
		/// Optional. Describes the parameters to this function. Reflects the Open API 3.03 Parameter Object string Key: the name of the parameter. Parameter names are case sensitive. Schema Value: the Schema defining the type used for the parameter.
		/// </summary>
		public Schema? Parameters { get; set; }
		/// <summary>
		/// Optional. Describes the parameters to the function in JSON Schema format. The schema must describe an object where the properties are the parameters to the function. For example: ``<see cref=" { "type": "object", "properties": { "name": { "type": "string" }, "age": { "type": "integer" } }, "additionalProperties": false, "required": ["name", "age"], "propertyOrdering": ["name", "age"] } "/>`<see cref=" This field is mutually exclusive with "/>parameters`.
		/// </summary>
		public object? ParametersJsonSchema { get; set; }
		/// <summary>
		/// Optional. Describes the output from this function in JSON Schema format. Reflects the Open API 3.03 Response Object. The Schema defines the type used for the response value of the function.
		/// </summary>
		public Schema? Response { get; set; }
		/// <summary>
		/// Optional. Describes the output from this function in JSON Schema format. The value specified by the schema is the response value of the function. This field is mutually exclusive with <see cref="response"/>.
		/// </summary>
		public object? ResponseJsonSchema { get; set; }
    }
}
