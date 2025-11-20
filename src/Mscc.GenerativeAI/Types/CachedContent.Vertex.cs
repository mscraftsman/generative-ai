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

using System;
using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI.Types
{
    public partial class CachedContent
    {
        private string _model;
        private string _name;
        
        /// <summary>
        /// Required. Immutable. The name of the `Model` to use for cached content Format: `models/{model}`
        /// </summary>
        public string Model
        {
            get => _model; 
            set => _model = value.SanitizeModelName() ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Optional. Identifier. The resource name referring to the cached content. Format: `cachedContents/{id}`
        /// </summary>
        public string Name
        {
            get => _name; 
            set => _name = value.SanitizeCachedContentName() ?? throw new InvalidOperationException();
        }
        /// <summary>
        /// Specifies when this resource will expire.
        /// </summary>
        [JsonIgnore]
        public string Expiration
        {
            get
            {
                var value = ExpireTime?.ToString("o");
                value ??= TtlString;
                return value;
            }
        }

        /// <summary>
        /// Input only. New TTL for this resource, input only.
        /// </summary>
        [JsonIgnore]
        public TimeSpan Ttl { get; set; }

        [JsonPropertyName("ttl")]
        public string TtlString
        {
            get => $"{Ttl.TotalSeconds}s";
            set
            {
                if (TimeSpan.TryParse(value, out var ttl))
                {
                    Ttl = ttl;
                }
            }
        }
    }
}