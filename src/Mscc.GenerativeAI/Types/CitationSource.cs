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
	/// A citation to a source for a portion of a specific response.
	/// </summary>
	public partial class CitationSource
	{
		/// <summary>
		/// Optional. End of the attributed segment, exclusive.
		/// </summary>
		public int? EndIndex { get; set; }
		/// <summary>
		/// Optional. License for the GitHub project that is attributed as a source for segment. License info is required for code citations.
		/// </summary>
		public string? License { get; set; }
		/// <summary>
		/// Optional. Start of segment of the response that is attributed to this source. Index indicates the start of the segment, measured in bytes.
		/// </summary>
		public int? StartIndex { get; set; }
		/// <summary>
		/// Optional. URI that is attributed as a source for a portion of the text.
		/// </summary>
		public string? Uri { get; set; }
    }
}