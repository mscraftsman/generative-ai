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
	/// Response from <c>ListFileSearchStores</c> containing a paginated list of <c>FileSearchStores</c>. The results are sorted by ascending <c>file_search_store.create_time</c>.
	/// </summary>
	public partial class ListFileSearchStoresResponse
	{
		/// <summary>
		/// The returned rag_stores.
		/// </summary>
		public List<FileSearchStore>? FileSearchStores { get; set; }
		/// <summary>
		/// A token, which can be sent as <c>page_token</c> to retrieve the next page. If this field is omitted, there are no more pages.
		/// </summary>
		public string? NextPageToken { get; set; }
    }
}