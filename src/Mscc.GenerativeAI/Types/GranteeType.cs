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
	[JsonConverter(typeof(JsonStringEnumConverter<GranteeType>))]
    public enum GranteeType
    {
        /// <summary>
        /// The default value. This value is unused.
        /// </summary>
        GranteeTypeUnspecified,
        /// <summary>
        /// Represents a user. When set, you must provide email_address for the user.
        /// </summary>
        User,
        /// <summary>
        /// Represents a group. When set, you must provide email_address for the group.
        /// </summary>
        Group,
        /// <summary>
        /// Represents access to everyone. No extra information is required.
        /// </summary>
        Everyone,
    }
}