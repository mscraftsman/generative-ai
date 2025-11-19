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
	/// Response from <see cref="ListDocuments"/> containing a paginated list of <see cref="Document"/>s. The <see cref="Document"/>s are sorted by ascending <see cref="document.create_time"/>.
	/// </summary>
	public partial class ListDocumentsResponse
	{
		/// <summary>
		/// The returned <see cref="Document"/>s.
		/// </summary>
		public List<Document>? Documents { get; set; }
		/// <summary>
		/// A token, which can be sent as <see cref="page_token"/> to retrieve the next page. If this field is omitted, there are no more pages.
		/// </summary>
		public string? NextPageToken { get; set; }
    }
}
