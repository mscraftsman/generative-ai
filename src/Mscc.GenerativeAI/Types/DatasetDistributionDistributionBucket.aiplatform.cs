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
	/// Dataset bucket used to create a histogram for the distribution given a population of values.
	/// </summary>
	public partial class DatasetDistributionDistributionBucket
	{
		/// <summary>
		/// Output only. Number of values in the bucket.
		/// </summary>
		public long? Count { get; set; }
		/// <summary>
		/// Output only. Left bound of the bucket.
		/// </summary>
		public double? Left { get; set; }
		/// <summary>
		/// Output only. Right bound of the bucket.
		/// </summary>
		public double? Right { get; set; }
    }
}