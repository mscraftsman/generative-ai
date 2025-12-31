
using Build5Nines.SharpVector;
using Build5Nines.SharpVector.Embeddings;
using Build5Nines.SharpVector.Id;
using Build5Nines.SharpVector.VectorCompare;
using Build5Nines.SharpVector.VectorStore;
using EmbeddingFix.Build5Nines;


/// <summary>
/// A simple in-memory database for storing and querying vectorized text items.
/// This database uses Gemini to generate embeddings, and performs Cosine similarity search.
/// </summary>
/// <typeparam name="TMetadata">Defines the data type for the Metadata stored with the Text.</typeparam>
public class GeminiMemoryVectorDatabase<TMetadata>
     : MemoryVectorDatabaseBase<
        int,
        TMetadata,
        MemoryDictionaryVectorStore<int, TMetadata>,
        IntIdGenerator,
        CosineSimilarityVectorComparer
        >, IGeminiMemoryVectorDatabase<int, TMetadata>
{
    public GeminiMemoryVectorDatabase()
        : this(
            new Build5NinesGeminiEmbeddingGenerator()
            )
    { }


    public GeminiMemoryVectorDatabase(IBatchEmbeddingsGenerator embeddingsGenerator)
        : base(
            embeddingsGenerator,
            new MemoryDictionaryVectorStore<int, TMetadata>()
            )
    { }
}
