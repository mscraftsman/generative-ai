using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Mscc.CodeGenerator
{
	class Program
	{
		private const string GeneratedNamespace = "Mscc.GenerativeAI.Types";

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
		private readonly string _pattern;
		private readonly Dictionary<string, string> _generatedEnums = new Dictionary<string, string>();

		private readonly Dictionary<string, string> _typeReplacements = new Dictionary<string, string>
		{
			{ "Category", "HarmCategory" },
			{ "Environment", "ComputerUseEnvironment" },
			{ "File", "FileResource" },
			{ "Model", "ModelResponse" },
			{ "Probability", "HarmProbability" },
			{ "ResponseModalities", "ResponseModality" },
			{ "Role", "PermissionRole" },
			{ "Threshold", "HarmBlockThreshold" },
			{ "TunedModel", "TunedModelResponse" },
			{ "Type", "ParameterType" },
			{ "List<File>", "List<FileResource>" },
			{ "List<Model>", "List<ModelResponse>" },
			{ "List<ResponseModalities>", "List<ResponseModality>" },
			{ "List<Tool>", "Tools" },
			{ "List<TunedModel>", "List<ModelResponse>" }
		};

		public CSharpCodeGenerator(string ns, JsonElement schemas)
		{
			_namespace = ns;
			_schemas = schemas;
			// 1. Escape keys (Handle things like +, ., *, etc.)
			// We DO NOT use \b here.
			string joinedKeys = string.Join("|", _typeReplacements.Keys
				.OrderByDescending(k => k.Length)
				.Select(Regex.Escape));

			// 2. Construct Pattern with Lookarounds
			// (?<!\S) -> Lookbehind: Ensure preceding char is NOT a non-whitespace char (i.e., it is space or start of line)
			// (...)   -> The escaped keys
			// (?!\S)  -> Lookahead: Ensure following char is NOT a non-whitespace char (i.e., it is space or end of line)
			_pattern = $@"(?<!\S)({joinedKeys})(?!\S)";
		}

		public void Generate(string outputDirectory)
		{
			foreach (var schema in _schemas.EnumerateObject())
			{
				var name = ReplaceTypeName(schema.Name);

				var sb = new StringBuilder();
				sb.AppendLine($"namespace {_namespace}");
				sb.AppendLine("{");
				GenerateType(sb, schema, outputDirectory);
				sb.Append("}");
				PrefixOutput(sb);
				File.WriteAllText(Path.Combine(outputDirectory, $"{name}.cs"), sb.ToString());
			}
		}

		private void GenerateType(StringBuilder sb, JsonProperty schema, string outputDirectory)
		{
			string typeName = ReplaceTypeName(schema.Name);
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
							var enumDescriptions = schemaValue.TryGetProperty("enumDescriptions", out var desc)
								? desc.EnumerateArray().Select(x => x.GetString()).ToList()
								: new List<string?>();
							GenerateEnum(sb, typeName, enumElement, enumDescriptions);
						}

						break;
				}
			}
		}

		private string ReplaceTypeName(string typeName)
		{
			// 2. Use Regex.Replace with a MatchEvaluator
			// This executes the replacement logic in a single pass.
			return Regex.Replace(typeName, _pattern, match => _typeReplacements[match.Value]);
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
						string enumName = $"{ReplaceTypeName(propertyName)}";
						if (!_generatedEnums.ContainsKey(enumName))
						{
							var enumSb = new StringBuilder();
							enumSb.AppendLine($"namespace {_namespace}");
							enumSb.AppendLine("{");
							var enumDescriptions = enumValue.TryGetProperty("enumDescriptions", out var desc)
								? desc.EnumerateArray().Select(x => x.GetString()).ToList()
								: new List<string?>();
							GenerateEnum(enumSb, enumName, enumElement, enumDescriptions);
							enumSb.Append("}");
							_generatedEnums.Add(enumName, enumSb.ToString());
							PrefixOutput(enumSb);
							File.WriteAllText(Path.Combine("Types", $"{enumName}.cs"), enumSb.ToString());
						}
					}

					string propertyType = ReplaceTypeName(GetCSharpType(propertyValue, className, propertyName));

					if (propertyValue.TryGetProperty("description", out JsonElement propertyDescription))
					{
						sb.AppendLine($"\t\t/// <summary>");
						sb.AppendLine($"\t\t/// {GetTypeReference(propertyDescription.GetString())}");
						sb.AppendLine($"\t\t/// </summary>");
					}

					var optional = propertyType == "bool" ? "" : "?";
					sb.AppendLine($"\t\tpublic {propertyType}{optional} {propertyName} {{ get; set; }}");
				}
			}

			sb.AppendLine("    }");
		}

		private void GenerateEnum(StringBuilder sb, string enumName, JsonElement enumValues,
			List<string?> enumDescriptions)
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
				"google-duration" => "string", // TimeSpan
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

		private static void PrefixOutput(StringBuilder stringBuilder)
		{
			var prefix = new StringBuilder();
			prefix.AppendLine("""
			                  /*
			                   * Copyright 2024-2025 Jochen Kirstätter
			                   *
			                   * Licensed under the Apache License, Version 2.0 (the "License");
			                   * you may not use this file except in compliance with the License.
			                   * You may obtain a copy of the License at
			                   *
			                   *      https://www.apache.org/licenses/LICENSE-2.0
			                   *
			                   * Unless required by applicable law or agreed to in writing, software
			                   * distributed under the License is distributed on an "AS IS" BASIS,
			                   * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
			                   * See the License for the specific language governing permissions and
			                   * limitations under the License.
			                   */
			                  """);

			var content = stringBuilder.ToString();
			if (content.Contains("DateTime"))
			{
				prefix.AppendLine("using System;");
			}

			if (content.Contains("List<"))
			{
				prefix.AppendLine("using System.Collections.Generic;");
			}

			if (content.Contains("JsonStringEnumConverter<"))
			{
				prefix.AppendLine("using System.Text.Json.Serialization;");
			}

			prefix.AppendLine("");

			stringBuilder.Insert(0, prefix.ToString());
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