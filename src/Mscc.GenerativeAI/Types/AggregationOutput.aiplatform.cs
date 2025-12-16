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

// *** AUTO-GENERATED FILE - DO NOT EDIT MANUALLY *** //

namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// The aggregation result for the entire dataset and all metrics.
	/// </summary>
	public partial class AggregationOutput
	{
		/// <summary>
		/// One AggregationResult per metric.
		/// </summary>
		public List<AggregationResult>? AggregationResults { get; set; }
		/// <summary>
		/// The dataset used for evaluation &amp; aggregation.
		/// </summary>
		public EvaluationDataset? Dataset { get; set; }
    }
}