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
	/// The spec for a pre-defined metric.
	/// </summary>
	public partial class PredefinedMetricSpec
	{
		/// <summary>
		/// Required. The name of a pre-defined metric, such as &quot;instruction_following_v1&quot; or &quot;text_quality_v1&quot;.
		/// </summary>
		public string? MetricSpecName { get; set; }
		/// <summary>
		/// Optional. The parameters needed to run the pre-defined metric.
		/// </summary>
		public object? MetricSpecParameters { get; set; }
    }
}