# Project Overview

This project is a .NET SDK for the Google Gemini API. It allows developers to integrate generative AI features into their .NET and ASP.NET Core applications. The SDK supports both Google AI Studio and Vertex AI, providing a comprehensive set of functionalities including content generation, model management, embeddings, and more.

The repository is structured as a .NET solution with multiple projects, including:
*   Main SDK (`Mscc.GenerativeAI`)
*   Web-specific version (`Mscc.GenerativeAI.Web`)
*   Version for Google API Client Library users (`Mscc.GenerativeAI.Google`)
*   Version for Microsoft.Extensions.AI and Semantic Kernel (`Mscc.GenerativeAI.Microsoft`)
*   Code generator utility (`Mscc.CodeGenerator`)

It also includes sample applications and extensive test suites.

# Building and Running

The project uses the .NET SDK and is built and packaged using standard `dotnet` commands. A `Makefile` is also provided for common tasks.

## Building the project

To build the entire solution, run the following command from the root directory:

```bash
dotnet build ./GenerativeAI.sln -c Release
```

Alternatively, use the Makefile:

```bash
make build
```

## Running tests

The project has three main test projects. To run all tests, use the Makefile:

```bash
make test
```

Or run individual test projects:

```bash
# To run tests for the main SDK
dotnet test ./tests/Mscc.GenerativeAI/Test.Mscc.GenerativeAI.csproj -c Release --logger "console;verbosity=detailed"

# To run tests for the Google API Client Library version
dotnet test ./tests/Mscc.GenerativeAI.Google/Test.Mscc.GenerativeAI.Google.csproj -c Release --logger "console;verbosity=detailed"

# To run tests for the Microsoft version
dotnet test ./tests/Mscc.GenerativeAI.Microsoft/Test.Mscc.GenerativeAI.Microsoft.csproj -c Release --logger "console;verbosity=detailed"
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

*   **Coding Style:** The project follows standard C# and .NET coding conventions. It uses modern C# features like `Nullable` and `ImplicitUsings`, targeting `netstandard2.0`, `net8.0`, `net9.0`, and `net10.0`.
*   **Testing:** The project has a strong emphasis on testing, with separate test projects for different parts of the SDK.
*   **CI/CD:** The project uses GitHub Actions and Codeberg (Forgejo) for continuous integration and deployment. Workflow files are located in `.github/workflows/` and `.forgejo/workflows/`.
*   **Versioning:** The project version is managed in a `VERSION` file at the root of the repository.
*   **Dependencies:** The project uses Central Package Management (CPM) via `Directory.Packages.props` at the root to manage dependency versions.

---

## Git & Branch Protection Rules

1. **Protected Default Branch (`main` / `master`)**:
   - Direct pushes and force-pushes to the default branch are blocked by repository rulesets.
   - **Never** push directly to `main` or `master`. Always work on a feature/fix branch and open a Pull Request (`gh pr create`).
2. **Mandatory Commit Sign-Off (`-s` / DCO)**:
   - Every commit **must** be signed off using `git commit -s` so that it includes a `Signed-off-by: Name <email>` trailer. Commits without this trailer are rejected by the repository ruleset.

