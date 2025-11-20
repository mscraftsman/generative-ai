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
	/// Configuration for specifying function calling behavior.
	/// </summary>
	public partial class FunctionCallingConfig
	{
		/// <summary>
		/// Optional. Specifies the mode in which function calling should execute. If unspecified, the default value will be set to AUTO.
		/// </summary>
		public FunctionCallingConfigMode? Mode { get; set; }
		/// <summary>
		/// Optional. When set to true, arguments of a single function call will be streamed out in
		/// multiple parts/contents/responses. Partial parameter results will be returned in the
		/// [FunctionCall.partial_args] field.
		/// </summary>
		/// <remarks>
		/// This field is not supported in Gemini API.
		/// </remarks>
		public bool? StreamFunctionCallArguments {get; set;}
    }
}