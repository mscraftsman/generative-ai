#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Linq;
using System.Text.Json;
#endif

namespace Mscc.GenerativeAI
{
    public static class GenerativeModelExtensions
    {
        public static string? SanitizeModelName(this string? value)
        {
            if (value == null) return value;

            if (value.StartsWith("model", StringComparison.InvariantCultureIgnoreCase))
            {
                var parts = value.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                value = parts.Last();
            }
            return value.ToLower();
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
    }
}