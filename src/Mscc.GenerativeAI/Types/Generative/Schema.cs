#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Linq;
using System.Text.Json;
using System.Collections.Generic;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// The Schema object allows the definition of input and output data types. These types can be objects, but also primitives and arrays. Represents a select subset of an OpenAPI 3.0 schema object.
    /// </summary>
    public class Schema
    {
        /// <summary>
        /// Required. Data type.
        /// </summary>
        public ParameterType? Type { get; set; }

        /// <summary>
        /// Optional. The title of the schema.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Optional. The format of the data.
        /// This is used only for primitive datatypes.
        /// Supported formats:
        /// for NUMBER type: float, double
        /// for INTEGER type: int32, int64
        /// for STRING type: enum, date-time
        /// </summary>
        public string Format { get; set; } = "";

        /// <summary>
        /// Optional. A brief description of the parameter. This could contain examples of use. Parameter description may be formatted as Markdown.
        /// </summary>
        public string Description { get; set; } = "";

        /// <summary>
        /// Optional. Indicates if the value may be null.
        /// </summary>
        public bool? Nullable { get; set; }

        /// <summary>
        /// Optional. Schema of the elements of Type.ARRAY.
        /// </summary>
        public Schema? Items { get; set; }

        /// <summary>
        /// Optional. Maximum number of the elements for Type.ARRAY.
        /// </summary>
        public long? MaxItems { get; set; }

        /// <summary>
        /// Optional. Minimum number of the elements for Type.ARRAY.
        /// </summary>
        public long? MinItems { get; set; }

        /// <summary>
        /// Optional. Possible values of the element of Type.STRING with enum format.
        /// For example we can define an Enum Direction as :
        /// {type:STRING, format:enum, enum:["EAST", NORTH", "SOUTH", "WEST"]}
        /// </summary>
        public List<string>? Enum { get; set; }

        /// <summary>
        /// Optional. Properties of Type.OBJECT.
        /// An object containing a list of "key": value pairs. Example: { "name": "wrench", "mass": "1.3kg", "count": "3" }.
        /// </summary>
        public dynamic? Properties { get; set; }

        /// <summary>
        /// Optional. The order of the properties. Not a standard field in open api spec. Used to determine the order of the properties in the response.
        /// </summary>
        public List<string>? PropertyOrdering { get; set; }

        /// <summary>
        /// Optional. Required properties of Type.OBJECT.
        /// </summary>
        public List<string>? Required { get; set; }

        /// <summary>
        /// Optional. The value should be validated against any (one or more) of the subschemas in the list.
        /// </summary>
        public List<Schema>? AnyOf { get; set; }

        /// <summary>
        /// Optional. Maximum value of the Type.INTEGER and Type.NUMBER
        /// </summary>
        /// <remarks>SCHEMA FIELDS FOR TYPE INTEGER and NUMBER</remarks>
        public long? Maximum { get; set; }

        /// <summary>
        /// Optional. Minimum value of the Type.INTEGER and Type.NUMBER
        /// </summary>
        /// <remarks>SCHEMA FIELDS FOR TYPE INTEGER and NUMBER</remarks>
        public long? Minimum { get; set; }

        /// <summary>
        /// Optional. Default value of the field.
        /// Per JSON Schema, this field is intended for documentation generators and doesn't affect validation. Thus it's included here and ignored so that developers who send schemas with a `default` field don't get unknown-field errors.
        /// </summary>
        public object? Default { get; set; }

        /// <summary>
        /// Optional. Example of the object. Will only populated when the object is the root.
        /// </summary>
        public object? Example { get; set; }

        /// <summary>
        /// Optional. Maximum length of the Type.STRING
        /// </summary>
        public string? MaxLength { get; set; }

        /// <summary>
        /// Optional. Minimum length of the Type.STRING
        /// </summary>
        public string? MinLength { get; set; }

        /// <summary>
        /// Optional. Maximum number of the properties for Type.OBJECT.
        /// </summary>
        public string? MaxProperties { get; set; }

        /// <summary>
        /// Optional. Minimum number of the properties for Type.OBJECT.
        /// </summary>
        public string? MinProperties { get; set; }

        /// <summary>
        /// Optional. Pattern of the Type.STRING to restrict a string to a regular expression.
        /// </summary>
        public string? Pattern { get; set; }

        /// <summary>
        /// Converts a JsonElement representing a JSON Schema to a Schema object.
        /// </summary>
        /// <param name="jsonElement">The JsonElement containing the JSON Schema</param>
        /// <returns>A Schema object representing the JSON Schema</returns>
        public static Schema FromJsonElement(JsonElement jsonElement)
        {
            var schema = new Schema();

            if (jsonElement.ValueKind != JsonValueKind.Object)
            {
                throw new ArgumentException("JSON Schema must be an object", nameof(jsonElement));
            }

            // Parse type (can be string or array)
            if (jsonElement.TryGetProperty("type", out var typeElement))
            {
                var (parameterType, isNullable) = ParseParameterType(typeElement);
                schema.Type = parameterType;

                // Set nullable if "null" was found in type array, but don't override explicit nullable property
                if (isNullable && !jsonElement.TryGetProperty("nullable", out _))
                {
                    schema.Nullable = true;
                }
            }

            // Parse basic properties
            if (jsonElement.TryGetProperty("title", out var titleElement))
            {
                schema.Title = titleElement.GetString();
            }

            if (jsonElement.TryGetProperty("format", out var formatElement))
            {
                schema.Format = formatElement.GetString() ?? "";
            }

            if (jsonElement.TryGetProperty("description", out var descriptionElement))
            {
                schema.Description = descriptionElement.GetString() ?? "";
            }

            // Explicit nullable property takes precedence over type array inference
            if (jsonElement.TryGetProperty("nullable", out var nullableElement))
            {
                schema.Nullable = nullableElement.GetBoolean();
            }

            if (jsonElement.TryGetProperty("default", out var defaultElement))
            {
                schema.Default = ParseValue(defaultElement);
            }

            if (jsonElement.TryGetProperty("example", out var exampleElement))
            {
                schema.Example = ParseValue(exampleElement);
            }

            // Parse array-specific properties
            if (jsonElement.TryGetProperty("items", out var itemsElement))
            {
                schema.Items = FromJsonElement(itemsElement);
            }

            if (jsonElement.TryGetProperty("maxItems", out var maxItemsElement))
            {
                schema.MaxItems = maxItemsElement.GetInt64();
            }

            if (jsonElement.TryGetProperty("minItems", out var minItemsElement))
            {
                schema.MinItems = minItemsElement.GetInt64();
            }

            // Parse string-specific properties
            if (jsonElement.TryGetProperty("enum", out var enumElement))
            {
                schema.Enum = ParseStringArray(enumElement);
            }

            if (jsonElement.TryGetProperty("maxLength", out var maxLengthElement))
            {
                schema.MaxLength = maxLengthElement.GetString();
            }

            if (jsonElement.TryGetProperty("minLength", out var minLengthElement))
            {
                schema.MinLength = minLengthElement.GetString();
            }

            if (jsonElement.TryGetProperty("pattern", out var patternElement))
            {
                schema.Pattern = patternElement.GetString();
            }

            // Parse number-specific properties
            if (jsonElement.TryGetProperty("maximum", out var maximumElement))
            {
                schema.Maximum = maximumElement.GetInt64();
            }

            if (jsonElement.TryGetProperty("minimum", out var minimumElement))
            {
                schema.Minimum = minimumElement.GetInt64();
            }

            // Parse object-specific properties
            if (jsonElement.TryGetProperty("properties", out var propertiesElement))
            {
                schema.Properties = ParseProperties(propertiesElement);
            }

            if (jsonElement.TryGetProperty("required", out var requiredElement))
            {
                schema.Required = ParseStringArray(requiredElement);
            }

            if (jsonElement.TryGetProperty("propertyOrdering", out var propertyOrderingElement))
            {
                schema.PropertyOrdering = ParseStringArray(propertyOrderingElement);
            }

            if (jsonElement.TryGetProperty("maxProperties", out var maxPropertiesElement))
            {
                schema.MaxProperties = maxPropertiesElement.GetString();
            }

            if (jsonElement.TryGetProperty("minProperties", out var minPropertiesElement))
            {
                schema.MinProperties = minPropertiesElement.GetString();
            }

            // Parse anyOf
            if (jsonElement.TryGetProperty("anyOf", out var anyOfElement))
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
                var type = ParseSingleParameterType(typeElement.GetString());
                return (type, false);
            }
            else if (typeElement.ValueKind == JsonValueKind.Array)
            {
                // Array of types
                var types = new List<string>();
                foreach (var item in typeElement.EnumerateArray())
                {
                    if (item.ValueKind == JsonValueKind.String)
                    {
                        var typeStr = item.GetString();
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
                var actualType = types.FirstOrDefault(t => t != "null");
                var parameterType = ParseSingleParameterType(actualType);

                return (parameterType, isNullable);
            }
            else
            {
                return (ParameterType.TypeUnspecified, false);
            }
        }

        private static ParameterType ParseSingleParameterType(string? typeString)
        {
            return typeString?.ToLowerInvariant() switch
            {
                "string" => ParameterType.String,
                "number" => ParameterType.Number,
                "integer" => ParameterType.Integer,
                "boolean" => ParameterType.Boolean,
                "array" => ParameterType.Array,
                "object" => ParameterType.Object,
                "null" => ParameterType.Null,
                _ => ParameterType.TypeUnspecified
            };
        }

        private static object? ParseValue(JsonElement element)
        {
            return element.ValueKind switch
            {
                JsonValueKind.String => element.GetString(),
                JsonValueKind.Number => element.TryGetInt64(out var longValue) ? longValue : element.GetDouble(),
                JsonValueKind.True => true,
                JsonValueKind.False => false,
                JsonValueKind.Null => null,
                JsonValueKind.Array => ParseArray(element),
                JsonValueKind.Object => ParseObject(element),
                _ => null
            };
        }

        private static List<object> ParseArray(JsonElement arrayElement)
        {
            var result = new List<object>();
            foreach (var item in arrayElement.EnumerateArray())
            {
                var value = ParseValue(item);
                if (value != null)
                {
                    result.Add(value);
                }
            }

            return result;
        }

        private static Dictionary<string, object> ParseObject(JsonElement objectElement)
        {
            var result = new Dictionary<string, object>();
            foreach (var property in objectElement.EnumerateObject())
            {
                var value = ParseValue(property.Value);
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

            var result = new List<string>();
            foreach (var item in arrayElement.EnumerateArray())
            {
                var value = item.GetString();
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

            var result = new List<Schema>();
            foreach (var item in arrayElement.EnumerateArray())
            {
                result.Add(FromJsonElement(item));
            }

            return result.Count > 0 ? result : null;
        }

        private static dynamic? ParseProperties(JsonElement propertiesElement)
        {
            if (propertiesElement.ValueKind != JsonValueKind.Object)
            {
                return null;
            }

            var properties = new Dictionary<string, Schema>();
            foreach (var property in propertiesElement.EnumerateObject())
            {
                properties[property.Name] = FromJsonElement(property.Value);
            }

            return properties;
        }
    }
}