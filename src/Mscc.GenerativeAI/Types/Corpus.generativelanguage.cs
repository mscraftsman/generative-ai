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
	/// A <c>Corpus</c> is a collection of <c>Document</c>s. A project can create up to 10 corpora.
	/// </summary>
	public partial class Corpus
	{
		/// <summary>
		/// Output only. The Timestamp of when the <c>Corpus</c> was created.
		/// </summary>
		public DateTime? CreateTime { get; set; }
		/// <summary>
		/// Optional. The human-readable display name for the <c>Corpus</c>. The display name must be no more than 512 characters in length, including spaces. Example: &quot;Docs on Semantic Retriever&quot;
		/// </summary>
		public string? DisplayName { get; set; }
		/// <summary>
		/// Output only. Immutable. Identifier. The <c>Corpus</c> resource name. The ID (name excluding the &quot;corpora/&quot; prefix) can contain up to 40 characters that are lowercase alphanumeric or dashes (-). The ID cannot start or end with a dash. If the name is empty on create, a unique name will be derived from <c>display_name</c> along with a 12 character random suffix. Example: <c>corpora/my-awesome-corpora-123a456b789c</c>
		/// </summary>
		public string? Name { get; set; }
		/// <summary>
		/// Output only. The Timestamp of when the <c>Corpus</c> was last updated.
		/// </summary>
		public DateTime? UpdateTime { get; set; }
    }
}