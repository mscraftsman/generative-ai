using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI
{
    [JsonConverter(typeof(JsonStringEnumConverter<Type>))]
    public enum Type
    {
        /// <summary>
        /// TypeUnspecified means not specified, should not be used.
        /// </summary>
        TypeUnspecified = 0,
        /// <summary>
        /// TypeString means openAPI string type
        /// </summary>
        TypeString = 1,
        /// <summary>
        /// TypeNumber means openAPI number type
        /// </summary>
        TypeNumber = 2,
        /// <summary>
        /// TypeInteger means openAPI integer type
        /// </summary>
        TypeInteger = 3,
        /// <summary>
        /// TypeBoolean means openAPI boolean type
        /// </summary>
        TypeBoolean = 4,
        /// <summary>
        /// TypeArray means openAPI array type
        /// </summary>
        TypeArray = 5,
        /// <summary>
        /// TypeObject means openAPI object type
        /// </summary>
        TypeObject = 6
    }
}
