using System;
using mea = Microsoft.Extensions.AI;

namespace Mscc.GenerativeAI.Microsoft
{
    public static class GeminiClientExtensions
    {
        /// <summary>Gets an <see cref="mea.IChatClient"/> for use with this <see cref="GenerativeModel"/>.</summary>
        /// <param name="chatClient">The client.</param>
        /// <returns>An <see cref="mea.IChatClient"/> that can be used to converse via the <see cref="GenerativeModel"/>.</returns>
	    public static mea.ChatClientBuilder AsBuilder(this mea.IChatClient client) => new mea.ChatClientBuilder(client);
        public static mea.IChatClient AsIChatClient(this GenerativeModel chatClient) =>
            new GeminiChatClient(chatClient);
        
        /// <summary>Gets an <see cref="mea.IChatClient"/> for use with this <see cref="GeminiClient"/>.</summary>
        /// <param name="client">The client.</param>
        /// <param name="model">The model to use.</param>
        /// <returns>An <see cref="mea.IChatClient"/> that can be used to converse via the <see cref="GenerativeModel"/>.</returns>
        public static mea.IChatClient AsIChatClient(this GeminiClient client, string? model = null)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));
            return client.GetGenerativeModel(model).AsIChatClient();
        }

        /// <summary>Gets an <see cref="mea.IEmbeddingGenerator{String, Embedding}"/> for use with this <see cref="GenerativeModel"/>.</summary>
        /// <param name="embeddingClient">The client.</param>
        /// <param name="defaultModelDimensions">The number of dimensions to generate in each embedding.</param>
        /// <returns>An <see cref="mea.IEmbeddingGenerator{String, Embedding}"/> that can be used to generate embeddings via the <see cref="GenerativeModel"/>.</returns>
        public static mea.IEmbeddingGenerator<string, mea.Embedding<float>> AsIEmbeddingGenerator(
            this GenerativeModel embeddingClient, int? defaultModelDimensions = null) =>
            new GeminiEmbeddingGenerator(embeddingClient, defaultModelDimensions);
        
        /// <summary>Gets an <see cref="mea.IEmbeddingGenerator{String, Embedding}"/> for use with this <see cref="GeminiClient"/>.</summary>
        /// <param name="client">The client.</param>
        /// <param name="model">The model to use.</param>
        /// <param name="defaultModelDimensions">The number of dimensions to generate in each embedding.</param>
        /// <returns>An <see cref="mea.IEmbeddingGenerator{String, Embedding}"/> that can be used to generate embeddings via the <see cref="GenerativeModel"/>.</returns>
        public static mea.IEmbeddingGenerator<string, mea.Embedding<float>> AsIEmbeddingGenerator(
            this GeminiClient client,
            string? model = null,
            int? defaultModelDimensions = null)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));
            return client.GetGenerativeModel(model).AsIEmbeddingGenerator(defaultModelDimensions);
        }
        
        /// <summary>Gets an <see cref="mea.ISpeechToTextClient"/> for use with this <see cref="GenerativeModel"/>.</summary>
        /// <param name="audioClient">The client.</param>
        /// <returns>An <see cref="mea.ISpeechToTextClient"/> that can be used to transcribe audio via the <see cref="GenerativeModel"/>.</returns>
        //[Experimental("MEAI001")]
        public static mea.ISpeechToTextClient AsISpeechToTextClient(this GenerativeModel audioClient) =>
            new GeminiSpeechToTextClient(audioClient);
        
        /// <summary>Gets an <see cref="mea.ISpeechToTextClient"/> for use with this <see cref="GeminiClient"/>.</summary>
        /// <param name="client">The client.</param>
        /// <param name="model">The model to use.</param>
        /// <returns>An <see cref="mea.ISpeechToTextClient"/> that can be used to transcribe audio via the <see cref="GenerativeModel"/>.</returns>
        //[Experimental("MEAI001")]
        public static mea.ISpeechToTextClient AsISpeechToTextClient(this GeminiClient client, string? model = null)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));
            return client.GetGenerativeModel(model).AsISpeechToTextClient();
        }
        
        /// <summary>
        /// Converts a Gemini <see cref="ITool"/> to a <see cref="mea.AITool"/>.
        /// </summary>
        /// <typeparam name="T">Gemini tool</typeparam>
        /// <param name="tool">Instance of a Gemini tool</param>
        /// <returns>An instance of a <see cref="mea.AITool"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the specified tool is null.</exception>
        public static mea.AITool AsAITool<T>(this T tool) where T : ITool
        {
	        if (tool is null) throw new ArgumentNullException(nameof(tool));

	        return new GeminiChatClient.GeminiAITool<T>(tool);
        }
    }
}