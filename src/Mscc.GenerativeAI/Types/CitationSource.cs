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

using System;

namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// A citation to a source for a portion of a specific response.
	/// </summary>
	public partial class CitationSource
	{
		/// <summary>
		/// Output only. The title of the source of the citation.
		/// </summary>
		public string? Title { get; internal set; }
		/// <summary>
		/// Output only. The publication date of the source of the citation.
		/// </summary>
		public DateTimeOffset? PublicationDate { get; internal set; }
	}
}