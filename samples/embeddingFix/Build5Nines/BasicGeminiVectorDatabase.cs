
namespace EmbeddingFix.Build5Nines;

public class BasicGeminiVectorDatabase : GeminiMemoryVectorDatabase<string>
{
    public BasicGeminiVectorDatabase() : base(new Build5NinesGeminiEmbeddingGenerator())
    {
    }
}
