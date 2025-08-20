# Grounding with Google Search

Grounding with Google Search connects the Gemini model to real-time web content and works with all available languages. This allows Gemini to provide more accurate answers and cite verifiable sources beyond its knowledge cutoff.

Grounding helps you build applications that can:

- Increase factual accuracy: Reduce model hallucinations by basing responses on real-world information.
- Access real-time information: Answer questions about recent events and topics.
- Provide citations: Build user trust by showing the sources for the model's claims.

To activate Google Search as a tool, set the boolean property `UseGoogleSearch` to true, like the following example.

```csharp
var apiKey = "your_api_key";
var prompt = "When is the next total solar eclipse in Mauritius?";
var genAi = new GoogleAI(apiKey);
var model = genAi.GenerativeModel(Model.Gemini20FlashExperimental);
model.UseGoogleSearch = true;

var response = await model.GenerateContent(prompt);
Console.WriteLine(string.Join(Environment.NewLine,
    response.Candidates![0].Content!.Parts
        .Select(x => x.Text)
        .ToArray()));
```

More details are described in the API documentation on [Search as a tool](https://ai.google.dev/gemini-api/docs/models/gemini-v2#search-tool).

## What's next

- Learn about other available tools, like [Function Calling](function-calling.md).
- Learn how to augment prompts with specific URLs using the [URL context tool](url-context.md).
