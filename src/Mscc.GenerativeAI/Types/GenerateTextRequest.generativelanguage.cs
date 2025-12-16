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
	/// Request to generate a text completion response from the model.
	/// </summary>
	public partial class GenerateTextRequest
	{
		/// <summary>
		/// Optional. Number of generated responses to return. This value must be between [1, 8], inclusive. If unset, this will default to 1.
		/// </summary>
		public int? CandidateCount { get; set; }
		/// <summary>
		/// Optional. The maximum number of tokens to include in a candidate. If unset, this will default to output_token_limit specified in the <c>Model</c> specification.
		/// </summary>
		public int? MaxOutputTokens { get; set; }
		/// <summary>
		/// Required. The free-form input text given to the model as a prompt. Given a prompt, the model will generate a TextCompletion response it predicts as the completion of the input text.
		/// </summary>
		public TextPrompt? Prompt { get; set; }
		/// <summary>
		/// Optional. A list of unique <c>SafetySetting</c> instances for blocking unsafe content. that will be enforced on the <c>GenerateTextRequest.prompt</c> and <c>GenerateTextResponse.candidates</c>. There should not be more than one setting for each <c>SafetyCategory</c> type. The API will block any prompts and responses that fail to meet the thresholds set by these settings. This list overrides the default settings for each <c>SafetyCategory</c> specified in the safety_settings. If there is no <c>SafetySetting</c> for a given <c>SafetyCategory</c> provided in the list, the API will use the default safety setting for that category. Harm categories HARM_CATEGORY_DEROGATORY, HARM_CATEGORY_TOXICITY, HARM_CATEGORY_VIOLENCE, HARM_CATEGORY_SEXUAL, HARM_CATEGORY_MEDICAL, HARM_CATEGORY_DANGEROUS are supported in text service.
		/// </summary>
		public List<SafetySetting>? SafetySettings { get; set; }
		/// <summary>
		/// The set of character sequences (up to 5) that will stop output generation. If specified, the API will stop at the first appearance of a stop sequence. The stop sequence will not be included as part of the response.
		/// </summary>
		public List<string>? StopSequences { get; set; }
		/// <summary>
		/// Optional. Controls the randomness of the output. Note: The default value varies by model, see the <c>Model.temperature</c> attribute of the <c>Model</c> returned the <c>getModel</c> function. Values can range from [0.0,1.0], inclusive. A value closer to 1.0 will produce responses that are more varied and creative, while a value closer to 0.0 will typically result in more straightforward responses from the model.
		/// </summary>
		public double? Temperature { get; set; }
		/// <summary>
		/// Optional. The maximum number of tokens to consider when sampling. The model uses combined Top-k and nucleus sampling. Top-k sampling considers the set of <c>top_k</c> most probable tokens. Defaults to 40. Note: The default value varies by model, see the <c>Model.top_k</c> attribute of the <c>Model</c> returned the <c>getModel</c> function.
		/// </summary>
		public int? TopK { get; set; }
		/// <summary>
		/// Optional. The maximum cumulative probability of tokens to consider when sampling. The model uses combined Top-k and nucleus sampling. Tokens are sorted based on their assigned probabilities so that only the most likely tokens are considered. Top-k sampling directly limits the maximum number of tokens to consider, while Nucleus sampling limits number of tokens based on the cumulative probability. Note: The default value varies by model, see the <c>Model.top_p</c> attribute of the <c>Model</c> returned the <c>getModel</c> function.
		/// </summary>
		public double? TopP { get; set; }
    }
}