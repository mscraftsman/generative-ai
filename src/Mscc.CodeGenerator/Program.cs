using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Mscc.CodeGenerator
{
	sealed class Program
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

					var generator =
						new CSharpCodeGenerator(GeneratedNamespace, schemasElement, name, version, revision);
					generator.Generate(outputDirectory);
				}
				else
				{
					Console.WriteLine("'schemas' not found in the JSON file.");
				}
			}
		}
	}

	internal sealed class CSharpCodeGenerator
	{
		private readonly JsonElement _schemas;
		private static string? _name;
		private static string? _version;
		private static string? _revision;
		private static string? _namespace;
		private static string _pattern;

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
			"ContentFilter.Reason",
			"ContentEmbedding.Values",
			"CountTokensResponse.CachedContentTokenCount",
			"CountTokensResponse.TotalTokens",
			"CustomLongRunningOperation.Done",
			"EmbedContentRequest.Content",
			"Embedding.Value",
			"FunctionResponsePart.InlineData",
			"GenerateContentRequest.Labels",
			"ImageConfig.AspectRatio",
			"ImageConfig.ImageSize",
			"ModelResponse.Etag",
			"ModelResponse.Labels",
			"ModelResponse.VersionId",
			"Operation.Done",
			"Part.InlineData",
			"Part.Text",
			"PartMediaResolution.Level"
		};

		private readonly Dictionary<string, string> _typeReplacements = new Dictionary<string, string>
		{
			{ "Behavior", "BehaviorType" },
			{ "Category", "HarmCategory" },
			{ "EmbedContentRequest.TaskType", "TaskType" },
			{ "Environment", "ComputerUseEnvironment" },
			{ "File", "FileResource" },
			{ "Method", "HarmBlockMethod" },
			{ "Model", "ModelResponse" },
			{ "Probability", "HarmProbability" },
			{ "ResponseModalities", "ResponseModality" },
			{ "Role", "PermissionRole" },
			{ "Schema.Type", "ParameterType" },
			{ "Severity", "HarmSeverity" },
			{ "SupervisedTuningSpec.TuningModeType", "TuningMode" },
			{ "Threshold", "HarmBlockThreshold" },
			{ "TunedModel", "TunedModelResponse" },
			{ "List<File>", "List<FileResource>" },
			{ "List<Model>", "List<ModelResponse>" },
			{ "List<ResponseModalities>", "List<ResponseModality>" }, // List<Modality>
			{ "List<Tool>", "Tools" },
			{ "List<TunedModel>", "List<ModelResponse>" }
		};

		private readonly HashSet<string> _globalEnums = new HashSet<string>
		{
			"AdapterSize",
			"AnswerStyle",
			"BlockingConfidence",
			"BlockReason",
			"ComputerUseEnvironment",
			"DynamicRetrievalConfig.Mode",
			"EmbedContentRequest.TaskType",
			"FinishReason",
			"FunctionCallingConfig.Mode",
			"HarmCategory",
			"HarmBlockMethod",
			"HarmBlockThreshold",
			"HarmProbability",
			"HarmSeverity",
			"HttpElementLocation",
			"MediaResolution",
			"Outcome",
			"ParameterType",
			"PermissionRole",
			"Reason",
			"ResponseModality",
			"Schema.Type",
			"TaskType",
			"TuningMode",
			"UrlRetrievalStatus"
		};

		public CSharpCodeGenerator(string ns, JsonElement schemas, string? name = null, string? version = null,
			string? revision = null)
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
				sb.AppendLine(CultureInfo.InvariantCulture, $"namespace {_namespace}");
				sb.AppendLine("{");
				GenerateType(sb, schema, outputDirectory);
				sb.Append('}');
				PrefixOutput(sb);
				var filename = $"{name}";
				bool isEnum = schemaValue.TryGetProperty("type", out var typeProp) &&
				              typeProp.GetString() == "string" &&
				              schemaValue.TryGetProperty("enum", out _);

				if (!isEnum && !string.IsNullOrEmpty(_name))
				{
					filename += $".{_name}";
				}

				// Check if it is an enum and if it already exists
				if (isEnum)
				{
					var filePath = Path.Combine(outputDirectory, $"{filename}.cs");
					var existingMembers = GetExistingEnumMembers(filePath);
					var enumDeprecated = schemaValue.TryGetProperty("enumDeprecated", out var dep)
						? dep.EnumerateArray().Select(x => x.GetBoolean()).ToList()
						: new List<bool>();

					var newMembers = GetEnumMembers(schemaValue.GetProperty("enum"),
						schemaValue.TryGetProperty("enumDescriptions", out var desc)
							? desc.EnumerateArray().Select(x => x.GetString()).ToList()
							: new List<string?>(),
						enumDeprecated);

					foreach (var member in newMembers)
					{
						if (!existingMembers.Any(m => m.Name == member.Name))
						{
							existingMembers.Add(member);
						}
					}

					sb.Clear();
					sb.AppendLine(CultureInfo.InvariantCulture, $"namespace {_namespace}");
					sb.AppendLine("{");
					GenerateEnum(sb, name, existingMembers);
					sb.Append('}');
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
							var enumDeprecated = schemaValue.TryGetProperty("enumDeprecated", out var dep)
								? dep.EnumerateArray().Select(x => x.GetBoolean()).ToList()
								: new List<bool>();

							GenerateEnum(sb, typeName, GetEnumMembers(enumElement, enumDescriptions, enumDeprecated));
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
		private static string ExtractLastWord(string propertyName)
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
		private static string ToCamelCase(string pascalCase)
		{
			if (string.IsNullOrEmpty(pascalCase))
				return pascalCase;

			return char.ToLowerInvariant(pascalCase[0]) + pascalCase.Substring(1);
		}

		private void GenerateClass(StringBuilder sb, string className, JsonElement schema, string outputDirectory)
		{
			if (schema.TryGetProperty("description", out JsonElement classDescription))
			{
				sb.AppendLine(CultureInfo.InvariantCulture, $"\t/// <summary>");
				sb.AppendLine(CultureInfo.InvariantCulture, $"\t/// {GetTypeReference(classDescription.GetString()!)}");
				sb.AppendLine(CultureInfo.InvariantCulture, $"\t/// </summary>");
			}

			sb.AppendLine(CultureInfo.InvariantCulture, $"\tpublic partial class {className}");
			sb.AppendLine("	{");

			var existingProperties = GetExistingProperties(outputDirectory, className);
			var nestedEnums =
				new List<(string Name, List<(string Name, string? Description, bool IsDeprecated)> Members)>();

			if (schema.TryGetProperty("properties", out JsonElement properties))
			{
				if (properties.ValueKind == JsonValueKind.Object)
				{
					if (existingProperties.Count > 0)
					{
						sb.AppendLine();
					}

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

						string propertyType = ReplaceTypeName(GetCSharpType(propertyValue, className, propertyName));

						if (enumValue.TryGetProperty("enum", out JsonElement enumElement))
						{
							var enumDescriptions = enumValue.TryGetProperty("enumDescriptions", out var desc)
								? desc.EnumerateArray().Select(x => x.GetString()).ToList()
								: new List<string?>();

							var enumDeprecated = enumValue.TryGetProperty("enumDeprecated", out var dep)
								? dep.EnumerateArray().Select(x => x.GetBoolean()).ToList()
								: new List<bool>();

							var newMembers = GetEnumMembers(enumElement, enumDescriptions, enumDeprecated);

							// Special handling for "Type" property to avoid "TypeType"
							// Use className + propertyName instead (e.g., Schema.Type → SchemaType)
							string baseEnumName = (propertyName.EndsWith("Type", StringComparison.Ordinal)
							                       || propertyName.EndsWith("Mode", StringComparison.Ordinal))
								? $"{className}.{propertyName}"
								: propertyName;

							baseEnumName = ReplaceTypeName(baseEnumName);

							if (_globalEnums.Contains(baseEnumName))
							{
								GenerateGlobalEnumFile(outputDirectory, baseEnumName, newMembers);
								propertyType = baseEnumName.Replace(".", "", StringComparison.OrdinalIgnoreCase);
							}
							else
							{
								// Avoid "TypeType" by checking if baseEnumName already ends with "Type"
								string nestedEnumName = baseEnumName.EndsWith("Type", StringComparison.Ordinal)
									? $"{baseEnumName}"
									: $"{baseEnumName}Type";
								propertyType = nestedEnumName.Replace(".", "", StringComparison.OrdinalIgnoreCase);
								// Defer nested enum generation
								nestedEnums.Add((nestedEnumName, newMembers));
							}

							// Update property type logic
							if (propertyValue.TryGetProperty("items", out _))
							{
								propertyType = $"List<{propertyType}>";
							}
						}

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
							sb.AppendLine(CultureInfo.InvariantCulture, $"\t\t/// <summary>");
							sb.AppendLine(CultureInfo.InvariantCulture,
								$"\t\t/// {GetTypeReference(propertyDescription.GetString()!)}");
							sb.AppendLine(CultureInfo.InvariantCulture, $"\t\t/// </summary>");
						}

						// Add JsonPropertyName attribute if there's a naming conflict
						if (jsonPropertyName != null)
						{
							sb.AppendLine(CultureInfo.InvariantCulture,
								$"\t\t[JsonPropertyName(\"{jsonPropertyName}\")]");
						}

						// ToDo: Check JSON serialization for default / non-null values.
						var optional = "?"; // propertyType == "bool" ? "" : "?";
						sb.AppendLine(CultureInfo.InvariantCulture,
							$"\t\tpublic {propertyType}{optional} {actualPropertyName} {{ get; set; }}");
						existingProperties.Add(actualPropertyName);
					}
				}
			}

			foreach (var nestedEnum in nestedEnums)
			{
				GenerateEnum(sb, nestedEnum.Name, nestedEnum.Members, "\t\t");
			}

			sb.AppendLine("    }");
		}

		private static void GenerateGlobalEnumFile(string outputDirectory, string enumName,
			List<(string Name, string? Description, bool IsDeprecated)> members)
		{
			var sb = new StringBuilder();
			sb.AppendLine(CultureInfo.InvariantCulture, $"namespace {_namespace}");
			sb.AppendLine("{");

			GenerateEnum(sb, enumName, members, "\t");

			sb.Append("}");

			PrefixOutput(sb);
			File.WriteAllText(Path.Combine(outputDirectory, $"{enumName}.cs"), sb.ToString());
		}

		private static void GenerateEnum(StringBuilder sb, string enumName,
			List<(string Name, string? Description, bool IsDeprecated)> members, string indent = "\t")
		{
			enumName = enumName.Replace(".", "", StringComparison.OrdinalIgnoreCase);
			sb.AppendLine("");
			sb.AppendLine(CultureInfo.InvariantCulture,
				$"{indent}[JsonConverter(typeof(JsonStringEnumConverter<{enumName}>))]");
			sb.AppendLine(CultureInfo.InvariantCulture, $"{indent}public enum {enumName}");
			sb.AppendLine(CultureInfo.InvariantCulture, $"{indent}{{");

			foreach (var member in members)
			{
				if (!string.IsNullOrEmpty(member.Description))
				{
					sb.AppendLine(CultureInfo.InvariantCulture, $"{indent}\t/// <summary>");
					sb.AppendLine(CultureInfo.InvariantCulture, $"{indent}\t/// {EscapeXml(member.Description)}");
					sb.AppendLine(CultureInfo.InvariantCulture, $"{indent}\t/// </summary>");
				}

				if (member.IsDeprecated)
				{
					sb.AppendLine(CultureInfo.InvariantCulture,
						$"{indent}\t[Obsolete(\"The member type '{member.Name}' has been marked as deprecated. It might be removed in future versions.\")]");
				}

				sb.AppendLine(CultureInfo.InvariantCulture, $"{indent}\t{member.Name},");
			}

			sb.AppendLine(CultureInfo.InvariantCulture, $"{indent}}}");
		}

		private static List<(string Name, string? Description, bool IsDeprecated)> GetEnumMembers(
			JsonElement enumValues, List<string?> enumDescriptions, List<bool> enumDeprecated)
		{
			var members = new List<(string Name, string? Description, bool IsDeprecated)>();
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

					bool isDeprecated = false;
					if (i < enumDeprecated.Count)
					{
						isDeprecated = enumDeprecated[i];
					}

					members.Add((ToPascalCase(value, enumMember: true), description, isDeprecated));
				}

				i++;
			}

			return members;
		}

		private static string GetCSharpType(JsonElement property, string? className = null, string? propertyName = null)
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

		private static void PrefixOutput(StringBuilder stringBuilder)
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
			if (content.Contains("DateTime", StringComparison.OrdinalIgnoreCase))
			{
				prefix.AppendLine("using System;");
			}

			if (content.Contains("List<", StringComparison.OrdinalIgnoreCase))
			{
				prefix.AppendLine("using System.Collections.Generic;");
			}

			if (content.Contains("JsonStringEnumConverter<", StringComparison.OrdinalIgnoreCase)
			    || content.Contains("[Json", StringComparison.OrdinalIgnoreCase))
			{
				prefix.AppendLine("using System.Text.Json.Serialization;");
			}

			if (content.Contains("[Obsolete", StringComparison.OrdinalIgnoreCase))
			{
				prefix.AppendLine("using System;");
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

		private static string ToPascalCase(string s, string? containingClassName = null, bool enumMember = false)
		{
			if (string.IsNullOrEmpty(s))
				return s;

			if (s.StartsWith('_'))
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
				return s.ToUpperInvariant();

			var pascalCase = s.Substring(0, 1).ToUpperInvariant() + s.Substring(1);
			if (containingClassName == null && enumMember)
				pascalCase = s.Substring(0, 1).ToUpperInvariant() + s.Substring(1).ToLowerInvariant();
			if (s.IndexOfAny(['_', '-']) != -1)
			{
				pascalCase = string.Concat(s.Split(['_', '-'], StringSplitOptions.RemoveEmptyEntries)
					.Select(word => word.Substring(0, 1).ToUpperInvariant() + word.Substring(1).ToLowerInvariant()));
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

		private static List<(string Name, string? Description, bool IsDeprecated)> GetExistingEnumMembers(
			string filePath)
		{
			var members = new List<(string Name, string? Description, bool IsDeprecated)>();
			if (!File.Exists(filePath)) return members;

			var lines = File.ReadAllLines(filePath);
			string? currentDescription = null;
			bool nextMemberIsDeprecated = false;
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

				if (trimmed.StartsWith("[Obsolete", StringComparison.OrdinalIgnoreCase))
				{
					nextMemberIsDeprecated = true;
					continue;
				}

				var match = Regex.Match(trimmed, @"^(\w+),");
				if (match.Success)
				{
					var name = match.Groups[1].Value;
					members.Add((name, currentDescription, nextMemberIsDeprecated));
					currentDescription = null;
					nextMemberIsDeprecated = false;
				}
			}

			return members;
		}

		private static HashSet<string> GetExistingProperties(string outputDirectory, string className)
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

		private static bool TypeExists(string outputDirectory, string typeName)
		{
			if (!Directory.Exists(outputDirectory)) return false;
			return Directory.GetFiles(outputDirectory, $"{typeName}.*.cs").Length > 0 ||
			       File.Exists(Path.Combine(outputDirectory, $"{typeName}.cs"));
		}
	}
}