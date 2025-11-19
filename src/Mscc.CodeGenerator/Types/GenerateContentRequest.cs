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
	/// Request to generate a completion from the model.
	/// </summary>
	public partial class GenerateContentRequest
	{
		/// <summary>
		/// Optional. The name of the content [cached](https://ai.google.dev/gemini-api/docs/caching) to use as context to serve the prediction. Format: <see cref="cachedContents/{cachedContent}"/>
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
		/// Required. The name of the <see cref="Model"/> to use for generating the completion. Format: <see cref="models/{model}"/>.
		/// </summary>
		public string? Model { get; set; }
		/// <summary>
		/// Optional. A list of unique <see cref="SafetySetting"/> instances for blocking unsafe content. This will be enforced on the <see cref="GenerateContentRequest.contents"/> and <see cref="GenerateContentResponse.candidates"/>. There should not be more than one setting for each <see cref="SafetyCategory"/> type. The API will block any contents and responses that fail to meet the thresholds set by these settings. This list overrides the default settings for each <see cref="SafetyCategory"/> specified in the safety_settings. If there is no <see cref="SafetySetting"/> for a given <see cref="SafetyCategory"/> provided in the list, the API will use the default safety setting for that category. Harm categories HARM_CATEGORY_HATE_SPEECH, HARM_CATEGORY_SEXUALLY_EXPLICIT, HARM_CATEGORY_DANGEROUS_CONTENT, HARM_CATEGORY_HARASSMENT, HARM_CATEGORY_CIVIC_INTEGRITY are supported. Refer to the [guide](https://ai.google.dev/gemini-api/docs/safety-settings) for detailed information on available safety settings. Also refer to the [Safety guidance](https://ai.google.dev/gemini-api/docs/safety-guidance) to learn how to incorporate safety considerations in your AI applications.
		/// </summary>
		public List<SafetySetting>? SafetySettings { get; set; }
		/// <summary>
		/// Optional. Developer set [system instruction(s)](https://ai.google.dev/gemini-api/docs/system-instructions). Currently, text only.
		/// </summary>
		public Content? SystemInstruction { get; set; }
		/// <summary>
		/// Optional. Tool configuration for any <see cref="Tool"/> specified in the request. Refer to the [Function calling guide](https://ai.google.dev/gemini-api/docs/function-calling#function_calling_mode) for a usage example.
		/// </summary>
		public ToolConfig? ToolConfig { get; set; }
		/// <summary>
		/// Optional. A list of <see cref="Tools"/> the <see cref="Model"/> may use to generate the next response. A <see cref="Tool"/> is a piece of code that enables the system to interact with external systems to perform an action, or set of actions, outside of knowledge and scope of the <see cref="Model"/>. Supported <see cref="Tool"/>s are <see cref="Function"/> and <see cref="code_execution"/>. Refer to the [Function calling](https://ai.google.dev/gemini-api/docs/function-calling) and the [Code execution](https://ai.google.dev/gemini-api/docs/code-execution) guides to learn more.
		/// </summary>
		public List<Tool>? Tools { get; set; }
    }
}