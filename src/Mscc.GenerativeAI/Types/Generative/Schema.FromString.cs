#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Collections.Generic;
using System.Text.Json;
#endif

namespace Mscc.GenerativeAI
{
    public partial class Schema
    {
        /// <summary>
        /// Creates a Schema from a string. Supports either:
        /// - A JSON object string representing a JSON Schema, or
        /// - A simple OpenAPI type name (string, number, integer, boolean, array, object, null).
        /// If the string is a JSON string containing a nested JSON object, it will be parsed recursively.
        /// </summary>
        /// <param name="value">JSON schema text or a simple type name</param>
        /// <exception cref="ArgumentNullException">Thrown when value is null or whitespace</exception>
        /// <exception cref="ArgumentException">Thrown when the provided string cannot be parsed into a Schema</exception>
        public static Schema FromString(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(value));

            // Try to parse as JSON (object or string-encoded JSON)
            if (!TryParseJson(value, out JsonElement element))
            {
                return FromTypeName(value);
            }

            if (element.ValueKind == JsonValueKind.Object)
            {
                return FromJsonElement(element);
            }

            if (element.ValueKind == JsonValueKind.String)
            {
                string? inner = element.GetString();
                if (!string.IsNullOrWhiteSpace(inner) && TryParseJson(inner, out JsonElement innerElement) && innerElement.ValueKind == JsonValueKind.Object)
                {
                    return FromJsonElement(innerElement);
                }

                // Fall through to type name handling
                return FromTypeName(inner);
            }

            if (element.ValueKind == JsonValueKind.Array)
            {
                // Interpret as anyOf list of schemas
                List<Schema> list = [];
                foreach (JsonElement item in element.EnumerateArray())
                {
                    if (item.ValueKind == JsonValueKind.Object)
                    {
                        list.Add(FromJsonElement(item));
                    }
                    else if (item.ValueKind == JsonValueKind.String)
                    {
                        list.Add(FromTypeName(item.GetString()));
                    }
                }

                if (list.Count > 0)
                {
                    return new Schema { AnyOf = list };
                }
            }

            // Fallback: interpret the string as a type name
            return FromTypeName(value);
        }

        private static Schema FromTypeName(string? typeName)
        {
            string? normalized = typeName?.Trim().ToLowerInvariant();
            return normalized switch
            {
                "string" => new Schema { Type = ParameterType.String },
                "number" => new Schema { Type = ParameterType.Number },
                "integer" => new Schema { Type = ParameterType.Integer },
                "boolean" => new Schema { Type = ParameterType.Boolean },
                "array" => new Schema { Type = ParameterType.Array },
                "object" => new Schema { Type = ParameterType.Object },
                "null" => new Schema { Type = ParameterType.Null, Nullable = true },
                _ => throw new ArgumentException($"Unrecognized schema string '{typeName}'. Provide JSON schema or a known type name.")
            };
        }

        private static bool TryParseJson(string text, out JsonElement element)
        {
            try
            {
                using JsonDocument doc = JsonDocument.Parse(text);
                element = doc.RootElement.Clone();
                return true;
            }
            catch
            {
                element = default;
                return false;
            }
        }
    }
}