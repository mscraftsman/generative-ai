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
	/// A citation for a piece of generatedcontent.
	/// </summary>
	public partial class Citation
	{
		/// <summary>
		/// Output only. The end index of the citation in the content.
		/// </summary>
		public int? EndIndex { get; set; }
		/// <summary>
		/// Output only. The license of the source of the citation.
		/// </summary>
		public string? License { get; set; }
		/// <summary>
		/// Output only. The publication date of the source of the citation.
		/// </summary>
		public GoogleTypeDate? PublicationDate { get; set; }
		/// <summary>
		/// Output only. The start index of the citation in the content.
		/// </summary>
		public int? StartIndex { get; set; }
		/// <summary>
		/// Output only. The title of the source of the citation.
		/// </summary>
		public string? Title { get; set; }
		/// <summary>
		/// Output only. The URI of the source of the citation.
		/// </summary>
		public string? Uri { get; set; }
    }
}