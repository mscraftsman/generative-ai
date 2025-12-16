using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Json.Schema;
using Json.Schema.Generation;
using System.Reflection;

namespace Mscc.GenerativeAI.Types
{
    public partial class Schema
    {
        private static readonly SchemaGeneratorConfiguration _schemaGeneratorConfiguration = new() { PropertyNameResolver = PropertyNameResolvers.CamelCase };
        
        /// <summary>
        /// Builds a Schema from a .NET object using Json.Schema generation, then maps it into the internal Schema model.
        /// </summary>
        public static Schema FromObject(object obj) => FromType(obj.GetType(), _schemaGeneratorConfiguration);

        /// <summary>
        /// Builds a Schema from a .NET type using Json.Schema generation, then maps it into the internal Schema model.
        /// </summary>
        /// <typeparam name="T">Any type.</typeparam>
        public static Schema FromType<T>() => FromType(typeof(T), _schemaGeneratorConfiguration);

        /// <summary>
        /// Builds a Schema from a .NET type using Json.Schema generation, then maps it into the internal Schema model.
        /// </summary>
        /// <param name="filename">The file name of the assembly's XML comment file.</param>
        /// <typeparam name="T">Any type in the assembly.</typeparam>
        public static Schema FromType<T>(string filename)
        {
            var config = _schemaGeneratorConfiguration;
            if (!string.IsNullOrEmpty(filename))
            {
                config = new SchemaGeneratorConfiguration() { PropertyNameResolver = PropertyNameResolvers.CamelCase };
                config.RegisterXmlCommentFile<T>(filename);
            }
            return FromType(typeof(T), config);
        }

        /// <summary>
        /// Builds a Schema from a .NET type using Json.Schema generation, then maps it into the internal Schema model.
        /// </summary>
        /// <param name="type">The type to generate.</param>
        public static Schema FromType(Type type) => FromType(type, _schemaGeneratorConfiguration);

        /// <summary>
        /// Builds a Schema from a .NET type using Json.Schema generation, then maps it into the internal Schema model.
        /// </summary>
        /// <param name="type">The type to generate.</param>
        /// <param name="config">The <see cref="SchemaGeneratorConfiguration"/> to use.</param>
        private static Schema FromType(Type type, SchemaGeneratorConfiguration config)
        {
            if (type is null) throw new ArgumentNullException(nameof(type));
            
            // Handle Nullable<T>
            bool isNullable = false;
            if (IsNullableValueType(type, out Type? underlyingNullable))
            {
                isNullable = true;
                type = underlyingNullable!;
            }

            // Special-case enums -> string enum of names
            if (type.IsEnum)
            {
                return BuildEnumSchema(type, isNullable);
            }

            // Handle arrays or enumerable of enum -> array with items as string enum
            if (TryGetEnumerableElementType(type, out Type? elementType) && elementType!.IsEnum)
            {
                return new Schema
                {
                    Type = ParameterType.Array,
                    Items = BuildEnumSchema(elementType, false),
                    Nullable = isNullable,
                };
            }

            JsonSchemaBuilder schemaBuilder = new();
            config ??= _schemaGeneratorConfiguration;
            JsonSchema jsonSchema = schemaBuilder.FromType(type, config).Build();

            JsonElement element = JsonSerializer.SerializeToElement(jsonSchema, jsonSchema.GetType());
            Schema schema = FromJsonElement(element);
            if (isNullable)
            {
                schema.Nullable = true;
            }
            return schema;
        }

        private static Schema BuildEnumSchema(Type enumType, bool nullable)
        {
            return new Schema
            {
                Type = ParameterType.String,
                Format = "enum",
                Description = GetTypeDescription(enumType),
                Enum = [..System.Enum.GetNames(enumType)],
                Nullable = nullable,
            };
        }

        private static bool IsNullableValueType(Type t, out Type? underlying)
        {
            if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                underlying = System.Nullable.GetUnderlyingType(t)!;
                return true;
            }
            underlying = null;
            return false;
        }

        private static bool TryGetEnumerableElementType(Type t, out Type? elementType)
        {
            if (t.IsArray)
            {
                elementType = t.GetElementType();
                return elementType != null;
            }

            if (t.IsGenericType)
            {
                var genDef = t.GetGenericTypeDefinition();
                if (genDef == typeof(IEnumerable<>) || genDef == typeof(ICollection<>) || genDef == typeof(IList<>) ||
                    genDef == typeof(List<>) || genDef == typeof(IReadOnlyCollection<>) || genDef == typeof(IReadOnlyList<>))
                {
                    elementType = t.GetGenericArguments()[0];
                    return true;
                }
            }

            elementType = null;
            return false;
        }

        private static string? GetTypeDescription(Type type)
        {
	        var attribute = type.GetCustomAttribute<DescriptionAttribute>();
	        return attribute?.Description;
        }
        
        private static string? GetMemberDescription(Type enumType)
        {
	        string? description = null;
	        try
	        {
				MemberInfo[] memberInfo = enumType.GetMember(enumType.Name);
				if (memberInfo.Length > 0)
				{
					var attributes = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
					if (attributes.Length > 0)
					{
						description = ((DescriptionAttribute)attributes[0]).Description;
					}
				}
	        }
	        catch (Exception e) { }

	        return description;
        }

        /// <summary>
        /// Builds a parameters <see cref="Schema"/> from a delegate's signature.
        /// Skips framework parameters such as <see cref="CancellationToken"/> that should not be exposed to the model.
        /// </summary>
        /// <param name="callback">The delegate whose parameters will be used to construct the schema.</param>
        /// <returns>
        /// A <see cref="Schema"/> representing the parameters of the delegate as an object with named properties,
        /// or <c>null</c> if there are no user-exposed parameters.
        /// </returns>
        internal static Schema? BuildParametersSchemaFromDelegate(Delegate callback)
        {
            MethodInfo method = callback.Method;
            Dictionary<string, Schema> properties = new();
            List<string> required = [];
            List<string> ordering = [];

            int orderIndex = 0;
            foreach (ParameterInfo parameter in method.GetParameters())
            {
                // Skip framework parameters that shouldn't be exposed to the model
                if (parameter.ParameterType == typeof(CancellationToken))
                {
                    continue;
                }

                string name = parameter.Name ?? $"arg{orderIndex}";
                ordering.Add(name);
                orderIndex++;

                Type paramType = parameter.ParameterType;
                Schema paramSchema = FromType(paramType);
                properties[name] = paramSchema;

                if (!parameter.IsOptional && !parameter.HasDefaultValue)
                {
                    required.Add(name);
                }
            }

            if (properties.Count == 0)
            {
                return null;
            }

            return new Schema { Type = ParameterType.Object, Properties = properties, Required = required.Count > 0 ? required : null, PropertyOrdering = ordering };
        }

        /// <summary>
        /// Builds a response <see cref="Schema"/> from a delegate's return type.
        /// Returns <c>null</c> for <c>void</c>, <c>Task</c>, or <c>ValueTask</c> without a result.
        /// </summary>
        /// <param name="callback">The delegate whose return type will be used to generate the schema.</param>
        /// <returns>
        /// A <see cref="Schema"/> representing the return type of the delegate,
        /// or <c>null</c> if the return type is <c>void</c>, <c>Task</c>, or <c>ValueTask</c> without a result.
        /// </returns>
        internal static Schema? BuildResponseSchemaFromDelegate(Delegate callback)
        {
            MethodInfo method = callback.Method;
            Type returnType = method.ReturnType;

            // Unwrap Task<T>
            if (typeof(Task).IsAssignableFrom(returnType))
            {
                if (returnType.IsGenericType)
                {
                    returnType = returnType.GetGenericArguments()[0];
                }
                else
                {
                    return null; // Task (non-generic)
                }
            }

            // Unwrap ValueTask and ValueTask<T>
            if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(ValueTask<>))
            {
                returnType = returnType.GetGenericArguments()[0];
            }
            else if (returnType == typeof(ValueTask))
            {
                return null; // ValueTask without result
            }

            if (returnType == typeof(void))
            {
                return null;
            }

            return FromType(returnType, _schemaGeneratorConfiguration);
        }
    }
}