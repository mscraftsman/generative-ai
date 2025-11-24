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
	/// Preset configuration for example-based explanations
	/// </summary>
	public partial class Presets
	{
		/// <summary>
		/// The modality of the uploaded model, which automatically configures the distance measurement and feature normalization for the underlying example index and queries. If your model does not precisely fit one of these types, it is okay to choose the closest type.
		/// </summary>
		public ModalityType? Modality { get; set; }
		/// <summary>
		/// Preset option controlling parameters for speed-precision trade-off when querying for examples. If omitted, defaults to <c>PRECISE</c>.
		/// </summary>
		public QueryType? Query { get; set; }

		[JsonConverter(typeof(JsonStringEnumConverter<ModalityType>))]
		public enum ModalityType
		{
			/// <summary>
			/// Should not be set. Added as a recommended best practice for enums
			/// </summary>
			ModalityUnspecified,
			/// <summary>
			/// IMAGE modality
			/// </summary>
			Image,
			/// <summary>
			/// TEXT modality
			/// </summary>
			Text,
			/// <summary>
			/// TABULAR modality
			/// </summary>
			Tabular,
		}

		[JsonConverter(typeof(JsonStringEnumConverter<QueryType>))]
		public enum QueryType
		{
			/// <summary>
			/// More precise neighbors as a trade-off against slower response.
			/// </summary>
			Precise,
			/// <summary>
			/// Faster response as a trade-off against less precise neighbors.
			/// </summary>
			Fast,
		}
    }
}