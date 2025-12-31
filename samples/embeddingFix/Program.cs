
using EmbeddingFix.Build5Nines;

//  Before running:
//  Ensure that  Mscc.GenerativeAI.Microsoft.GeminiEmbeddingGenerator has been setup in Build5NinesGeminiEmbeddingGenerator class

 var vectorDb = new BasicGeminiVectorDatabase();

// this works in version 3.0.0, but not 3.0.1 of Mscc.GenerativeAI.Microsoft
 var singleEmbedding = vectorDb.AddText("Some text to get embeddings");

 Console.WriteLine(singleEmbedding.ToString());

 var batchEmbedding = new (string text, string? metadata)[]
        {
            ("one", "m1"),
            ("two", "m2"),
            ("three", "m3")
        };
 
 // this throws IndexOutOfRangeException work in either version of Mscc.GenerativeAI.Microsoft
 await vectorDb.AddTextsAsync(batchEmbedding);