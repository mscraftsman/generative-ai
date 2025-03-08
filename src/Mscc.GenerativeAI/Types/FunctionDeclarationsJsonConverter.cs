#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
#endif
using Json.Schema.Generation;
using System.Collections;
using System.Reflection;
using System.Text.Json.Nodes;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace Mscc.GenerativeAI
{
    public sealed class FunctionDeclarationsJsonConverter : JsonConverter<List<FunctionDeclaration>>
    {
        /// <inheritdoc cref="JsonConverter"/>
        public override List<FunctionDeclaration>? Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options)
        {
            return JsonSerializer.Deserialize<List<FunctionDeclaration>>(reader.GetString()!, options);
        }

        /// <inheritdoc cref="JsonConverter"/>
        public override void Write(Utf8JsonWriter writer,
            List<FunctionDeclaration> value,
            JsonSerializerOptions options)
        {
            var functionsArray = new JsonArray();
            foreach (var function in value)
            {
                functionsArray.Add(SerializeFunction(function));
            }

            JsonSerializer.Serialize(writer, functionsArray, options);
        }

        private JsonObject SerializeFunction(FunctionDeclaration function)
        {
            var propertiesObject = new JsonObject();
            var requiredArray = new JsonArray();
            var allRequired = true;

            if (function.Parameters is not null)
            {
                // foreach (var parameter in function.Parameters)
                // {
                // var normalizedName = parameter.Name.ToSnakeCase();
                // if (string.IsNullOrWhiteSpace(normalizedName))
                // {
                //     continue;
                // }
                //
                // var propertyObject = SerializeProperty(parameter.Type);
                //
                // if (!string.IsNullOrWhiteSpace(parameter.Description))
                // {
                //     propertyObject.Add("description", parameter.Description);
                // }
                //
                // if (parameter.EnumValues.Count > 0)
                // {
                //     _ = propertiesObject.Remove("enum");
                //
                //     var enumValuesArray = new JsonArray();
                //     var normalizedEnumValues = parameter.EnumValues.Select(v => v.ToSnakeCase()).Distinct();
                //
                //     foreach (var enumValue in normalizedEnumValues)
                //     {
                //         enumValuesArray.Add(enumValue);
                //     }
                //
                //     propertyObject.Add("enum", enumValuesArray);
                // }
                //
                // propertiesObject.Add(normalizedName, propertyObject);
                //
                // if (parameter.IsRequired)
                // {
                //     requiredArray.Add(normalizedName);
                // }
                // else
                // {
                //     allRequired = false;
                // }
                // }
            }
            else
            {
                foreach (var parameter in function.Callback!.Method.GetParameters())
                {
                    if (parameter.ParameterType == typeof(CancellationToken))
                    {
                        continue;
                    }

                    var parameterName = parameter.Name.ToSnakeCase();
                    var propertyObject = SerializeParameter(parameter);

                    propertiesObject.Add(parameterName, propertyObject);

                    if (!parameter.IsOptional || IsRequired(parameter))
                    {
                        requiredArray.Add(parameterName);
                    }
                }
            }

            var parametersObject = new JsonObject
            {
                { "type", Enum.GetName(typeof(SchemaType), SchemaType.Object)!.ToLower() }, 
                { "properties", propertiesObject }
            };

            if (requiredArray.Count != 0)
            {
                parametersObject.Add("required", requiredArray);
            }

            // var functionObject = new JsonObject { { "name", function.Name.ToSnakeCase() } };
            var functionObject = new JsonObject { { "name", function.Name } };

            var description = function.Description;
            if (string.IsNullOrWhiteSpace(description) && function.Callback is not null)
            {
                description = GetDescription(function.Callback.Method);
            }

            if (!string.IsNullOrWhiteSpace(description))
            {
                functionObject.Add("description", description);
            }

            // if (allRequired)
            // {
            //     functionObject.Add("strict", true);
            // }

            if (propertiesObject.Count > 0)
            {
                functionObject.Add("parameters", parametersObject);
            }

            return functionObject;
        }

        private static JsonObject SerializeParameter(ParameterInfo parameter)
        {
            var parameterType = parameter.ParameterType;
            var propertyObject = SerializeProperty(parameterType);
            var description = GetDescription(parameter);
            var format = GetFormatInfo(parameterType);

            if (!string.IsNullOrWhiteSpace(description))
            {
                propertyObject.Remove("description");
                propertyObject.Add("description", description);
            }

            if (!string.IsNullOrWhiteSpace(format))
            {
//                propertyObject.Add("format", format);
            }

            if (parameterType.IsNullableNumber())
            {
                propertyObject.Add("nullable", true);
            }

            if (parameter.IsOptional && parameter.DefaultValue is not null)
            {
                propertyObject.Add("default", parameter.DefaultValue.ToString());
            }

            return propertyObject;
        }

        private static JsonObject SerializeProperty(Type propertyType)
        {
            var (typeName, typeDescription) = GetTypeInfo(propertyType);
            var format = GetFormatInfo(propertyType);
            var propertyObject = new JsonObject
            {
                { "type", Enum.GetName(typeof(SchemaType), typeName)!.ToLower() }
            };

            if (typeDescription is not null)
            {
                propertyObject.Add("description", typeDescription);
            }

            if (!string.IsNullOrWhiteSpace(format))
            {
                propertyObject.Add("format", format);
            }

            if (propertyType.IsEnum)
            {
                var membersArray = new JsonArray();
                foreach (var enumMember in Enum.GetNames(propertyType))
                {
                    membersArray.Add(enumMember.ToSnakeCase());
                }

                propertyObject.Add("enum", membersArray);
            }
            else if (propertyType.IsArray && propertyType.HasElementType)
            {
                var itemType = propertyType.GetElementType()!;
                propertyObject.Add("items", SerializeProperty(itemType));
            }
            else if (typeof(IEnumerable).IsAssignableFrom(propertyType) &&
                     propertyType.GenericTypeArguments.Length == 1)
            {
                var itemType = propertyType.GenericTypeArguments[0];
                propertyObject.Add("items", SerializeProperty(itemType));
            }
            else if (propertyType.IsClass)
            {
                var properties = propertyType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.CanWrite);
                if (properties.Any())
                {
                    var propertiesObject = new JsonObject();
                    var requiredArray = new JsonArray();

                    foreach (var property in properties)
                    {
                        var propertyName = property.Name.ToSnakeCase();

                        propertiesObject.Add(propertyName, SerializeProperty(property.PropertyType));
                        requiredArray.Add(propertyName);
                    }

                    propertyObject.Add("properties", propertiesObject);
                    propertyObject.Add("required", requiredArray);
                }
            }

            return propertyObject;
        }

        private static string? GetDescription(MemberInfo member)
        {
            return member.GetCustomAttribute<DescriptionAttribute>()?.Description;
        }

        private static string? GetDescription(ParameterInfo parameter)
        {
            return parameter.GetCustomAttribute<DescriptionAttribute>()?.Description;
        }

        private static bool IsRequired(ParameterInfo parameter)
        {
            return parameter.GetCustomAttribute<RequiredAttribute>() is not null;
        }

        private static bool IsNullable(ParameterInfo parameter)
        {
            return parameter.ParameterType.IsNullableNumber();
        }

        private static (SchemaType name, string? description) GetTypeInfo(Type type)
        {
            var isNullable = type.IsNullableNumber();
            if (isNullable && type.GenericTypeArguments.Length == 1)
            {
                type = type.GenericTypeArguments[0];
            }

            if (type == typeof(bool))
            {
                return (SchemaType.Boolean, null);
            }

            if (type == typeof(sbyte))
            {
                return (SchemaType.Integer, "8-bit signed integer from -128 to 127");
            }

            if (type == typeof(byte))
            {
                return (SchemaType.Integer, "8-bit unsigned integer from 0 to 255");
            }

            if (type == typeof(short))
            {
                return (SchemaType.Integer, "16-bit signed integer from -32,768 to 32,767");
            }

            if (type == typeof(ushort))
            {
                return (SchemaType.Integer, "16-bit unsigned integer from 0 to 65,535");
            }

            if (type == typeof(int) || type == typeof(long) || type == typeof(nint))
            {
                return (SchemaType.Integer, null);
            }

            if (type == typeof(uint) || type == typeof(ulong) || type == typeof(nuint))
            {
                return (SchemaType.Integer, "unsigned integer, greater than or equal to 0");
            }

            if (type == typeof(float) || type == typeof(double) || type == typeof(decimal))
            {
                return (SchemaType.Number, "floating point number");
            }

            if (type == typeof(char))
            {
                return (SchemaType.String, "single character");
            }

            if (type == typeof(string))
            {
                return (SchemaType.String, null);
            }

            if (type == typeof(Uri))
            {
                return (SchemaType.String, "URI in C# .NET format https://example.com/abc");
            }

            if (type == typeof(Guid))
            {
                return (SchemaType.String, "GUID in C# .NET format separated by hyphens xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx");
            }

            if (type == typeof(DateTime) || type == typeof(DateTimeOffset))
            {
                return (SchemaType.String, "date and time in C# .NET ISO 8601 format yyyy-mm-ddThh:mm:ss");
            }

            if (type == typeof(TimeSpan))
            {
                return (SchemaType.String, "time interval in C# .NET ISO 8601 format hh:mm:ss");
            }
            
#if NET8_0_OR_GREATER
            if (type == typeof(DateOnly))
            {
                return (SchemaType.String, "date in C# .NET ISO 8601 format yyyy-mm-dd");
            }

            if (type == typeof(TimeOnly))
            {
                return (SchemaType.String, "time in C# .NET ISO 8601 format hh:mm:ss");
            }
#endif

            if (type.IsEnum)
            {
                return (SchemaType.String, null);
            }

            if (type.IsArray && type.HasElementType)
            {
                return (SchemaType.Array, null);
            }

            if (typeof(IEnumerable).IsAssignableFrom(type) && type.GenericTypeArguments.Length == 1)
            {
                return (SchemaType.Array, null);
            }

            return (SchemaType.Object, null);
        }

        private static string GetFormatInfo(Type type)
        {
            if (type.GenericTypeArguments.Length == 1)
            {
                type = type.GenericTypeArguments[0];
            }
            
            if (type == typeof(int) || type == typeof(nint) || type == typeof(uint) || type == typeof(nuint))
            {
                return "int32";
            }

            if (type == typeof(long) || type == typeof(ulong))
            {
                return "int64";
            }

            if (type == typeof(float) || type == typeof(decimal))
            {
                return "float";
            }
            
            if (type == typeof(double))
            {
                return "double";
            }

            if (type == typeof(DateTime) || type == typeof(DateTimeOffset))
            {
                return "date-time";
            }

            if (type.IsEnum)
            {
                return "enum";
            }

            return "";
        }
    }
}