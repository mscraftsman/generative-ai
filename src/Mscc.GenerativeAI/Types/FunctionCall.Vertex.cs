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

using System.Collections.Generic;

namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// A predicted <see cref="FunctionCall"/> returned from the model that contains a string representing the <see cref="FunctionDeclaration.name"/> with the arguments and their values.
	/// </summary>
	public partial class FunctionCall : IPart
	{
		/// <summary>
		/// Optional. The partial argument value of the function call. If provided, represents the
		/// arguments/fields that are streamed incrementally.
		/// </summary>
		/// <remarks>
		/// This field is not supported in Gemini API.
		/// </remarks>
		public List<PartialArg>? PartialArgs { get; set; }
		/// <summary>
		/// Optional. Whether this is the last part of the FunctionCall. If true, another partial
		/// message for the current FunctionCall is expected to follow.
		/// </summary>
		/// <remarks>
		/// This field is not supported in Gemini API.
		/// </remarks>
		public bool? WillContinue { get; set; }
    }
}