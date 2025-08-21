using mea = Microsoft.Extensions.AI;

namespace Mscc.GenerativeAI.Microsoft
{
    internal static class GeminiClientExtensions
    {
        /// <summary>Gets an <see cref="mea.IChatClient"/> for use with this <see cref="GenerativeModel"/>.</summary>
        /// <param name="chatClient">The client.</param>
        /// <returns>An <see cref="mea.IChatClient"/> that can be used to converse via the <see cref="GenerativeModel"/>.</returns>
        public static mea.IChatClient AsIChatClient(this GenerativeModel chatClient) =>
            new GeminiChatClient(chatClient);
        
        /// <summary>Gets an <see cref="mea.IEmbeddingGenerator{String, Single}"/> for use with this <see cref="GenerativeModel"/>.</summary>
        /// <param name="embeddingClient">The client.</param>
        /// <param name="defaultModelDimensions">The number of dimensions to generate in each embedding.</param>
        /// <returns>An <see cref="mea.IEmbeddingGenerator{String, Embedding}"/> that can be used to generate embeddings via the <see cref="GenerativeModel"/>.</returns>
        public static mea.IEmbeddingGenerator<string, mea.Embedding<float>> AsIEmbeddingGenerator(
            this GenerativeModel embeddingClient, int? defaultModelDimensions = null) =>
            new GeminiEmbeddingGenerator(embeddingClient, defaultModelDimensions);
        
        /// <summary>Gets an <see cref="mea.ISpeechToTextClient"/> for use with this <see cref="GenerativeModel"/>.</summary>
        /// <param name="audioClient">The client.</param>
        /// <returns>An <see cref="mea.ISpeechToTextClient"/> that can be used to transcribe audio via the <see cref="GenerativeModel"/>.</returns>
        //[Experimental("MEAI001")]
        public static mea.ISpeechToTextClient AsISpeechToTextClient(this GenerativeModel audioClient) =>
            new GeminiSpeechToTextClient(audioClient);
    }
}