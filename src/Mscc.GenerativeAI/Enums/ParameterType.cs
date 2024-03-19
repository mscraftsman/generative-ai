#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Text.Json.Serialization;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Type contains the list of OpenAPI data types as defined by https://spec.openapis.org/oas/v3.0.3#data-types
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<ParameterType>))]
    public enum ParameterType
    {
        /// <summary>
        /// Unspecified means not specified, should not be used.
        /// </summary>
        Unspecified = 0,
        /// <summary>
        /// String means openAPI string type
        /// </summary>
        String = 1,
        /// <summary>
        /// Number means openAPI number type
        /// </summary>
        Number = 2,
        /// <summary>
        /// Integer means openAPI integer type
        /// </summary>
        Integer = 3,
        /// <summary>
        /// Boolean means openAPI boolean type
        /// </summary>
        Boolean = 4,
        /// <summary>
        /// Array means openAPI array type
        /// </summary>
        Array = 5,
        /// <summary>
        /// Object means openAPI object type
        /// </summary>
        Object = 6
    }
}
