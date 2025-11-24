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
using System.Text.Json.Serialization;

// *** AUTO-GENERATED FILE - DO NOT EDIT MANUALLY *** //

namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Usage metadata about the content generation request and response. This message provides a detailed breakdown of token usage and other relevant metrics.
	/// </summary>
	public partial class GenerateContentResponseUsageMetadata
	{
		/// <summary>
		/// Output only. A detailed breakdown of the token count for each modality in the cached content.
		/// </summary>
		public List<ModalityTokenCount>? CacheTokensDetails { get; set; }
		/// <summary>
		/// Output only. The number of tokens in the cached content that was used for this request.
		/// </summary>
		public int? CachedContentTokenCount { get; set; }
		/// <summary>
		/// The total number of tokens in the generated candidates.
		/// </summary>
		public int? CandidatesTokenCount { get; set; }
		/// <summary>
		/// Output only. A detailed breakdown of the token count for each modality in the generated candidates.
		/// </summary>
		public List<ModalityTokenCount>? CandidatesTokensDetails { get; set; }
		/// <summary>
		/// The total number of tokens in the prompt. This includes any text, images, or other media provided in the request. When <c>cached_content</c> is set, this also includes the number of tokens in the cached content.
		/// </summary>
		public int? PromptTokenCount { get; set; }
		/// <summary>
		/// Output only. A detailed breakdown of the token count for each modality in the prompt.
		/// </summary>
		public List<ModalityTokenCount>? PromptTokensDetails { get; set; }
		/// <summary>
		/// Output only. The number of tokens that were part of the model&apos;s generated &quot;thoughts&quot; output, if applicable.
		/// </summary>
		public int? ThoughtsTokenCount { get; set; }
		/// <summary>
		/// Output only. The number of tokens in the results from tool executions, which are provided back to the model as input, if applicable.
		/// </summary>
		public int? ToolUsePromptTokenCount { get; set; }
		/// <summary>
		/// Output only. A detailed breakdown by modality of the token counts from the results of tool executions, which are provided back to the model as input.
		/// </summary>
		public List<ModalityTokenCount>? ToolUsePromptTokensDetails { get; set; }
		/// <summary>
		/// The total number of tokens for the entire request. This is the sum of <c>prompt_token_count</c>, <c>candidates_token_count</c>, <c>tool_use_prompt_token_count</c>, and <c>thoughts_token_count</c>.
		/// </summary>
		public int? TotalTokenCount { get; set; }
		/// <summary>
		/// Output only. The traffic type for this request.
		/// </summary>
		public GenerateContentResponseUsageMetadataTrafficType? TrafficType { get; set; }

		[JsonConverter(typeof(JsonStringEnumConverter<GenerateContentResponseUsageMetadataTrafficType>))]
		public enum GenerateContentResponseUsageMetadataTrafficType
		{
			/// <summary>
			/// Unspecified request traffic type.
			/// </summary>
			TrafficTypeUnspecified,
			/// <summary>
			/// The request was processed using Pay-As-You-Go quota.
			/// </summary>
			OnDemand,
			/// <summary>
			/// Type for Provisioned Throughput traffic.
			/// </summary>
			ProvisionedThroughput,
		}
    }
}