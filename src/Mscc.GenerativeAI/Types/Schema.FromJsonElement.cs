using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Globalization;

namespace Mscc.GenerativeAI
{
    public partial class Schema
    {
        /// <summary>
        /// Converts a JsonElement representing a JSON Schema to a Schema object.
        /// </summary>
        /// <param name="jsonElement">The JsonElement containing the JSON Schema</param>
        /// <returns>A Schema object representing the JSON Schema</returns>
        public static Schema FromJsonElement(JsonElement jsonElement)
        {
            Schema schema = new();

            if (jsonElement.ValueKind != JsonValueKind.Object)
            {
                throw new ArgumentException("JSON Schema must be an object", nameof(jsonElement));
            }

            // Parse type (can be string or array)
            if (jsonElement.TryGetProperty("type", out JsonElement typeElement))
            {
                (ParameterType parameterType, bool isNullable) = ParseParameterType(typeElement);
                schema.Type = parameterType;

                // Set nullable if "null" was found in type array, but don't override explicit nullable property
                if (isNullable && !jsonElement.TryGetProperty("nullable", out _))
                {
                    schema.Nullable = true;
                }
            }

            // Parse basic properties
            if (jsonElement.TryGetProperty("title", out JsonElement titleElement))
            {
                schema.Title = titleElement.GetString();
            }

            if (jsonElement.TryGetProperty("format", out JsonElement formatElement))
            {
                schema.Format = formatElement.GetString();
            }

            if (jsonElement.TryGetProperty("description", out JsonElement descriptionElement))
            {
                schema.Description = descriptionElement.GetString();
            }

            // Explicit nullable property takes precedence over type array inference
            if (jsonElement.TryGetProperty("nullable", out JsonElement nullableElement))
            {
                schema.Nullable = nullableElement.GetBoolean();
            }

            if (jsonElement.TryGetProperty("default", out JsonElement defaultElement))
            {
                schema.Default = ParseValue(defaultElement);
            }

            if (jsonElement.TryGetProperty("example", out JsonElement exampleElement))
            {
                schema.Example = ParseValue(exampleElement);
            }

            // Parse array-specific properties
            if (jsonElement.TryGetProperty("items", out JsonElement itemsElement))
            {
                schema.Items = FromJsonElement(itemsElement);
            }

            if (jsonElement.TryGetProperty("maxItems", out JsonElement maxItemsElement))
            {
                schema.MaxItems = maxItemsElement.GetInt64();
            }

            if (jsonElement.TryGetProperty("minItems", out JsonElement minItemsElement))
            {
                schema.MinItems = minItemsElement.GetInt64();
            }

            // Parse string-specific properties
            if (jsonElement.TryGetProperty("enum", out JsonElement enumElement))
            {
                schema.Enum = ParseStringArray(enumElement);
                schema.Type ??= ParameterType.String;
            }

            if (jsonElement.TryGetProperty("maxLength", out JsonElement maxLengthElement))
            {
                schema.MaxLength = maxLengthElement.ValueKind switch
                {
                    JsonValueKind.Number => Convert.ToString(maxLengthElement.GetInt32(), CultureInfo.InvariantCulture),
                    _ => maxLengthElement.GetString()
                };
            }

            if (jsonElement.TryGetProperty("minLength", out JsonElement minLengthElement))
            {
                schema.MinLength = minLengthElement.ValueKind switch
                {
                    JsonValueKind.Number => Convert.ToString(minLengthElement.GetInt32(), CultureInfo.InvariantCulture),
                    _ => minLengthElement.GetString()
                };
            }

            if (jsonElement.TryGetProperty("pattern", out JsonElement patternElement))
            {
                schema.Pattern = patternElement.GetString();
            }

            // Parse number-specific properties
            if (jsonElement.TryGetProperty("maximum", out JsonElement maximumElement))
            {
                schema.Maximum = maximumElement.GetInt64();
            }

            if (jsonElement.TryGetProperty("minimum", out JsonElement minimumElement))
            {
                schema.Minimum = minimumElement.GetInt64();
            }

            // Parse object-specific properties
            if (jsonElement.TryGetProperty("properties", out JsonElement propertiesElement))
            {
                schema.Properties = ParseProperties(propertiesElement);
            }

            if (jsonElement.TryGetProperty("required", out JsonElement requiredElement))
            {
                schema.Required = ParseStringArray(requiredElement);
            }

            if (jsonElement.TryGetProperty("propertyOrdering", out JsonElement propertyOrderingElement))
            {
                schema.PropertyOrdering = ParseStringArray(propertyOrderingElement);
            }

            if (jsonElement.TryGetProperty("maxProperties", out JsonElement maxPropertiesElement))
            {
                schema.MaxProperties = maxPropertiesElement.ValueKind switch
                {
                    JsonValueKind.Number => Convert.ToString(maxPropertiesElement.GetInt32(), CultureInfo.InvariantCulture),
                    _ => maxPropertiesElement.GetString()
                };
            }

            if (jsonElement.TryGetProperty("minProperties", out JsonElement minPropertiesElement))
            {
                schema.MinProperties = minPropertiesElement.ValueKind switch
                {
                    JsonValueKind.Number => Convert.ToString(minPropertiesElement.GetInt32(), CultureInfo.InvariantCulture),
                    _ => minPropertiesElement.GetString()
                };
            }

            // Parse anyOf
            if (jsonElement.TryGetProperty("anyOf", out JsonElement anyOfElement))
            {
                schema.AnyOf = ParseSchemaArray(anyOfElement);
            }

            return schema;
        }

        private static (ParameterType, bool) ParseParameterType(JsonElement typeElement)
        {
            if (typeElement.ValueKind == JsonValueKind.String)
            {
                // Single type as string
                ParameterType type = ParseSingleParameterType(typeElement.GetString());
                return (type, false);
            }
            else if (typeElement.ValueKind == JsonValueKind.Array)
            {
                // Array of types
                List<string> types = new();
                foreach (JsonElement item in typeElement.EnumerateArray())
                {
                    if (item.ValueKind == JsonValueKind.String)
                    {
                        string? typeStr = item.GetString();
                        if (typeStr != null)
                        {
                            types.Add(typeStr);
                        }
                    }
                }

                if (types.Count == 0)
                {
                    return (ParameterType.TypeUnspecified, false);
                }

                // Check if "null" is present
                bool isNullable = types.Contains("null");

                // Get the first non-null type as the actual type
                string? actualType = types.FirstOrDefault(t => t != "null");
                ParameterType parameterType = ParseSingleParameterType(actualType);

                return (parameterType, isNullable);
            }
            else
            {
                return (ParameterType.TypeUnspecified, false);
            }
        }

        private static ParameterType ParseSingleParameterType(string? typeString)
        {
            string? normalized = typeString?.Trim().ToUpperInvariant();
            return normalized switch
            {
                "STRING" => ParameterType.String,
                "NUMBER" => ParameterType.Number,
                "INTEGER" => ParameterType.Integer,
                "BOOLEAN" => ParameterType.Boolean,
                "ARRAY" => ParameterType.Array,
                "OBJECT" => ParameterType.Object,
                "NULL" => ParameterType.Null,
                _ => ParameterType.TypeUnspecified,
            };
        }

        private static object? ParseValue(JsonElement element)
        {
            return element.ValueKind switch
            {
                JsonValueKind.String => element.GetString(),
                JsonValueKind.Number => element.TryGetInt64(out long longValue) ? longValue : element.GetDouble(),
                JsonValueKind.True => true,
                JsonValueKind.False => false,
                JsonValueKind.Null => null,
                JsonValueKind.Array => ParseArray(element),
                JsonValueKind.Object => ParseObject(element),
                _ => null,
            };
        }

        private static List<object> ParseArray(JsonElement arrayElement)
        {
            List<object> result = new();
            foreach (JsonElement item in arrayElement.EnumerateArray())
            {
                object? value = ParseValue(item);
                if (value != null)
                {
                    result.Add(value);
                }
            }

            return result;
        }

        private static Dictionary<string, object> ParseObject(JsonElement objectElement)
        {
            Dictionary<string, object> result = new();
            foreach (JsonProperty property in objectElement.EnumerateObject())
            {
                object? value = ParseValue(property.Value);
                if (value != null)
                {
                    result[property.Name] = value;
                }
            }

            return result;
        }

        private static List<string>? ParseStringArray(JsonElement arrayElement)
        {
            if (arrayElement.ValueKind != JsonValueKind.Array)
            {
                return null;
            }

            List<string> result = new();
            foreach (JsonElement item in arrayElement.EnumerateArray())
            {
                string? value = item.GetString();
                if (value != null)
                {
                    result.Add(value);
                }
            }

            return result.Count > 0 ? result : null;
        }

        private static List<Schema>? ParseSchemaArray(JsonElement arrayElement)
        {
            if (arrayElement.ValueKind != JsonValueKind.Array)
            {
                return null;
            }

            List<Schema> result = new();
            foreach (JsonElement item in arrayElement.EnumerateArray())
            {
                result.Add(FromJsonElement(item));
            }

            return result.Count > 0 ? result : null;
        }

        private static Dictionary<string, Schema>? ParseProperties(JsonElement propertiesElement)
        {
            // From: https://json-schema.org/understanding-json-schema/reference/object#properties
            // The properties (key-value pairs) on an object are defined using the `properties` keyword.
            // The value of `properties` is an object, where each key is the name of a property and each
            // value is a schema used to validate that property. Any property that doesn't match any of
            // the property names in the `properties` keyword is ignored by this keyword.

            if (propertiesElement.ValueKind != JsonValueKind.Object)
            {
                return null;
            }

            Dictionary<string, Schema> properties = new();
            foreach (JsonProperty property in propertiesElement.EnumerateObject())
            {
                properties[property.Name] = FromJsonElement(property.Value);
            }

            return properties;
        }
    }
}