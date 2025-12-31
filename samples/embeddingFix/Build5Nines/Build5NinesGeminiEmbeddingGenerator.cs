

using Build5Nines.SharpVector.Embeddings;

namespace EmbeddingFix.Build5Nines;

public class Build5NinesGeminiEmbeddingGenerator : IBatchEmbeddingsGenerator
{

    private const string ApiKey = "AIzaSyB3jq7xZWyxdsFs0GWpUlwdUPJtIfiT1kU";
    private const string Model = "gemini-embedding-001";

    public async Task<float[]> GenerateEmbeddingsAsync(string text)
    {
        if (string.IsNullOrEmpty(ApiKey))
        {
            throw new InvalidOperationException("Please set the API Key for GeminiEmbeddingGenerator");
        }

        using var embeddingsGenerator = new Mscc.GenerativeAI.Microsoft.GeminiEmbeddingGenerator(ApiKey, Model);

        var embeddings = await embeddingsGenerator.GenerateAsync(new List<string> { text }, null, CancellationToken.None);

        return embeddings.First().Vector.ToArray();
    }


    public async Task<IReadOnlyList<float[]>> GenerateEmbeddingsAsync(IEnumerable<string> texts)
    {
        if (string.IsNullOrEmpty(ApiKey))
        {
            throw new InvalidOperationException("Please set the API Key for GeminiEmbeddingGenerator");
        }

        using var embeddingsGenerator = new Mscc.GenerativeAI.Microsoft.GeminiEmbeddingGenerator(ApiKey, Model);

        var embeddings = await embeddingsGenerator.GenerateAsync(texts, null);

        return embeddings.Select(e => e.Vector.ToArray()).ToList();
    }
}