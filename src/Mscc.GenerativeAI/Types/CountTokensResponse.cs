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
	/// A response from `CountTokens`. It returns the model's `token_count` for the `prompt`.
	/// </summary>
	public partial class CountTokensResponse
	{
		private int _totalTokens;
		private int _totalCachedTokens;

		/// <summary>
		/// The number of tokens that the <see cref="Model"/> tokenizes the <see cref="prompt"/> into. Always non-negative.
		/// </summary>
		public int TotalTokens
		{
			get => _totalTokens;
			set => _totalTokens = value < 0 ? 0 : value < int.MaxValue ? value : int.MaxValue;
		}

		/// <summary>
		/// The total number of tokens counted across all instances from the request.
		/// </summary>
		public int TokenCount
		{
			get => _totalTokens;
			set => _totalTokens = value < 0 ? 0 : value < int.MaxValue ? value : int.MaxValue;
		}
		
		/// <summary>
		/// Number of tokens in the cached part of the prompt (the cached content).
		/// </summary>
		public int CachedContentTokenCount
		{
			get => _totalCachedTokens;
			set => _totalCachedTokens = value < 0 ? 0 : value < int.MaxValue ? value : int.MaxValue;
		}
	}
}