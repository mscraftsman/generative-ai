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

// *** AUTO-GENERATED FILE - DO NOT EDIT MANUALLY *** //

namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Response from the model for a grounded answer.
	/// </summary>
	public partial class GenerateAnswerResponse
	{
		/// <summary>
		/// Candidate answer from the model. Note: The model *always* attempts to provide a grounded answer, even when the answer is unlikely to be answerable from the given passages. In that case, a low-quality or ungrounded answer may be provided, along with a low <c>answerable_probability</c>.
		/// </summary>
		public Candidate? Answer { get; set; }
		/// <summary>
		/// Output only. The model&apos;s estimate of the probability that its answer is correct and grounded in the input passages. A low <c>answerable_probability</c> indicates that the answer might not be grounded in the sources. When <c>answerable_probability</c> is low, you may want to: * Display a message to the effect of &quot;We couldn’t answer that question&quot; to the user. * Fall back to a general-purpose LLM that answers the question from world knowledge. The threshold and nature of such fallbacks will depend on individual use cases. <c>0.5</c> is a good starting threshold.
		/// </summary>
		public double? AnswerableProbability { get; set; }
		/// <summary>
		/// Output only. Feedback related to the input data used to answer the question, as opposed to the model-generated response to the question. The input data can be one or more of the following: - Question specified by the last entry in <c>GenerateAnswerRequest.content</c> - Conversation history specified by the other entries in <c>GenerateAnswerRequest.content</c> - Grounding sources (<c>GenerateAnswerRequest.semantic_retriever</c> or <c>GenerateAnswerRequest.inline_passages</c>)
		/// </summary>
		public InputFeedback? InputFeedback { get; set; }
    }
}