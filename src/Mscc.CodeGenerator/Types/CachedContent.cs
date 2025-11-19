namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Content that has been preprocessed and can be used in subsequent request to GenerativeService. Cached content can be only used with model it was created for.
	/// </summary>
	public partial class CachedContent
	{
		/// <summary>
		/// Optional. Input only. Immutable. The content to cache.
		/// </summary>
		public List<Content>? Contents { get; set; }
		/// <summary>
		/// Output only. Creation time of the cache entry.
		/// </summary>
		public DateTime? CreateTime { get; set; }
		/// <summary>
		/// Optional. Immutable. The user-generated meaningful display name of the cached content. Maximum 128 Unicode characters.
		/// </summary>
		public string? DisplayName { get; set; }
		/// <summary>
		/// Timestamp in UTC of when this resource is considered expired. This is *always* provided on output, regardless of what was sent on input.
		/// </summary>
		public DateTime? ExpireTime { get; set; }
		/// <summary>
		/// Required. Immutable. The name of the <see cref="Model"/> to use for cached content Format: <see cref="models/{model}"/>
		/// </summary>
		public string? Model { get; set; }
		/// <summary>
		/// Output only. Identifier. The resource name referring to the cached content. Format: <see cref="cachedContents/{id}"/>
		/// </summary>
		public string? Name { get; set; }
		/// <summary>
		/// Optional. Input only. Immutable. Developer set system instruction. Currently text only.
		/// </summary>
		public Content? SystemInstruction { get; set; }
		/// <summary>
		/// Optional. Input only. Immutable. Tool config. This config is shared for all tools.
		/// </summary>
		public ToolConfig? ToolConfig { get; set; }
		/// <summary>
		/// Optional. Input only. Immutable. A list of <see cref="Tools"/> the model may use to generate the next response
		/// </summary>
		public List<Tool>? Tools { get; set; }
		/// <summary>
		/// Input only. New TTL for this resource, input only.
		/// </summary>
		public string? Ttl { get; set; }
		/// <summary>
		/// Output only. When the cache entry was last updated in UTC time.
		/// </summary>
		public DateTime? UpdateTime { get; set; }
		/// <summary>
		/// Output only. Metadata on the usage of the cached content.
		/// </summary>
		public CachedContentUsageMetadata? UsageMetadata { get; set; }
    }
}
