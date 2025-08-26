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
        // Discontinuation date: April 9, 2025.
        public const string Gemini10Pro = Gemini10Pro002;

        // Discontinuation date: April 9, 2025.
        public const string Gemini10Pro001 = "gemini-1.0-pro-001";

        // Discontinuation date: April 9, 2025.
        public const string Gemini10Pro002 = "gemini-1.0-pro-002";
        //public const string GeminiProVision = "gemini-pro-vision";
        public const string Gemini10ProVision = "gemini-1.0-pro-vision";

        // Discontinuation date: April 9, 2025.
        public const string Gemini10ProVision001 = "gemini-1.0-pro-vision-001";
        //public const string GeminiProVisionLatest = "gemini-1.0-pro-vision-latest";
        public const string GeminiUltra = "gemini-ultra";
        public const string GeminiUltraLatest = "gemini-1.0-ultra-latest";

        // Gemini 1.5
        public const string GeminiPro = Gemini25Pro;
        public const string Gemini15Pro = "gemini-1.5-pro";
        //public const string Gemini15Pro001 = "gemini-1.5-pro-001";
        public const string Gemini15Pro002 = "gemini-1.5-pro-002";
        public const string Gemini15ProTuning = Gemini15Pro002;
        public const string Gemini15ProPreview = "gemini-1.5-pro-preview-0409";
        public const string Gemini15ProExperimental0801 = GeminiExperimental1206;
        public const string Gemini15ProExperimental0827 = GeminiExperimental1206;
        public const string Gemini15ProExperimental = GeminiExperimental1206;
        public const string Gemini15ProLatest = "gemini-1.5-pro-latest";
        public const string Gemini15Flash = "gemini-1.5-flash";
        public const string Gemini15FlashLatest = "gemini-1.5-flash-latest";
        //public const string Gemini15Flash001 = "gemini-1.5-flash-001";
        public const string Gemini15Flash002 = "gemini-1.5-flash-002";
        //public const string Gemini15Flash001Tuning = "gemini-1.5-flash-001-tuning";
        //public const string Gemini15FlashTuning = Gemini15Flash001Tuning;
        public const string Gemini15Flash8B = "gemini-1.5-flash-8b";
        public const string Gemini15Flash8B001 = "gemini-1.5-flash-8b-001";
        public const string Gemini15Flash8BLatest = "gemini-1.5-flash-8b-latest";
        public const string Gemini15FlashExperimental0827 = GeminiExperimental1206;
        //public const string Gemini15FlashExperimental0827_8B = "gemini-1.5-flash-8b-exp-0827";
        //public const string Gemini15FlashExperimental0924_8B = "gemini-1.5-flash-8b-exp-0924";

        public const string GeminiExperimental = GeminiExperimental1206;
        public const string GeminiExperimental1114 = GeminiExperimental1206;
        public const string GeminiExperimental1121 = GeminiExperimental1206;
        public const string GeminiExperimental1206 = "gemini-exp-1206";
        public const string LearnLM = LearnLMExperimental;
        public const string LearnLM15 = LearnLMExperimental;
        public const string LearnLMExperimental = "learnlm-1.5-pro-experimental";

        // Gemini 2.0
        public const string Gemini20Pro = Gemini20ProExperimental;
        public const string Gemini20ProExperimental = "gemini-2.0-pro-exp";
        public const string Gemini20ProExperimental0205 = "gemini-2.0-pro-exp-02-05";
        public const string Gemini20Flash = "gemini-2.0-flash";
        public const string Gemini20Flash001 = "gemini-2.0-flash-001";
        public const string Gemini20FlashLite = "gemini-2.0-flash-lite";
        public const string Gemini20FlashLite001 = "gemini-2.0-flash-lite-001";
        public const string Gemini20FlashLitePreview = "gemini-2.0-flash-lite-preview";
        public const string Gemini20FlashLitePreview0205 = "gemini-2.0-flash-lite-preview-02-05";
        public const string Gemini20FlashLive = Gemini20FlashLive001;
        public const string Gemini20FlashLive001 = "gemini-2.0-flash-live-001";
        public const string Gemini20FlashExperimental = "gemini-2.0-flash-exp";
        public const string Gemini20FlashImageGeneration = Gemini20FlashImageGenerationPreview;
        public const string Gemini20FlashPreviewImageGeneration = "gemini-2.0-flash-preview-image-generation";
        public const string Gemini20FlashImageGenerationPreview = "gemini-2.0-flash-preview-image-generation";
        public const string Gemini20FlashImageGenerationExperimental = "gemini-2.0-flash-exp-image-generation";
        public const string Gemini20FlashThinking = Gemini20FlashThinkingExperimental;
        public const string Gemini20FlashThinkingExperimental = "gemini-2.0-flash-thinking-exp";
        public const string Gemini20FlashThinkingExperimentalNoThoughts = "gemini-2.0-flash-thinking-exp-no-thoughts";
        public const string Gemini20FlashThinkingExperimental1219 = "gemini-2.0-flash-thinking-exp-1219";
        public const string Gemini20FlashThinkingExperimental0121 = "gemini-2.0-flash-thinking-exp-01-21";

        public const string GeminiEmbedding = GeminiEmbedding001;
        public const string GeminiEmbedding001 = "gemini-embedding-001";
        public const string GeminiEmbeddingExperimental = "gemini-embedding-exp";
        public const string GeminiEmbeddingExperimental0307 = "gemini-embedding-exp-03-07";

        // Gemini 2.5
        public const string Gemini25Flash = "gemini-2.5-flash";
        public const string Gemini25FlashPreview = Gemini25FlashPreview0520;
        public const string Gemini25FlashPreview0520 = "gemini-2.5-flash-preview-05-20";
        public const string Gemini25FlashLite = "gemini-2.5-flash-lite";
        public const string Gemini25FlashLitePreview = Gemini25FlashLitePreview0617;
        public const string Gemini25FlashLitePreview0617 = "gemini-2.5-flash-lite-preview-06-17";
        public const string Gemini25FlashPreviewTts = "gemini-2.5-flash-preview-tts";
        public const string Gemini25FlashPreviewNativeAudio = "gemini-2.5-flash-preview-native-audio-dialog";
        public const string Gemini25FlashPreviewNativeAudioThinking = "gemini-2.5-flash-exp-native-audio-thinking-dialog";
        public const string Gemini25FlashPreviewNativeAudioDialog = "gemini-2.5-flash-preview-native-audio-dialog-rai-v3";
        public const string Gemini25FlashImage = Gemini25FlashImagePreview;
        public const string Gemini25FlashImagePreview = "gemini-2.5-flash-image-preview";
        public const string Gemini25Pro = "gemini-2.5-pro";
        public const string Gemini25ProExperimental = Gemini25ProExperimental0325;
        public const string Gemini25ProExperimental0325 = "gemini-2.5-pro-exp-03-25";
        public const string Gemini25ProPreview = Gemini25ProPreview0605;
        public const string Gemini25ProPreview0325 = "gemini-2.5-pro-preview-03-25";
        public const string Gemini25ProPreview0506 = "gemini-2.5-pro-preview-05-06";
        public const string Gemini25ProPreview0605 = "gemini-2.5-pro-preview-06-05";
        public const string Gemini25ProPreviewTts = "gemini-2.5-pro-preview-tts";
        public const string Gemini25LiveFlash = Gemini25LiveFlashPreview;
        public const string Gemini25LiveFlashPreview = "gemini-2.5-flash-live-preview";

        // Gemma 3
        public const string Gemma3 = Gemma3_27B;
        public const string Gemma3_1B = "gemma-3-1b-it";
        public const string Gemma3_4B = "gemma-3-4b-it";
        public const string Gemma3_12B = "gemma-3-12b-it";
        public const string Gemma3_27B = "gemma-3-27b-it";
        public const string Gemma3n = Gemma3n_E4B;
        public const string Gemma3n_E2B = "gemma-3n-e2b-it";
        public const string Gemma3n_E4B = "gemma-3n-e4b-it";

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

        // Discontinuation date: April 9, 2025.
        public const string CodeGecko002 = "code-gecko-002"; // Vertex: code-gecko@002
        public const string CodeGeckoLatest = "code-gecko@latest";
        public const string CodeGecko = "code-gecko";
        public const string GeckoEmbedding = "embedding-gecko-001";
        public const string Embedding001 = "embedding-001";
        public const string Embedding = Embedding001;
        public const string AttributedQuestionAnswering = "aqa";

        // Text Embeddings on Vertex AI
        // Discontinuation date: Nov 18, 2025.
        public const string TextEmbedding004 = "text-embedding-004";
        public const string TextEmbedding005 = "text-embedding-005";
        public const string TextEmbedding = TextEmbedding004;
        public const string TextEmbeddingPreview0815 = "text-embedding-preview-0815";
        public const string TextEmbeddingPreview = TextEmbeddingPreview0815;
        public const string TextMultilingualEmbedding = "text-multilingual-embedding-002";

        // Discontinuation date: April 9, 2025.
        public const string GeckoTextEmbedding001 = "textembedding-gecko@001";

        // Discontinuation date: April 9, 2025.
        public const string GeckoTextEmbedding002 = "textembedding-gecko@002";

        // Discontinuation date: May 14, 2025.
        public const string GeckoTextEmbedding003 = "textembedding-gecko@003";

        public const string GeckoTextEmbedding = GeckoTextEmbedding003;

        // Discontinuation date: May 14, 2025.
        public const string GeckoTextMultilingualEmbedding001 = "textembedding-gecko-multilingual@001";
        public const string GeckoTextMultilingualEmbedding = GeckoTextMultilingualEmbedding001;

        // Multimodal Embeddings on Vertex AI
        public const string MultimodalEmbedding001 = "multimodalembedding@001";
        public const string MultimodalEmbedding = MultimodalEmbedding001;

        // Models for Imagen on Vertex AI - image generation and editing
        public const string Imagen4 = Imagen4Generate001;
        public const string Imagen4Generate001 = "imagen-4.0-generate-001";
        public const string Imagen4Preview = Imagen4Preview0606;
        public const string Imagen4Preview0520 = "imagen-4.0-generate-preview-05-20";
        public const string Imagen4Preview0606 = "imagen-4.0-generate-preview-06-06";
        public const string Imagen4Fast = Imagen4FastGenerate001;
        public const string Imagen4FastGenerate001 = "imagen-4.0-fast-generate-001";
        public const string Imagen4Ultra = Imagen4UltraGenerate001;
        public const string Imagen4UltraGenerate001 = "imagen-4.0-ultra-generate-001";
        public const string Imagen4UltraPreview = Imagen4UltraPreview0606;
        public const string Imagen4UltraPreview0520 = "imagen-4.0-ultra-generate-exp-05-20";
        public const string Imagen4UltraPreview0606 = "imagen-4.0-ultra-generate-preview-06-06";
        public const string Imagen3 = Imagen3Generate002;
        public const string Imagen3Generate001 = "imagen-3.0-generate-001";
        public const string Imagen3Generate002 = "imagen-3.0-generate-002";
        public const string Imagen3Experimental = "imagen-3.0-generate-002-exp";
        public const string Imagen3Fast = Imagen3GenerateFast001;
        public const string Imagen3GenerateFast = Imagen3GenerateFast001;
        public const string Imagen3GenerateFast001 = "imagen-3.0-fast-generate-001";
        public const string Imagen3Customization = "imagen-3.0-capability-001";
        public const string ImageGeneration3 = "imagen-3.0-generate-0611";
        public const string ImageGeneration3Fast = "imagen-3.0-fast-generate-0611";
        public const string ImageVerification = "image-verification-001";
        public const string ImageGeneration006 = "imagegeneration@006";
        public const string ImageGeneration005 = "imagegeneration@005";
        public const string Imagen2 = ImageGeneration006;
        public const string ImageGeneration002 = "imagegeneration@002";
        public const string Imagen = ImageGeneration002;
        public const string ImageGeneration = Imagen2;
        public const string ImageText001 = "imagetext@001";
        public const string ImageText = "imagetext";

        // Veo
        public const string Veo = Veo2Generate001;
        public const string Veo2 = Veo2Generate001;
        public const string Veo2Generate = Veo2Generate001;
        public const string Veo2Generate001 = "veo-2.0-generate-001";
        public const string Veo3 = Veo3Preview;
        public const string Veo3Preview = "veo-3.0-generate-preview";
        public const string Veo3Fast = Veo3FastPreview;
        public const string Veo3FastPreview = "veo-3.0-fast-generate-preview";

        // Lyria
        public const string Lyria = LyriaBase001;
        public const string LyriaBase001 = "lyria-base-001";
        public const string Lyria2 = "lyria-002";
    }
}