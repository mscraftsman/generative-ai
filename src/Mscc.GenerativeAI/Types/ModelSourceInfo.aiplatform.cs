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
	/// Detail description of the source information of the model.
	/// </summary>
	public partial class ModelSourceInfo
	{
		/// <summary>
		/// If this Model is copy of another Model. If true then source_type pertains to the original.
		/// </summary>
		public bool? Copy { get; set; }
		/// <summary>
		/// Type of the model source.
		/// </summary>
		public ModelSourceInfoSourceType? SourceType { get; set; }

		[JsonConverter(typeof(JsonStringEnumConverter<ModelSourceInfoSourceType>))]
		public enum ModelSourceInfoSourceType
		{
			/// <summary>
			/// Should not be used.
			/// </summary>
			ModelSourceTypeUnspecified,
			/// <summary>
			/// The Model is uploaded by automl training pipeline.
			/// </summary>
			Automl,
			/// <summary>
			/// The Model is uploaded by user or custom training pipeline.
			/// </summary>
			Custom,
			/// <summary>
			/// The Model is registered and sync&apos;ed from BigQuery ML.
			/// </summary>
			Bqml,
			/// <summary>
			/// The Model is saved or tuned from Model Garden.
			/// </summary>
			ModelGarden,
			/// <summary>
			/// The Model is saved or tuned from Genie.
			/// </summary>
			Genie,
			/// <summary>
			/// The Model is uploaded by text embedding finetuning pipeline.
			/// </summary>
			CustomTextEmbedding,
			/// <summary>
			/// The Model is saved or tuned from Marketplace.
			/// </summary>
			Marketplace,
		}
    }
}