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

using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// Response from the model supporting multiple candidates.
    /// Ref: https://ai.google.dev/api/rest/v1beta/GenerateContentResponse
    /// </summary>
    public partial class GenerateContentResponse : BaseLogger
    {
        /// <summary>
        /// A convenience property to get the responded text information of first candidate.
        /// </summary>
        [JsonIgnore]
        public string? Text
        {
            get
            {
                if (Candidates is null) return string.Empty;
                if (Candidates?.Count == 0) return string.Empty;
                if (Candidates?.FirstOrDefault()?.FinishReason is
                    FinishReason.MaxTokens or
                    FinishReason.Safety or
                    FinishReason.Recitation or
                    FinishReason.Other)
                    return string.Empty;
                if (Candidates?.Count > 1) Logger.LogMultipleCandidates(Candidates!.Count);

                return string.Join(Environment.NewLine,
                    Candidates?.FirstOrDefault()?.Content?.Parts
                        .Where(p => p.Thought is null or false)
                        .Select(x => x.Text)
                        .ToArray()!);
            }
        }

        /// <summary>
        /// A convenience property to get the function calls.
        /// </summary>
        [JsonIgnore]
        public List<FunctionCall>? FunctionCalls => Candidates?.FirstOrDefault()?.Content?.Parts
            .Where(p => p.FunctionCall is not null)
            .Select(p => p.FunctionCall)
            .ToList();

        /// <summary>
        /// A convenience property to get the responded thinking information of first candidate.
        /// </summary>
        [JsonIgnore]
        public string? Thinking
        {
            get
            {
                if (Candidates is null) return string.Empty;
                if (Candidates?.Count == 0) return string.Empty;
                if (Candidates?.FirstOrDefault()?.FinishReason is
                    FinishReason.MaxTokens or
                    FinishReason.Safety or
                    FinishReason.Recitation or
                    FinishReason.Other)
                    return string.Empty;
                if (Candidates?.Count > 1) Logger.LogMultipleCandidates(Candidates!.Count);

                return string.Join(Environment.NewLine,
                    Candidates?.FirstOrDefault()?.Content?.Parts
                        .Where(p => p.Thought == true)
                        .Select(x => x.Text)
                        .ToArray()!);
            }
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public GenerateContentResponse() { }

        /// <summary>
        /// Base constructor to set the <see cref="ILogger"/> instance.
        /// </summary>
        /// <param name="logger">Optional. Logger instance used for logging</param>
        protected GenerateContentResponse(ILogger? logger) : base(logger) { }

        /// <summary>
        /// A convenience overload to easily access the responded text.
        /// </summary>
        /// <returns>The responded text information of first candidate.</returns>
        public override string ToString()
        {
            return Text ?? String.Empty;
        }
    }
}