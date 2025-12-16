using Microsoft.Extensions.Logging;
using Mscc.GenerativeAI.Types;
using System.Text.Json;

namespace Mscc.GenerativeAI.Types
{
    public static class TypeExtensions
    {
        public static T? FromJson<T>(string json,
            JsonSerializerOptions? options = null,
            ILogger? logger = null) where T : class
        {
            try
            {
                return JsonSerializer.Deserialize<T>(json, options);
            }
            catch (JsonException e)
            {
                logger?.LogJsonDeserialization(e.ToString(), json);
                return null;
            }
        }
    }
}