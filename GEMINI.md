# Project Overview

This project is a .NET SDK for the Google Gemini API. It allows developers to integrate generative AI features into their .NET and ASP.NET Core applications. The SDK supports both Google AI Studio and Vertex AI, providing a comprehensive set of functionalities including content generation, model management, embeddings, and more.

The repository is structured as a .NET solution with multiple projects, including the main SDK (`Mscc.GenerativeAI`), a web-specific version (`Mscc.GenerativeAI.Web`), a version for Google API Client Library users (`Mscc.GenerativeAI.Google`), and a version for Microsoft.Extensions.AI and Semantic Kernel (`Mscc.GenerativeAI.Microsoft`). It also includes sample applications and extensive test suites.

# Building and Running

The project uses the .NET SDK and is built and packaged using standard `dotnet` commands.

## Building the project

To build the entire solution, run the following command from the root directory:

```bash
dotnet build ./GenerativeAI.sln -c Release
```

## Running tests

The project has two main test projects. To run the tests, use the following commands:

```bash
# To run tests for the main SDK
dotnet test ./tests/Mscc.GenerativeAI/Test.Mscc.GenerativeAI.csproj -c Release --logger "console;verbosity=detailed"

# To run tests for the Google API Client Library version
dotnet test ./tests/Mscc.GenerativeAI.Google/Test.Mscc.GenerativeAI.Google.csproj -c Release --logger "console;verbosity=detailed"
```

**Note:** Running the tests requires setting up authentication credentials as described in the `README.md` file.

## Packaging the project

The project is packaged into NuGet packages using the following commands:

```bash
# Pack for .NET
dotnet pack -c Release ./src/Mscc.GenerativeAI/Mscc.GenerativeAI.csproj -o output/

# Pack for ASP.NET Core
dotnet pack -c Release ./src/Mscc.GenerativeAI.Web/Mscc.GenerativeAI.Web.csproj -o output/

# Pack for .NET using Google Cloud Client Library
dotnet pack -c Release ./src/Mscc.GenerativeAI.Google/Mscc.GenerativeAI.Google.csproj -o output/

# Pack for Microsoft.Extension.AI and Microsoft Semantic Kernel
dotnet pack -c Release ./src/Mscc.GenerativeAI.Microsoft/Mscc.GenerativeAI.Microsoft.csproj -o output/
```

# Development Conventions

*   **Coding Style:** The project follows standard C# and .NET coding conventions. It uses modern C# features like `Nullable` and `ImplicitUsings`.
*   **Testing:** The project has a strong emphasis on testing, with separate test projects for different parts of the SDK.
*   **CI/CD:** The project uses GitHub Actions for continuous integration and deployment. The workflow file (`.github/workflows/dotnetcore.yml`) defines the build, test, and packaging process.
*   **Versioning:** The project version is managed in a `VERSION` file at the root of the repository.
*   **Dependencies:** The project uses several external libraries, which are managed as `PackageReference` in the `.csproj` files.
