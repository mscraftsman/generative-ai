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
	/// Specification for a computation based metric.
	/// </summary>
	public partial class ComputationBasedMetricSpec
	{
		/// <summary>
		/// Optional. A map of parameters for the metric, e.g. {&quot;rouge_type&quot;: &quot;rougeL&quot;}.
		/// </summary>
		public object? Parameters { get; set; }
		/// <summary>
		/// Required. The type of the computation based metric.
		/// </summary>
		public ComputationBasedMetricSpecType? Type { get; set; }

		[JsonConverter(typeof(JsonStringEnumConverter<ComputationBasedMetricSpecType>))]
		public enum ComputationBasedMetricSpecType
		{
			/// <summary>
			/// Unspecified computation based metric type.
			/// </summary>
			ComputationBasedMetricTypeUnspecified,
			/// <summary>
			/// Exact match metric.
			/// </summary>
			ExactMatch,
			/// <summary>
			/// BLEU metric.
			/// </summary>
			Bleu,
			/// <summary>
			/// ROUGE metric.
			/// </summary>
			Rouge,
		}
    }
}