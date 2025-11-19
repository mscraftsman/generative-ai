using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Mscc.CodeGenerator
{
    class Program
    {
	    internal const string GeneratedNamespace = "Mscc.GenerativeAI.Types";
	    
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Please provide the path to the JSON schema file and the output directory.");
                return;
            }

            string schemaFilePath = args[0];
            if (!File.Exists(schemaFilePath))
            {
                Console.WriteLine($"File not found: {schemaFilePath}");
                return;
            }

            string outputDirectory = args[1];
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            string schemaContent = File.ReadAllText(schemaFilePath);
            using (JsonDocument doc = JsonDocument.Parse(schemaContent))
            {
                JsonElement root = doc.RootElement;
                if (root.TryGetProperty("schemas", out JsonElement schemasElement))
                {
                    var generator = new CSharpCodeGenerator(GeneratedNamespace, schemasElement);
                    generator.Generate(outputDirectory);
                }
                else
                {
                    Console.WriteLine("'schemas' not found in the JSON file.");
                }
            }
        }
    }

    public class CSharpCodeGenerator
    {
        private readonly JsonElement _schemas;
        private readonly string _namespace;
        private readonly Dictionary<string, string> _generatedEnums = new Dictionary<string, string>();

        public CSharpCodeGenerator(string ns, JsonElement schemas)
        {
            _namespace = ns;
            _schemas = schemas;
        }

        public void Generate(string outputDirectory)
        {
            foreach (var schema in _schemas.EnumerateObject())
            {
                var sb = new StringBuilder();
                sb.AppendLine($"namespace {_namespace}");
                sb.AppendLine("{");
                GenerateType(sb, schema, outputDirectory);
                sb.AppendLine("}");
                File.WriteAllText(Path.Combine(outputDirectory, $"{schema.Name}.cs"), sb.ToString());
            }
        }

        private void GenerateType(StringBuilder sb, JsonProperty schema, string outputDirectory)
        {
            string typeName = schema.Name;
            JsonElement schemaValue = schema.Value;

            if (schemaValue.TryGetProperty("type", out JsonElement typeElement))
            {
                string? type = typeElement.GetString();
                if (type == null) return;
                switch (type)
                {
                    case "object":
                        GenerateClass(sb, typeName, schemaValue, outputDirectory);
                        break;
                    case "string":
                        if (schemaValue.TryGetProperty("enum", out JsonElement enumElement))
                        {
                            var enumDescriptions = schemaValue.TryGetProperty("enumDescriptions", out var desc) ? desc.EnumerateArray().Select(x => x.GetString()).ToList() : new List<string?>();
                            GenerateEnum(sb, typeName, enumElement, enumDescriptions);
                        }
                        break;
                }
            }
        }

        private void GenerateClass(StringBuilder sb, string className, JsonElement schema, string outputDirectory)
        {
	        if (schema.TryGetProperty("description", out JsonElement classDescription))
	        {
		        sb.AppendLine($"\t/// <summary>");
		        sb.AppendLine($"\t/// {GetTypeReference(classDescription.GetString())}");
		        sb.AppendLine($"\t/// </summary>");
	        }
            sb.AppendLine($"\tpublic partial class {className}");
            sb.AppendLine("	{");

            if (schema.TryGetProperty("properties", out JsonElement properties))
            {
                foreach (var property in properties.EnumerateObject())
                {
                    string propertyName = ToPascalCase(property.Name, className);
                    JsonElement propertyValue = property.Value;
                    JsonElement enumValue = propertyValue;

                    if (propertyValue.TryGetProperty("items", out JsonElement itensElement))
                    {
	                    enumValue = itensElement;
                    }

                    if (enumValue.TryGetProperty("enum", out JsonElement enumElement))
                    {
                        string enumName = $"{propertyName}";
                        if (!_generatedEnums.ContainsKey(enumName))
                        {
                            var enumSb = new StringBuilder();
                            var enumDescriptions = enumValue.TryGetProperty("enumDescriptions", out var desc) ? desc.EnumerateArray().Select(x => x.GetString()).ToList() : new List<string?>();
                            GenerateEnum(enumSb, enumName, enumElement, enumDescriptions);
                            _generatedEnums.Add(enumName, enumSb.ToString());
                            File.WriteAllText(Path.Combine("Types", $"{enumName}.cs"), $"using System.Text.Json.Serialization;\n\nnamespace {_namespace}\n{{\n{enumSb.ToString()}}}");
                        }
                    }

                    string propertyType = GetCSharpType(propertyValue, className, propertyName);
                    
                    if (propertyValue.TryGetProperty("description", out JsonElement propertyDescription))
                    {
                        sb.AppendLine($"\t\t/// <summary>");
                        sb.AppendLine($"\t\t/// {GetTypeReference(propertyDescription.GetString())}");
                        sb.AppendLine($"\t\t/// </summary>");
                    }
                    sb.AppendLine($"\t\tpublic {propertyType}? {propertyName} {{ get; set; }}");
                }
            }

            sb.AppendLine("    }");
        }

        private void GenerateEnum(StringBuilder sb, string enumName, JsonElement enumValues, List<string?> enumDescriptions)
        {
	        sb.AppendLine($"	[JsonConverter(typeof(JsonStringEnumConverter<{enumName}>))]");
            sb.AppendLine($"    public enum {enumName}");
            sb.AppendLine("    {");

            int i = 0;
            foreach (var enumValue in enumValues.EnumerateArray())
            {
                var value = enumValue.GetString();
                if (value != null)
                {
                    if (i < enumDescriptions.Count && !string.IsNullOrEmpty(enumDescriptions[i]))
                    {
                        sb.AppendLine("        /// <summary>");
                        sb.AppendLine($"        /// {enumDescriptions[i]}");
                        sb.AppendLine("        /// </summary>");
                    }
                    sb.AppendLine($"        {ToPascalCase(value)},");
                }
                i++;
            }

            sb.AppendLine("    }");
        }

        private string GetCSharpType(JsonElement property, string? className = null, string? propertyName = null)
        {
            if (property.TryGetProperty("$ref", out JsonElement refElement))
            {
                return refElement.GetString() ?? "object";
            }

            if (property.TryGetProperty("type", out JsonElement typeElement))
            {
                string? type = typeElement.GetString();
                if (type == null) return "object";
                switch (type)
                {
                    case "string":
	                    if (property.TryGetProperty("enum", out _))
	                    {
		                    return $"{propertyName}";
	                    }
	                    if (property.TryGetProperty("format", out JsonElement stringFormatElement))
	                    {
		                    return GetTypeByFormat(stringFormatElement, type);
	                    }
                        return "string";
                    case "integer":
                        return "int";
                    case "number":
	                    if (property.TryGetProperty("format", out JsonElement numberFormatElement))
	                    {
		                    return GetTypeByFormat(numberFormatElement, type);
	                    }
                        return "double";
                    case "boolean":
                        return "bool";
                    case "array":
                        if (property.TryGetProperty("items", out JsonElement items))
                        {
                            return $"List<{GetCSharpType(items, className, propertyName)}>";
                        }
                        return "List<object>";
                    case "object":
                        return "object";
                    default:
                        return "object";
                }
            }

            return "object";
        }

        private static string GetTypeByFormat(JsonElement formatElement, string type)
        {
	        return (formatElement.GetString() switch
	        {
		        "byte" => "byte[]",
		        "int32" => "int",
		        "int64" => "long",
		        "google-datetime" => "DateTime",
		        "google-duration" => "string",	// TimeSpan
		        "google-fieldmask" => "string",
		        "double" => "double",
		        "float" => "double",
		        _ => type
	        })!;
        }

        private static string GetTypeReference(string value)
        {
	        string pattern = @"`([^`]+)`";
	        string replacement = @$"<see cref=""$1""/>";
	        string result = Regex.Replace(value, pattern, replacement);
	        return result;
        }

        private string ToPascalCase(string s, string? containingClassName = null)
        {
            if (string.IsNullOrEmpty(s))
                return s;

            if (s.Length == 1)
                return s.ToUpper();

            var pascalCase = s.Substring(0, 1).ToUpper() + s.Substring(1);
            if (containingClassName == null)
	            pascalCase = s.Substring(0, 1).ToUpper() + s.Substring(1).ToLower();
            if (s.IndexOfAny(['_', '-']) != -1)
            {
	            pascalCase = string.Concat(s.Split(['_', '-'], StringSplitOptions.RemoveEmptyEntries)
		            .Select(word => word.Substring(0, 1).ToUpper() + word.Substring(1).ToLower()));
            }

            if (pascalCase == containingClassName)
            {
                return pascalCase + "Property";
            }
            return pascalCase;
        }
    }
}