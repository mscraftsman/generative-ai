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
	/// Config for ANN search. RagManagedDb uses a tree-based structure to partition data and facilitate faster searches. As a tradeoff, it requires longer indexing time and manual triggering of index rebuild via the ImportRagFiles and UpdateRagCorpus API.
	/// </summary>
	public partial class RagVectorDbConfigRagManagedDbANN
	{
		/// <summary>
		/// Number of leaf nodes in the tree-based structure. Each leaf node contains groups of closely related vectors along with their corresponding centroid. Recommended value is 10 * sqrt(num of RagFiles in your RagCorpus). Default value is 500.
		/// </summary>
		public int? LeafCount { get; set; }
		/// <summary>
		/// The depth of the tree-based structure. Only depth values of 2 and 3 are supported. Recommended value is 2 if you have if you have O(10K) files in the RagCorpus and set this to 3 if more than that. Default value is 2.
		/// </summary>
		public int? TreeDepth { get; set; }
    }
}