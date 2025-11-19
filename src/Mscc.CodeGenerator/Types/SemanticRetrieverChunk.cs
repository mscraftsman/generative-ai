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

namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Identifier for a <see cref="Chunk"/> retrieved via Semantic Retriever specified in the <see cref="GenerateAnswerRequest"/> using <see cref="SemanticRetrieverConfig"/>.
	/// </summary>
	public partial class SemanticRetrieverChunk
	{
		/// <summary>
		/// Output only. Name of the <see cref="Chunk"/> containing the attributed text. Example: <see cref="corpora/123/documents/abc/chunks/xyz"/>
		/// </summary>
		public string? Chunk { get; set; }
		/// <summary>
		/// Output only. Name of the source matching the request's <see cref="SemanticRetrieverConfig.source"/>. Example: <see cref="corpora/123"/> or <see cref="corpora/123/documents/abc"/>
		/// </summary>
		public string? Source { get; set; }
    }
}
