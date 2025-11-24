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
	/// Tuning Spec for Preference Optimization.
	/// </summary>
	public partial class PreferenceOptimizationSpec
	{
		/// <summary>
		/// Optional. If set to true, disable intermediate checkpoints for Preference Optimization and only the last checkpoint will be exported. Otherwise, enable intermediate checkpoints for Preference Optimization. Default is false.
		/// </summary>
		public bool? ExportLastCheckpointOnly { get; set; }
		/// <summary>
		/// Optional. Hyperparameters for Preference Optimization.
		/// </summary>
		public PreferenceOptimizationHyperParameters? HyperParameters { get; set; }
		/// <summary>
		/// Required. Cloud Storage path to file containing training dataset for preference optimization tuning. The dataset must be formatted as a JSONL file.
		/// </summary>
		public string? TrainingDatasetUri { get; set; }
		/// <summary>
		/// Optional. Cloud Storage path to file containing validation dataset for preference optimization tuning. The dataset must be formatted as a JSONL file.
		/// </summary>
		public string? ValidationDatasetUri { get; set; }
    }
}