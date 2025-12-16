using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Globalization;

namespace Mscc.GenerativeAI.Types
{
    internal class NullableLongJsonConverter : JsonConverter<long?>
    {
        public override long? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }

            if (reader.TokenType == JsonTokenType.String)
            {
                string? stringValue = reader.GetString();
                if (string.IsNullOrEmpty(stringValue))
                {
                    return null;
                }

                if (long.TryParse(stringValue, out long longValue))
                {
                    return longValue;
                }

                throw new JsonException($"Could not parse string '{stringValue}' to long?.");
            }

            if (reader.TokenType == JsonTokenType.Number)
            {
                return reader.GetInt64();
            }

            throw new JsonException(
                $"Unexpected token type {reader.TokenType} when parsing long?. Expected String, Number, or Null.");
        }

        public override void Write(Utf8JsonWriter writer, long? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
            {
                writer.WriteNumberValue(value.Value);
            }
            else
            {
                writer.WriteNullValue();
            }
        }
    }
}