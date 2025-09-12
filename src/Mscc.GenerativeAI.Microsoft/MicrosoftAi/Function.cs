using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using mea = Microsoft.Extensions.AI;

namespace Mscc.GenerativeAI.Microsoft;

public class Function
{
    //private static readonly MethodInfo s_createMethod = typeof(Function).GetMethod(nameof(Create), BindingFlags.Static | BindingFlags.NonPublic)!;
    public static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
/*
    public static mea.AIFunction Create(Delegate function, string? name = null, string? description = null)
    {
        if (function is null)
        {
            throw new ArgumentNullException(nameof(function));
        }

        var methodInfo = function.Method;
        var parameters = methodInfo.GetParameters();
        var returnType = methodInfo.ReturnType;
        var functionName = name ?? methodInfo.Name;

        var parameterProperties = new JsonObject();
        var requiredParameters = new List<string>();

        foreach (var parameter in parameters)
        {
            var parameterName = parameter.Name!;
            var parameterType = ToJsonSchemaType(parameter.ParameterType);
            var parameterDescription = parameter.GetCustomAttribute<DescriptionAttribute>()?.Description;

            var parameterObject = new JsonObject
            {
                ["type"] = parameterType
            };

            if (parameterDescription is not null)
            {
                parameterObject["description"] = parameterDescription;
            }

            parameterProperties[parameterName] = parameterObject;

            if (!parameter.IsOptional)
            {
                requiredParameters.Add(parameterName);
            }
        }

        var functionParameters = new JsonObject
        {
            ["type"] = "object",
            ["properties"] = parameterProperties
        };

        if (requiredParameters.Count > 0)
        {
            functionParameters["required"] = new JsonArray(requiredParameters.Select(p => (JsonNode)p).ToArray());
        }

        return new mea.AIFunction(functionName, description ?? "", new BinaryData(functionParameters.ToJsonString(SerializerOptions)));
    }
*/
    
    private static string ToJsonSchemaType(Type type)
    {
        if (type == typeof(string)) return "string";
        if (type == typeof(int) || type == typeof(long) || type == typeof(short) || type == typeof(byte)) return "integer";
        if (type == typeof(float) || type == typeof(double) || type == typeof(decimal)) return "number";
        if (type == typeof(bool)) return "boolean";
        if (type.IsArray) return "array";
        return "object";
    }
}