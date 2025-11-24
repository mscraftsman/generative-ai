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
using System.Text.Json.Serialization;

// *** AUTO-GENERATED FILE - DO NOT EDIT MANUALLY *** //

namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Tuning Spec for Supervised Tuning for first party models.
	/// </summary>
	public partial class SupervisedTuningSpec
	{
		/// <summary>
		/// Optional. Evaluation Config for Tuning Job.
		/// </summary>
		public EvaluationConfig? EvaluationConfig { get; set; }
		/// <summary>
		/// Optional. If set to true, disable intermediate checkpoints for SFT and only the last checkpoint will be exported. Otherwise, enable intermediate checkpoints for SFT. Default is false.
		/// </summary>
		public bool? ExportLastCheckpointOnly { get; set; }
		/// <summary>
		/// Optional. Hyperparameters for SFT.
		/// </summary>
		public SupervisedHyperParameters? HyperParameters { get; set; }
		/// <summary>
		/// Required. Training dataset used for tuning. The dataset can be specified as either a Cloud Storage path to a JSONL file or as the resource name of a Vertex Multimodal Dataset.
		/// </summary>
		public string? TrainingDatasetUri { get; set; }
		/// <summary>
		/// Tuning mode.
		/// </summary>
		public TuningModeType? TuningMode { get; set; }
		/// <summary>
		/// Optional. Validation dataset used for tuning. The dataset can be specified as either a Cloud Storage path to a JSONL file or as the resource name of a Vertex Multimodal Dataset.
		/// </summary>
		public string? ValidationDatasetUri { get; set; }

		[JsonConverter(typeof(JsonStringEnumConverter<TuningModeType>))]
		public enum TuningModeType
		{
			/// <summary>
			/// Tuning mode is unspecified.
			/// </summary>
			TuningModeUnspecified,
			/// <summary>
			/// Full fine-tuning mode.
			/// </summary>
			TuningModeFull,
			/// <summary>
			/// PEFT adapter tuning mode.
			/// </summary>
			TuningModePeftAdapter,
		}
    }
}