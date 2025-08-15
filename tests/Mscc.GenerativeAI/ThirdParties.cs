using Json.Schema;
using Json.Schema.Generation;
using Json.Schema.Generation.DataAnnotations;
using Microsoft.Extensions.Logging;
using Neovolve.Logging.Xunit;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Test.Mscc.GenerativeAI
{
    public class ThirdParties : LoggingTestsBase
    {
        public ThirdParties(ITestOutputHelper output) : base(output, LogLevel.Trace)
        {
            DataAnnotationsSupport.AddDataAnnotations();
        }

#if NET472_OR_GREATER || NETSTANDARD2_0
        public class Root();
#endif
#if NET9_0
        public record Root([System.ComponentModel.Description("A list of menus, each representing a specific day.")] List<Menu> Menus);

        public record Menu(DateOnly Date, List<Meal> Meals);

        public record Meal(string Type, [System.ComponentModel.DataAnnotations.MaxLength(50)] string Name, double? Weight, bool Selected)
        {
            public string FullName => $"{Weight}g {Name}";
        }

        [Fact]
        public Task Check_JsonSchemaGeneration_with_Nested_Records()
        {
            // object value = typeof(Root);
            object value = new Root([]);
            // object value = new List<Root>();
            var type = value.GetType();
            var options = DefaultJsonSerializerOptions();
            var schemaBuilder = new JsonSchemaBuilder();
//            JsonSchema schema = schemaBuilder.Build();
//            JsonSchema schemaType = schemaBuilder.Build();
            var config = new SchemaGeneratorConfiguration() { PropertyNameResolver = PropertyNameResolvers.CamelCase };
            var schema = schemaBuilder.FromType(type, config).Build();
            if (IsRecord(type))
            {
            }
            else
            {
//                schemaType = schemaBuilder.FromType<Root>(config).Build();
            }

            Output.WriteLine(JsonSerializer.Serialize(schema, schema.GetType(), options));
//            Output.WriteLine(JsonSerializer.Serialize(schemaType, schemaType.GetType(), options));
            return Task.CompletedTask;
        }

        [Fact]
        public Task Check_JsonSchemaGeneration_From_Type()
        {
            object value = new Root([]);
            var type = value.GetType();
            var options = DefaultJsonSerializerOptions();
            var config = new SchemaGeneratorConfiguration() { PropertyNameResolver = PropertyNameResolvers.CamelCase };
            var schemaBuilder = new JsonSchemaBuilder();
            // var schemaType = schemaBuilder.FromType<Root>(config).Build();
            // var schemaType = schemaBuilder.FromType(typeof(Root), config).Build();
            // var schemaType = schemaBuilder.FromType(value.GetType(), config).Build();
            var schemaType = schemaBuilder.FromType(type, config).Build();
            Output.WriteLine(JsonSerializer.Serialize(schemaType, schemaType.GetType(), options));
            return Task.CompletedTask;
        }
#endif

        /// <summary>
        /// Get default options for JSON serialization.
        /// </summary>
        /// <returns>default options for JSON serialization.</returns>
        private JsonSerializerOptions DefaultJsonSerializerOptions()
        {
            var options = new JsonSerializerOptions(JsonSerializerDefaults.Web)
            {
#if DEBUG
                WriteIndented = true,
#endif
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                NumberHandling = JsonNumberHandling.AllowReadingFromString,
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true,
#if NET9_0_OR_GREATER
                RespectNullableAnnotations = true
#endif
            };
            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseUpper));

            return options;
        }

        private static bool IsRecord(Type type)
        {
            // Check for the IsRecord property (C# 10 and later)
            var isRecordProperty = type.GetProperty("IsRecord", BindingFlags.Public | BindingFlags.Static);
            if (isRecordProperty != null)
            {
                return (bool)isRecordProperty.GetValue(null);
            }

            // Check for the EqualityContract property (C# 9 and later)
            var equalityContractProperty = type.GetProperty("EqualityContract",
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (equalityContractProperty != null)
            {
                return true;
            }

            // Check for the <Clone>$ method (C# 9)
            var cloneMethod = type.GetMethod("<Clone>$",
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (cloneMethod != null)
            {
                return true;
            }

            return false;
        }
    }
}