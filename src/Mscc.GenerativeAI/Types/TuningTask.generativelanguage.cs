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

// *** AUTO-GENERATED FILE - DO NOT EDIT MANUALLY *** //

namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Tuning tasks that create tuned models.
	/// </summary>
	public partial class TuningTask
	{
		/// <summary>
		/// Output only. The timestamp when tuning this model completed.
		/// </summary>
		public DateTime? CompleteTime { get; set; }
		/// <summary>
		/// Immutable. Hyperparameters controlling the tuning process. If not provided, default values will be used.
		/// </summary>
		public Hyperparameters? Hyperparameters { get; set; }
		/// <summary>
		/// Output only. Metrics collected during tuning.
		/// </summary>
		public List<TuningSnapshot>? Snapshots { get; set; }
		/// <summary>
		/// Output only. The timestamp when tuning this model started.
		/// </summary>
		public DateTime? StartTime { get; set; }
		/// <summary>
		/// Required. Input only. Immutable. The model training data.
		/// </summary>
		public Dataset? TrainingData { get; set; }
    }
}