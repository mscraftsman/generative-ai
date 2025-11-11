using System.Text.Json.Serialization;

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