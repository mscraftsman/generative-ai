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
using System.Collections.Generic;
using System.Text.Json.Serialization;

// *** AUTO-GENERATED FILE - DO NOT EDIT MANUALLY *** //

namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// The metric used for running evaluations.
	/// </summary>
	public partial class Metric
	{
		/// <summary>
		/// Optional. The aggregation metrics to use.
		/// </summary>
		public List<AggregationMetricsType>? AggregationMetrics { get; set; }
		/// <summary>
		/// Spec for bleu metric.
		/// </summary>
		public BleuSpec? BleuSpec { get; set; }
		/// <summary>
		/// Spec for Custom Code Execution metric.
		/// </summary>
		public CustomCodeExecutionSpec? CustomCodeExecutionSpec { get; set; }
		/// <summary>
		/// Spec for exact match metric.
		/// </summary>
		public ExactMatchSpec? ExactMatchSpec { get; set; }
		/// <summary>
		/// Spec for an LLM based metric.
		/// </summary>
		public LLMBasedMetricSpec? LlmBasedMetricSpec { get; set; }
		/// <summary>
		/// Spec for pairwise metric.
		/// </summary>
		public PairwiseMetricSpec? PairwiseMetricSpec { get; set; }
		/// <summary>
		/// Spec for pointwise metric.
		/// </summary>
		public PointwiseMetricSpec? PointwiseMetricSpec { get; set; }
		/// <summary>
		/// The spec for a pre-defined metric.
		/// </summary>
		public PredefinedMetricSpec? PredefinedMetricSpec { get; set; }
		/// <summary>
		/// Spec for rouge metric.
		/// </summary>
		public RougeSpec? RougeSpec { get; set; }

		[JsonConverter(typeof(JsonStringEnumConverter<AggregationMetricsType>))]
		public enum AggregationMetricsType
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