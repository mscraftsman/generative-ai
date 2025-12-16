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
	[JsonConverter(typeof(JsonStringEnumConverter<State>))]
    public enum State
    {
        /// <summary>
        /// The default value. This value is used if the state is omitted.
        /// </summary>
        StateUnspecified,
        /// <summary>
        /// Some `Chunks` of the `Document` are being processed (embedding and vector storage).
        /// </summary>
        StatePending,
        /// <summary>
        /// All `Chunks` of the `Document` is processed and available for querying.
        /// </summary>
        StateActive,
        /// <summary>
        /// Some `Chunks` of the `Document` failed processing.
        /// </summary>
        StateFailed,
        /// <summary>
        /// The batch state is unspecified.
        /// </summary>
        BatchStateUnspecified,
        /// <summary>
        /// The service is preparing to run the batch.
        /// </summary>
        BatchStatePending,
        /// <summary>
        /// The batch is in progress.
        /// </summary>
        BatchStateRunning,
        /// <summary>
        /// The batch completed successfully.
        /// </summary>
        BatchStateSucceeded,
        /// <summary>
        /// The batch failed.
        /// </summary>
        BatchStateFailed,
        /// <summary>
        /// The batch has been cancelled.
        /// </summary>
        BatchStateCancelled,
        /// <summary>
        /// The batch has expired.
        /// </summary>
        BatchStateExpired,
        /// <summary>
        /// File is being processed and cannot be used for inference yet.
        /// </summary>
        Processing,
        /// <summary>
        /// File is processed and available for inference.
        /// </summary>
        Active,
        /// <summary>
        /// File failed processing.
        /// </summary>
        Failed,
        /// <summary>
        /// Being generated.
        /// </summary>
        Generating,
        /// <summary>
        /// Generated and is ready for download.
        /// </summary>
        Generated,
        /// <summary>
        /// The model is being created.
        /// </summary>
        Creating,
        /// <summary>
        /// A state used by systems like Vertex AI Pipelines to indicate that the underlying data item represented by this Artifact is being created.
        /// </summary>
        Pending,
        /// <summary>
        /// A state indicating that the Artifact should exist, unless something external to the system deletes it.
        /// </summary>
        Live,
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
        /// <summary>
        /// This state is not supposed to happen.
        /// </summary>
        Unknown,
        /// <summary>
        /// RagCorpus resource entry is initialized, but hasn&apos;t done validation.
        /// </summary>
        Initialized,
        /// <summary>
        /// RagCorpus is in a problematic situation. See `error_message` field for details.
        /// </summary>
        Error,
        /// <summary>
        /// The evaluation run is running.
        /// </summary>
        Running,
        /// <summary>
        /// The evaluation run has succeeded.
        /// </summary>
        Succeeded,
        /// <summary>
        /// The evaluation run has been cancelled.
        /// </summary>
        Cancelled,
        /// <summary>
        /// The evaluation run is performing inference.
        /// </summary>
        Inference,
        /// <summary>
        /// The evaluation run is performing rubric generation.
        /// </summary>
        GeneratingRubrics,
        /// <summary>
        /// The Execution is new
        /// </summary>
        New,
        /// <summary>
        /// The Execution has finished running
        /// </summary>
        Complete,
        /// <summary>
        /// The Execution completed through Cache hit.
        /// </summary>
        Cached,
        /// <summary>
        /// State when the featureOnlineStore configuration is not being updated and the fields reflect the current configuration of the featureOnlineStore. The featureOnlineStore is usable in this state.
        /// </summary>
        Stable,
        /// <summary>
        /// The state of the featureOnlineStore configuration when it is being updated. During an update, the fields reflect either the original configuration or the updated configuration of the featureOnlineStore. The featureOnlineStore is still usable in this state.
        /// </summary>
        Updating,
        /// <summary>
        /// The default behavior of whether to enable the monitoring. EntityType-level config: disabled. Feature-level config: inherited from the configuration of EntityType this Feature belongs to.
        /// </summary>
        Default,
        /// <summary>
        /// Explicitly enables import features analysis. EntityType-level config: by default enables import features analysis for all Features under it. Feature-level config: enables import features analysis regardless of the EntityType-level config.
        /// </summary>
        Enabled,
        /// <summary>
        /// Explicitly disables import features analysis. EntityType-level config: by default disables import features analysis for all Features under it. Feature-level config: disables import features analysis regardless of the EntityType-level config.
        /// </summary>
        Disabled,
        /// <summary>
        /// Indicates that a specific NasTrial has been requested, but it has not yet been suggested by the service.
        /// </summary>
        Requested,
        /// <summary>
        /// Indicates that the NasTrial should stop according to the service.
        /// </summary>
        Stopping,
        /// <summary>
        /// Indicates that the NasTrial should not be attempted again. The service will set a NasTrial to INFEASIBLE when it&apos;s done but missing the final_measurement.
        /// </summary>
        Infeasible,
        /// <summary>
        /// Should not be used.
        /// </summary>
        PscAutomationStateUnspecified,
        /// <summary>
        /// The PSC service automation is successful.
        /// </summary>
        PscAutomationStateSuccessful,
        /// <summary>
        /// The PSC service automation has failed.
        /// </summary>
        PscAutomationStateFailed,
        /// <summary>
        /// The PROVISIONING state indicates the persistent resources is being created.
        /// </summary>
        Provisioning,
        /// <summary>
        /// The REBOOTING state indicates the persistent resource is being rebooted (PR is not available right now but is expected to be ready again later).
        /// </summary>
        Rebooting,
        /// <summary>
        /// The pipeline state is unspecified.
        /// </summary>
        PipelineStateUnspecified,
        /// <summary>
        /// The pipeline has been created or resumed, and processing has not yet begun.
        /// </summary>
        PipelineStateQueued,
        /// <summary>
        /// The service is preparing to run the pipeline.
        /// </summary>
        PipelineStatePending,
        /// <summary>
        /// The pipeline is in progress.
        /// </summary>
        PipelineStateRunning,
        /// <summary>
        /// The pipeline completed successfully.
        /// </summary>
        PipelineStateSucceeded,
        /// <summary>
        /// The pipeline failed.
        /// </summary>
        PipelineStateFailed,
        /// <summary>
        /// The pipeline is being cancelled. From this state, the pipeline may only go to either PIPELINE_STATE_SUCCEEDED, PIPELINE_STATE_FAILED or PIPELINE_STATE_CANCELLED.
        /// </summary>
        PipelineStateCancelling,
        /// <summary>
        /// The pipeline has been cancelled.
        /// </summary>
        PipelineStateCancelled,
        /// <summary>
        /// The pipeline has been stopped, and can be resumed.
        /// </summary>
        PipelineStatePaused,
        /// <summary>
        /// Specifies Task cancel is in pending state.
        /// </summary>
        CancelPending,
        /// <summary>
        /// Specifies task is being cancelled.
        /// </summary>
        Cancelling,
        /// <summary>
        /// Specifies task was skipped due to cache hit.
        /// </summary>
        Skipped,
        /// <summary>
        /// Specifies that the task was not triggered because the task&apos;s trigger policy is not satisfied. The trigger policy is specified in the `condition` field of PipelineJob.pipeline_spec.
        /// </summary>
        NotTriggered,
        /// <summary>
        /// Runtime resources are being allocated for the sandbox environment.
        /// </summary>
        StateProvisioning,
        /// <summary>
        /// Sandbox runtime is ready for serving.
        /// </summary>
        StateRunning,
        /// <summary>
        /// Sandbox runtime is halted, performing tear down tasks.
        /// </summary>
        StateDeprovisioning,
        /// <summary>
        /// Sandbox has terminated with underlying runtime failure.
        /// </summary>
        StateTerminated,
        /// <summary>
        /// Sandbox runtime has been deleted.
        /// </summary>
        StateDeleted,
        /// <summary>
        /// The schedule is paused. No new runs will be created until the schedule is resumed. Already started runs will be allowed to complete.
        /// </summary>
        Paused,
        /// <summary>
        /// The Schedule is completed. No new runs will be scheduled. Already started runs will be allowed to complete. Schedules in completed state cannot be paused or resumed.
        /// </summary>
        Completed,
        /// <summary>
        /// The study is stopped due to an internal error.
        /// </summary>
        Inactive,
    }
}