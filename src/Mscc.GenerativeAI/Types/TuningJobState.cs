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
	[JsonConverter(typeof(JsonStringEnumConverter<TuningJobState>))]
    public enum TuningJobState
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