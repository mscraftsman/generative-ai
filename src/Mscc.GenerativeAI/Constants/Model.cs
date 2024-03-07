#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Linq;
#endif

namespace Mscc.GenerativeAI
{
    public static class Model
    {
        public const string GeminiPro = "gemini-pro";
        public const string GeminiProVision = "gemini-pro-vision";
        public const string Gemini10Pro = "gemini-1.0-pro";
        public const string Gemini10ProVision = "gemini-1.0-pro-vision";
        public const string Gemini15Pro = "gemini-1.5-pro";
        public const string Gemini15ProVision = "gemini-1.5-pro-vision";
        public const string BisonText = "text-bison-001";
        public const string BisonChat = "chat-bison-001";
        public const string GeckoEmbedding = "embedding-gecko-001";
        public const string Embedding = "embedding-001";
        public const string AttributedQuestionAnswering = "aqa";

        public static string Sanitize(this string value)
        {
            if (value.StartsWith("model", StringComparison.InvariantCultureIgnoreCase))
            {
                var parts = value.Split(new char[] { '/' }, System.StringSplitOptions.RemoveEmptyEntries);
                value = parts.Last();
            }
            return value.ToLower();
        }
    }
}
