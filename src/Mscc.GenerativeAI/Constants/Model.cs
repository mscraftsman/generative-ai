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

namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// Helper class to provide model names.
    /// Ref: https://cloud.google.com/vertex-ai/generative-ai/docs/learn/model-versioning#latest-version
    /// </summary>
    public static class Model
    {
        // Gemini 1.0
        // Discontinuation date: April 9, 2025.

        // Gemini 1.5
        // Discontinuation date: September 9, 2025.

        public const string GeminiExperimental = GeminiExperimental1206;
        public const string GeminiExperimental1114 = GeminiExperimental1206;
        public const string GeminiExperimental1121 = GeminiExperimental1206;
        public const string GeminiExperimental1206 = "gemini-exp-1206";

        // Gemini 2.0
        public const string Gemini20Pro = Gemini25Pro;
        public const string Gemini20Flash = "gemini-2.0-flash";
        public const string Gemini20Flash001 = "gemini-2.0-flash-001";
        public const string Gemini20FlashLite = "gemini-2.0-flash-lite";
        public const string Gemini20FlashLite001 = "gemini-2.0-flash-lite-001";
        public const string Gemini20FlashLitePreview = "gemini-2.0-flash-lite-preview";
        public const string Gemini20FlashLitePreview0205 = "gemini-2.0-flash-lite-preview-02-05";
        public const string Gemini20FlashExperimental = "gemini-2.0-flash-exp";
        public const string Gemini20FlashImageGenerationExperimental = "gemini-2.0-flash-exp-image-generation";

        public const string GeminiEmbedding = GeminiEmbedding001;
        public const string GeminiEmbedding001 = "gemini-embedding-001";
        public const string GeminiEmbeddingExperimental = "gemini-embedding-exp";
        public const string GeminiEmbeddingExperimental0307 = "gemini-embedding-exp-03-07";
        
        // Gemini 2.5
        public const string GeminiPro = Gemini25Pro;
        public const string GeminiFlashLatest = "gemini-flash-latest";
        public const string GeminiFlashLiteLatest = "gemini-flash-lite-latest";
        public const string GeminiProLatest = "gemini-pro-latest";
        public const string Gemini25Flash = "gemini-2.5-flash";
        public const string Gemini25FlashPreview = Gemini25FlashPreview092025;
        public const string Gemini25FlashPreview092025 = "gemini-2.5-flash-preview-09-2025";
        public const string Gemini25FlashLite = "gemini-2.5-flash-lite";
        public const string Gemini25FlashLitePreview = Gemini25FlashLitePreview092025;
        public const string Gemini25FlashLitePreview092025 = "gemini-2.5-flash-lite-preview-09-2025";
        public const string Gemini25FlashPreviewTts = "gemini-2.5-flash-preview-tts";
        public const string Gemini25FlashPreviewNativeAudioDialog = "gemini-2.5-flash-preview-native-audio-dialog-rai-v3";
        public const string Gemini25FlashImage = "gemini-2.5-flash-image";
        public const string Gemini25FlashImagePreview = "gemini-2.5-flash-image-preview";
        public const string Gemini25FlashNativeAudio = Gemini25FlashNativeAudioPreview122025;
        public const string Gemini25FlashNativeAudioPreview = Gemini25FlashNativeAudioPreview122025;
        public const string Gemini25FlashNativeAudioLatest = "gemini-2.5-flash-native-audio-latest";
        public const string Gemini25FlashNativeAudioPreview092025 = "gemini-2.5-flash-native-audio-preview-09-2025";
        public const string Gemini25FlashNativeAudioPreview122025 = "gemini-2.5-flash-native-audio-preview-12-2025";
        public const string Gemini25Pro = "gemini-2.5-pro";
        public const string Gemini25ProExperimental = Gemini25ProExperimental0325;
        public const string Gemini25ProExperimental0325 = "gemini-2.5-pro-exp-03-25";
        public const string Gemini25ProPreviewTts = "gemini-2.5-pro-preview-tts";
        public const string Gemini25LiveFlash = Gemini25LiveFlashPreview;
        public const string Gemini25LiveFlashPreview = "gemini-live-2.5-flash-preview";

        public const string GeminiRoboticsEr15 = GeminiRoboticsEr15Preview;
        public const string GeminiRoboticsEr15Preview = "gemini-robotics-er-1.5-preview";
        public const string Gemini25ComputerUse = Gemini25ComputerUsePreview102025;
        public const string Gemini25ComputerUsePreview = Gemini25ComputerUsePreview102025;
        public const string Gemini25ComputerUsePreview102025 = "gemini-2.5-computer-use-preview-10-2025";

        // Deprecated
        public const string Gemini25FlashPreview0520 = "gemini-2.5-flash-preview-05-20";
        public const string Gemini25FlashLitePreview0617 = "gemini-2.5-flash-lite-preview-06-17";
        
        // Gemini 3
        public const string Gemini3Pro = Gemini3ProPreview;
        public const string Gemini3ProPreview = "gemini-3-pro-preview";
        public const string Gemini3ProImage = Gemini3ProImagePreview;
        public const string Gemini3ProImagePreview = "gemini-3-pro-image-preview";
        public const string Gemini3Flash = Gemini3FlashPreview;
        public const string Gemini3FlashPreview = "gemini-3-flash-preview"; 
        
        // Nano Banana!
        public const string NanoBanana = Gemini25FlashImage;
        public const string NanoBananaPro = NanoBananaProPreview;
        public const string NanoBananaProPreview = "nano-banana-pro-preview";

        public const string DeepResearch = DeepResearchPro;
        public const string DeepResearchPro = DeepResearchProPreview;
        public const string DeepResearchProPreview = DeepResearchProPreview122025;
        public const string DeepResearchProPreview122025 = "deep-research-pro-preview-12-2025";
        
        // Gemma 3
        public const string Gemma3 = Gemma3_27B;
        // public const string Gemma3_270M = "gemma-3-270m-it";
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
        // Discontinuation date: Nov 18, 2025 or January 14, 2026.
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
        public const string Imagen4Preview0606 = "imagen-4.0-generate-preview-06-06";
        public const string Imagen4Fast = Imagen4FastGenerate001;
        public const string Imagen4FastGenerate001 = "imagen-4.0-fast-generate-001";
        public const string Imagen4Ultra = Imagen4UltraGenerate001;
        public const string Imagen4UltraGenerate001 = "imagen-4.0-ultra-generate-001";
        public const string Imagen3 = Imagen3Generate002;
        public const string Imagen3Generate002 = "imagen-3.0-generate-002";
        public const string Imagen3Fast = Imagen3GenerateFast001;
        public const string Imagen3GenerateFast = Imagen3GenerateFast001;
        public const string Imagen3GenerateFast001 = "imagen-3.0-fast-generate-001";
        public const string Imagen3Customization = Imagen3Capability001;
        public const string Imagen3Capability = Imagen3Capability001;
        public const string Imagen3Capability001 = "imagen-3.0-capability-001";
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
        public const string Veo = Veo3Generate;
        public const string Veo3 = Veo3Generate;
        public const string Veo3Generate = Veo3Generate001;
        public const string Veo3Generate001 = "veo-3.0-generate-001";
        public const string Veo3Preview = "veo-3.0-generate-preview";
        public const string Veo3Fast = Veo3FastGenerate;
        public const string Veo3FastGenerate = Veo3FastGenerate001;
        public const string Veo3FastGenerate001 = "veo-3.0-fast-generate-001";
        public const string Veo31 = Veo31Generate;
        public const string Veo31Generate = Veo31GeneratePreview;
        public const string Veo31GeneratePreview = "veo-3.1-generate-preview";
        public const string Veo31Fast = Veo31FastGenerate;
        public const string Veo31FastGenerate = Veo31FastGeneratePreview;
        public const string Veo31FastGeneratePreview = "veo-3.1-fast-generate-preview";

        // Lyria
        public const string Lyria = Lyria2;
        public const string Lyria2 = "lyria-002";
        public const string LyriaRealtime = LyriaRealtimeExperimental;
        public const string LyriaRealtimeExperimental = "lyria-realtime-exp";
    }
}