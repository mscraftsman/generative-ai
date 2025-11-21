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
					string? name = null;
					if (root.TryGetProperty("name", out JsonElement nameElement))
					{
						name = nameElement.GetString();
					}
					string? version = null;
					if (root.TryGetProperty("version", out JsonElement versionElement))
					{
						version = versionElement.GetString();
					}
					string? revision = null;
					if (root.TryGetProperty("revision", out JsonElement revisionElement))
					{
						revision = revisionElement.GetString();
					}
					var generator = new CSharpCodeGenerator(GeneratedNamespace, schemasElement, name, version, revision);
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
		private readonly string? _name;
		private readonly string? _version;
		private readonly string? _revision;
		private readonly string _namespace;
		private readonly string _pattern;
		private readonly Dictionary<string, string> _generatedEnums = new Dictionary<string, string>();

		private readonly Dictionary<string, string> _typeReplacements = new Dictionary<string, string>
		{
			{ "Behavior", "BehaviorType" },
			{ "Category", "HarmCategory" },
			{ "Environment", "ComputerUseEnvironment" },
			{ "File", "FileResource" },
			{ "Method", "HarmBlockMethod" },
			{ "Model", "ModelResponse" },
			{ "Probability", "HarmProbability" },
			{ "ResponseModalities", "ResponseModality" },
			{ "Role", "PermissionRole" },
			{ "Threshold", "HarmBlockThreshold" },
			{ "TunedModel", "TunedModelResponse" },
			{ "Type", "ParameterType" },
			{ "List<File>", "List<FileResource>" },
			{ "List<Model>", "List<ModelResponse>" },
			{ "List<ResponseModalities>", "List<ResponseModality>" },	// List<Modality>
			{ "List<Tool>", "Tools" },
			{ "List<TunedModel>", "List<ModelResponse>" }
		};

		private readonly List<string> _ignoredMembers = new List<string>
		{
			"CachedContent.Model",
			"CachedContent.Name",
			"CachedContent.Ttl",
			"Candidate.Content",
			"Chunk.Data",
			"Condition.Operation",
			"Content.Parts",
			"Content.Role",
			"ContentEmbedding.Values",
			"ContentFilter.Reason",
			"CountTokensResponse.CachedContentTokenCount",
			"CountTokensResponse.TotalTokens",
			"EmbedContentRequest.Content",
			"Embedding.Value",
			"FunctionResponsePart.InlineData",
			"GenerateContentRequest.Labels",
			"ImageConfig.AspectRatio",
			"ImageConfig.ImageSize",
			"ModelResponse.Labels",
			"ModelResponse.VersionId",
			"Part.InlineData",
			"Part.Text"
		};

		public CSharpCodeGenerator(string ns, JsonElement schemas, string? name = null, string? version = null, string? revision = null)
		{
			_namespace = ns;
			_schemas = schemas;
			_name = name;
			_version = version;
			_revision = revision;
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
				JsonElement schemaValue = schema.Value;

				var sb = new StringBuilder();
				sb.AppendLine($"namespace {_namespace}");
				sb.AppendLine("{");
				GenerateType(sb, schema, outputDirectory);
				sb.Append("}");
				PrefixOutput(sb);
				var filename = $"{name}";
				bool isEnum = sb.ToString().Contains($"public enum {name}");

				if (!isEnum && !string.IsNullOrEmpty(_name))
				{
					filename += $".{_name}";
				}
				
				// Check if it is an enum and if it already exists
				if (isEnum)
				{
					var filePath = Path.Combine(outputDirectory, $"{filename}.cs");
					var existingMembers = GetExistingEnumMembers(filePath);
					var newMembers = GetEnumMembers(schemaValue.GetProperty("enum"), 
						schemaValue.TryGetProperty("enumDescriptions", out var desc) 
							? desc.EnumerateArray().Select(x => x.GetString()).ToList() 
							: new List<string?>());
					
					foreach (var member in newMembers)
					{
						if (!existingMembers.Any(m => m.Name == member.Name))
						{
							existingMembers.Add(member);
						}
					}
					
					sb.Clear();
					sb.AppendLine($"namespace {_namespace}");
					sb.AppendLine("{");
					GenerateEnum(sb, name, existingMembers);
					sb.Append("}");
					PrefixOutput(sb);
				}

				File.WriteAllText(Path.Combine(outputDirectory, $"{filename}.cs"), sb.ToString());
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
							GenerateEnum(sb, typeName, GetEnumMembers(enumElement, enumDescriptions));
						}

						break;
				}
			}
		}

		private string ReplaceTypeName(string typeName)
		{
			var text = typeName;

			if (!string.IsNullOrEmpty(_name))
			{
				text = text.Replace(_name, "", StringComparison.InvariantCultureIgnoreCase);
			}
			if (!string.IsNullOrEmpty(_version))
			{
				text = text.Replace(_version, "", StringComparison.InvariantCultureIgnoreCase);
			}
			text = text.Replace("GoogleCloud", "", StringComparison.InvariantCultureIgnoreCase);

			// 2. Use Regex.Replace with a MatchEvaluator
			// This executes the replacement logic in a single pass.
			text = Regex.Replace(text, _pattern, match => _typeReplacements[match.Value]);

			return text;
		}

	/// <summary>
	/// Extracts the last word from a PascalCase property name.
	/// E.g., "SharePointSources" -> "Sources", "RawOutput" -> "Output"
	/// </summary>
	private string ExtractLastWord(string propertyName)
	{
		// Find uppercase letters to split the word
		var matches = Regex.Matches(propertyName, @"[A-Z][a-z]*");
		if (matches.Count > 1)
		{
			// Return the last word
			return matches[matches.Count - 1].Value;
		}
		// If only one word or no pattern found, return as-is
		return propertyName;
	}

	/// <summary>
	/// Converts a PascalCase string to camelCase.
	/// E.g., "SharePointSources" -> "sharePointSources"
	/// </summary>
	private string ToCamelCase(string pascalCase)
	{
		if (string.IsNullOrEmpty(pascalCase))
			return pascalCase;
		
		return char.ToLowerInvariant(pascalCase[0]) + pascalCase.Substring(1);
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

			var existingProperties = GetExistingProperties(outputDirectory, className);
			if (schema.TryGetProperty("properties", out JsonElement properties))
			{
				foreach (var property in properties.EnumerateObject())
				{
					string propertyName = ToPascalCase(property.Name);
					if (existingProperties.Contains(propertyName) || 
					    _ignoredMembers.Contains($"{className}.{propertyName}"))
					{
						continue;
					}

					JsonElement propertyValue = property.Value;
					JsonElement enumValue = propertyValue;

					if (propertyValue.TryGetProperty("items", out JsonElement itensElement))
					{
						enumValue = itensElement;
					}

					if (enumValue.TryGetProperty("enum", out JsonElement enumElement))
			{
				string enumName = $"{ReplaceTypeName(propertyName)}";
				
				// Always process and merge enum values
				var enumDescriptions = enumValue.TryGetProperty("enumDescriptions", out var desc)
					? desc.EnumerateArray().Select(x => x.GetString()).ToList()
					: new List<string?>();
				
				var newMembers = GetEnumMembers(enumElement, enumDescriptions);
				var filename = $"{enumName}";
				var filePath = Path.Combine(outputDirectory, $"{filename}.cs");

				// Merge with existing enum values if file exists
				if (TypeExists(outputDirectory, enumName))
				{
					var existingMembers = GetExistingEnumMembers(filePath);
					foreach (var member in newMembers)
					{
						if (!existingMembers.Any(m => m.Name == member.Name))
						{
							existingMembers.Add(member);
						}
					}
					newMembers = existingMembers;
				}

				// Generate or update the enum file
				var enumSb = new StringBuilder();
				enumSb.AppendLine($"namespace {_namespace}");
				enumSb.AppendLine("{");
				GenerateEnum(enumSb, enumName, newMembers);
				enumSb.Append("}");
				
				// Update the generated enums dictionary
				if (_generatedEnums.ContainsKey(enumName))
				{
					_generatedEnums[enumName] = enumSb.ToString();
				}
				else
				{
					_generatedEnums.Add(enumName, enumSb.ToString());
				}
				
				PrefixOutput(enumSb);
				File.WriteAllText(filePath, enumSb.ToString());
			}

		string propertyType = ReplaceTypeName(GetCSharpType(propertyValue, className, propertyName));

		// Check if property name conflicts with class name
		string actualPropertyName = propertyName;
		string? jsonPropertyName = null;
		
		if (propertyName.Equals(className, StringComparison.Ordinal))
		{
			// Extract the last word to use as the shortened property name
			actualPropertyName = ExtractLastWord(propertyName);
			// Store the original name in camelCase for JsonPropertyName attribute
			jsonPropertyName = ToCamelCase(propertyName);
		}

		if (propertyValue.TryGetProperty("description", out JsonElement propertyDescription))
		{
			sb.AppendLine($"\t\t/// <summary>");
			sb.AppendLine($"\t\t/// {GetTypeReference(propertyDescription.GetString())}");
			sb.AppendLine($"\t\t/// </summary>");
		}

		// Add JsonPropertyName attribute if there's a naming conflict
		if (jsonPropertyName != null)
		{
			sb.AppendLine($"\t\t[JsonPropertyName(\"{jsonPropertyName}\")]");
		}

		var optional = propertyType == "bool" ? "" : "?";
		sb.AppendLine($"\t\tpublic {propertyType}{optional} {actualPropertyName} {{ get; set; }}");
		existingProperties.Add(actualPropertyName);
	}
}

	sb.AppendLine("    }");
}

		private void GenerateEnum(StringBuilder sb, string enumName, List<(string Name, string? Description)> members)
		{
			sb.AppendLine($"	[JsonConverter(typeof(JsonStringEnumConverter<{enumName}>))]");
			sb.AppendLine($"    public enum {enumName}");
			sb.AppendLine("    {");

			foreach (var member in members)
			{
				if (!string.IsNullOrEmpty(member.Description))
				{
					sb.AppendLine("        /// <summary>");
					sb.AppendLine($"        /// {EscapeXml(member.Description)}");
					sb.AppendLine("        /// </summary>");
				}

				sb.AppendLine($"        {member.Name},");
			}

			sb.AppendLine("    }");
		}

		private List<(string Name, string? Description)> GetEnumMembers(JsonElement enumValues, List<string?> enumDescriptions)
		{
			var members = new List<(string Name, string? Description)>();
			int i = 0;
			foreach (var enumValue in enumValues.EnumerateArray())
			{
				var value = enumValue.GetString();
				if (value != null)
				{
					string? description = null;
					if (i < enumDescriptions.Count && !string.IsNullOrEmpty(enumDescriptions[i]))
					{
						description = enumDescriptions[i];
					}
					members.Add((ToPascalCase(value, enumMember:true), description));
				}
				i++;
			}
			return members;
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
			value = EscapeXml(value);
			string pattern = @"`([^`]+)`";
			string replacement = @$"<c>$1</c>";
			string result = Regex.Replace(value, pattern, replacement);
			return result;
		}

		private void PrefixOutput(StringBuilder stringBuilder)
		{
			var prefix = new StringBuilder();
			prefix.AppendLine("""
			                  /*
			                   * Copyleft 2024-2025 Jochen Kirstätter and contributors
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

			if (content.Contains("JsonStringEnumConverter<")
				|| content.Contains("[Json"))
			{
				prefix.AppendLine("using System.Text.Json.Serialization;");
			}

			prefix.AppendLine("");
			
			// Add auto-generation warning
			prefix.AppendLine("// *** AUTO-GENERATED FILE - DO NOT EDIT MANUALLY *** //");
			// if (!string.IsNullOrEmpty(_revision))
			// { 
			// 	prefix.AppendLine($" * Generated from schema {_name} - revision: {_revision}");
			// }
			// prefix.AppendLine($" * Generated on: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC");
			// prefix.AppendLine(" */");
			prefix.AppendLine("");

			stringBuilder.Insert(0, prefix.ToString());
		}

		private string ToPascalCase(string s, string? containingClassName = null, bool enumMember = false)
		{
			if (string.IsNullOrEmpty(s))
				return s;

			if (!string.IsNullOrEmpty(_name))
			{
				s = s.Replace(_name, "", StringComparison.InvariantCultureIgnoreCase);
			}
			if (!string.IsNullOrEmpty(_version))
			{
				s = s.Replace(_version, "", StringComparison.InvariantCultureIgnoreCase);
			}
			s = s.Replace("GoogleCloud", "", StringComparison.InvariantCultureIgnoreCase);

			if (s.Length == 1)
				return s.ToUpper();

			var pascalCase = s.Substring(0, 1).ToUpper() + s.Substring(1);
			if (containingClassName == null && enumMember)
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
		private static string EscapeXml(string? value)
		{
			if (string.IsNullOrEmpty(value)) return string.Empty;
			return value.Replace("&", "&amp;", StringComparison.InvariantCultureIgnoreCase)
				.Replace("<", "&lt;", StringComparison.InvariantCultureIgnoreCase)
				.Replace(">", "&gt;", StringComparison.InvariantCultureIgnoreCase)
				.Replace("\"", "&quot;", StringComparison.InvariantCultureIgnoreCase)
				.Replace("'", "&apos;", StringComparison.InvariantCultureIgnoreCase);
		}

		private static string UnescapeXml(string? value)
		{
			if (string.IsNullOrEmpty(value)) return string.Empty;
			return value.Replace("&amp;", "&", StringComparison.InvariantCultureIgnoreCase)
				.Replace("&lt;", "<", StringComparison.InvariantCultureIgnoreCase)
				.Replace("&gt;", ">", StringComparison.InvariantCultureIgnoreCase)
				.Replace("&quot;", "\"", StringComparison.InvariantCultureIgnoreCase)
				.Replace("&apos;", "'", StringComparison.InvariantCultureIgnoreCase);
		}

		private List<(string Name, string? Description)> GetExistingEnumMembers(string filePath)
		{
			var members = new List<(string Name, string? Description)>();
			if (!File.Exists(filePath)) return members;

			var lines = File.ReadAllLines(filePath);
			string? currentDescription = null;
			bool inSummary = false;

			foreach (var line in lines)
			{
				var trimmed = line.Trim();
				if (trimmed.StartsWith("/// <summary>", StringComparison.InvariantCultureIgnoreCase))
				{
					inSummary = true;
					continue;
				}
				if (trimmed.StartsWith("/// </summary>", StringComparison.InvariantCultureIgnoreCase))
				{
					inSummary = false;
					continue;
				}
				if (inSummary && trimmed.StartsWith("///", StringComparison.InvariantCultureIgnoreCase))
				{
					currentDescription = UnescapeXml(trimmed.Substring(3).Trim());
					continue;
				}

				var match = Regex.Match(trimmed, @"^(\w+),");
				if (match.Success)
				{
					members.Add((match.Groups[1].Value, currentDescription));
					currentDescription = null;
				}
			}
			return members;
		}

		private HashSet<string> GetExistingProperties(string outputDirectory, string className)
		{
			var existingProperties = new HashSet<string>();
			if (!Directory.Exists(outputDirectory))
			{
				return existingProperties;
			}

			var files = Directory.GetFiles(outputDirectory, $"{className}.*.cs").ToList();
			var mainFile = Path.Combine(outputDirectory, $"{className}.cs");
			if (File.Exists(mainFile))
			{
				files.Add(mainFile);
			}

			foreach (var file in files)
			{
				var content = File.ReadAllText(file);
				var matches = Regex.Matches(content, @"public\s+[\w\?<>\[\]]+\s+(\w+)\s*\{\s*get;\s*set;\s*\}");
				foreach (Match match in matches)
				{
					if (match.Success && match.Groups.Count > 1)
					{
						var prop = match.Groups[1].Value;
						existingProperties.Add(prop);
					}
				}
			}
			return existingProperties;
		}

		private bool TypeExists(string outputDirectory, string typeName)
		{
			if (!Directory.Exists(outputDirectory)) return false;
			return Directory.GetFiles(outputDirectory, $"{typeName}.*.cs").Any() || 
			       File.Exists(Path.Combine(outputDirectory, $"{typeName}.cs"));
		}
	}
}