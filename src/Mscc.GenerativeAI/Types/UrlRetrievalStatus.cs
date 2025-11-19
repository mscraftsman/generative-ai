using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI.Types
{
	[JsonConverter(typeof(JsonStringEnumConverter<UrlRetrievalStatus>))]
    public enum UrlRetrievalStatus
    {
        /// <summary>
        /// Default value. This value is unused.
        /// </summary>
        UrlRetrievalStatusUnspecified,
        /// <summary>
        /// Url retrieval is successful.
        /// </summary>
        UrlRetrievalStatusSuccess,
        /// <summary>
        /// Url retrieval is failed due to error.
        /// </summary>
        UrlRetrievalStatusError,
        /// <summary>
        /// Url retrieval is failed because the content is behind paywall.
        /// </summary>
        UrlRetrievalStatusPaywall,
        /// <summary>
        /// Url retrieval is failed because the content is unsafe.
        /// </summary>
        UrlRetrievalStatusUnsafe,
    }
}