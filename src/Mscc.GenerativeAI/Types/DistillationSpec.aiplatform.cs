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
	/// Tuning Spec for Distillation.
	/// </summary>
	public partial class DistillationSpec
	{
		/// <summary>
		/// The base teacher model that is being distilled. See [Supported models](https://cloud.google.com/vertex-ai/generative-ai/docs/model-reference/tuning#supported_models).
		/// </summary>
		public string? BaseTeacherModel { get; set; }
		/// <summary>
		/// Optional. Hyperparameters for Distillation.
		/// </summary>
		public DistillationHyperParameters? HyperParameters { get; set; }
		/// <summary>
		/// Deprecated. A path in a Cloud Storage bucket, which will be treated as the root output directory of the distillation pipeline. It is used by the system to generate the paths of output artifacts.
		/// </summary>
		public string? PipelineRootDirectory { get; set; }
		/// <summary>
		/// The student model that is being tuned, e.g., &quot;google/gemma-2b-1.1-it&quot;. Deprecated. Use base_model instead.
		/// </summary>
		public string? StudentModel { get; set; }
		/// <summary>
		/// Deprecated. Cloud Storage path to file containing training dataset for tuning. The dataset must be formatted as a JSONL file.
		/// </summary>
		public string? TrainingDatasetUri { get; set; }
		/// <summary>
		/// The resource name of the Tuned teacher model. Format: <c>projects/{project}/locations/{location}/models/{model}</c>.
		/// </summary>
		public string? TunedTeacherModelSource { get; set; }
		/// <summary>
		/// Optional. Cloud Storage path to file containing validation dataset for tuning. The dataset must be formatted as a JSONL file.
		/// </summary>
		public string? ValidationDatasetUri { get; set; }
    }
}