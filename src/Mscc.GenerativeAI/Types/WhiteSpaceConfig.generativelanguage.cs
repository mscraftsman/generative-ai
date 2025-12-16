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
	/// Configuration for a white space chunking algorithm [white space delimited].
	/// </summary>
	public partial class WhiteSpaceConfig
	{
		/// <summary>
		/// Maximum number of overlapping tokens between two adjacent chunks.
		/// </summary>
		public int? MaxOverlapTokens { get; set; }
		/// <summary>
		/// Maximum number of tokens per chunk. Tokens are defined as words for this chunking algorithm. Note: we are defining tokens as words split by whitespace as opposed to the output of a tokenizer. The context window of the latest gemini embedding model as of 2025-04-17 is currently 8192 tokens. We assume that the average word is 5 characters. Therefore, we set the upper limit to 2**9, which is 512 words, or 2560 tokens, assuming worst case a character per token. This is a conservative estimate meant to prevent context window overflow.
		/// </summary>
		public int? MaxTokensPerChunk { get; set; }
    }
}