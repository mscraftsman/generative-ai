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
	[JsonConverter(typeof(JsonStringEnumConverter<AnswerStyle>))]
    public enum AnswerStyle
    {
        /// <summary>
        /// Unspecified answer style.
        /// </summary>
        AnswerStyleUnspecified,
        /// <summary>
        /// Succinct but abstract style.
        /// </summary>
        Abstractive,
        /// <summary>
        /// Very brief and extractive style.
        /// </summary>
        Extractive,
        /// <summary>
        /// Verbose style including extra details. The response may be formatted as a sentence, paragraph, multiple paragraphs, or bullet points, etc.
        /// </summary>
        Verbose,
    }
}