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
	/// Request to generate a completion from the model.
	/// </summary>
	public partial class GenerateContentRequest
	{
		/// <summary>
		/// Optional. The name of the content [cached](https://ai.google.dev/gemini-api/docs/caching) to use as context to serve the prediction. Format: <c>cachedContents/{cachedContent}</c>
		/// </summary>
		public string? CachedContent { get; set; }
		/// <summary>
		/// Required. The content of the current conversation with the model. For single-turn queries, this is a single instance. For multi-turn queries like [chat](https://ai.google.dev/gemini-api/docs/text-generation#chat), this is a repeated field that contains the conversation history and the latest request.
		/// </summary>
		public List<Content>? Contents { get; set; }
		/// <summary>
		/// Optional. Configuration options for model generation and outputs.
		/// </summary>
		public GenerationConfig? GenerationConfig { get; set; }
		/// <summary>
		/// Required. The name of the <c>Model</c> to use for generating the completion. Format: <c>models/{model}</c>.
		/// </summary>
		public string? Model { get; set; }
		/// <summary>
		/// Optional. A list of unique <c>SafetySetting</c> instances for blocking unsafe content. This will be enforced on the <c>GenerateContentRequest.contents</c> and <c>GenerateContentResponse.candidates</c>. There should not be more than one setting for each <c>SafetyCategory</c> type. The API will block any contents and responses that fail to meet the thresholds set by these settings. This list overrides the default settings for each <c>SafetyCategory</c> specified in the safety_settings. If there is no <c>SafetySetting</c> for a given <c>SafetyCategory</c> provided in the list, the API will use the default safety setting for that category. Harm categories HARM_CATEGORY_HATE_SPEECH, HARM_CATEGORY_SEXUALLY_EXPLICIT, HARM_CATEGORY_DANGEROUS_CONTENT, HARM_CATEGORY_HARASSMENT, HARM_CATEGORY_CIVIC_INTEGRITY are supported. Refer to the [guide](https://ai.google.dev/gemini-api/docs/safety-settings) for detailed information on available safety settings. Also refer to the [Safety guidance](https://ai.google.dev/gemini-api/docs/safety-guidance) to learn how to incorporate safety considerations in your AI applications.
		/// </summary>
		public List<SafetySetting>? SafetySettings { get; set; }
		/// <summary>
		/// Optional. Developer set [system instruction(s)](https://ai.google.dev/gemini-api/docs/system-instructions). Currently, text only.
		/// </summary>
		public Content? SystemInstruction { get; set; }
		/// <summary>
		/// Optional. Tool configuration for any <c>Tool</c> specified in the request. Refer to the [Function calling guide](https://ai.google.dev/gemini-api/docs/function-calling#function_calling_mode) for a usage example.
		/// </summary>
		public ToolConfig? ToolConfig { get; set; }
		/// <summary>
		/// Optional. A list of <c>Tools</c> the <c>Model</c> may use to generate the next response. A <c>Tool</c> is a piece of code that enables the system to interact with external systems to perform an action, or set of actions, outside of knowledge and scope of the <c>Model</c>. Supported <c>Tool</c>s are <c>Function</c> and <c>code_execution</c>. Refer to the [Function calling](https://ai.google.dev/gemini-api/docs/function-calling) and the [Code execution](https://ai.google.dev/gemini-api/docs/code-execution) guides to learn more.
		/// </summary>
		public Tools? Tools { get; set; }
    }
}