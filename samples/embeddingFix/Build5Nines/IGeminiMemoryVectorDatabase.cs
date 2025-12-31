using Build5Nines.SharpVector;

namespace EmbeddingFix.Build5Nines;

/// <summary>
/// An interface for a vector database that uses Gemini for embedding generation.
/// </summary>
/// <typeparam name="TId"></typeparam>
/// <typeparam name="TMetadata"></typeparam>
public interface IGeminiMemoryVectorDatabase<TId, TMetadata> : IVectorDatabase<TId, TMetadata>
    where TId : notnull
{ }
