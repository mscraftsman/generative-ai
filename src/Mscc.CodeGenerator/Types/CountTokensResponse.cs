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
	/// A response from <see cref="CountTokens"/>. It returns the model's <see cref="token_count"/> for the <see cref="prompt"/>.
	/// </summary>
	public partial class CountTokensResponse
	{
		/// <summary>
		/// Output only. List of modalities that were processed in the cached content.
		/// </summary>
		public List<ModalityTokenCount>? CacheTokensDetails { get; set; }
		/// <summary>
		/// Number of tokens in the cached part of the prompt (the cached content).
		/// </summary>
		public int? CachedContentTokenCount { get; set; }
		/// <summary>
		/// Output only. List of modalities that were processed in the request input.
		/// </summary>
		public List<ModalityTokenCount>? PromptTokensDetails { get; set; }
		/// <summary>
		/// The number of tokens that the <see cref="Model"/> tokenizes the <see cref="prompt"/> into. Always non-negative.
		/// </summary>
		public int? TotalTokens { get; set; }
    }
}
