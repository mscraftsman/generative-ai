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
using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI.Types
{
	[JsonConverter(typeof(JsonStringEnumConverter<MediaResolution>))]
    public enum MediaResolution
    {
        /// <summary>
        /// Media resolution has not been set.
        /// </summary>
        MediaResolutionUnspecified,
        /// <summary>
        /// Media resolution set to low (64 tokens).
        /// </summary>
        MediaResolutionLow,
        /// <summary>
        /// Media resolution set to medium (256 tokens).
        /// </summary>
        MediaResolutionMedium,
        /// <summary>
        /// Media resolution set to high (zoomed reframing with 256 tokens).
        /// </summary>
        MediaResolutionHigh,
    }
}
