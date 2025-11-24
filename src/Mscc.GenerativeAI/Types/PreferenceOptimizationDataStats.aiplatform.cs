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
	/// Statistics computed for datasets used for preference optimization.
	/// </summary>
	public partial class PreferenceOptimizationDataStats
	{
		/// <summary>
		/// Output only. A partial sample of the indices (starting from 1) of the dropped examples.
		/// </summary>
		public List<long>? DroppedExampleIndices { get; set; }
		/// <summary>
		/// Output only. For each index in <c>dropped_example_indices</c>, the user-facing reason why the example was dropped.
		/// </summary>
		public List<string>? DroppedExampleReasons { get; set; }
		/// <summary>
		/// Output only. Dataset distributions for scores variance per example.
		/// </summary>
		public DatasetDistribution? ScoreVariancePerExampleDistribution { get; set; }
		/// <summary>
		/// Output only. Dataset distributions for scores.
		/// </summary>
		public DatasetDistribution? ScoresDistribution { get; set; }
		/// <summary>
		/// Output only. Number of billable tokens in the tuning dataset.
		/// </summary>
		public long? TotalBillableTokenCount { get; set; }
		/// <summary>
		/// Output only. Number of examples in the tuning dataset.
		/// </summary>
		public long? TuningDatasetExampleCount { get; set; }
		/// <summary>
		/// Output only. Number of tuning steps for this Tuning Job.
		/// </summary>
		public long? TuningStepCount { get; set; }
		/// <summary>
		/// Output only. Sample user examples in the training dataset.
		/// </summary>
		public List<GeminiPreferenceExample>? UserDatasetExamples { get; set; }
		/// <summary>
		/// Output only. Dataset distributions for the user input tokens.
		/// </summary>
		public DatasetDistribution? UserInputTokenDistribution { get; set; }
		/// <summary>
		/// Output only. Dataset distributions for the user output tokens.
		/// </summary>
		public DatasetDistribution? UserOutputTokenDistribution { get; set; }
    }
}