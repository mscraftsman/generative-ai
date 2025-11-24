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
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

// *** AUTO-GENERATED FILE - DO NOT EDIT MANUALLY *** //

namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Represents a TuningJob that runs with Google owned models.
	/// </summary>
	public partial class TuningJob
	{
		/// <summary>
		/// The base model that is being tuned. See [Supported models](https://cloud.google.com/vertex-ai/generative-ai/docs/model-reference/tuning#supported_models).
		/// </summary>
		public string? BaseModel { get; set; }
		/// <summary>
		/// Output only. Time when the TuningJob was created.
		/// </summary>
		public DateTime? CreateTime { get; set; }
		/// <summary>
		/// Optional. The user-provided path to custom model weights. Set this field to tune a custom model. The path must be a Cloud Storage directory that contains the model weights in .safetensors format along with associated model metadata files. If this field is set, the base_model field must still be set to indicate which base model the custom model is derived from. This feature is only available for open source models.
		/// </summary>
		public string? CustomBaseModel { get; set; }
		/// <summary>
		/// Optional. The description of the TuningJob.
		/// </summary>
		public string? Description { get; set; }
		/// <summary>
		/// Tuning Spec for Distillation.
		/// </summary>
		public DistillationSpec? DistillationSpec { get; set; }
		/// <summary>
		/// Customer-managed encryption key options for a TuningJob. If this is set, then all resources created by the TuningJob will be encrypted with the provided encryption key.
		/// </summary>
		public EncryptionSpec? EncryptionSpec { get; set; }
		/// <summary>
		/// Output only. Time when the TuningJob entered any of the following JobStates: <c>JOB_STATE_SUCCEEDED</c>, <c>JOB_STATE_FAILED</c>, <c>JOB_STATE_CANCELLED</c>, <c>JOB_STATE_EXPIRED</c>.
		/// </summary>
		public DateTime? EndTime { get; set; }
		/// <summary>
		/// Output only. Only populated when job&apos;s state is <c>JOB_STATE_FAILED</c> or <c>JOB_STATE_CANCELLED</c>.
		/// </summary>
		public GoogleRpcStatus? Error { get; set; }
		/// <summary>
		/// Output only. Evaluation runs for the Tuning Job.
		/// </summary>
		public List<EvaluateDatasetRun>? EvaluateDatasetRuns { get; set; }
		/// <summary>
		/// Output only. The Experiment associated with this TuningJob.
		/// </summary>
		public string? Experiment { get; set; }
		/// <summary>
		/// Tuning Spec for Full Fine Tuning.
		/// </summary>
		public FullFineTuningSpec? FullFineTuningSpec { get; set; }
		/// <summary>
		/// Optional. The labels with user-defined metadata to organize TuningJob and generated resources such as Model and Endpoint. Label keys and values can be no longer than 64 characters (Unicode codepoints), can only contain lowercase letters, numeric characters, underscores and dashes. International characters are allowed. See https://goo.gl/xmQnxf for more information and examples of labels.
		/// </summary>
		public object? Labels { get; set; }
		/// <summary>
		/// Output only. Identifier. Resource name of a TuningJob. Format: <c>projects/{project}/locations/{location}/tuningJobs/{tuning_job}</c>
		/// </summary>
		public string? Name { get; set; }
		/// <summary>
		/// Optional. Cloud Storage path to the directory where tuning job outputs are written to. This field is only available and required for open source models.
		/// </summary>
		public string? OutputUri { get; set; }
		/// <summary>
		/// Tuning Spec for open sourced and third party Partner models.
		/// </summary>
		public PartnerModelTuningSpec? PartnerModelTuningSpec { get; set; }
		/// <summary>
		/// Output only. The resource name of the PipelineJob associated with the TuningJob. Format: <c>projects/{project}/locations/{location}/pipelineJobs/{pipeline_job}</c>.
		/// </summary>
		public string? PipelineJob { get; set; }
		/// <summary>
		/// The pre-tuned model for continuous tuning.
		/// </summary>
		public PreTunedModel? PreTunedModel { get; set; }
		/// <summary>
		/// Tuning Spec for Preference Optimization.
		/// </summary>
		public PreferenceOptimizationSpec? PreferenceOptimizationSpec { get; set; }
		/// <summary>
		/// Output only. Reserved for future use.
		/// </summary>
		public bool? SatisfiesPzi { get; set; }
		/// <summary>
		/// Output only. Reserved for future use.
		/// </summary>
		public bool? SatisfiesPzs { get; set; }
		/// <summary>
		/// The service account that the tuningJob workload runs as. If not specified, the Vertex AI Secure Fine-Tuned Service Agent in the project will be used. See https://cloud.google.com/iam/docs/service-agents#vertex-ai-secure-fine-tuning-service-agent Users starting the pipeline must have the <c>iam.serviceAccounts.actAs</c> permission on this service account.
		/// </summary>
		public string? ServiceAccount { get; set; }
		/// <summary>
		/// Output only. Time when the TuningJob for the first time entered the <c>JOB_STATE_RUNNING</c> state.
		/// </summary>
		public DateTime? StartTime { get; set; }
		/// <summary>
		/// Output only. The detailed state of the job.
		/// </summary>
		public StateType? State { get; set; }
		/// <summary>
		/// Tuning Spec for Supervised Fine Tuning.
		/// </summary>
		public SupervisedTuningSpec? SupervisedTuningSpec { get; set; }
		/// <summary>
		/// Output only. The tuned model resources associated with this TuningJob.
		/// </summary>
		public TunedModelResponse? TunedModel { get; set; }
		/// <summary>
		/// Optional. The display name of the TunedModel. The name can be up to 128 characters long and can consist of any UTF-8 characters. For continuous tuning, tuned_model_display_name will by default use the same display name as the pre-tuned model. If a new display name is provided, the tuning job will create a new model instead of a new version.
		/// </summary>
		public string? TunedModelDisplayName { get; set; }
		/// <summary>
		/// Output only. The tuning data statistics associated with this TuningJob.
		/// </summary>
		public TuningDataStats? TuningDataStats { get; set; }
		/// <summary>
		/// Output only. The detail state of the tuning job (while the overall <c>JobState</c> is running).
		/// </summary>
		public TuningJobStateType? TuningJobState { get; set; }
		/// <summary>
		/// Output only. Time when the TuningJob was most recently updated.
		/// </summary>
		public DateTime? UpdateTime { get; set; }
		/// <summary>
		/// Tuning Spec for Veo Tuning.
		/// </summary>
		public VeoTuningSpec? VeoTuningSpec { get; set; }

		[JsonConverter(typeof(JsonStringEnumConverter<StateType>))]
		public enum StateType
		{
			/// <summary>
			/// The job state is unspecified.
			/// </summary>
			JobStateUnspecified,
			/// <summary>
			/// The job has been just created or resumed and processing has not yet begun.
			/// </summary>
			JobStateQueued,
			/// <summary>
			/// The service is preparing to run the job.
			/// </summary>
			JobStatePending,
			/// <summary>
			/// The job is in progress.
			/// </summary>
			JobStateRunning,
			/// <summary>
			/// The job completed successfully.
			/// </summary>
			JobStateSucceeded,
			/// <summary>
			/// The job failed.
			/// </summary>
			JobStateFailed,
			/// <summary>
			/// The job is being cancelled. From this state the job may only go to either `JOB_STATE_SUCCEEDED`, `JOB_STATE_FAILED` or `JOB_STATE_CANCELLED`.
			/// </summary>
			JobStateCancelling,
			/// <summary>
			/// The job has been cancelled.
			/// </summary>
			JobStateCancelled,
			/// <summary>
			/// The job has been stopped, and can be resumed.
			/// </summary>
			JobStatePaused,
			/// <summary>
			/// The job has expired.
			/// </summary>
			JobStateExpired,
			/// <summary>
			/// The job is being updated. Only jobs in the `RUNNING` state can be updated. After updating, the job goes back to the `RUNNING` state.
			/// </summary>
			JobStateUpdating,
			/// <summary>
			/// The job is partially succeeded, some results may be missing due to errors.
			/// </summary>
			JobStatePartiallySucceeded,
		}

		[JsonConverter(typeof(JsonStringEnumConverter<TuningJobStateType>))]
		public enum TuningJobStateType
		{
			/// <summary>
			/// Default tuning job state.
			/// </summary>
			TuningJobStateUnspecified,
			/// <summary>
			/// Tuning job is waiting for job quota.
			/// </summary>
			TuningJobStateWaitingForQuota,
			/// <summary>
			/// Tuning job is validating the dataset.
			/// </summary>
			TuningJobStateProcessingDataset,
			/// <summary>
			/// Tuning job is waiting for hardware capacity.
			/// </summary>
			TuningJobStateWaitingForCapacity,
			/// <summary>
			/// Tuning job is running.
			/// </summary>
			TuningJobStateTuning,
			/// <summary>
			/// Tuning job is doing some post processing steps.
			/// </summary>
			TuningJobStatePostProcessing,
		}
    }
}