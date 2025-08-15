#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Custom DateTime JSON converter to serialize and deserialize ISO 8601 format without nanoseconds.
    /// </summary>
    public sealed class DateTimeFormatJsonConverter(string format = "yyyy-MM-ddTHH:mm:ssZ") : JsonConverter<DateTime>
    {
        //ISO 8601 format without nanos

        /// <inheritdoc cref="JsonConverter"/>
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateTime.Parse(reader.GetString()); // Or DateTime.ParseExact if you need strict parsing
        }

        /// <inheritdoc cref="JsonConverter"/>
        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(format));
        }
    }
}