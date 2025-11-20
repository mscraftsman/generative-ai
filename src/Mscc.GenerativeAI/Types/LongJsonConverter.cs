using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI.Types
{
    internal class LongJsonConverter : JsonConverter<long>
    {
        public override long Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                string? stringValue = reader.GetString();
                if (long.TryParse(stringValue, out long longValue))
                {
                    return longValue;
                }

                throw new JsonException($"Could not parse string '{stringValue}' to long.");
            }

            if (reader.TokenType == JsonTokenType.Number)
            {
                return reader.GetInt64();
            }

            throw new JsonException(
                $"Unexpected token type {reader.TokenType} when parsing long. Expected String or Number.");
        }

        public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value);
        }
    }
}