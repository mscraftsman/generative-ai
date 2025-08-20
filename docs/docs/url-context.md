# URL context

The URL context tool lets you provide additional context to the models in the form of URLs. By including URLs in your request, the model will access the content from those pages (as long as it's not a URL type listed in the limitations section) to inform and enhance its response.

The URL context tool is useful for tasks like the following:

- **Extract Data**: Pull specific info like prices, names, or key findings from multiple URLs.
- **Compare Documents**: Analyze multiple reports, articles, or PDFs to identify differences and track trends.
- **Synthesize & Create Content**: Combine information from several source URLs to generate accurate summaries, blog posts, or reports.
- **Analyze Code & Docs**: Point to a GitHub repository or technical documentation to explain code, generate setup instructions, or answer questions.

The following example shows how to compare two recipes from different websites.

```csharp
using Mscc.GenerativeAI;

var url = "https://conference.mscc.mu/";
var prompt = $"Summarize this document: {url}";
var googleAi = new GoogleAI();
var model = googleAi.GenerativeModel(model: _model,
    tools: [new Tool { UrlContext = new() }]);

var response = await model.GenerateContent(prompt);

Console.WriteLine(response.Text);
response.Candidates![0].UrlContextMetadata!.UrlMetadata
    .ForEach(m =>
        Console.WriteLine($"{m.RetrievedUrl} - {m.UrlRetrievalStatus}"));
```

## How it works

The URL Context tool uses a two-step retrieval process to balance speed, cost, and access to fresh data. When you provide a URL, the tool first attempts to fetch the content from an internal index cache. This acts as a highly optimized cache. If a URL is not available in the index (for example, if it's a very new page), the tool automatically falls back to do a live fetch. This directly accesses the URL to retrieve its content in real-time.

## Combining with other tools

You can combine the URL context tool with other tools to create more powerful workflows.

### Grounding with search

When both URL context and Grounding with Google Search are enabled, the model can use its search capabilities to find relevant information online and then use the URL context tool to get a more in-depth understanding of the pages it finds. This approach is powerful for prompts that require both broad searching and deep analysis of specific pages.

```csharp

```

## Understanding the response

When the model uses the URL context tool, the response includes a `UrlContextMetadata` object. This object lists the URLs the model retrieved content from and the status of each retrieval attempt, which is useful for verification and debugging.
