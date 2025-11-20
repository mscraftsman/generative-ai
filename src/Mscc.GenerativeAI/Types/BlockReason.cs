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
	[JsonConverter(typeof(JsonStringEnumConverter<BlockReason>))]
    public enum BlockReason
    {
	    /// <summary>
	    /// BlockedReasonUnspecified means unspecified blocked reason.
	    /// </summary>
	    BlockedReasonUnspecified,
        /// <summary>
        /// Default value. This value is unused.
        /// </summary>
        BlockReasonUnspecified,
        /// <summary>
        /// Input was blocked due to safety reasons. Inspect `safety_ratings` to understand which safety category blocked it.
        /// </summary>
        Safety,
        /// <summary>
        /// Input was blocked due to other reasons.
        /// </summary>
        Other,
        /// <summary>
        /// Prompt was blocked due to the terms which are included from the terminology blocklist.
        /// </summary>
        Blocklist,
        /// <summary>
        /// Prompt was blocked due to prohibited content.
        /// </summary>
        ProhibitedContent,
        /// <summary>
        /// Candidates blocked due to unsafe image generation content.
        /// </summary>
        ImageSafety,
        /// <summary>
        /// The prompt was blocked as a jailbreak attempt.
        /// </summary>
        Jailbreak
    }
}