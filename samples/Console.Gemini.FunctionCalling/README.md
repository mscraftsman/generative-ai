# Gemini Function Calling Sample

This sample demonstrates how to use function calling with the Gemini API via the `Mscc.GenerativeAI.Microsoft` package.

## Prerequisites

- .NET 8.0 SDK
- A Google AI API key

## Setup

1.  Set the `GOOGLE_API_KEY` environment variable:
    ```bash
    export GOOGLE_API_KEY="YOUR_API_KEY"
    ```
2.  Navigate to the sample directory:
    ```bash
    cd samples/Console.Gemini.FunctionCalling
    ```
3.  Run the application:
    ```bash
    dotnet run
    ```

## Usage

The application will prompt you for input. You can ask questions that trigger the `get_user_information` tool, for example:

```
User > What is my name?
```

The model should respond with a tool call request.
