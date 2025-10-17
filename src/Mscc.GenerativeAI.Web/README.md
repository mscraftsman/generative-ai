# Gemini AI Client for ASP.NET Core
[![GitHub](https://img.shields.io/github/license/mscraftsman/generative-ai)](https://github.com/mscraftsman/generative-ai/blob/main/LICENSE)
![GitHub last commit](https://img.shields.io/github/last-commit/mscraftsman/generative-ai)

Integrate Gemini API into your web projects. It works both Google AI (Studio) and Google Cloud Vertex AI.

Client for ASP.NET Core: 
[![NuGet Version](https://img.shields.io/nuget/v/Mscc.GenerativeAI.Web)](https://www.nuget.org/packages/Mscc.GenerativeAI.Web/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Mscc.GenerativeAI.Web)](https://www.nuget.org/packages/Mscc.GenerativeAI.Web/)

## Getting started

Either create a new ASP.NET Core project or use an existing one.

```text
> dotnet new web -o Web.Minimal.Api
```

Navigate to the project's root folder and install the package [Mscc.GenerativeAI.Web](https://www.nuget.org/packages/Mscc.GenerativeAI.Web/) from NuGet. You can install the package from the command line using either the command line or the NuGet Package Manager Console. Or you add it directly to your .NET project.

Add the package using the `dotnet` command line tool in your .NET project folder.

```text
> dotnet add package Mscc.GenerativeAI.Web
```

Working with Visual Studio use the NuGet Package Manager to install the package Mscc.GenerativeAI.Web.

```text
PM> Install-Package Mscc.GenerativeAI.Web
```

Alternatively, add the following line to your `.csproj` file.

```text
  <ItemGroup>
    <PackageReference Include="Mscc.GenerativeAI.Web" Version="2.8.18" />
  </ItemGroup>
```

You can then add this code to your sources whenever you need to access any Gemini API provided by Google. This package works currently for Google AI (Google AI Studio) only. Use with Google Cloud Vertex AI is provided by the underlying Mscc.GenerativeAI package but not exposed yet.

## Using Google AI Gemini API

Working with Google AI in your application requires an API key. Get an API key from [Google AI Studio](https://aistudio.google.com/app/apikey).

Add the following configuration to the `appsettings.json` file.

```json
{
// section name and location to your liking.
  "Gemini": {
    "Credentials": {
      "ApiKey": "YOUR_API_KEY"  // replace value with key from AI Studio
    },
    "ProjectId": "",
    "Region": "us-central1",
    "Model": "gemini-1.5-pro"   // default value
  },
// any other settings...
  "Logging": {
  },
}

```

The section name `Gemini` is arbitrary as well as the location of the section. Although, it needs to be referenced correctly in the `Configuration` builder.

### Using the `AddGenerativeAI` service

Next, add the service `AddGenerativeAI()` to the ASP.NET Core web app and map the routes as needed. Following is the most minimal implementation.

```csharp
using Mscc.GenerativeAI.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGenerativeAI(builder.Configuration.GetSection("Gemini"));

var app = builder.Build();

app.MapGet("/", async (IGenerativeModelService service) =>
{
    var model = service.CreateInstance();
    var result = await model.GenerateContent("Write about the history of Mauritius.");
    return result.Text;
});

app.Run();
```

There are overloads of the extension method `AddGenerativeAI()` according to [Options pattern guidance for .NET library authors](https://learn.microsoft.com/en-us/dotnet/core/extensions/options-library-authors).
Following approaches are available:

- Parameterless
- IConfiguration parameter (as shown above)
- Configuration section path parameter
- Action<TOptions> parameter
- Options instance parameter

Hoping this provides enough flexibility for individual preferences.

### Using a typed `HttpClient`

Alternatively to the above described service extension methods `Mscc.GenerativeAI.Web` also provides a typed `HttpClient` that can be added to your ASP.NET web application.

```csharp
sing Mscc.GenerativeAI.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<GenerativeAIClient>()
{
    BaseAddress = "";
};

var app = builder.Build();

app.MapGet("/", async (IGenerativeModelService service) =>
{
    var model = service.CreateInstance();
    var result = await model.GenerateContent("Write about the history of Mauritius.");
    return result.Text;
});

app.Run();
```

### Use global usings

Keeping the code base minimal, one should consider creating a `GlobalUsings.cs` file and define the `using` statements there.

```csharp
global using Mscc.GenerativeAI.Web;
```

This approach renders repetitive use of `using Mscc.GenerativeAI.Web;` in each `.cs` file obsolete.

## Configure the service

Find each approach documented. Surrounding source code skipped for brevity.

### Parameterless[🔗](https://learn.microsoft.com/en-us/dotnet/core/extensions/options-library-authors#parameterless)

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGenerativeAI();

var app = builder.Build();
// ...
```

### IConfiguration parameter (as shown above)[🔗](https://learn.microsoft.com/en-us/dotnet/core/extensions/options-library-authors#iconfiguration-parameter)

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGenerativeAI(builder.Configuration.GetSection("Gemini"));

var app = builder.Build();
// ...
```

### Configuration section path parameter[🔗](https://learn.microsoft.com/en-us/dotnet/core/extensions/options-library-authors#configuration-section-path-parameter)

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGenerativeAI("Gemini");

var app = builder.Build();
// ...
```

### Action<TOptions> parameter[🔗](https://learn.microsoft.com/en-us/dotnet/core/extensions/options-library-authors#actiontoptions-parameter)

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGenerativeAI(options =>
{
    // User defined option values
    options.ProjectId = string.Empty;
    options.Model = GenerativeAI.Model.GeminiProVision;
    options.Credentials.ApiKey = "YOUR_API_KEY";
});

var app = builder.Build();
// ...
```

### Options instance parameter[🔗](https://learn.microsoft.com/en-us/dotnet/core/extensions/options-library-authors#options-instance-parameter)

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGenerativeAI(new GenerativeAIOptions
{
    // Specify option values
    ProjectId = Environment.GetEnvironmentVariable("GOOGLE_PROJECT_ID"),
    Region = Environment.GetEnvironmentVariable("GOOGLE_REGION"),
    Model = Environment.GetEnvironmentVariable("GOOGLE_MODEL"),
    Credentials = new() { ApiKey = Environment.GetEnvironmentVariable("GOOGLE_API_KEY") }
});

var app = builder.Build();
// ...
```

The choice of configuring the service is yours.

## More examples

The folders [samples](../samples/) and [tests](../tests/) contain more examples.

- [Simple console application](../samples/Console.Minimal.Prompt/)

## Troubleshooting

tba

## Feedback

You can create issues at the <https://github.com/mscraftsman/generative-ai> repository.
