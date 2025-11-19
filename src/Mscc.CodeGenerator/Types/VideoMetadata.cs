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
	/// Metadata describes the input video content.
	/// </summary>
	public partial class VideoMetadata
	{
		/// <summary>
		/// Optional. The end offset of the video.
		/// </summary>
		public string? EndOffset { get; set; }
		/// <summary>
		/// Optional. The frame rate of the video sent to the model. If not specified, the default value will be 1.0. The fps range is (0.0, 24.0].
		/// </summary>
		public double? Fps { get; set; }
		/// <summary>
		/// Optional. The start offset of the video.
		/// </summary>
		public string? StartOffset { get; set; }
    }
}
