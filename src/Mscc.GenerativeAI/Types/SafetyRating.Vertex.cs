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

using System.Diagnostics;

namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// A safety rating for a piece of content. The safety rating contains the harm category and the
    /// harm probability level.
    /// Ref: https://ai.google.dev/api/rest/v1beta/SafetyRating
    /// </summary>
    [DebuggerDisplay("{Category}: {Probability} ({Blocked})")]
    public partial class SafetyRating
    {
        /// <summary>
        /// Output only. The overwritten threshold for the safety category of Gemini 2.0 image out. If
        /// minors are detected in the output image, the threshold of each safety category will be
        /// overwritten if user sets a lower threshold. This field is not supported in Gemini API.
        /// </summary>
        public HarmBlockThreshold? OverwrittenThreshold { get; set; }
        /// <summary>
        /// Output only. The probability score of harm for this category.
        /// </summary>
        /// <remarks>
        /// This field is not supported in Gemini API.
        /// </remarks>
        public float? ProbabilityScore { get; set; }
        /// <summary>
        /// Output only. The severity of harm for this category.
        /// </summary>
        /// <remarks>
        /// This field is not supported in Gemini API.
        /// </remarks>
        public HarmSeverity? Severity { get; set; }
        /// <summary>
        /// Output only. The severity score of harm for this category.
        /// </summary>
        /// <remarks>
        /// This field is not supported in Gemini API.
        /// </remarks>
        public float? SeverityScore { get; set; }
    }
}
