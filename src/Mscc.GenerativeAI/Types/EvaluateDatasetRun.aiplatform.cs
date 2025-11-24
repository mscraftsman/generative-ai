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
	/// Evaluate Dataset Run Result for Tuning Job.
	/// </summary>
	public partial class EvaluateDatasetRun
	{
		/// <summary>
		/// Output only. The checkpoint id used in the evaluation run. Only populated when evaluating checkpoints.
		/// </summary>
		public string? CheckpointId { get; set; }
		/// <summary>
		/// Output only. The error of the evaluation run if any.
		/// </summary>
		public GoogleRpcStatus? Error { get; set; }
		/// <summary>
		/// Output only. Results for EvaluationService.EvaluateDataset.
		/// </summary>
		public EvaluateDatasetResponse? EvaluateDatasetResponse { get; set; }
		/// <summary>
		/// Output only. The operation ID of the evaluation run. Format: <c>projects/{project}/locations/{location}/operations/{operation_id}</c>.
		/// </summary>
		public string? OperationName { get; set; }
    }
}