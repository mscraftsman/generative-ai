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
using System;

// *** AUTO-GENERATED FILE - DO NOT EDIT MANUALLY *** //

namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// All parameters related to queuing and scheduling of custom jobs.
	/// </summary>
	public partial class Scheduling
	{
		/// <summary>
		/// Optional. Indicates if the job should retry for internal errors after the job starts running. If true, overrides <c>Scheduling.restart_job_on_worker_restart</c> to false.
		/// </summary>
		public bool? DisableRetries { get; set; }
		/// <summary>
		/// Optional. This is the maximum duration that a job will wait for the requested resources to be provisioned if the scheduling strategy is set to [Strategy.DWS_FLEX_START]. If set to 0, the job will wait indefinitely. The default is 24 hours.
		/// </summary>
		public string? MaxWaitDuration { get; set; }
		/// <summary>
		/// Optional. Restarts the entire CustomJob if a worker gets restarted. This feature can be used by distributed training jobs that are not resilient to workers leaving and joining a job.
		/// </summary>
		public bool? RestartJobOnWorkerRestart { get; set; }
		/// <summary>
		/// Optional. This determines which type of scheduling strategy to use.
		/// </summary>
		public StrategyType? Strategy { get; set; }
		/// <summary>
		/// Optional. The maximum job running time. The default is 7 days.
		/// </summary>
		public string? Timeout { get; set; }

		[JsonConverter(typeof(JsonStringEnumConverter<StrategyType>))]
		public enum StrategyType
		{
			/// <summary>
			/// Strategy will default to STANDARD.
			/// </summary>
			StrategyUnspecified,
			/// <summary>
			/// Deprecated. Regular on-demand provisioning strategy.
			/// </summary>
			[Obsolete("The member type 'OnDemand' has been marked as deprecated. It might be removed in future versions.")]
			OnDemand,
			/// <summary>
			/// Deprecated. Low cost by making potential use of spot resources.
			/// </summary>
			[Obsolete("The member type 'LowCost' has been marked as deprecated. It might be removed in future versions.")]
			LowCost,
			/// <summary>
			/// Standard provisioning strategy uses regular on-demand resources.
			/// </summary>
			Standard,
			/// <summary>
			/// Spot provisioning strategy uses spot resources.
			/// </summary>
			Spot,
			/// <summary>
			/// Flex Start strategy uses DWS to queue for resources.
			/// </summary>
			FlexStart,
		}
    }
}