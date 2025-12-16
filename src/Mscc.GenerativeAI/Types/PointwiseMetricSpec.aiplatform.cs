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
	/// Spec for pointwise metric.
	/// </summary>
	public partial class PointwiseMetricSpec
	{
		/// <summary>
		/// Optional. CustomOutputFormatConfig allows customization of metric output. By default, metrics return a score and explanation. When this config is set, the default output is replaced with either: - The raw output string. - A parsed output based on a user-defined schema. If a custom format is chosen, the <c>score</c> and <c>explanation</c> fields in the corresponding metric result will be empty.
		/// </summary>
		public CustomOutputFormatConfig? CustomOutputFormatConfig { get; set; }
		/// <summary>
		/// Required. Metric prompt template for pointwise metric.
		/// </summary>
		public string? MetricPromptTemplate { get; set; }
		/// <summary>
		/// Optional. System instructions for pointwise metric.
		/// </summary>
		public string? SystemInstruction { get; set; }
    }
}