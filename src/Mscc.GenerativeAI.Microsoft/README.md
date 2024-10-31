# MSCC GenerativeAI for .NET integrating with Microsoft.Extensions.AI and Microsoft Semantic Kernel.
[![GitHub](https://img.shields.io/github/license/mscraftsman/generative-ai)](https://github.com/mscraftsman/generative-ai/blob/main/LICENSE)
![GitHub last commit](https://img.shields.io/github/last-commit/mscraftsman/generative-ai)

Integrate Gemini API into your .NET applications using Microsoft.Extensions.AI. 
This package implements the `IChatClient` and `IEmbeddingGenerator` interfaces offering a unified experience for .NET applications and libraries.

## Benefits of Microsoft.Extensions.AI
Microsoft.Extensions.AI offers a unified API abstraction for AI services, similar to our successful logging and dependency injection (DI) abstractions. Our goal is to provide standard implementations for caching, telemetry, tool calling, and other common tasks that work with any provider.

Read more [Introducing Microsoft.Extensions.AI Preview – Unified AI Building Blocks for .NET](https://devblogs.microsoft.com/dotnet/introducing-microsoft-extensions-ai-preview/)

## Getting started

To get started, you can create a console application and install the `Mscc.GenerativeAI.Microsoft` package.
Have your API key from Google AI Studio and you're ready to go.

## Chat

```csharp
using Microsoft.Extensions.AI;
using Mscc.GenerativeAI.Microsoft;

// Chat with Gemini API.
var apiKey = Environment.GetEnvironmentVariable("GOOGLE_API_KEY");
var model = "gemini-1.5-pro-latest";
var prompt = "What is AI?";

IChatClient chatClient = new GeminiChatClient(apiKey, model);

var response = await chatClient.CompleteAsync(prompt);
Console.WriteLine(response.Message);

response = await chatClient.CompleteAsync( 
    "Translate the following text into Pig Latin: I love .NET and AI"); 
Console.WriteLine(response.Message);
```

## Embeddings

```csharp
using Microsoft.Extensions.AI;
using Mscc.GenerativeAI.Microsoft;

// Create embeddings using the appropriate model.
var apiKey = Environment.GetEnvironmentVariable("GOOGLE_API_KEY");
var model = "text-embedding-004";
var prompt = "What is AI?";

IEmbeddingGenerator<string,Embedding<float>> generator = 
    new GeminiEmbeddingGenerator(apiKey, model);

var embeddings = await generator.GenerateAsync([prompt]);
Console.WriteLine(string.Join(", ", embeddings[0].Vector.ToArray()));
```


## More examples

The folders [samples](../samples/) and [tests](../tests/) contain more examples.

## Troubleshooting

tba

## Feedback

You can create issues at the <https://github.com/mscraftsman/generative-ai> repository.
