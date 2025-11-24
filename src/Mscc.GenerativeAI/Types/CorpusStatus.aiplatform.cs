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
	/// RagCorpus status.
	/// </summary>
	public partial class CorpusStatus
	{
		/// <summary>
		/// Output only. Only when the <c>state</c> field is ERROR.
		/// </summary>
		public string? ErrorStatus { get; set; }
		/// <summary>
		/// Output only. RagCorpus life state.
		/// </summary>
		public StateType? State { get; set; }

		[JsonConverter(typeof(JsonStringEnumConverter<StateType>))]
		public enum StateType
		{
			/// <summary>
			/// This state is not supposed to happen.
			/// </summary>
			Unknown,
			/// <summary>
			/// RagCorpus resource entry is initialized, but hasn&apos;t done validation.
			/// </summary>
			Initialized,
			/// <summary>
			/// RagCorpus is provisioned successfully and is ready to serve.
			/// </summary>
			Active,
			/// <summary>
			/// RagCorpus is in a problematic situation. See `error_message` field for details.
			/// </summary>
			Error,
		}
    }
}