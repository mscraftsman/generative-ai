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
	/// Response for <c>ListFiles</c>.
	/// </summary>
	public partial class ListFilesResponse
	{
		/// <summary>
		/// The list of <c>File</c>s.
		/// </summary>
		public List<FileResource>? Files { get; set; }
		/// <summary>
		/// A token that can be sent as a <c>page_token</c> into a subsequent <c>ListFiles</c> call.
		/// </summary>
		public string? NextPageToken { get; set; }
    }
}