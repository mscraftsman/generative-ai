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

// *** AUTO-GENERATED FILE - DO NOT EDIT MANUALLY *** //

namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Spec for pairwise metric.
	/// </summary>
	public partial class PairwiseMetricSpec
	{
		/// <summary>
		/// Optional. The field name of the baseline response.
		/// </summary>
		public string? BaselineResponseFieldName { get; set; }
		/// <summary>
		/// Optional. The field name of the candidate response.
		/// </summary>
		public string? CandidateResponseFieldName { get; set; }
		/// <summary>
		/// Optional. CustomOutputFormatConfig allows customization of metric output. When this config is set, the default output is replaced with the raw output string. If a custom format is chosen, the <c>pairwise_choice</c> and <c>explanation</c> fields in the corresponding metric result will be empty.
		/// </summary>
		public CustomOutputFormatConfig? CustomOutputFormatConfig { get; set; }
		/// <summary>
		/// Required. Metric prompt template for pairwise metric.
		/// </summary>
		public string? MetricPromptTemplate { get; set; }
		/// <summary>
		/// Optional. System instructions for pairwise metric.
		/// </summary>
		public string? SystemInstruction { get; set; }
    }
}