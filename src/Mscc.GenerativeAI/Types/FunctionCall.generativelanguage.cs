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
	/// A predicted <c>FunctionCall</c> returned from the model that contains a string representing the <c>FunctionDeclaration.name</c> with the arguments and their values.
	/// </summary>
	public partial class FunctionCall
	{
		/// <summary>
		/// Optional. The function parameters and values in JSON object format.
		/// </summary>
		public object? Args { get; set; }
		/// <summary>
		/// Optional. The unique id of the function call. If populated, the client to execute the <c>function_call</c> and return the response with the matching <c>id</c>.
		/// </summary>
		public string? Id { get; set; }
		/// <summary>
		/// Required. The name of the function to call. Must be a-z, A-Z, 0-9, or contain underscores and dashes, with a maximum length of 64.
		/// </summary>
		public string? Name { get; set; }
    }
}