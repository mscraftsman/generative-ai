using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// A custom JSON converter factory for enums that allows for flexible parsing.
    /// It can handle enums represented as strings (including snake_case and kebab-case)
    /// and integers.
    /// </summary>
	public class FlexibleEnumConverterFactory : JsonConverterFactory
	{
        /// <summary>
        /// Determines whether this converter can convert the specified type.
        /// </summary>
        /// <param name="typeToConvert">The type to check.</param>
        /// <returns><c>true</c> if the type is an enum; otherwise, <c>false</c>.</returns>
		public override bool CanConvert(Type typeToConvert)
		{
			return typeToConvert.IsEnum;
		}

        /// <summary>
        /// Creates a converter for the specified type.
        /// </summary>
        /// <param name="typeToConvert">The type to convert.</param>
        /// <param name="options">The JSON serializer options.</param>
        /// <returns>A new instance of the generic <see cref="FlexibleEnumConverter{TEnum}"/>.</returns>
		public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
		{
			// Create the specific converter for this Enum type
			Type converterType = typeof(FlexibleEnumConverter<>).MakeGenericType(typeToConvert);
			return (JsonConverter)Activator.CreateInstance(converterType);
		}

		private class FlexibleEnumConverter<TEnum> : JsonConverter<TEnum> where TEnum : struct, Enum
		{
            /// <summary>
            /// Reads and converts the JSON to type <typeparamref name="TEnum"/>.
            /// </summary>
            /// <param name="reader">The <see cref="Utf8JsonReader"/> to read from.</param>
            /// <param name="typeToConvert">The type to convert.</param>
            /// <param name="options">The JSON serializer options.</param>
            /// <returns>The converted enum value.</returns>
            /// <exception cref="JsonException">Thrown if the JSON value cannot be converted to the enum type.</exception>
			public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
			{
				if (reader.TokenType == JsonTokenType.String)
				{
					string enumText = reader.GetString();

					if (string.IsNullOrEmpty(enumText))
						throw new JsonException($"Unable to convert empty string to Enum {typeof(TEnum).Name}");

					// 1. Try exact/standard parsing first (fastest)
					if (Enum.TryParse(enumText, true, out TEnum result))
					{
						return result;
					}

					// 2. Fallback: Normalize for Snake Case / Kebab Case
					// Remove underscores and dashes, then try parsing again
					string normalized = enumText.Replace("_", "").Replace("-", "");

					if (Enum.TryParse(normalized, true, out result))
					{
						return result;
					}
				}
				else if (reader.TokenType == JsonTokenType.Number)
				{
					// Optional: Support integer values if needed
					int intValue = reader.GetInt32();
					if (Enum.IsDefined(typeof(TEnum), intValue))
					{
						return (TEnum)(object)intValue;
					}
				}

				throw new JsonException($"Unable to convert value '{reader.GetString()}' to Enum {typeof(TEnum).Name}");
			}

            /// <summary>
            /// Writes a specified value as JSON.
            /// </summary>
            /// <param name="writer">The <see cref="Utf8JsonWriter"/> to write to.</param>
            /// <param name="value">The value to write.</param>
            /// <param name="options">The JSON serializer options.</param>
			public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
			{
				// For Serialization: You can choose your preferred output format.
				// Option A: Write the exact Enum name (Default)
				// writer.WriteStringValue(value.ToString());

				// Option B: Force CamelCase on output (replicates JsonStringEnumConverter behavior)
				string name = value.ToString();
				string camelCase = JsonNamingPolicy.CamelCase.ConvertName(name);
				writer.WriteStringValue(camelCase);
			}
		}
	}
}