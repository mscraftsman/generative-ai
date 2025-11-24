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
	/// Dataset distribution for Supervised Tuning.
	/// </summary>
	public partial class SupervisedTuningDatasetDistribution
	{
		/// <summary>
		/// Output only. Sum of a given population of values that are billable.
		/// </summary>
		public long? BillableSum { get; set; }
		/// <summary>
		/// Output only. Defines the histogram bucket.
		/// </summary>
		public List<SupervisedTuningDatasetDistributionDatasetBucket>? Buckets { get; set; }
		/// <summary>
		/// Output only. The maximum of the population values.
		/// </summary>
		public double? Max { get; set; }
		/// <summary>
		/// Output only. The arithmetic mean of the values in the population.
		/// </summary>
		public double? Mean { get; set; }
		/// <summary>
		/// Output only. The median of the values in the population.
		/// </summary>
		public double? Median { get; set; }
		/// <summary>
		/// Output only. The minimum of the population values.
		/// </summary>
		public double? Min { get; set; }
		/// <summary>
		/// Output only. The 5th percentile of the values in the population.
		/// </summary>
		public double? P5 { get; set; }
		/// <summary>
		/// Output only. The 95th percentile of the values in the population.
		/// </summary>
		public double? P95 { get; set; }
		/// <summary>
		/// Output only. Sum of a given population of values.
		/// </summary>
		public long? Sum { get; set; }
    }
}