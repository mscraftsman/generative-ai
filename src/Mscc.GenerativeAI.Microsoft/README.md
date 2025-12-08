# MSCC GenerativeAI for .NET integrating with Microsoft.Extensions.AI and Microsoft Semantic Kernel.
[![GitHub](https://img.shields.io/github/license/mscraftsman/generative-ai)](https://github.com/mscraftsman/generative-ai/blob/main/LICENSE)
![GitHub last commit](https://img.shields.io/github/last-commit/mscraftsman/generative-ai)

Integrate Gemini API into your .NET applications using Microsoft.Extensions.AI. 
This package implements the `IChatClient`, `IEmbeddingGenerator` and `IImageGenerator` interfaces offering a unified experience for .NET applications and libraries.

## Benefits of Microsoft.Extensions.AI
Microsoft.Extensions.AI offers a unified API abstraction for AI services, similar to our successful logging and dependency injection (DI) abstractions. Our goal is to provide standard implementations for caching, telemetry, tool calling, and other common tasks that work with any provider.

Read more [Introducing Microsoft.Extensions.AI Preview – Unified AI Building Blocks for .NET](https://devblogs.microsoft.com/dotnet/introducing-microsoft-extensions-ai-preview/)

## Getting started

To get started, you can create a console application and install the `Mscc.GenerativeAI.Microsoft` package.
Have your API key from Google AI Studio and you're ready to go.

## Chat

Microsoft.Extensions.AI provides abstractions and exchange types for working with AI services. The Google.GenAI library includes an implementation of the IChatClient interface to enable
easy integration with applications that use these abstractions.

```csharp
using Microsoft.Extensions.AI;
using Mscc.GenerativeAI.Microsoft;

// Chat with Gemini API.
var apiKey = Environment.GetEnvironmentVariable("GOOGLE_API_KEY");
var model = "gemini-2.5-flash";
var prompt = "What is AI?";

IChatClient chatClient = new GeminiChatClient(apiKey, model);

var response = await chatClient.CompleteAsync(prompt);
Console.WriteLine(response.Message);

response = await chatClient.CompleteAsync( 
    "Translate the following text into Pig Latin: I love .NET and AI"); 
Console.WriteLine(response.Message);
```

## Streaming content generation

```csharp
using Microsoft.Extensions.AI;
using Mscc.GenerativeAI.Microsoft;
using System.ComponentModel

// assuming credentials are set up in environment variables as instructed above.
IChatClient chatClient = new GeminiChatClient()
    .AsIChatClient("gemini-2.5-flash")
    .AsBuilder()
    .UseFunctionInvocation()
    .UseOpenTelemetry()
    .Build();

ChatOptions options = new()
{
    Tools = [AIFunctionFactory.Create(([Description("The name of the person whose age is to be retrieved")] string personName) => personName switch
    {
        "Alice" => 30,
        "Bob" => 25,
        _ => 35
    }, "get_person_age", "Gets the age of the specified person");
};

await foreach (var update in chatClient.GetStreamingResponseAsync("How much older is Alice than Bob?", options))
{
    Console.Write(update);
}
```

## Grounding using Google Search

```csharp
using Microsoft.Extensions.AI;
using Mscc.GenerativeAI.Microsoft;

// Chat with Gemini API.
var apiKey = Environment.GetEnvironmentVariable("GOOGLE_API_KEY");
var model = "gemini-2.5-flash";
var prompt = "What is AI?";

IChatClient chatClient = new GeminiChatClient(apiKey: apiKey, model);

var chatOptions = new ChatOptions { Tools = [new HostedWebSearchTool()] };

var response = await chatClient.GetResponseAsync(prompt, chatOptions);
Console.WriteLine(response.Text);
```

## Code execution

```csharp
using Microsoft.Extensions.AI;
using Mscc.GenerativeAI.Microsoft;

// Chat with Gemini API.
var apiKey = Environment.GetEnvironmentVariable("GOOGLE_API_KEY");
var model = "gemini-2.5-flash";
var prompt = "What is the sum of the first 42 fibonacci numbers? Generate and run code to do the calculation?";

IChatClient chatClient = new GeminiChatClient(apiKey: apiKey, model);

var chatOptions = new ChatOptions { Tools = [new HostedCodeInterpreterTool()] };

var response = await chatClient.GetResponseAsync(prompt);
Console.WriteLine(response.Text);
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
    new GeminiEmbeddingGenerator(apiKey: apiKey, model);

var embeddings = await generator.GenerateAsync([prompt]);
Console.WriteLine(string.Join(", ", embeddings[0].Vector.ToArray()));
```


## More examples

The folders [samples](../samples/) and [tests](../tests/) contain more examples.

## Troubleshooting

tba

## Feedback

You can create issues at the <https://github.com/mscraftsman/generative-ai> repository.
