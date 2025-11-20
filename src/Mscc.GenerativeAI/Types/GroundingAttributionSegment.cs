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
    public class GroundingAttributionSegment
    {
        /// <summary>
        /// Output only. Start index into the content.
        /// </summary>
        public int? StartIndex { get; internal set; }
        /// <summary>
        /// Output only. End index into the content.
        /// </summary>
        public int? EndIndex { get; internal set; }
        /// <summary>
        /// Output only. Part index into the content.
        /// </summary>
        public int? PartIndex { get; internal set; }
    }
}