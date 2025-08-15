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
        TypeUnspecified = 0,
        /// <summary>
        /// String means openAPI string type
        /// </summary>
        String,
        /// <summary>
        /// Number means openAPI number type
        /// </summary>
        Number,
        /// <summary>
        /// Integer means openAPI integer type
        /// </summary>
        Integer,
        /// <summary>
        /// Boolean means openAPI boolean type
        /// </summary>
        Boolean,
        /// <summary>
        /// Array means openAPI array type
        /// </summary>
        Array,
        /// <summary>
        /// Object means openAPI object type
        /// </summary>
        Object,
        /// <summary>
        /// Null type.
        /// </summary>
        Null
    }
}
