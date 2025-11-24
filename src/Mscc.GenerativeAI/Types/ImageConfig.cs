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
		/// Optional. The aspect ratio of the image to generate. Supported aspect ratios: 1:1, 2:3, 3:2, 3:4, 4:3, 9:16, 16:9, 21:9. If not specified, the model will choose a default aspect ratio based on any reference images provided.
		/// </summary>
		public ImageAspectRatio? AspectRatio { get; set; }
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
    }
}