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
    /// <summary>
    /// Defines the valid operators that can be applied to a key-value pair.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<Operator>))]
    public enum Operator
    {
        /// <summary>
        /// The default value. This value is unused.
        /// </summary>
        OperatorUnspecified,
        /// <summary>
        /// Supported by numeric.
        /// </summary>
        Less,
        /// <summary>
        /// Supported by numeric.
        /// </summary>
        LessEqual,
        /// <summary>
        /// Supported by numeric and string.
        /// </summary>
        Equal,
        /// <summary>
        /// Supported by numeric.
        /// </summary>
        GreaterEqual,
        /// <summary>
        /// Supported by numeric.
        /// </summary>
        Greater,
        /// <summary>
        /// Supported by numeric and string.
        /// </summary>
        NotEqual,
        /// <summary>
        /// Supported by string only when <see cref="CustomMetadata" /> value type for the given key has a stringListValue.
        /// </summary>
        Includes,
        /// <summary>
        /// Supported by string only when <see cref="CustomMetadata" /> value type for the given key has a stringListValue.
        /// </summary>
        Excludes
    }
}