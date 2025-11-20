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
using System.Collections.Generic;

namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// Usage metadata about the content generation request and response. This message provides a
    /// detailed breakdown of token usage and other relevant metrics. This data type is not supported
    /// in Gemini API.
    /// </summary>
    public partial class UsageMetadata
    {
        /// <summary>
        /// Number of text characters.
        /// </summary>
        public int TextCount { get; set; } = default;
        /// <summary>
        /// Number of images.
        /// </summary>
        public int ImageCount { get; set; } = default;
        /// <summary>
        /// Duration of video in seconds.
        /// </summary>
        public int VideoDurationSeconds { get; set; } = default;
        /// <summary>
        /// Duration of audio in seconds.
        /// </summary>
        public int AudioDurationSeconds { get; set; } = default;
        /// <summary>
        /// Output only. The traffic type for this request.
        /// </summary>
        public TrafficType? TrafficType { get; set; }
    }
}