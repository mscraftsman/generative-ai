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
        public const string Gemini10Pro001 = "gemini-1.0-pro-001";
        public const string Gemini10ProVision001 = "gemini-1.0-pro-vision-001";
        public const string Gemini15Pro = "gemini-1.5-pro";
        public const string Gemini15ProVision = "gemini-1.5-pro-vision";
        public const string BisonText001 = "text-bison@001";
        public const string BisonText002 = "text-bison@002";
        public const string BisonText = BisonText002;
        public const string BisonText32k002 = "text-bison-32k@002";
        public const string BisonText32k = BisonChat32k002;
        public const string UnicornText001 = "text-unicorn@001";
        public const string BisonChat001 = "chat-bison@001";
        public const string BisonChat002 = "chat-bison@002";
        public const string BisonChat = BisonChat002;
        public const string BisonChat32k002 = "chat-bison-32k@002";
        public const string BisonChat32k = BisonChat32k002;
        public const string CodeBisonChat001 = "codechat-bison@001";
        public const string CodeBisonChat002 = "codechat-bison@002";
        public const string CodeBisonChat = BisonChat002;
        public const string CodeBisonChat32k002 = "codechat-bison-32k@002";
        public const string CodeBisonChat32k = BisonChat32k002;
        public const string CodeGecko001 = "code-gecko@001";
        public const string CodeGecko002 = "code-gecko@002";
        public const string CodeGeckoLatest = "code-gecko@latest";
        public const string CodeGecko = CodeGeckoLatest;
        public const string GeckoEmbedding = "embedding-gecko-001";
        public const string Embedding = "embedding-001";
        public const string AttributedQuestionAnswering = "aqa";
    }
}
