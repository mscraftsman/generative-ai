#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
#endif
using System.Text;

namespace Mscc.GenerativeAI
{
    public static class GenerativeModelExtensions
    {
        public static string? SanitizeModelName(this string? value)
        {
            if (value == null) return value;

            if (value.StartsWith("tuned", StringComparison.InvariantCultureIgnoreCase))
                return value;
            
            if (!value.StartsWith("model", StringComparison.InvariantCultureIgnoreCase))
            {
                return $"models/{value}";
            }
            return value;
        }

        public static string? GetValue(this JsonElement element, string key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            string result = null;
            if (element.TryGetProperty(key, out JsonElement value))
            {
                result = value.GetString();
            }

            return result;
        }

        public static void ReadDotEnv(string dotEnvFile = ".env")
        {
            if (!File.Exists(dotEnvFile)) return;

            foreach (var line in File.ReadAllLines(dotEnvFile))
            {
                var parts = line.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length != 2) continue;

                Environment.SetEnvironmentVariable(parts[0], parts[1]);
            }
        }

        public static string AddQueryString(this string requestUri, Dictionary<string, string?> queryStringParams)
        {
            bool startingQuestionMarkAdded = false;
            var sb = new StringBuilder();
            sb.Append(requestUri);
            foreach (var parameter in queryStringParams)
            {
                if (parameter.Value == null)
                {
                    continue;
                }

                sb.Append(startingQuestionMarkAdded ? '&' : '?');
                sb.Append($"{parameter.Key}={parameter.Value}");
                startingQuestionMarkAdded = true;
            }
            return sb.ToString();
        }
    }
}