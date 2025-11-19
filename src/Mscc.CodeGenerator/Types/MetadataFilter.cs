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
	/// User provided filter to limit retrieval based on <see cref="Chunk"/> or <see cref="Document"/> level metadata values. Example (genre = drama OR genre = action): key = "document.custom_metadata.genre" conditions = [{string_value = "drama", operation = EQUAL}, {string_value = "action", operation = EQUAL}]
	/// </summary>
	public partial class MetadataFilter
	{
		/// <summary>
		/// Required. The <see cref="Condition"/>s for the given key that will trigger this filter. Multiple <see cref="Condition"/>s are joined by logical ORs.
		/// </summary>
		public List<Condition>? Conditions { get; set; }
		/// <summary>
		/// Required. The key of the metadata to filter on.
		/// </summary>
		public string? Key { get; set; }
    }
}