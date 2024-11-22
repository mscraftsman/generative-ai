# Changelog (Release Notes)

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html) (SemVer).

## [Unreleased]

### Added
- Feature suggestion: Retry mechanism ([#2](https://github.com/mscraftsman/generative-ai/issues/2))
- implement Automatic Function Call (AFC)
### Changed
### Fixed

## 1.9.5

### Added

- add model `gemini-exp-1121` - [#47](https://github.com/mscraftsman/generative-ai/issues/47) thanks to @doggy8088
- set package identifier as User-Agent and Google API Client

### Changed

- improve inheritance modifiers
- amend NuGet packages for .NET 6

## 1.9.4

### Added

- add LearnLM model `learnlm-1.5-pro-experimental`
- add Imagen3 on Google AI
- extend interface for Imagen Generation Model

### Changed

- guard initialisation of models/services (Google AI)
 
## 1.9.3

### Changed

- overwrite HTTP handling of API key
- mark properties as optional
- deserialize and return chat completions response

## 1.9.2

### Added 

- add model `gemini-exp-1114` - [#45](https://github.com/mscraftsman/generative-ai/issues/45) thanks to @shankarvashist
- add models `gemini-1.5-flash-8b` and `gemini-1.5-flash-8b-latest`
- add services for `Chat`, `Embeddings`, and `OpenAI`
- add `EnableEnhancedCivicAnswers` property

## 1.9.1

### Changed

- update NuGet package(s)

## 1.9.0

### Added

- add .NET 9.0 targeting
- add feature: Interact with Vertex Tuned Models ([#36](https://github.com/mscraftsman/generative-ai/issues/36))
- add model/service for generated files
- add method(s) to call Predict endpoints

### Changed

- refactor handling of base URLs and API endpoints
- check request(s) for unsupported combination of options
- update method to batch embeddings

## 1.8.3

### Added 

- add Grounding with Google Search
- add `ModelVersion` property

## 1.8.2

### Added

- new NuGet package `Mscc.GenerativeAI.Microsoft` leveraging Microsoft.Extensions.AI abstractions to build a unified AI client

### Changed

- set role for embedding request

### Fixed

- fix endpoint method of `text-embedding-004`

## 1.8.1

### Added

- add logs with LogLevel using the Standard logging in .NET ([#6](https://github.com/mscraftsman/generative-ai/issues/6)) - thanks @doggy8088 

### Changed

- improve regarding XMLdoc, typos, and non-nullable properties

### Fixed

- fix Application Default Credentials (ADC) has been loaded automatically even I use API Key auth. [#9](https://github.com/mscraftsman/generative-ai/issues/9)
- fix Exception thrown in Google App Engine [#26](https://github.com/mscraftsman/generative-ai/issues/26)

## 1.8.0

### Added

- add context caching: https://ai.google.dev/gemini-api/docs/caching
- add code execution: https://ai.google.dev/gemini-api/docs/code-execution
- add model `gemini-1.5-flash-8b-001`
- add Logprobs handling
- add required model name and optional cached content to request

### Changed

- sanitize name of cached content
- extend list of supported MIME types
- extend `FinishReason`
- extend `VideoMetadata`

### Fixed

- disable HTTP/3 (Quic) due to issue [#34](https://github.com/mscraftsman/generative-ai/issues/34)

## 1.7.0

### Added

- add methods using File API to class `GoogleAI`
- mark methods using File API as obsolete/deprecated
- add types and functionality for `CachedContents`
- add types and functionality for `GeneratedFile`
- add extension methods for `GeneratedFiles` and `CachedContents`
- add more XMLdoc

### Changed

- change access modifier of some properties

## 1.6.5

### Added

- add properties `State`, `Error`, and `VideoMetadata` to type `FileResource`. [#33](https://github.com/mscraftsman/generative-ai/issues/33)
- overload method of `UploadMedia` to support stream types ([#38](https://github.com/mscraftsman/generative-ai/issues/38))

### Changed

- use of using expression to dispose `FileStream` after upload [#35](https://github.com/mscraftsman/generative-ai/pull/37) - thanks @rsmithsa
- enhance returned error information [#33](https://github.com/mscraftsman/generative-ai/issues/33)
- update enums according to $discovery
- sync target frameworks among projects

## 1.6.4

### Changed

- upgrade NuGet packages
- housekeeping

## 1.6.3

### Added

- add model `gemini-1.5-pro-002`
- add model `gemini-1.5-flash-002`
- add experimental model `gemini-1.5-flash-8b-exp-0924`

## 1.6.2

### Added

- add RequestOptions to override default values
- add ResponseSchema for JSON response mode

### Changed

- change default model to Gemini 1.5 
- [.NET] use HTTP/1.1 or higher protocol

## 1.6.1

### Added

- add Imagen 3 model `imagen-3.0-generate-001`
- add Imagen 3 model `imagen-3.0-fast-generate-001`

## 1.6.0

### Added

- add tuning model `gemini-1.5-pro-001`
- add tuning model `gemini-1.5-flash-001`
- add tuning model `gemini-1.5-flash-001-tuning`
- add experimental model `gemini-1.5-pro-exp-0801`
- add experimental model `gemini-1.5-pro-exp-0827`
- add experimental model `gemini-1.5-flash-exp-0827`
- add experimental model `gemini-1.5-flash-8b-exp-0827`

### Changed

- removed targeting for .NET 7 (end of support)
- re-linked constant `Model.Gemini15Pro`
- re-linked constant `Model.Gemini15Flash`

## 1.5.2

### Added

- add model `gemini-1.5-flash-001`
- add model `gemini-1.5-flash-001-tuning`

## 1.5.1

### Changed

- Update System.Text.Json to 8.0.4

## 1.5.0

### Added

- add model `gemini-1.5-flash-latest`

## 1.4.0

### Added

- implement Imagen 2 model (Vertex AI)
- implement Image Captioning (Vertex AI)
- implement Visual question and answering (VQA)
- add tests for `ImageGenerationModel` and `ImageTextModel`

### Changed

- refactor constant mimetype
- improve XML doc
- move types to subfolder

## 1.3.0

### Added

- implement Server-Sent Events (SSE)
- add enum `FunctionCallingMode`
- implement type `ToolConfig`
- add model `gemini-1.0-pro-vision-001`
- implement exception for max file upload size
- expose `Timeout` property

### Changed

- rename method `UploadMedia` to `UploadFile` (in sync with other SDKs)
- rename `TaskType` Unspecified property
- refactor `FileResource.SizeBytes` to long data type (int64)
- refactor response type of `ListFiles` (discovery)
- streaming response using SSE format works for other models than gemini-pro (original limitation)
- specify default values for `pageSize`
- refactor constants to external file
- add and amend enum identifiers
- add and amend XML doc

## 1.2.0

### Added

- use TLS 1.2 protocol (.NET Fx)
- troubleshooting for streaming HttpIOException (.NET runtime issue)

### Changed

- improve writing of model name
- refactor Content type used for SystemInstruction
- update tests regarding Content type

### Fixed

- fix response checking in ChatSession

## 1.1.4

### Added

- new values in enum FinishReason
- new enum HarmBlockMethod

### Changed

- improve enums (ref: Google.Cloud.AIPlatform.V1)
- improve response in SSE format
- update samples to latest NuGet package

## 1.1.3

### Changed

- improve Grounding for Google Search and Vertex AI Search

### Fixed

- system instruction is an instance of content, not a list of same.

## 1.1.2

### Added

- test cases for FinishReason.MaxTokens

### Changed

- improve accessor of response.Text
- upgrade NuGet packages dependencies

## 1.1.1

### Fixed

- upload via File API (Display name was missing)

## 1.1.0

### Added

- implement JSON mode
- implement Grounding for Google Search and Vertex AI Search
- implement system instructions
- add model `text-embedding-004`
- add model `gemini-1.0-pro-002`
- add Audio / File API support

### Changed

- add tools collection
- generate XML docs

### Fixed

## 1.0.1

### Added

- implement part type of VideoMetadata
- enable Server Sent Events (SSE) for `gemini-1.0-pro`
- add models Gemini 1.5 Pro (FC patch, PIv5 and DI) and Gemini 1.0 Ultra

### Changed

- improve XML documentation
- remove/reduce snake_case JSON elements

## 1.0.0

### Added

- implement File API to support large files
- full support of Gemini 1.5 and Gemini 1.0 Ultra

### Changed

- improve XML documentation

## 0.9.4

### Added

- implement patching of tuned models in .NET Framework
- guard for unsupported features or API backend
- expose GetModel on IGenerative

### Changed

- extend XML documentation

### Fixed

- Assigning an API_KEY using model.ApiKey is not working ([#20](https://github.com/mscraftsman/generative-ai/issues/20))

## 0.9.3

### Changed

- apply default config and settings to request

### Fixed

- Fix a bug in Initialize_Model() test by Will @doggy8088 ([#13](https://github.com/mscraftsman/generative-ai/issues/13))
- Fixes ContentResponse class issue by Will @doggy8088 ([#16](https://github.com/mscraftsman/generative-ai/issues/16))
- ignore Text member in ContentResponse by Will @doggy8088 ([#14](https://github.com/mscraftsman/generative-ai/issues/14))

## 0.9.2

### Added

- models of Gemini 1.5 and Gemini 1.0 Ultra
- tests for Gemini 1.5 and Gemini 1.0 Ultra

## 0.9.1

### Added

- add interface IGenerativeAI
- simplify image/media handling in requests
- extend generateAnswer feature
- more tests for Gemini Pro Vision model
- add exceptions from API reference

### Changed

- improve creation of generative model in Google AI class
- SafetySettings can be easier and less error-prone. ([#8](https://github.com/mscraftsman/generative-ai/issues/8))
- remove _useHeaderApiKey ([#10](https://github.com/mscraftsman/generative-ai/issues/10]))

## 0.9.0

### Added

- compatibility methods for PaLM models

### Changed
### Fixed

## 0.8.4

### Added

- missing comments and better explanations
- add GoogleAI type ([#3](https://github.com/mscraftsman/generative-ai/issues/3))
- read environment variables in GoogleAI and VertexAI

## 0.8.3

### Added

- simplify creation of tuned model

### Fixed

- check of model for Tuning, Answering and Embedding

## 0.8.2

### Added
 
- ability to rewind chat history
- access text of content response easier

### Changed

- improve handling of chat history (streaming)

## 0.8.1

### Changed

- access modifier to avoid ambiguous type reference (ClientSecrets)

## 0.8.0

### Added

- implement tuned model patching (.NET 6 and higher only)
- implement transfer of ownership of tuned model
- implement batched Embeddings
- query string parameters to list models (pagination and filter support)
- type documentation
- generate a grounded answer
- constants for method names/endpoints
- enumeration of state of created tuned model

### Changed

- text prompts have `user` role assigned
- improve Embeddings
- refactor types according to API reference 
- extend type documentation
- improve .NET targetting of source code

## 0.7.2

### Added

- delete tuned model

### Changed

- method to list models supports both - regular and tuned - model types

## 0.7.1

### Added

- implement model tuning (works with stable models only)
  - `text-bison-001`
  - `gemini-1.0-pro-001`
- tests for model tuning
 
### Changed

- improved authentication regarding API key or OAuth/ADC
- added scope https://www.googleapis.com/auth/generative-language.tuning
- harmonized version among NuGet packages
- provide empty response on Safety stop
- merged regular and tuned ModelResponse

## 0.7.0

### Added

- use Environment Variables for default values (parameterless constructor)
- support of .env file

### Changed

- improve Function Calling
- improve Chat streaming
- improve Embeddings

## 0.6.1

### Added

- implement Function Calling

## 0.6.0

### Added

- implement streaming of content
- support of HTTP/3 protocol
- specify JSON order of properties

### Changed

- improve handling of config and settings

## 0.5.4

### Added

- implement Embeddings
- brief sanity check on model selection

### Changed

- refactor handling of parts
- ⛳ allow configuration, safety settings and tools for Chat

## 0.5.3

### Added

- Implement Chat

## 0.5.2

### Added

- Use of enumerations

### Changed

- Correct JSON conversion of SafetySettings

## 0.5.1

### Added

- Handle GenerationConfig, SafetySeetings and Tools

### Changed

- Append streamGenerateContent

## 0.5.0

### Added

- Add NuGet package Mscc.GenerativeAI.Web for use with ASP.NET Core.

### Changed

- Refactor folder structure

## 0.4.5

### Changed

- Extend methods

## 0.4.4

### Added

- Automate package build process

## 0.4.3

### Added

- Add x-goog-api-key header

## 0.4.2

### Changed

- Minor correction

## 0.4.1

### Added

- Add OAuth to Google AI

## 0.3.2

### Changed

- Improve package attributes

## 0.3.1

### Added

- Add methods ListModels and GetModel

## 0.2.1

### Added

- Initial Release

## 0.1.2

### Changed

- Update README.md
