# Gemini API: Console Schema XmlDoc Sample

This sample demonstrates how to use the `ResponseSchema` feature with XML documentation to get structured data from the Gemini API.

## Prerequisites

-   [.NET 9.0 SDK](https TBD)
-   An API key for the Gemini API. You can get one from [Google AI Studio](https TBD).

## Running the sample

1.  Clone the repository.
2.  Set the `GOOGLE_API_KEY` environment variable to your API key.
3.  Navigate to the `samples/Console.Schema.XmlDoc` directory.
4.  Run the sample using the `dotnet run` command.

```bash
dotnet run
```

## Sample Output

```json
{
  "ReviewSummary": "The new Gemini SDK for .NET is easy to use and has great documentation.",
  "Sentiment": "Positive",
  "Language": "en"
}
```
