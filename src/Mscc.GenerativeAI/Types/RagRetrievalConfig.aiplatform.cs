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
	/// Specifies the context retrieval config.
	/// </summary>
	public partial class RagRetrievalConfig
	{
		/// <summary>
		/// Optional. Config for filters.
		/// </summary>
		public RagRetrievalConfigFilter? Filter { get; set; }
		/// <summary>
		/// Optional. Config for Hybrid Search.
		/// </summary>
		public RagRetrievalConfigHybridSearch? HybridSearch { get; set; }
		/// <summary>
		/// Optional. Config for ranking and reranking.
		/// </summary>
		public RagRetrievalConfigRanking? Ranking { get; set; }
		/// <summary>
		/// Optional. The number of contexts to retrieve.
		/// </summary>
		public int? TopK { get; set; }
    }
}