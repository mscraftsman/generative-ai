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
	/// The FileSearch tool that retrieves knowledge from Semantic Retrieval corpora. Files are imported to Semantic Retrieval corpora using the ImportFile API.
	/// </summary>
	public partial class FileSearch : ITool
	{
		/// <summary>
		/// Required. The names of the file_search_stores to retrieve from. Example: <see cref="fileSearchStores/my-file-search-store-123"/>
		/// </summary>
		public List<string>? FileSearchStoreNames { get; set; }
		/// <summary>
		/// Optional. Metadata filter to apply to the semantic retrieval documents and chunks.
		/// </summary>
		public string? MetadataFilter { get; set; }
		/// <summary>
		/// Optional. The number of semantic retrieval chunks to retrieve.
		/// </summary>
		public int? TopK { get; set; }
    }
}