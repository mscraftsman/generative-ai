#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Text.Json.Serialization;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Status of the url retrieval.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<UrlRetrievalStatus>))]
    public enum UrlRetrievalStatus
    {
        /// <summary>
        /// Default value. This value is unused.
        /// </summary>
        UrlRetrievalStatusUnspecificed,
        /// <summary>
        /// Url retrieval is successful.
        /// </summary>
        UrlRetrievalStatusSuccess,
        /// <summary>
        /// Url retrieval is failed due to error.
        /// </summary>
        UrlRetrievalStatusError
    }
}