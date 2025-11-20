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
	/// The result output from a <see cref="FunctionCall"/> that contains a string representing the <see cref="FunctionDeclaration.name"/> and a structured JSON object containing any output from the function is used as context to the model. This should contain the result of a<see cref="FunctionCall"/> made based on model prediction.
	/// </summary>
	public partial class FunctionResponse
	{
		/// <summary>
		/// Optional. The id of the function call this response is for. Populated by the client to match the corresponding function call <see cref="id"/>.
		/// </summary>
		public string? Id { get; set; }
		/// <summary>
		/// Required. The name of the function to call. Must be a-z, A-Z, 0-9, or contain underscores and dashes, with a maximum length of 64.
		/// </summary>
		public string? Name { get; set; }
		/// <summary>
		/// Optional. Ordered <see cref="Parts"/> that constitute a function response. Parts may have different IANA MIME types.
		/// </summary>
		public List<FunctionResponsePart>? Parts { get; set; }
		/// <summary>
		/// Required. The function response in JSON object format. Callers can use any keys of their choice that fit the function's syntax to return the function output, e.g. "output", "result", etc. In particular, if the function call failed to execute, the response can have an "error" key to return error details to the model.
		/// </summary>
		public object? Response { get; set; }
		/// <summary>
		/// Optional. Specifies how the response should be scheduled in the conversation. Only applicable to NON_BLOCKING function calls, is ignored otherwise. Defaults to WHEN_IDLE.
		/// </summary>
		public Scheduling? Scheduling { get; set; }
		/// <summary>
		/// Optional. Signals that function call continues, and more responses will be returned, turning the function call into a generator. Is only applicable to NON_BLOCKING function calls, is ignored otherwise. If set to false, future responses will not be considered. It is allowed to return empty <see cref="response"/> with <see cref="will_continue=False"/> to signal that the function call is finished. This may still trigger the model generation. To avoid triggering the generation and finish the function call, additionally set <see cref="scheduling"/> to <see cref="SILENT"/>.
		/// </summary>
		public bool? WillContinue { get; set; }
    }
}