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
	/// A predicted [FunctionCall] returned from the model that contains a string representing the [FunctionDeclaration.name] and a structured JSON object containing the parameters and their values.
	/// </summary>
	public partial class FunctionCall
	{

		/// <summary>
		/// Optional. The partial argument value of the function call. If provided, represents the arguments/fields that are streamed incrementally.
		/// </summary>
		public List<PartialArg>? PartialArgs { get; set; }
		/// <summary>
		/// Optional. Whether this is the last part of the FunctionCall. If true, another partial message for the current FunctionCall is expected to follow.
		/// </summary>
		public bool? WillContinue { get; set; }
    }
}