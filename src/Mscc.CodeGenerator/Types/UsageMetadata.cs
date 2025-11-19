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
	/// Metadata on the generation request's token usage.
	/// </summary>
	public partial class UsageMetadata
	{
		/// <summary>
		/// Output only. List of modalities of the cached content in the request input.
		/// </summary>
		public List<ModalityTokenCount>? CacheTokensDetails { get; set; }
		/// <summary>
		/// Number of tokens in the cached part of the prompt (the cached content)
		/// </summary>
		public int? CachedContentTokenCount { get; set; }
		/// <summary>
		/// Total number of tokens across all the generated response candidates.
		/// </summary>
		public int? CandidatesTokenCount { get; set; }
		/// <summary>
		/// Output only. List of modalities that were returned in the response.
		/// </summary>
		public List<ModalityTokenCount>? CandidatesTokensDetails { get; set; }
		/// <summary>
		/// Number of tokens in the prompt. When <see cref="cached_content"/> is set, this is still the total effective prompt size meaning this includes the number of tokens in the cached content.
		/// </summary>
		public int? PromptTokenCount { get; set; }
		/// <summary>
		/// Output only. List of modalities that were processed in the request input.
		/// </summary>
		public List<ModalityTokenCount>? PromptTokensDetails { get; set; }
		/// <summary>
		/// Output only. Number of tokens of thoughts for thinking models.
		/// </summary>
		public int? ThoughtsTokenCount { get; set; }
		/// <summary>
		/// Output only. Number of tokens present in tool-use prompt(s).
		/// </summary>
		public int? ToolUsePromptTokenCount { get; set; }
		/// <summary>
		/// Output only. List of modalities that were processed for tool-use request inputs.
		/// </summary>
		public List<ModalityTokenCount>? ToolUsePromptTokensDetails { get; set; }
		/// <summary>
		/// Total token count for the generation request (prompt + response candidates).
		/// </summary>
		public int? TotalTokenCount { get; set; }
    }
}