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
	/// The aggregation result for a single metric.
	/// </summary>
	public partial class AggregationResult
	{
		/// <summary>
		/// Aggregation metric.
		/// </summary>
		public AggregationMetricType? AggregationMetric { get; set; }
		/// <summary>
		/// Results for bleu metric.
		/// </summary>
		public BleuMetricValue? BleuMetricValue { get; set; }
		/// <summary>
		/// Result for code execution metric.
		/// </summary>
		public CustomCodeExecutionResult? CustomCodeExecutionResult { get; set; }
		/// <summary>
		/// Results for exact match metric.
		/// </summary>
		public ExactMatchMetricValue? ExactMatchMetricValue { get; set; }
		/// <summary>
		/// Result for pairwise metric.
		/// </summary>
		public PairwiseMetricResult? PairwiseMetricResult { get; set; }
		/// <summary>
		/// Result for pointwise metric.
		/// </summary>
		public PointwiseMetricResult? PointwiseMetricResult { get; set; }
		/// <summary>
		/// Results for rouge metric.
		/// </summary>
		public RougeMetricValue? RougeMetricValue { get; set; }

		[JsonConverter(typeof(JsonStringEnumConverter<AggregationMetricType>))]
		public enum AggregationMetricType
		{
			/// <summary>
			/// Unspecified aggregation metric.
			/// </summary>
			AggregationMetricUnspecified,
			/// <summary>
			/// Average aggregation metric. Not supported for Pairwise metric.
			/// </summary>
			Average,
			/// <summary>
			/// Mode aggregation metric.
			/// </summary>
			Mode,
			/// <summary>
			/// Standard deviation aggregation metric. Not supported for pairwise metric.
			/// </summary>
			StandardDeviation,
			/// <summary>
			/// Variance aggregation metric. Not supported for pairwise metric.
			/// </summary>
			Variance,
			/// <summary>
			/// Minimum aggregation metric. Not supported for pairwise metric.
			/// </summary>
			Minimum,
			/// <summary>
			/// Maximum aggregation metric. Not supported for pairwise metric.
			/// </summary>
			Maximum,
			/// <summary>
			/// Median aggregation metric. Not supported for pairwise metric.
			/// </summary>
			Median,
			/// <summary>
			/// 90th percentile aggregation metric. Not supported for pairwise metric.
			/// </summary>
			PercentileP90,
			/// <summary>
			/// 95th percentile aggregation metric. Not supported for pairwise metric.
			/// </summary>
			PercentileP95,
			/// <summary>
			/// 99th percentile aggregation metric. Not supported for pairwise metric.
			/// </summary>
			PercentileP99,
		}
    }
}