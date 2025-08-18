#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Text.Json.Serialization;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Type of auth scheme.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<AuthType>))]
    public enum AuthType
    {
        /// <summary>
        /// 
        /// </summary>
        AuthTypeUnspecified,
        /// <summary>
        /// 
        /// </summary>
        NoAuth,
        /// <summary>
        /// 
        /// </summary>
        ApiKeyAuth,
        /// <summary>
        /// 
        /// </summary>
        HttpBasicAuth,
        /// <summary>
        /// 
        /// </summary>
        GoogleServiceAccountAuth,
        /// <summary>
        /// 
        /// </summary>
        Oauth,
        /// <summary>
        /// 
        /// </summary>
        OidcAuth
    }
}