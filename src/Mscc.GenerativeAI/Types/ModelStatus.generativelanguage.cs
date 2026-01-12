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
using System.Text.Json.Serialization;

// *** AUTO-GENERATED FILE - DO NOT EDIT MANUALLY *** //

namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// The status of the underlying model. This is used to indicate the stage of the underlying model and the retirement time if applicable.
	/// </summary>
	public partial class ModelStatus
	{
		/// <summary>
		/// A message explaining the model status.
		/// </summary>
		public string? Message { get; set; }
		/// <summary>
		/// The stage of the underlying model.
		/// </summary>
		public ModelStageType? ModelStage { get; set; }
		/// <summary>
		/// The time at which the model will be retired.
		/// </summary>
		public DateTime? RetirementTime { get; set; }

		[JsonConverter(typeof(JsonStringEnumConverter<ModelStageType>))]
		public enum ModelStageType
		{
			/// <summary>
			/// Unspecified model stage.
			/// </summary>
			ModelStageUnspecified,
			/// <summary>
			/// The underlying model is subject to lots of tunings.
			/// </summary>
			[Obsolete("The member type 'UnstableExperimental' has been marked as deprecated. It might be removed in future versions.")]
			UnstableExperimental,
			/// <summary>
			/// Models in this stage are for experimental purposes only.
			/// </summary>
			Experimental,
			/// <summary>
			/// Models in this stage are more mature than experimental models.
			/// </summary>
			Preview,
			/// <summary>
			/// Models in this stage are considered stable and ready for production use.
			/// </summary>
			Stable,
			/// <summary>
			/// If the model is on this stage, it means that this model is on the path to deprecation in near future. Only existing customers can use this model.
			/// </summary>
			Legacy,
			/// <summary>
			/// Models in this stage are deprecated. These models cannot be used.
			/// </summary>
			[Obsolete("The member type 'Deprecated' has been marked as deprecated. It might be removed in future versions.")]
			Deprecated,
			/// <summary>
			/// Models in this stage are retired. These models cannot be used.
			/// </summary>
			Retired,
		}
    }
}