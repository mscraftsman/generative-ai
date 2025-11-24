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
	/// The configuration for automated routing. When automated routing is specified, the routing will be determined by the pretrained routing model and customer provided model routing preference.
	/// </summary>
	public partial class GenerationConfigRoutingConfigAutoRoutingMode
	{
		/// <summary>
		/// The model routing preference.
		/// </summary>
		public ModelRoutingPreferenceType? ModelRoutingPreference { get; set; }

		[JsonConverter(typeof(JsonStringEnumConverter<ModelRoutingPreferenceType>))]
		public enum ModelRoutingPreferenceType
		{
			/// <summary>
			/// Unspecified model routing preference.
			/// </summary>
			Unknown,
			/// <summary>
			/// The model will be selected to prioritize the quality of the response.
			/// </summary>
			PrioritizeQuality,
			/// <summary>
			/// The model will be selected to balance quality and cost.
			/// </summary>
			Balanced,
			/// <summary>
			/// The model will be selected to prioritize the cost of the request.
			/// </summary>
			PrioritizeCost,
		}
    }
}