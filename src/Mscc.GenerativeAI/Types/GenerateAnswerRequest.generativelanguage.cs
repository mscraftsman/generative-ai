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
	/// Request to generate a grounded answer from the <c>Model</c>.
	/// </summary>
	public partial class GenerateAnswerRequest
	{
		/// <summary>
		/// Required. Style in which answers should be returned.
		/// </summary>
		public AnswerStyle? AnswerStyle { get; set; }
		/// <summary>
		/// Required. The content of the current conversation with the <c>Model</c>. For single-turn queries, this is a single question to answer. For multi-turn queries, this is a repeated field that contains conversation history and the last <c>Content</c> in the list containing the question. Note: <c>GenerateAnswer</c> only supports queries in English.
		/// </summary>
		public List<Content>? Contents { get; set; }
		/// <summary>
		/// Passages provided inline with the request.
		/// </summary>
		public GroundingPassages? InlinePassages { get; set; }
		/// <summary>
		/// Optional. A list of unique <c>SafetySetting</c> instances for blocking unsafe content. This will be enforced on the <c>GenerateAnswerRequest.contents</c> and <c>GenerateAnswerResponse.candidate</c>. There should not be more than one setting for each <c>SafetyCategory</c> type. The API will block any contents and responses that fail to meet the thresholds set by these settings. This list overrides the default settings for each <c>SafetyCategory</c> specified in the safety_settings. If there is no <c>SafetySetting</c> for a given <c>SafetyCategory</c> provided in the list, the API will use the default safety setting for that category. Harm categories HARM_CATEGORY_HATE_SPEECH, HARM_CATEGORY_SEXUALLY_EXPLICIT, HARM_CATEGORY_DANGEROUS_CONTENT, HARM_CATEGORY_HARASSMENT are supported. Refer to the [guide](https://ai.google.dev/gemini-api/docs/safety-settings) for detailed information on available safety settings. Also refer to the [Safety guidance](https://ai.google.dev/gemini-api/docs/safety-guidance) to learn how to incorporate safety considerations in your AI applications.
		/// </summary>
		public List<SafetySetting>? SafetySettings { get; set; }
		/// <summary>
		/// Content retrieved from resources created via the Semantic Retriever API.
		/// </summary>
		public SemanticRetrieverConfig? SemanticRetriever { get; set; }
		/// <summary>
		/// Optional. Controls the randomness of the output. Values can range from [0.0,1.0], inclusive. A value closer to 1.0 will produce responses that are more varied and creative, while a value closer to 0.0 will typically result in more straightforward responses from the model. A low temperature (~0.2) is usually recommended for Attributed-Question-Answering use cases.
		/// </summary>
		public double? Temperature { get; set; }
    }
}