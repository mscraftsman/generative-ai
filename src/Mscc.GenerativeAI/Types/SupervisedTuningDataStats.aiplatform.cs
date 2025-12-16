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
	/// Tuning data statistics for Supervised Tuning.
	/// </summary>
	public partial class SupervisedTuningDataStats
	{
		/// <summary>
		/// Output only. For each index in <c>truncated_example_indices</c>, the user-facing reason why the example was dropped.
		/// </summary>
		public List<string>? DroppedExampleReasons { get; set; }
		/// <summary>
		/// Output only. Number of billable characters in the tuning dataset.
		/// </summary>
		public long? TotalBillableCharacterCount { get; set; }
		/// <summary>
		/// Output only. Number of billable tokens in the tuning dataset.
		/// </summary>
		public long? TotalBillableTokenCount { get; set; }
		/// <summary>
		/// Output only. The number of examples in the dataset that have been dropped. An example can be dropped for reasons including: too many tokens, contains an invalid image, contains too many images, etc.
		/// </summary>
		public long? TotalTruncatedExampleCount { get; set; }
		/// <summary>
		/// Output only. Number of tuning characters in the tuning dataset.
		/// </summary>
		public long? TotalTuningCharacterCount { get; set; }
		/// <summary>
		/// Output only. A partial sample of the indices (starting from 1) of the dropped examples.
		/// </summary>
		public List<long>? TruncatedExampleIndices { get; set; }
		/// <summary>
		/// Output only. Number of examples in the tuning dataset.
		/// </summary>
		public long? TuningDatasetExampleCount { get; set; }
		/// <summary>
		/// Output only. Number of tuning steps for this Tuning Job.
		/// </summary>
		public long? TuningStepCount { get; set; }
		/// <summary>
		/// Output only. Sample user messages in the training dataset uri.
		/// </summary>
		public List<Content>? UserDatasetExamples { get; set; }
		/// <summary>
		/// Output only. Dataset distributions for the user input tokens.
		/// </summary>
		public SupervisedTuningDatasetDistribution? UserInputTokenDistribution { get; set; }
		/// <summary>
		/// Output only. Dataset distributions for the messages per example.
		/// </summary>
		public SupervisedTuningDatasetDistribution? UserMessagePerExampleDistribution { get; set; }
		/// <summary>
		/// Output only. Dataset distributions for the user output tokens.
		/// </summary>
		public SupervisedTuningDatasetDistribution? UserOutputTokenDistribution { get; set; }
    }
}