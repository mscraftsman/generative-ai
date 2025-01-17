#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
#endif
using Json.Schema;
using Json.Schema.Generation;

namespace Mscc.GenerativeAI
{
    public sealed class ResponseSchemaJsonConverter : JsonConverter<object>
    {
        public override object? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return JsonSerializer.Deserialize<dynamic>(reader.GetString()!, options);
        }

        public override void Write(
            Utf8JsonWriter writer,
            object value,
            JsonSerializerOptions options)
        {
            var type = value.GetType();
            // How to figure out: type vs anonymous vs dynamic?
            if (type.Name.Substring(0,Math.Min(type.Name.Length, 20)).Contains("AnonymousType"))
            {
                var newOptions = new JsonSerializerOptions(options);
                newOptions.Converters.Remove(this);
                JsonSerializer.Serialize(writer, value, value.GetType(), newOptions);
            }
            else
            {
                var config = new SchemaGeneratorConfiguration()
                {
                    PropertyNameResolver = PropertyNameResolvers.CamelCase
                };
                var schemaBuilder = new JsonSchemaBuilder();
                var schema = schemaBuilder.FromType(type, config).Build();
                JsonSerializer.Serialize(writer, schema, schema.GetType(), options);
            }
        }
    }
}