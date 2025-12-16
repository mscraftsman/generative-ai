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
	/// Configuration for image generation. This message allows you to control various aspects of image generation, such as the output format, aspect ratio, and whether the model can generate images of people.
	/// </summary>
	public partial class ImageConfig
	{
		/// <summary>
		/// Optional. The image output format for generated images.
		/// </summary>
		public ImageConfigImageOutputOptions? ImageOutputOptions { get; set; }
		/// <summary>
		/// Optional. Controls whether the model can generate people.
		/// </summary>
		public PersonGenerationType? PersonGeneration { get; set; }

		[JsonConverter(typeof(JsonStringEnumConverter<PersonGenerationType>))]
		public enum PersonGenerationType
		{
			/// <summary>
			/// The default behavior is unspecified. The model will decide whether to generate images of people.
			/// </summary>
			PersonGenerationUnspecified,
			/// <summary>
			/// Allows the model to generate images of people, including adults and children.
			/// </summary>
			AllowAll,
			/// <summary>
			/// Allows the model to generate images of adults, but not children.
			/// </summary>
			AllowAdult,
			/// <summary>
			/// Prevents the model from generating images of people.
			/// </summary>
			AllowNone,
		}
    }
}