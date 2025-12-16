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
	/// Identifier for a <c>Chunk</c> retrieved via Semantic Retriever specified in the <c>GenerateAnswerRequest</c> using <c>SemanticRetrieverConfig</c>.
	/// </summary>
	public partial class SemanticRetrieverChunk
	{
		/// <summary>
		/// Output only. Name of the <c>Chunk</c> containing the attributed text. Example: <c>corpora/123/documents/abc/chunks/xyz</c>
		/// </summary>
		public string? Chunk { get; set; }
		/// <summary>
		/// Output only. Name of the source matching the request&apos;s <c>SemanticRetrieverConfig.source</c>. Example: <c>corpora/123</c> or <c>corpora/123/documents/abc</c>
		/// </summary>
		public string? Source { get; set; }
    }
}