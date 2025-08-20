# Gemini SDK for .NET quickstart

This quickstart shows you how to install our packages and make your first Gemini API request.

## Before you begin

You need a Gemini API key. If you don't already have one, you can [get it for free in Google AI Studio.](https://aistudio.google.com/app/apikey)

## Install the Gemini SDK for .NET
```text
dotnet add package Mscc.GenerativeAI
```

## Make your first request

Here is an example that uses the [GenerateContent]() method to send a request to the Gemini API using the Gemini 2.5 Flash model.

If you set your API key as the environment variable `GEMINI_API_KEY`, it will be picked up automatically by the client when using the libraries. Otherwise you will need to pass your API key as an argument when initializing the client.

Note that all code samples in the docs assume that you have set the environment variable `GEMINI_API_KEY`.

```csharp
using Mscc.GenerativeAI;

# The client gets the API key from the environment variable `GEMINI_API_KEY`.
var googleAI = new GoogleAI();
var model = googleAI.GenerativeModel(model: Model.Gemini25Flash);

var response = await model.GenerateContent("Explain how AI works in a few words");
Console.WriteLine(response.Text);
```

## License

This project is licensed under the [Apache 2.0 License](https://github.com/mscraftsman/generative-ai/blob/main/LICENSE).

## What's next

Now that you made your first API request, you might want to explore the following guides that show Gemini in action:

- [Text generation]()
- [Structured Output]()
- [Embeddings]()