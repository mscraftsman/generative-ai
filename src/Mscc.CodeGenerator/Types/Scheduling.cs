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
	[JsonConverter(typeof(JsonStringEnumConverter<Scheduling>))]
    public enum Scheduling
    {
        /// <summary>
        /// This value is unused.
        /// </summary>
        SchedulingUnspecified,
        /// <summary>
        /// Only add the result to the conversation context, do not interrupt or trigger generation.
        /// </summary>
        Silent,
        /// <summary>
        /// Add the result to the conversation context, and prompt to generate output without interrupting ongoing generation.
        /// </summary>
        WhenIdle,
        /// <summary>
        /// Add the result to the conversation context, interrupt ongoing generation and prompt to generate output.
        /// </summary>
        Interrupt,
    }
}
