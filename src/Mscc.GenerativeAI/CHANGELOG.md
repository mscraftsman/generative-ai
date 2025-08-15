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

## 2.6.10

### Added

- add Gemini CLI instructions
- add model `imagen-4.0-generate-001`
- add model `imagen-4.0-fast-generate-001`
- add model `imagen-4.0-ultra-generate-001`
- add new URL retrieval statuse

### Changed

- improve handling of embeddings
- use IHttpClientFactory to create HttpClient instances [#92](https://github.com/mscraftsman/generative-ai/issues/92)
- use default model of test
- upgrade NuGet packages
- remove AI excludes because of Gemini agent mode and Gemini CLI
- replace deprecated models in tests

## 2.6.9

### Added

- add model `gemini-2.5-flash-lite`
- add model `veo-3.0-fast-generate-preview`
- add functionality of batched content generation
- add more batch endpoints
- add type `Document`
- add enum `BatchState`
- add finish reason `TooManyToolCalls`
- add test with `Dynamic Thinking`

### Changed

- mark `HarmCategoryCivicIntegrity` obsolete
- remove model `gemini-pro-vision`
- remove model `gemini-1.0-pro-vision-latest`
- replace model GeminiProVision with Gemini25Pro
- upgrade NuGet packages

## 2.6.8

### Added

- add model `veo-3.0-generate-preview`
- add test for tool `UrlContext`
- add convenience property `Thinking`

### Changed

- change convenience property `Text`
- upgrade NuGet packages

## 2.6.7

### Changed

- Structured output support for Mscc.GenerativeAI.Microsoft [#97](https://github.com/mscraftsman/generative-ai/pull/97) thanks to @bharathm03
- upgrade NuGet packages
- bump version

## 2.6.6

### Added

- add model `gemini-embedding-001`
- add handling for Gemini Embedding models

### Changed

- use parameter `logger`
- update embedding tests
- add IPart constructors to `Content` class and improve flexibility [#95](https://github.com/mscraftsman/generative-ai/pull/95) thanks to @dannyball710
- upgrade NuGet packages

## 2.6.5

### Added

- add model `gemini-2.0-flash-exp-image-generation`
- add model `gemini-2.0-flash-preview-image-generation`
- add model `gemma-3n-e2b-it`
- add model `gemini-2.5-flash-live-preview`
- add method `delete` for batches
- add enum value `UnexpectedToolCall`
- add property `Thinking`

### Changed

- remove model `gemini-2.5-pro-exp-03-25`
- upgrade NuGet packages

### Fixed

- Update RagEngineModel.cs [#94](https://github.com/mscraftsman/generative-ai/pull/94) thanks to @rfrcarvalho

## 2.6.4

### Added

- add model `gemini-2.5-pro`
- add model `gemini-2.5-flash`
- add model `gemini-2.5-flash-lite-preview-06-17`
- add model `gemini-live-2.5-flash-preview`
- add model `imagen-4.0-generate-preview-06-06`
- add model `imagen-4.0-ultra-generate-preview-06-06`
- add model `gemini-2.5-pro-preview-06-05`
- add `DateTimeFormatJsonConverter`
- add test for 3rd party libraries
- add test to handle Thinking response

### Changed

- upgrade NuGet packages
- change model to run tests with
- change test to use Gemini 2.0 Flash
- remove model `gemini-1.5-pro-001`
- remove model `gemini-1.5-flash-001`
- remove model `gemini-1.5-flash-001-tuning`
- remove model `gemini-1.5-flash-8b-exp-0827`
- remove model `gemini-1.5-flash-8b-exp-0924`

## 2.6.3

### Added 

- add model `gemini-2.5-pro-preview-06-05`

### Changed

- upgrade NuGet packages

## 2.6.2

### Added

- add model `gemini-2.5-flash-preview-native-audio-dialog-rai-v3`
- add image manipulation config types
- add reference image types
- add types to edit and upscale images
- add upscale factor enum
- add batches `Cancel` method
- add properties `ParametersJsonSchema`, `ResponseJsonSchema` and `ThoughtSignature`

### Changes

- add optional `role` to Content constructor

## 2.6.1

### Added

- add `batches` client
- add property `ResponseId`

## 2.6.0

### Added

- add model `gemini-2.5-flash-preview-05-20`
- add model `gemini-2.5-flash-preview-tts`
- add model `gemini-2.5-pro-preview-tts`
- add model `gemma-3n-e4b-it`
- add model `gemini-2.5-flash-preview-native-audio-dialog`
- add model `gemini-2.5-flash-exp-native-audio-thinking-dialog`
- add model `imagen-4.0-generate-preview-05-20`
- add model `imagen-4.0-ultra-generate-exp-05-20`
- add model `lyria-002`
- add multi-speaker configuration
- add function behavior
- add URL context and metadata

### Changed

- Update to stable Microsoft.Extensions.AI and update some implementation [#87](https://github.com/mscraftsman/generative-ai/pull/87) thanks to @stephentoub

### Fixed

- Add missing optional CancellationToken parameters [#89](https://github.com/mscraftsman/generative-ai/pull/89) thanks to @Ibuki-Suika

## 2.5.6

### Changed

- Update Microsoft.Extensions.AI version to 9.5.0-preview.1.25262.9

## 2.5.5

### Changed

- Update Microsoft.Extensions.AI version to 9.4.3-preview.1.25230.7 [85](https://github.com/mscraftsman/generative-ai/pull/85) thanks to @MackinnonBuck

## 2.5.4

### Added

- add model 'gemini-2.5-flash-preview-04-17-thinking' 
- add model 'gemini-2.5-pro-preview-05-06' 
- add model 'gemini-2.0-flash-preview-image-generation'
- add Schema properties
- add property 'UrlRetrievalMetadata' to 'Candidate'

### Changed

- rename type 'VideoMetadata' to 'VideoFileMetadata'

## 2.5.3

### Added

- add model `gemini-2.0-flash-exp-image-generation`

### Changed

- fix typo for Gemini 2.5 Flash constants #84 thanks to @Oskar-Przyborski
- upgrade NuGet packages

## 2.5.2

### Added

- add ThinkingBudget property #82
- add Validated function calling mode

## 2.5.1

### Added

- add model `gemini-2.5-flash-preview-04-17`
- add `dynamic` API
- add language code

### Changed

- upgrade NuGet packages

## 2.5.0

### Added

- add model `lyria-base-001`
- add model `gemini-2.5-pro-preview-03-25`
- add model `gemini-2.0-flash-live-001`
- add model `gemma-3-1b-it`
- add model `gemma-3-4b-it`
- add model `gemma-3-12b-it`

### Changed

- Update Microsoft.Extensions.AI version

## 2.4.1

### Added

- add test case for #77

### Changed

- process `ResponseSchema` as string #53 #76
- separate `record` type in `ResponseSchema` due to disallowed properties #76
- refactor POST calls
- update NuGet packages

### Fixed

- fix typo in model constant.

## 2.4.0

### Added

- add model `gemini-2.5-pro-exp-03-25`
- add model `gemini-2.0-flash-exp-image-generation`
- add `CacheTokensDetails` attribute in response
- add `title` attribute

### Changed

- read envVar `GOOGLE_GENAI_USE_VERTEXAI`
- adjust endpoint method of Veo 2 model
- update NuGet packages

## 2.3.6

### Added

- add test for issue #74

### Fixed

- Problems with generating a response based on the function call #74 thanks to @NotroDev

## 2.3.5

### Added

- add new Schema attributes
- add tests for ResponseModality
- add test to use Youtube video
- add test to classify (remote) image using enum
- add test for models using GoogleSearchRetrieval tool

### Changed

- update NuGet packages

### Fixed

- Problems with generating a response based on the function call #74 thanks to @NotroDev

## 2.3.4

### Changed

- bump version

## 2.3.3

### Added

- add model `gemma-3-27b-it`

### Changed

- update NuGet packages

## 2.3.2

### Added

- add API key of Vertex AI in express mode
- add sample using safety settings

### Fixed

- fix for Vertex AI in express mode

## 2.3.1

### Added

- add model `gemini-embedding-exp`

## 2.3.0

### Added

- add model `gemini-embedding-exp-03-07`
- add new `CodeRetrievalQuery` task type
- add model `gemini-2.0-flash-lite`
- add model `gemini-2.0-flash-lite-001`
- add model `veo-2.0-generate-001`
- add video generation
- add `genai` config types
- add RAG Engine API (Vertex AI only)
- add new metadata properties
- add discontinuation dates
- add more and consistent use of `ILogger` #6
- add logging when there are multiple candidates
- add OpenAI compatible enums for image generation
- add OpenAI properties
- add extension to (download and) inline remote resources
- add extension methods to create `Part` types
- add more environment variables to read

### Changed

- improvements to test cases
- extend REST playground samples
- extend Python playground samples (incl. setup env)
- update NuGet packages
- remove Gemini 1.0 test case
- add and update enums
- rename enum `FunctionCallingMode` to `FunctionCallingConfigMode`

### Fixed

- add workaround for MIME type of Rich Text Format (RTF)
- add default constructor due to serialization
- fix OpenAI endpoints for image generation

## 2.2.11

### Added

- add more model constants for Imagen 3
- add model `code-gecko`

### Changed

- rename model `imagetext`

### Fixed

- fix regression error

## 2.2.10

### Added

- add `ThoughtsTokenCount` property

### Changed 

- remove Gemini 1.0 references
- update NuGet packages

## 2.2.9

### Fixed

- fix regression error

## 2.2.8

### Added

- add v1alpha types/resources
- add `MaxTemperature` property
- add cancellationToken for .NET 8+
- add more test cases
- add IsValidJson helper method
- create release notes on tag

### Changed

- use .NET version-specific PATCH method
- improve XML doc
- update NuGet packages

## 2.2.7

### Changed

- revert changes of RTF MIME type #71
- add test case for RTF mime type(s)
- accept `application/rtf`
- add comment regarding `application/rtf` "workaround"

## 2.2.6

### Added

- add more MIME types #71

### Changed

- review OpenAI get model endpoint

## 2.2.5

### Added

- add support for `enum` as response schema
- add more logging

### Changed

- Flash 2.0 models removed bidiGenerateContent endpoint method
- update NuGet packages

## 2.2.4

### Changed

- change endpoint URL
- mark NegativePrompt as deprecated

## 2.2.3

### Added

- add usage meta data for tokens in tool-use prompt(s)
- add model `gemini-2.0-flash-thinking-exp-no-thoughts`
- add model `imagen-3.0-generate-002-exp`
- add model `image-verification-001`

### Changed

- extend OpenAI REST calls
- specify JSON key for Open AI model owner

## 2.2.2

### Changed

- update NuGet packages

## 2.2.1

### Added

- add properties from Vertex AI

### Changed

- improve guard of API key

## 2.2.0

### Added

- add Vertex AI in express mode

## 2.1.8

### Added

- Add HTTP status code to HttpRequestException #64 thanks to @eirikwah

### Changed

- refactor image generation (Imagen 3)

## 2.1.7

### Added

- add new model `gemini-2.0-flash`
- add new model `gemini-2.0-flash-001`
- add new model `gemini-2.0-flash-lite-preview`
- add new model `gemini-2.0-flash-lite-preview-02-05`
- add new model `gemini-2.0-pro-exp`
- add new model `gemini-2.0-pro-exp-02-05`

### Changed

- remove model `gemini-1.5-flash-exp-0827`
- remove model `gemini-1.5-pro-exp-0801`
- remove model `gemini-1.5-pro-exp-0827`

## 2.1.6

### Added

- add parameter `EnhancePrompt` 
- add parameter `AddWatermark`
- add handling for `JsonElement` and `JsonNode`

### Changed

- map previous models

## 2.1.5

### Added

- add parameter to specify API version
- add `ThinkingConfig` parameter
- revision 20250130 - removal of OpenAI, Coscientist, etc.

### Changed

- update Imagen3 model to `imagen-3.0-generate-002`

## 2.1.4

### Added

- OpenAI comp: list models REST
- revision 20250127

### Changed

- correct and update README and test cases #62

## 2.1.3

### Added

- add model `gemini-2.0-flash-thinking-exp-01-21`
- add several new properties of Gemini API

## 2.1.2

### Added

- allow custom response schema [#59](https://github.com/mscraftsman/generative-ai/pull/59) thanks to @biegehydra
- NuGet: embed untracked sources
- add raw base64 addMedia method [#61](https://github.com/mscraftsman/generative-ai/pull/61) thanks to @cecingua

### Changed

- make clients thread safe [#59](https://github.com/mscraftsman/generative-ai/pull/59) thanks to @biegehydra
- rename tests for `ResponseSchema`
- update NuGet packages
- drop Newtonsoft.Json

### Fixed

- "System.Net.Http.HttpRequestException: The HTTP request headers length exceeded the server limit of 65536 bytes" in Mscc.GenerativeAI.GoogleAI.UploadFile [#56](https://github.com/mscraftsman/generative-ai/issues/56)

## 2.1.1

### Changed 

- Integrate JsonSchema.net to generate OpenApi schemas [#55](https://github.com/mscraftsman/generative-ai/issues/55) thanks to @rawb300

## 2.1.0

### Added

- add Schema properties
- add enum `MediaResolution`
- add `AudioOptions` property
  add `DownloadFile` method and `Source` property
- add optional parameter `CancellationToken` to all requests [#54](https://github.com/mscraftsman/generative-ai/issues/54) thanks to @Ar4ics

### Changed

- update NuGet packages
- drop .NET 6.0 targeting
- InvalidOperationException when setting Timeout [#57](https://github.com/mscraftsman/generative-ai/issues/57) thanks to @Ar4ics
- extend `OpenAI.GetModel` method
- change JSON default settings

## 2.0.2

### Added

- add model `gemini-2.0-flash-thinking-exp`
- add model `gemini-2.0-flash-thinking-exp-1219`
- add model `imagen-3.0-capability-001`
- add `images` API to OpenAI compatibility
- add `GenerateImages` method to Gemini model
- copy model on Vertex AI

### Changed

- Extend `SendMessage` - Chat conversations - How [#52](https://github.com/mscraftsman/generative-ai/issues/52) thanks to @Francks11
- update NuGet packages

### Fixed

- ResponseSchema property must be an object [#53](https://github.com/mscraftsman/generative-ai/issues/53) thanks to @Ar4ics
- fix typos

## 2.0.1

### Added

- add types, enums, and properties for Multimodal Live API
- add Google Search as a tool

### Changed

- extend README to show "Search as a tool"

## 2.0.0

### Added

- add model `gemini-2.0-flash-exp`

### Changed

- drop both `Experimental 0827` models

## 1.9.7

### Added

- add model `gemini-exp-1206`
- add OpenAI list models endpoint
- add OpenAI get model endpoint
- add Imagen 3 samples from blog article
- add list models functionality for Vertex AI
- add ComputeTokens method for Vertex AI

## 1.9.6

### Added

- add Project IDX development environment
- add OpenAI sample app

### Changed

- rename due to type/namespace conflict
- update NuGet packages

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
