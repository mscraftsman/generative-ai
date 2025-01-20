using Microsoft.Extensions.AI;
using Mscc.GenerativeAI;
using Mscc.GenerativeAI.Microsoft;

GenerativeAIExtensions.ReadDotEnv();
var apiKey = Environment.GetEnvironmentVariable("GOOGLE_API_KEY")!;
var model = "gemini-1.5-pro-latest";
var prompt = "What is AI?";

IChatClient chatClient = new GeminiChatClient(apiKey, model);

var response = await chatClient.CompleteAsync(prompt);
Console.WriteLine(response.Message);

response = await chatClient.CompleteAsync( 
    "Explain the following expression 'Fortis Fortuna Adiuvat' and give a prominent reference."); 
Console.WriteLine(response.Message);

// Create embeddings using the appropriate model.
model = "text-embedding-004";
IEmbeddingGenerator<string,Embedding<float>> generator = 
    new GeminiEmbeddingGenerator(apiKey, model);
var embeddings = await generator.GenerateAsync([prompt]);
Console.WriteLine(string.Join(", ", embeddings[0].Vector.ToArray()));
