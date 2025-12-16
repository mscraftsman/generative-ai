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
using System.Text.Json.Serialization;

// *** AUTO-GENERATED FILE - DO NOT EDIT MANUALLY *** //

namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// RagFile status.
	/// </summary>
	public partial class FileStatus
	{
		/// <summary>
		/// Output only. Only when the <c>state</c> field is ERROR.
		/// </summary>
		public string? ErrorStatus { get; set; }
		/// <summary>
		/// Output only. RagFile state.
		/// </summary>
		public StateType? State { get; set; }

		[JsonConverter(typeof(JsonStringEnumConverter<StateType>))]
		public enum StateType
		{
			/// <summary>
			/// RagFile state is unspecified.
			/// </summary>
			StateUnspecified,
			/// <summary>
			/// RagFile resource has been created and indexed successfully.
			/// </summary>
			Active,
			/// <summary>
			/// RagFile resource is in a problematic state. See `error_message` field for details.
			/// </summary>
			Error,
		}
    }
}