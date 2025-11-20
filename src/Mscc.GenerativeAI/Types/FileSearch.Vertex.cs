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
using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// The FileSearch tool that retrieves knowledge from Semantic Retrieval corpora.
	/// Files are imported to Semantic Retrieval corpora using the ImportFile API.
	/// </summary>
	public partial class FileSearch
	{
		/// <summary>
		/// Optional. The configuration for the retrieval.
		/// </summary>
		public FileSearchRetrievalConfig? RetrievalConfig { get; set; }
		/// <summary>
		/// Required. Semantic retrieval resources to retrieve from. Currently only supports one corpus.
		/// In the future we may open up multiple corpora support.
		/// </summary>
		public List<RetrievalResource> RetrievalResources { get; set; }
		// /// <summary>
		// /// Optional. The number of semantic retrieval chunks to retrieve.
		// /// </summary>
		// public float? TopK { get; set; } = default;

		/// <summary>
		/// Convenience property.
		/// </summary>
		[JsonIgnore]
		public List<string>? Stores
		{
			get => FileSearchStoreNames;
			set => FileSearchStoreNames = value;
		}
		/// <summary>
		/// Convenience property.
		/// </summary>
		[JsonIgnore]
		public string? Filter
		{
			get => MetadataFilter;
			set => MetadataFilter = value;
		}
	}
}