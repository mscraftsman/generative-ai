#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Linq;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Helper class to provide model names.
    /// Ref: https://cloud.google.com/vertex-ai/generative-ai/docs/learn/model-versioning#latest-version
    /// </summary>
    public static class Model
    {
        // Gemini 1.0
        // Gemini 1.0 Pro will be discontinued on February 15, 2025.
        public const string GeminiPro = "gemini-pro";
        public const string Gemini10Pro = "gemini-1.0-pro";
        public const string Gemini10Pro001 = "gemini-1.0-pro-001";
        public const string Gemini10Pro002 = "gemini-1.0-pro-002";
        public const string Gemini10ProTuning = Gemini10Pro001;
        public const string GeminiProLatest = "gemini-1.0-pro-latest";
        public const string GeminiProVision = "gemini-pro-vision";
        public const string Gemini10ProVision = "gemini-1.0-pro-vision";
        public const string Gemini10ProVision001 = "gemini-1.0-pro-vision-001";
        public const string GeminiProVisionLatest = "gemini-1.0-pro-vision-latest";
        public const string GeminiUltra = "gemini-ultra";
        public const string GeminiUltraLatest = "gemini-1.0-ultra-latest";
        // Gemini 1.5
        public const string Gemini15Pro = "gemini-1.5-pro";
        public const string Gemini15Pro001 = "gemini-1.5-pro-001";
        public const string Gemini15Pro002 = "gemini-1.5-pro-002";
        public const string Gemini15ProTuning = Gemini15Pro002;
        public const string Gemini15ProPreview = "gemini-1.5-pro-preview-0409";
        public const string Gemini15ProExperimental0801 = "gemini-1.5-pro-exp-0801";
        public const string Gemini15ProExperimental0827 = "gemini-1.5-pro-exp-0827";
        public const string Gemini15ProExperimental = Gemini15ProExperimental0827;
        public const string Gemini15ProLatest = "gemini-1.5-pro-latest";
        public const string Gemini15Flash = "gemini-1.5-flash";
        public const string Gemini15FlashLatest = "gemini-1.5-flash-latest";
        public const string Gemini15Flash001 = "gemini-1.5-flash-001";
        public const string Gemini15Flash002 = "gemini-1.5-flash-002";
        public const string Gemini15Flash001Tuning = "gemini-1.5-flash-001-tuning";
        public const string Gemini15FlashTuning = Gemini15Flash001Tuning;
        public const string Gemini15Flash8B = "gemini-1.5-flash-8b";
        public const string Gemini15Flash8B001 = "gemini-1.5-flash-8b-001";
        public const string Gemini15Flash8BLatest = "gemini-1.5-flash-8b-latest";
        public const string Gemini15FlashExperimental0827 = "gemini-1.5-flash-exp-0827";
        public const string Gemini15FlashExperimental0827_8B = "gemini-1.5-flash-8b-exp-0827";
        public const string Gemini15FlashExperimental0924_8B = "gemini-1.5-flash-8b-exp-0924";

        public const string GeminiExperimental1114 = "gemini-exp-1114";
        public const string GeminiExperimental1121 = "gemini-exp-1121";
        public const string GeminiExperimental1206 = "gemini-exp-1206";
        public const string GeminiExperimental = GeminiExperimental1206;
        public const string LearnLM = LearnLMExperimental;
        public const string LearnLM15 = LearnLMExperimental;
        public const string LearnLMExperimental = "learnlm-1.5-pro-experimental";
        
        // PaLM 2 models
        public const string BisonText001 = "text-bison-001";
        public const string BisonText002 = "text-bison-002";
        public const string BisonText = BisonText001;
        public const string BisonText32k002 = "text-bison-32k-002";
        public const string BisonText32k = BisonChat32k002;
        public const string UnicornText001 = "text-unicorn-001";
        public const string UnicornText = UnicornText001;
        public const string BisonChat001 = "chat-bison-001";
        public const string BisonChat002 = "chat-bison-002";
        public const string BisonChat = BisonChat001;
        public const string BisonChat32k002 = "chat-bison-32k-002";
        public const string BisonChat32k = BisonChat32k002;
        public const string CodeBisonChat001 = "codechat-bison-001";
        public const string CodeBisonChat002 = "codechat-bison-002";
        public const string CodeBisonChat = BisonChat002;
        public const string CodeBisonChat32k002 = "codechat-bison-32k-002";
        public const string CodeBisonChat32k = BisonChat32k002;
        public const string CodeGecko001 = "code-gecko-001";
        public const string CodeGecko002 = "code-gecko-002";
        public const string CodeGeckoLatest = "code-gecko@latest";
        public const string CodeGecko = CodeGeckoLatest;
        public const string GeckoEmbedding = "embedding-gecko-001";
        public const string Embedding001 = "embedding-001";
        public const string Embedding = Embedding001;
        public const string AttributedQuestionAnswering = "aqa";
        
        // Text Embeddings on Vertex AI
        public const string TextEmbedding004 = "text-embedding-004";
        public const string TextEmbedding = TextEmbedding004;
        public const string TextEmbeddingPreview0815 = "text-embedding-preview-0815";
        public const string TextEmbeddingPreview = TextEmbeddingPreview0815;
        public const string TextMultilingualEmbedding = "text-multilingual-embedding-002";
        public const string GeckoTextEmbedding001 = "textembedding-gecko@001";
        public const string GeckoTextEmbedding003 = "textembedding-gecko@003";
        public const string GeckoTextEmbedding = GeckoTextEmbedding003;
        public const string GeckoTextMultilingualEmbedding001 = "textembedding-gecko-multilingual@001";
        public const string GeckoTextMultilingualEmbedding = GeckoTextMultilingualEmbedding001;

        // Multimodal Embeddings on Vertex AI
        public const string MultimodalEmbedding001 = "multimodalembedding@001";
        public const string MultimodalEmbedding = MultimodalEmbedding001;
        
        // Models for Imagen on Vertex AI - image generation and editing
        public const string Imagen3 = "imagen-3.0-generate-001";
        public const string Imagen3Fast = "imagen-3.0-fast-generate-001";
        /// <summary>
        /// Imagen 3 Generation is a Pre-GA. Allowlisting required.
        /// </summary>
        public const string ImageGeneration3 = "imagen-3.0-generate-0611";
        /// <summary>
        /// Imagen 3 Generation is a Pre-GA. Allowlisting required.
        /// </summary>
        public const string ImageGeneration3Fast = "imagen-3.0-fast-generate-0611";
        public const string ImageGeneration006 = "imagegeneration@006";
        public const string ImageGeneration005 = "imagegeneration@005";
        public const string Imagen2 = ImageGeneration006;
        public const string ImageGeneration002 = "imagegeneration@002";
        public const string Imagen = ImageGeneration002;
        public const string ImageGeneration = Imagen2;
        public const string ImageText001 = "imagetext@001";
        public const string ImageText = ImageText001;
    }
}
