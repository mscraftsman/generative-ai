# Gemini SDK for .NET quickstart

This quickstart shows you how to install our packages and make your first Gemini API request.

## Before you begin

You need a Gemini API key. If you don't already have one, you can [get it for free in Google AI Studio.](https://aistudio.google.com/app/apikey)

## Install the Gemini SDK for .NET

Install the package [Mscc.GenerativeAI](https://www.nuget.org/packages/Mscc.GenerativeAI/) from NuGet. You can install the package from the command line using either the command line or the NuGet Package Manager Console. Or you add it directly to your .NET project.

Add the package using the `dotnet` command line tool in your .NET project folder.

```text
 dotnet add package Mscc.GenerativeAI
```

Working with Visual Studio use the NuGet Package Manager to install the package Mscc.GenerativeAI.

```text
 Install-Package Mscc.GenerativeAI
```

Alternatively, add the following line to your `.csproj` file.

```text
  <ItemGroup>
    <PackageReference Include="Mscc.GenerativeAI" Version="2.8.10" />
  </ItemGroup>
```

You can then add this code to your sources whenever you need to access any Gemini API provided by Google. This package works for Google AI (Google AI Studio) and Google Cloud Vertex AI.

## Make your first request

Here is an example that uses the [GenerateContent]() method to send a request to the Gemini API using the Gemini 2.5 Flash model.

If you set your API key as the environment variable `GEMINI_API_KEY`, it will be picked up automatically by the client when using the libraries. Otherwise you will need to pass your API key as an argument when initializing the client.

Note that all code samples in the docs assume that you have set the environment variable `GEMINI_API_KEY`.

```csharp
using Mscc.GenerativeAI;

# The client gets the API key from the environment variable `GEMINI_API_KEY`.
var googleAI = new GoogleAI();  // or pass in apiKey parameter
var model = googleAI.GenerativeModel(model: Model.Gemini25Flash);

var response = await model.GenerateContent("Explain how AI works in a few words");
Console.WriteLine(response.Text);
```

## Features (as per Gemini analysis)

The provided code defines a C# library for interacting with Google's Generative AI models, specifically the Gemini models. It provides functionalities to:

- **List available models**: This allows users to see which models are available for use.
- **Get information about a specific model**: This provides details about a specific model, such as its capabilities and limitations.
- **Generate content**: This allows users to send prompts to a model and receive generated text in response.
- **Generate content stream**: This allows users to receive a stream of generated text from a model, which can be useful for real-time applications.
- **Generate a grounded answer**: This allows users to ask questions and receive answers that are grounded in provided context.
- **Generate embeddings**: This allows users to convert text into numerical representations that can be used for tasks like similarity search.
- **Count tokens**: This allows users to estimate the cost of using a model by counting the number of tokens in a prompt or response.
- **Start a chat session**: This allows users to have a back-and-forth conversation with a model.
- **Create tuned models**: This allows users to provide samples for tuning an existing model. Currently, only the `text-bison-001` and `gemini-1.0-pro-001` models are supported for tuning
- **File API**: This allows users to upload large files and use them with Gemini 1.5.

The package also defines various helper classes and enums to represent different aspects of the Gemini API, such as model names, request parameters, and response data.

## License

This project is licensed under the [Apache 2.0 License](https://github.com/mscraftsman/generative-ai/blob/main/LICENSE).

## What's next

Now that you made your first API request, you might want to explore the following guides that show Gemini in action:

- [Text generation](docs/text-generation.md)
- [Structured Output](docs/structured-output.md)
- [Embeddings](docs/embeddings.md)