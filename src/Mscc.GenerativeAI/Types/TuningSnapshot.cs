/*
 * Copyright 2024-2025 Jochen Kirstätter
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

namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Record for a single tuning step.
	/// </summary>
	public partial class TuningSnapshot
	{
		/// <summary>
		/// Output only. The timestamp when this metric was computed.
		/// </summary>
		public DateTime? ComputeTime { get; set; }
		/// <summary>
		/// Output only. The epoch this step was part of.
		/// </summary>
		public int? Epoch { get; set; }
		/// <summary>
		/// Output only. The mean loss of the training examples for this step.
		/// </summary>
		public double? MeanLoss { get; set; }
		/// <summary>
		/// Output only. The tuning step.
		/// </summary>
		public int? Step { get; set; }
    }
}