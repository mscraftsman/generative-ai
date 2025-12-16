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
	/// A ContentsExample to be used with GenerateContent alongside information required for storage and retrieval with Example Store.
	/// </summary>
	public partial class StoredContentsExample
	{
		/// <summary>
		/// Required. The example to be used with GenerateContent.
		/// </summary>
		public ContentsExample? ContentsExample { get; set; }
		/// <summary>
		/// Optional. (Optional) the search key used for retrieval. If not provided at upload-time, the search key will be generated from <c>contents_example.contents</c> using the method provided by <c>search_key_generation_method</c>. The generated search key will be included in retrieved examples.
		/// </summary>
		public string? SearchKey { get; set; }
		/// <summary>
		/// Optional. The method used to generate the search key from <c>contents_example.contents</c>. This is ignored when uploading an example if <c>search_key</c> is provided.
		/// </summary>
		public StoredContentsExampleSearchKeyGenerationMethod? SearchKeyGenerationMethod { get; set; }
    }
}