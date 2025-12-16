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
	public partial class Example
	{

		/// <summary>
		/// Output only. Timestamp when this Example was created.
		/// </summary>
		public DateTime? CreateTime { get; set; }
		/// <summary>
		/// Optional. The display name for Example.
		/// </summary>
		public string? DisplayName { get; set; }
		/// <summary>
		/// Optional. Immutable. Unique identifier of an example. If not specified when upserting new examples, the example_id will be generated.
		/// </summary>
		public string? ExampleId { get; set; }
		/// <summary>
		/// An example of chat history and its expected outcome to be used with GenerateContent.
		/// </summary>
		public StoredContentsExample? StoredContentsExample { get; set; }
    }
}