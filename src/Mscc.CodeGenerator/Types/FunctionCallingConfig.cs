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
	/// Configuration for specifying function calling behavior.
	/// </summary>
	public partial class FunctionCallingConfig
	{
		/// <summary>
		/// Optional. A set of function names that, when provided, limits the functions the model will call. This should only be set when the Mode is ANY or VALIDATED. Function names should match [FunctionDeclaration.name]. When set, model will predict a function call from only allowed function names.
		/// </summary>
		public List<string>? AllowedFunctionNames { get; set; }
		/// <summary>
		/// Optional. Specifies the mode in which function calling should execute. If unspecified, the default value will be set to AUTO.
		/// </summary>
		public Mode? Mode { get; set; }
    }
}