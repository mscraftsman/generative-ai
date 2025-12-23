using System;
using mea = Microsoft.Extensions.AI;

namespace Mscc.GenerativeAI.Microsoft
{
    /// <summary>
    /// Provides extension methods for integrating Mscc.GenerativeAI clients with Microsoft.Extensions.AI.
    /// These methods adapt Gemini clients to the interfaces used by the Microsoft.Extensions.AI framework.
    /// </summary>
    public static class GeminiClientExtensions
    {
        /// <summary>
        /// Creates a <see cref="mea.IChatClient"/> adapter for the specified <see cref="GenerativeModel"/>.
        /// </summary>
        /// <param name="chatClient">The <see cref="GenerativeModel"/> to adapt.</param>
        /// <returns>An <see cref="mea.IChatClient"/> that can be used to interact with the <see cref="GenerativeModel"/>.</returns>
        public static mea.IChatClient AsIChatClient(this GenerativeModel chatClient) =>
            new GeminiChatClient(chatClient);
        
        /// <summary>
        /// Creates a <see cref="mea.IChatClient"/> adapter for the specified <see cref="GeminiClient"/>.
        /// </summary>
        /// <param name="client">The <see cref="GeminiClient"/> to use.</param>
        /// <param name="model">The name of the model to use for chat. If not specified, the default model is used.</param>
        /// <returns>An <see cref="mea.IChatClient"/> that can be used to interact with the underlying <see cref="GenerativeModel"/>.</returns>
        public static mea.IChatClient AsIChatClient(this GeminiClient client, string? model = null)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));
            return client.GetGenerativeModel(model).AsIChatClient();
        }

        /// <summary>
        /// Creates an <see cref="mea.IEmbeddingGenerator{String, Embedding}"/> adapter for the specified <see cref="GenerativeModel"/>.
        /// </summary>
        /// <param name="embeddingClient">The <see cref="GenerativeModel"/> to adapt for embedding generation.</param>
        /// <param name="defaultModelDimensions">Optional. The number of dimensions to generate in each embedding.</param>
        /// <returns>An <see cref="mea.IEmbeddingGenerator{string, mea.Embedding{float}}"/> that can be used to generate embeddings.</returns>
        public static mea.IEmbeddingGenerator<string, mea.Embedding<float>> AsIEmbeddingGenerator(
            this GenerativeModel embeddingClient, int? defaultModelDimensions = null) =>
            new GeminiEmbeddingGenerator(embeddingClient, defaultModelDimensions);
        
        /// <summary>
        /// Creates an <see cref="mea.IEmbeddingGenerator{String, Embedding}"/> adapter for the specified <see cref="GeminiClient"/>.
        /// </summary>
        /// <param name="client">The <see cref="GeminiClient"/> to use.</param>
        /// <param name="model">The name of the model to use for embeddings. If not specified, the default model is used.</param>
        /// <param name="defaultModelDimensions">Optional. The number of dimensions to generate in each embedding.</param>
        /// <returns>An <see cref="mea.IEmbeddingGenerator{string, mea.Embedding{float}}"/> that can be used to generate embeddings.</returns>
        public static mea.IEmbeddingGenerator<string, mea.Embedding<float>> AsIEmbeddingGenerator(
            this GeminiClient client,
            string? model = null,
            int? defaultModelDimensions = null)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));
            return client.GetGenerativeModel(model).AsIEmbeddingGenerator(defaultModelDimensions);
        }
        
        /// <summary>
        /// Creates an <see cref="mea.ISpeechToTextClient"/> adapter for the specified <see cref="GenerativeModel"/>.
        /// </summary>
        /// <param name="audioClient">The <see cref="GenerativeModel"/> to adapt for speech-to-text.</param>
        /// <returns>An <see cref="mea.ISpeechToTextClient"/> that can be used to transcribe audio.</returns>
        //[Experimental("MEAI001")]
        public static mea.ISpeechToTextClient AsISpeechToTextClient(this GenerativeModel audioClient) =>
            new GeminiSpeechToTextClient(audioClient);
        
        /// <summary>
        /// Creates an <see cref="mea.ISpeechToTextClient"/> adapter for the specified <see cref="GeminiClient"/>.
        /// </summary>
        /// <param name="client">The <see cref="GeminiClient"/> to use.</param>
        /// <param name="model">The name of the model to use for speech-to-text. If not specified, the default model is used.</param>
        /// <returns>An <see cref="mea.ISpeechToTextClient"/> that can be used to transcribe audio.</returns>
        //[Experimental("MEAI001")]
        public static mea.ISpeechToTextClient AsISpeechToTextClient(this GeminiClient client, string? model = null)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));
            return client.GetGenerativeModel(model).AsISpeechToTextClient();
        }
        
        /// <summary>
        /// Converts a Gemini <see cref="ITool"/> to a Microsoft Extensions AI <see cref="mea.AITool"/>.
        /// </summary>
        /// <typeparam name="T">The type of the Gemini tool, which must implement <see cref="ITool"/>.</typeparam>
        /// <param name="tool">The instance of the Gemini tool to convert.</param>
        /// <returns>An instance of a <see cref="mea.AITool"/> that wraps the Gemini tool.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the specified <paramref name="tool"/> is null.</exception>
        public static mea.AITool AsAITool<T>(this T tool) where T : ITool
        {
	        if (tool is null) throw new ArgumentNullException(nameof(tool));

	        return new GeminiChatClient.GeminiAITool<T>(tool);
        }
    }
}