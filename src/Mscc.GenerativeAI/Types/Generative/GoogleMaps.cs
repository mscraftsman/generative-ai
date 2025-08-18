#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Text.Json.Serialization;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Tool to support Google Maps in Model.
    /// </summary>
    public class GoogleMaps
    {
        /// <summary>
        /// Optional. Auth config for the Google Maps tool.
        /// </summary>
        //[JsonPropertyName("auth_config")]
        public AuthConfig? AuthConfig { get; set; }
    }
}