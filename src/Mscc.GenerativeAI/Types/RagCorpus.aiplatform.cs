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
using System;

// *** AUTO-GENERATED FILE - DO NOT EDIT MANUALLY *** //

namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// A RagCorpus is a RagFile container and a project can have multiple RagCorpora.
	/// </summary>
	public partial class RagCorpus
	{
		/// <summary>
		/// Output only. RagCorpus state.
		/// </summary>
		public CorpusStatus? CorpusStatus { get; set; }
		/// <summary>
		/// Optional. The corpus type config of the RagCorpus.
		/// </summary>
		public RagCorpusCorpusTypeConfig? CorpusTypeConfig { get; set; }
		/// <summary>
		/// Output only. Timestamp when this RagCorpus was created.
		/// </summary>
		public DateTime? CreateTime { get; set; }
		/// <summary>
		/// Optional. The description of the RagCorpus.
		/// </summary>
		public string? Description { get; set; }
		/// <summary>
		/// Required. The display name of the RagCorpus. The name can be up to 128 characters long and can consist of any UTF-8 characters.
		/// </summary>
		public string? DisplayName { get; set; }
		/// <summary>
		/// Optional. Immutable. The CMEK key name used to encrypt at-rest data related to this Corpus. Only applicable to RagManagedDb option for Vector DB. This field can only be set at corpus creation time, and cannot be updated or deleted.
		/// </summary>
		public EncryptionSpec? EncryptionSpec { get; set; }
		/// <summary>
		/// Output only. The resource name of the RagCorpus.
		/// </summary>
		public string? Name { get; set; }
		/// <summary>
		/// Optional. Immutable. The embedding model config of the RagCorpus.
		/// </summary>
		public RagEmbeddingModelConfig? RagEmbeddingModelConfig { get; set; }
		/// <summary>
		/// Output only. Number of RagFiles in the RagCorpus. NOTE: This field is not populated in the response of VertexRagDataService.ListRagCorpora.
		/// </summary>
		public int? RagFilesCount { get; set; }
		/// <summary>
		/// Optional. Immutable. The Vector DB config of the RagCorpus.
		/// </summary>
		public RagVectorDbConfig? RagVectorDbConfig { get; set; }
		/// <summary>
		/// Output only. Reserved for future use.
		/// </summary>
		public bool? SatisfiesPzi { get; set; }
		/// <summary>
		/// Output only. Reserved for future use.
		/// </summary>
		public bool? SatisfiesPzs { get; set; }
		/// <summary>
		/// Output only. Timestamp when this RagCorpus was last updated.
		/// </summary>
		public DateTime? UpdateTime { get; set; }
		/// <summary>
		/// Optional. Immutable. The config for the Vector DBs.
		/// </summary>
		public RagVectorDbConfig? VectorDbConfig { get; set; }
		/// <summary>
		/// Optional. Immutable. The config for the Vertex AI Search.
		/// </summary>
		public VertexAiSearchConfig? VertexAiSearchConfig { get; set; }
    }
}