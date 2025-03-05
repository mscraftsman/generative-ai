#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Text.Json.Serialization;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// A basic data type.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<SchemaType>))]
    public enum SchemaType
    {
        TypeUnspecified,
        Object,
        String,
        Integer,
        Number,
        Boolean,
        Array
    }
}