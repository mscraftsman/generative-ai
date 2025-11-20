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

namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Config for image generation features.
	/// </summary>
	public partial class ImageConfig
	{
		/// <summary>
		/// Optional. Specifies the size of generated images. Supported values are `1K`, `2K`, `4K`. If
		/// not specified, the model will use default value `1K`
		/// </summary>
		public ImageSize? ImageSize { get; set; }
		/// <summary>
		/// MIME type of the generated image.
		/// </summary>
		public string? OutputMimeType { get; set; }
		/// <summary>
		/// Compression quality of the generated image (for ``image/jpeg`` only).
		/// </summary>
		public int? OutputCompressionQuality { get; set; }
		/// <summary>
		/// Optional. The image output format for generated images. This field is not supported in
		/// Gemini API.
		/// </summary>
		public ImageOutputOptions? ImageOutputOptions { get; set; }
		/// <summary>
		/// Optional. Controls whether the model can generate people. This field is not supported in
		/// Gemini API.
		/// </summary>
		public PersonGeneration? PersonGeneration { get; set; }
    }
}