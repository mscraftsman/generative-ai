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
	/// Tokens info with a list of tokens and the corresponding list of token ids.
	/// </summary>
	public partial class TokensInfo
	{
		/// <summary>
		/// Optional. Optional fields for the role from the corresponding Content.
		/// </summary>
		public string? Role { get; set; }
		/// <summary>
		/// A list of token ids from the input.
		/// </summary>
		public List<long>? TokenIds { get; set; }
		/// <summary>
		/// A list of tokens from the input.
		/// </summary>
		public List<byte[]>? Tokens { get; set; }
    }
}