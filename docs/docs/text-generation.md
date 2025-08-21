# Text generation

The Gemini SDK for .NET can generate text output from various inputs, including text, images, video, and audio, leveraging Gemini models.

Here's a basic example that takes a single text input:

```csharp
using Mscc.GenerativeAI;

var googleAI = new GoogleAI(apiKey: "your API key");
var model = googleAI.GenerativeModel(model: Model.Gemini25Flash);

var response = await model.GenerateContent("Explain how AI works in a few words");
Console.WriteLine(response.Text);
```

## Thinking with Gemini 2.5

2.5 Flash and Pro models have "thinking" enabled by default to enhance quality, which may take longer to run and increase token usage.

When using 2.5 Flash, you can disable thinking by setting the thinking budget to zero.

For more details, see the thinking guide.

```csharp
using Mscc.GenerativeAI;

var googleAI = new GoogleAI(apiKey: "your API key");
var model = googleAI.GenerativeModel(model: Model.Gemini25Flash);
var generationConfig = new GenerationConfig()
{
    ThinkingConfig = new ThinkingConfig()
    {
        ThinkingBudget = 0  // Disables thinking
    }
};

var response = await model.GenerateContent("Explain how AI works in a few words", 
    generationConfig: generationConfig);
Console.WriteLine(response.Text);
```

## System instructions and other configurations

You can guide the behavior of Gemini models with system instructions. To do so, pass the system instructions as parameter.

```csharp
using Mscc.GenerativeAI;

var systemInstruction = new Content("You are a friendly pirate. Speak like one.");
var prompt = "Good morning! How are you?";
var googleAI = new GoogleAI(apiKey: "your API key");
var model = googleAI.GenerativeModel(model: Model.Gemini25Flash, 
    systemInstruction: systemInstruction);
var request = new GenerateContentRequest(prompt);

var response = await model.GenerateContent(request);
Console.WriteLine(response.Text);
```

The [GenerationConfig](api/Mscc.GenerativeAI.GenerationConfig) object also lets you override default generation parameters, such as [Temperature](api/Mscc.GenerativeAI.GenerationConfig#Temperature).

```csharp
using Mscc.GenerativeAI;

var googleAI = new GoogleAI(apiKey: _fixture.ApiKey);
var model = googleAI.GenerativeModel(model: Model.Gemini25Flash);
var request = new GenerateContentRequest(prompt)
{
    GenerationConfig = new GenerationConfig()
    {
        Temperature = 0.4f,
    }
};

var response = await model.GenerateContent(request);
Console.WriteLine(response.Text);
```

Refer to the [GenerationConfig](api/Mscc.GenerativeAI.GenerationConfig) in our API reference for a complete list of configurable parameters and their descriptions.

## Multimodal inputs

The Gemini SDK for .NET supports multimodal inputs, allowing you to combine text with media files. The following example demonstrates providing an image:

```csharp
using Mscc.GenerativeAI;

var prompt = "Parse the time and city from the airport board shown in this image into a list, in Markdown";
var googleAI = new GoogleAI();
var model = googleAI.GenerativeModel(model: Model.Gemini25Flash);
var request = new GenerateContentRequest(prompt);
await request.AddMedia("https://raw.githubusercontent.com/mscraftsman/generative-ai/refs/heads/main/tests/Mscc.GenerativeAI/payload/timetable.png");

var response = await model.GenerateContent(request);
Console.WriteLine(response.Text);
```

For alternative methods of providing images and more advanced image processing, see our image understanding guide. The API also supports document, video, and audio inputs and understanding.

## Streaming responses

By default, the model returns a response only after the entire generation process is complete.

For more fluid interactions, use streaming to receive GenerateContentResponse instances incrementally as they're generated.

```csharp
using Mscc.GenerativeAI;

var prompt = "How are you doing today?";
var googleAi = new GoogleAI();
var model = googleAi.GenerativeModel(model: Model.Gemini25Flash);

var responseStream = model.GenerateContentStream(prompt);

await foreach (var response in responseStream)
{
    Console.WriteLine($"{response.Text}");
}
```

## Multi-turn conversations (Chat)

Our SDK provides functionality to collect multiple rounds of prompts and responses into a chat, giving you an easy way to keep track of the conversation history.

```csharp
using Mscc.GenerativeAI;

var googleAi = new GoogleAI();
var model = _googleAi.GenerativeModel(model: Model.Gemini25Flash);
var chat = model.StartChat();

var prompt = "Hello, let's talk a bit about nature.";
var response = await chat.SendMessage(prompt);
Console.WriteLine(prompt);
Console.WriteLine(response.Text);

prompt = "What are all the colors in a rainbow?";
response = await chat.SendMessage(prompt);
Console.WriteLine(prompt);
Console.WriteLine(response.Text);

prompt = "Why does it appear when it rains?";
response = await chat.SendMessage(prompt);
Console.WriteLine(prompt);
Console.WriteLine(response.Text);

// Access the chat history to store and replay the conversation.
Console.WriteLine($"{new string('-', 20)}");
Console.WriteLine("------ History -----");
chat.History.ForEach(c =>
{
    Console.WriteLine($"{new string('-', 20)}");
    Console.WriteLine($"{c.Role}: {c.Text}");
});
```

Streaming can also be used for multi-turn conversations.

```csharp
using Mscc.GenerativeAI;

var googleAi = new GoogleAI();
var model = googleAi.GenerativeModel(model: Model.Gemini25Flash);
var chat = model.StartChat();

var prompt = "How can I learn more about C#?";
var responseStream = chat.SendMessageStream(prompt);
await foreach (var response in responseStream)
{
    Console.WriteLine($"{response.Text}");
}

prompt = "Summarize the answer for a primary school student.";
responseStream = chat.SendMessageStream(prompt);
await foreach (var response in responseStream)
{
    Console.WriteLine($"{response.Text}");
}

// Access the chat history to store and replay the conversation.
Console.WriteLine($"{new string('-', 20)}");
Console.WriteLine("------ History -----");
chat.History.ForEach(c =>
{
    Console.WriteLine($"{new string('-', 20)}");
    Console.WriteLine($"{c.Role}: {c.Text}");
});
```

## Best practices

### Prompting tips

For basic text generation, a zero-shot prompt often suffices without needing examples, system instructions or specific formatting.

For more tailored outputs:

- Use [System instructions](#system-instructions-and-other-configurations) to guide the model. 
- Provide few example inputs and outputs to guide the model. This is often referred to as few-shot prompting.

Consult the prompt engineering guide for more tips.

### Structured output

In some cases, you may need structured output, such as JSON. Refer to our structured output guide to learn how.

## What's next

