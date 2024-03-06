# Gemini AI Client for .NET and ASP.NET Core
[![GitHub](https://img.shields.io/github/license/mscraftsman/generative-ai)](https://github.com/mscraftsman/generative-ai/blob/main/LICENSE)
![GitHub last commit](https://img.shields.io/github/last-commit/mscraftsman/generative-ai)

Access the Gemini API in Google AI Studio and Google Cloud Vertex AI.

Client for .NET: 
[![NuGet Version](https://img.shields.io/nuget/v/Mscc.GenerativeAI)](https://www.nuget.org/packages/Mscc.GenerativeAI/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Mscc.GenerativeAI)](https://www.nuget.org/packages/Mscc.GenerativeAI/)

Client for ASP.NET Core: 
[![NuGet Version](https://img.shields.io/nuget/v/Mscc.GenerativeAI.Web)](https://www.nuget.org/packages/Mscc.GenerativeAI.Web/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Mscc.GenerativeAI.Web)](https://www.nuget.org/packages/Mscc.GenerativeAI.Web/)

Read more about [Mscc.GenerativeAI.Web](./src/Mscc.GenerativeAI.Web) and how to add it to your ASP.NET web applications.

## Install the package

Install the package [Mscc.GenerativeAI](https://www.nuget.org/packages/Mscc.GenerativeAI/) from NuGet. You can install the package from the command line using either the command line or the NuGet Package Manager Console. Or you add it directly to your .NET project.

Add the package using the `dotnet` command line tool in your .NET project folder.

```text
> dotnet add package Mscc.GenerativeAI
```

Working with Visual Studio use the NuGet Package Manager to install the package Mscc.GenerativeAI.

```text
PM> Install-Package Mscc.GenerativeAI
```

Alternatively, add the following line to your `.csproj` file.

```text
  <ItemGroup>
    <PackageReference Include="Mscc.GenerativeAI" Version="0.5.2" />
  </ItemGroup>
```

You can then add this code to your sources whenever you need to access any Gemini API provided by Google. This package works for Google AI (Google AI Studio) and Google Cloud Vertex AI.

## Authentication use cases

The package supports the following use cases to authenticate.

- Google AI: [Authentication with an API key](https://ai.google.dev/tutorials/setup)
- Google AI: [Authentication with OAuth](https://ai.google.dev/docs/oauth_quickstart)
- Vertex AI: [Authentication with Application Default Credentials (ADC)](https://cloud.google.com/docs/authentication/provide-credentials-adc#local-dev)

This applies mainly to the instantiation procedure.

## Getting Started

Use of Gemini API in either Google AI or Vertex AI is almost identical. The major difference is the way to instantiate the model handling your prompt.

### Choose an API and authentication mode

Google AI with an API key

```csharp
using Mscc.GenerativeAI;
// Google AI with an API key
var model = new GenerativeModel(apiKey: "your API key", model: Model.GeminiPro);
```

Google AI with OAuth. Use `gcloud auth application-default print-access-token` to get the access token.

```csharp
using Mscc.GenerativeAI;
// Google AI with OAuth. Use `gcloud auth application-default print-access-token` to get the access token.
var model = new GenerativeModel(model: Model.GeminiPro);
model.AccessToken = accessToken;
```

Vertex AI with OAuth. Use `gcloud auth application-default print-access-token` to get the access token.

```csharp
using Mscc.GenerativeAI;
// Vertex AI with OAuth. Use `gcloud auth application-default print-access-token` to get the access token.
var vertex = new VertexAI(projectId: projectId, region: region);
var model = vertex.GenerativeModel(model: Model.GeminiPro);
model.AccessToken = accessToken;
```

The `ConfigurationFixture` type in the test project implements multiple options to retrieve sensitive information, ie. API key or access token.

### Using Google AI Gemini API

Working with Google AI in your application requires an API key. Get an API key from [Google AI Studio](https://aistudio.google.com/app/apikey).

```csharp
using Mscc.GenerativeAI;

var apiKey = "your_api_key";

var prompt = "Write a story about a magic backpack.";
var model = new GenerativeModel(apiKey: apiKey, model: Model.GeminiPro);

var response = model.GenerateContent(prompt).Result;
Console.WriteLine(response.Text);
```

### Using Vertex AI Gemini API

Use of [Vertex AI](https://console.cloud.google.com/vertex-ai) requires an account on [Google Cloud](https://console.cloud.google.com/), a project with billing and Vertex AI API enabled.

```csharp
using Mscc.GenerativeAI;

var projectId = "your_google_project_id"; // the ID of a project, not its name.
var region = "us-central1";     // see documentation for available regions.
var accessToken = "your_access_token";      // use `gcloud auth application-default print-access-token` to get it.

var prompt = "Write a story about a magic backpack.";
var vertex = new VertexAI(projectId: projectId, region: region);
var model = vertex.GenerativeModel(model: Model.GeminiPro);
model.AccessToken = accessToken;

var response = model.GenerateContent(prompt).Result;
Console.WriteLine(response.Text);
```

## More examples

The folders [samples](./samples/) and [tests](./tests/) contain more examples.

- [Simple console application](./samples/Console.Minimal.Prompt/)
- [ASP.NET Core Minimal web application](./samples/Web.Minimal.Api)
- [ASP.NET Core MVP web application](./samples/Web.Mvp) (work in progress!)

## Troubleshooting

Sometimes you might have authentication warnings (partiocularly with the text-to-speech API). You can fix it by re-authenticating through ADC:

```bash
gcloud config set project "$PROJECT_ID"

gcloud auth application-default set-quota-project "$PROJECT_ID"
gcloud auth application-default login
```

Make sure that the required API have been enabled.

```bash
# ENABLE APIs
gcloud services enable aiplatform.googleapis.com
```

## Using the tests

The repository contains a number of test cases for Google AI and Vertex AI. You will find them in the [tests](./tests/) folder. They are part of the [GenerativeAI solution].
To run the tests, either enter the relevant information into the [appsettings.json](./tests/appsettings.json), create a new `appsettings.user.json` file with the same JSON structure in the `tests` folder, or define the following environment variables

- GOOGLE_API_KEY
- GOOGLE_PROJECT_ID
- GOOGLE_REGION
- GOOGLE_ACCESS_TOKEN (optional: if absent, `gcloud auth application-default print-access-token` is executed)

The test cases should provide more insights and use cases on how to use the [Mscc.GenerativeAI](https://github.com/mscraftsman/generative-ai) package in your .NET projects.

## Feedback

You can create issues at the <https://github.com/mscraftsman/generative-ai> repository.
