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
	/// Counts the number of tokens in the <c>prompt</c> sent to a model. Models may tokenize text differently, so each model may return a different <c>token_count</c>.
	/// </summary>
	public partial class CountTokensRequest
	{
		/// <summary>
		/// Optional. The input given to the model as a prompt. This field is ignored when <c>generate_content_request</c> is set.
		/// </summary>
		public List<Content>? Contents { get; set; }
		/// <summary>
		/// Optional. The overall input given to the <c>Model</c>. This includes the prompt as well as other model steering information like [system instructions](https://ai.google.dev/gemini-api/docs/system-instructions), and/or function declarations for [function calling](https://ai.google.dev/gemini-api/docs/function-calling). <c>Model</c>s/<c>Content</c>s and <c>generate_content_request</c>s are mutually exclusive. You can either send <c>Model</c> + <c>Content</c>s or a <c>generate_content_request</c>, but never both.
		/// </summary>
		public GenerateContentRequest? GenerateContentRequest { get; set; }
    }
}