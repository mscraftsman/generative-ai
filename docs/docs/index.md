# Gemini Developer API

Get a Gemini API key and make your first API request in minutes.

```csharp
using Mscc.GenerativeAI;

var googleAI = new GoogleAI(apiKey: "your API key");
var model = googleAI.GenerativeModel(model: Model.Gemini25Flash);

var response = await model.GenerateContent("Explain how AI works in a few words");
Console.WriteLine(response.Text);
```

## Explore the API
