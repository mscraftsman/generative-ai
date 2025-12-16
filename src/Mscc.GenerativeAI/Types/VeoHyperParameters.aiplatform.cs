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
	/// Hyperparameters for Veo.
	/// </summary>
	public partial class VeoHyperParameters
	{
		/// <summary>
		/// Optional. Number of complete passes the model makes over the entire training dataset during training.
		/// </summary>
		public long? EpochCount { get; set; }
		/// <summary>
		/// Optional. Multiplier for adjusting the default learning rate.
		/// </summary>
		public double? LearningRateMultiplier { get; set; }
		/// <summary>
		/// Optional. The tuning task. Either I2V or T2V.
		/// </summary>
		public TuningTaskType? TuningTask { get; set; }

		[JsonConverter(typeof(JsonStringEnumConverter<TuningTaskType>))]
		public enum TuningTaskType
		{
			/// <summary>
			/// Default value. This value is unused.
			/// </summary>
			TuningTaskUnspecified,
			/// <summary>
			/// Tuning task for image to video.
			/// </summary>
			TuningTaskI2v,
			/// <summary>
			/// Tuning task for text to video.
			/// </summary>
			TuningTaskT2v,
			/// <summary>
			/// Tuning task for reference to video.
			/// </summary>
			TuningTaskR2v,
		}
    }
}