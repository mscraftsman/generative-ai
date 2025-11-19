using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// The status of the URL retrieval.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<UrlRetrievalStatus>))]
    public enum UrlRetrievalStatus
    {
        /// <summary>
        /// Default value. This value is unused.
        /// </summary>
        UrlRetrievalStatusUnspecified,
        /// <summary>
        /// The URL was retrieved successfully.
        /// </summary>
        UrlRetrievalStatusSuccess,
        /// <summary>
        /// The URL retrieval failed.
        /// </summary>
        UrlRetrievalStatusError,
        /// <summary>
        /// Url retrieval is failed because the content is behind paywall.
        /// </summary>
        /// <remarks>
        /// This data type is not supported in Vertex AI.
        /// </remarks>
        UrlRetrievalStatusPaywall,
        /// <summary>
        /// Url retrieval is failed because the content is unsafe.
        /// </summary>
        /// <remarks>
        /// This data type is not supported in Vertex AI.
        /// </remarks>
        UrlRetrievalStatusUnsafe
    }
}