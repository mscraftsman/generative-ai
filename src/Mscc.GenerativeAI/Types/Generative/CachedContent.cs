#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
#endif

namespace Mscc.GenerativeAI
{
    public class ListCachedContentsResponse
    {
        /// <summary>
        /// List of cached content resources.
        /// </summary>
        public List<CachedContent> CachedContents { get; set; }
        /// <summary>
        /// A token, which can be sent as pageToken to retrieve the next page.
        /// If this field is omitted, there are no more pages.
        /// </summary>
        public string? NextPageToken { get; set; }
    }
    
    public class CachedContent
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
        /// Optional. Immutable. The user-generated meaningful display name of the cached content. Maximum 128 Unicode characters.
        /// </summary>
        public string? DisplayName { get; set; }

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

        /// <summary>
        /// Output only. Creation time of the cache entry.
        /// </summary>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// Output only. When the cache entry was last updated in UTC time.
        /// </summary>
        public DateTime? UpdateTime { get; set; }
        /// <summary>
        /// Timestamp in UTC of when this resource is considered expired.
        /// This is *always* provided on output, regardless of what was sent on input.
        /// </summary>
        public DateTime? ExpireTime { get; set; }
        /// <summary>
        /// Optional. Input only. Immutable. The content to cache.
        /// </summary>
        public List<Content>? Contents { get; set; }
        /// <summary>
        /// Optional. Input only. Immutable. Developer set system instruction. Currently text only.
        /// </summary>
        public Content? SystemInstruction { get; set; }
        /// <summary>
        /// Optional. Input only. Immutable. A list of `Tools` the model may use to generate the next response
        /// </summary>
        public List<Tool>? Tools { get; set; }
        /// <summary>
        /// Optional. Input only. Immutable. Tool config. This config is shared for all tools.
        /// </summary>
        public ToolConfig? ToolConfig { get; set; }
        /// <summary>
        /// Output only. Metadata on the usage of the cached content.
        /// </summary>
        public CachedContentUsageMetadata UsageMetadata { get; set; }
    }
}