using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// The location of the API key.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<HttpElementLocation>))]
    public enum HttpElementLocation
    {
        /// <summary>
        /// Unspecified location.
        /// </summary>
        HttpInUnspecified,
        /// <summary>
        /// Element is in the HTTP request query.
        /// </summary>
        HttpInQuery,
        /// <summary>
        /// Element is in the HTTP request header.
        /// </summary>
        HttpInHeader,
        /// <summary>
        /// Element is in the HTTP request path.
        /// </summary>
        HttpInPath,
        /// <summary>
        /// Element is in the HTTP request body.
        /// </summary>
        HttpInBody,
        /// <summary>
        /// Element is in the HTTP request cookie.
        /// </summary>
        HttpInCookie
    }
}