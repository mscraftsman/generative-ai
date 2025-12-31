# Changelog (Release Notes)

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added
### Changed
### Fixed

## 3.0.2

### Added

- add interface `ILogger` to `GeminiEmbeddingGenerator` (edde334)

### Changed

- bump version

### Fixed

- fix mapping of embeddings #164 (2c5b343)

## 3.0.1

### Added

- add handling of `CreatedAt` (920782c)
- map `PromptFeedback` (319b715)
- map `CitationMetadata` (f16b007)
- add property `DisplayName` (7f6d835)
- add `RetryStatusCodes` to AdditionalProperties (0f95334)
- provide initial dictionary of function names (00184a6)

### Changed

- improve mapping of `UsageMetadata` (1e67c92)
- chore: add XML comment (851c52f)
- chore: formatting (e89cf32)
- upgrade NuGet packages (ac6c10b)
- upgrade NuGet packages (c15d2cf)

### Fixed

- refactor `ToChatMessage` improving handling of `ThoughtSignature` #163 (ddfaa8d)
- Merge pull request #165 from mscraftsman/fix-issue-164-12176495308426868827 (c8d748e)
- Fix: Return multiple embeddings for multiple values (3600715)

## 3.0.0

see main Changelog

## 2.9.8

### Added

- add constructor parameter `accessToken` #157 (81ad7f9)

### Changed

- bump version

## 2.9.7

### Added

- add handling of `ToolConfig` (34c565b)
- add generic handling of Gemini tools (9cd60a8)
- add tests for various tools (840c413)

### Changed

- amend tests for Vertex AI (82d6a7a)
- enable Vertex AI configuration (5a911c5)

## 2.9.6

### Added

- add various hosted tools (4892ae0)
- add MEAI samples of using Web Search and Code Interpreter (4d0e489)

### Fixed

- fix optional toolConfig.functionCallConfig handling (25be2b3)

## 2.9.5

### Added

- add mapping of `ThinkingLevel` (e7adca0)
- add handling of `Dimensions` (2505516)
- add `ILogger` parameter (88b00f2)
- add tests for GetResponseAsync (3425cb0)
- add .NET version specifics (eab893e)
- Merge pull request #153 from PederHP/FixComputerUseEnvironment (d054d02)

### Changed

- extend sample MEAI project (a514c9f)

### Fixed

- Fix bugs (734828f)
- Fix JsonConverter type for ComputerUseEnvironment enum (247ed9a)

## 2.9.4

### Added

- Add support for thought_signature in function calls #145 (e0197cd)
- Merge pull request #143 from xoofx/fix-thought (d03a3e3)
- Apply PR 146 to current code base (d2886fd)
- Merge pull request #146 from PederHP/fix_double_encoding (248a25d)
- Merge pull request #147 from PederHP/thought_issue_rebased (b488013)

### Changed

- Put check of thought before regular response (76be9c3)
- Refactor DataContent instantiation for readability (22b2d80)
- Change data encoding to Base64 for InlineData (532ee13)
- Complete review of Thought signatures #149 (e0197cd)
- Bump Microsoft.Extensions.AI from 10.0.0 to 10.0.1 (4441a00)
- Bump Microsoft.Extensions.AI.Abstractions from 10.0.0 to 10.0.1 (b622e5f)

### Fixed

- Fix thinking responses when they don't include a ThoughtSignature. (c8cc181)
- Fix thought_signature preservation for image parts in MEAI mapping (59f1c9f)
- Enhance ThoughtSignature handling in AbstractionMapper for improved serialization and preservation (99a8bc4)

### Changed

- bump version

## 2.9.3

### Changed

- bump version

## 2.9.2

### Changed

- update data types from discovery doc (2c547cb)
- bump version

## 2.9.1

### Added

- add .NET 10.0 targeting

### Changed

- bump version

## 2.9.0

### Changed

- bump version

## 2.8.25

### Added

- add test from `dotnet-genai` (MEAI) (f4520d8)
- add extension methods `AsI...` (921e508)

## 2.8.24

### Changed

- bump version

## 2.8.23

### Changed

- bump version

## 2.8.22

### Changed

- bump version

## 2.8.21

### Added

- add implementation of `IImageGenerator` as `GeminiImageGenerator` (2366e37)

### Changed

- provide another example using newer Gemini models (fe2b7b8)
- refactor to provide lazily-initialized metadata (4714360)

## 2.8.20

### Changed

- bump version

## 2.8.19

### Changed

## 2.8.18

### Changed

- bump version

## 2.8.17

### Changed

- bump version

## 2.8.16

### Changed

- Update IChatClient to utilize newer surface area (0377aa1)
- bump version

## 2.8.15

### Changed

- bump version

## 2.8.14

### Changed

- bump version

## 2.8.13

### Changed

- bump version

## 2.8.12

### Changed

- bump version

## 2.8.11

### Changed

- bump version

## 2.8.10

### Changed

- bump version

## 2.8.9

### Changed

- bump version

## 2.8.8

### Added

- add `virtual` modifier (c9cf1e7)

### Changed

- change Fact to Theory and use OP's prompt #90 (c3e9832)
- change scope (a93811a)
- change namespace (f0a4254)
- update test case #90 [skip ci] (06dfa17)

### Fixed

- fix #90: add type mapping in streaming (fb67fd1)

## 2.8.7

### Changed

- bump version

### Fixed

- use named parameter

## 2.8.6

### Changed

- bump version

## 2.8.5

### Added

- Merge pull request #106 from TeodorVecerdi/feature/meai-function-calling (2920962)

### Changed

- Improve M.E.AI support for function calls (9e2b1f4)
- bump version

## 2.8.4

### Added

- implement function calling properly @TeodorVecerdi (7d441bf)
- add tests for function calling (bad4bfd)

### Changed

- bump version

## 2.8.3

### Changed

- bump version

## 2.8.2

### Changed

- bump version

## 2.8.1

### Changed

- bump version

## 2.8.0

### Changed

- bump version

## 2.7.1

### Changed

- bump version

## 2.7.0

### Changed

- Pass `Timeout` through #79
- bump version

## 2.6.13

### Changed

- bump version

## 2.6.12

### Changed

- no code changes

## 2.6.11

### Changed

- bump version

## 2.6.10

### Changed

- bump version

## 2.6.9

### Changed

- bump version

## 2.6.8

### Changed

## 2.6.7

### Changed

- Structured output support for Mscc.GenerativeAI.Microsoft [#97](https://github.com/mscraftsman/generative-ai/pull/97) thanks to @bharathm03

## 2.6.6

### Changed

- bump version

## 2.6.5

### Changed

- bump version
- update NuGet packages

## 2.6.4

### Changed

- bump version

## 2.6.3

### Added

- add fluent approach for `GeminiClient` with extension methods
- add constructor for GenerativeModel instance

## 2.6.2

### Changed

- bump version

## 2.6.1

### Changed

- bump version

## 2.6.0

### Changed

- Update to stable Microsoft.Extensions.AI and update some implementation [#87](https://github.com/mscraftsman/generative-ai/pull/87) thanks to @stephentoub

## 2.5.6

### Changed

- Update Microsoft.Extensions.AI version to 9.5.0-preview.1.25262.9

## 2.5.5

### Changed

- Update Microsoft.Extensions.AI version to 9.4.3-preview.1.25230.7 [85](https://github.com/mscraftsman/generative-ai/pull/85) thanks to @MackinnonBuck

## 2.5.4

### Changed

- bump version

## 2.5.3

### Changed

- bump version

## 2.5.2

### Changed

- bump version

## 2.5.1

### Changed

- bump version

## 2.5.0

### Changed

- bump version

## 2.4.1

### Changed

- bump version

## 2.4.0

### Changed

- bump version

## 2.3.6

### Changed

- bump version

## 2.3.5

### Changed

- bump version

## 2.3.4

### Changed

- Update to M.E.AI 9.3.0-preview.1.25161.3 [#72](https://github.com/mscraftsman/generative-ai/pull/72) thanks to @stephentoub
- bump version

## 2.3.3

### Changed

- bump version

## 2.3.2

### Changed

- bump version

## 2.3.1

### Changed

- use named parameter to initialize Vertex AI
- bump version

## 2.3.0

### Changed

- update NuGet packages
- bump version

## 2.2.11

### Changed

- bump version

## 2.2.10

### Changed

- bump version

## 2.2.9

### Changed

- bump version

## 2.2.8

### Changed

- bump version

## 2.2.7

### Changed

- bump version

## 2.2.6

### Changed

- bump version

## 2.2.5

### Changed

- update NuGet packages
- bump version

## 2.2.4

### Changed

- refactor namespace
- Update Microsoft.Extensions.AI to 9.3.0-preview.1.25114.11 [#67](https://github.com/mscraftsman/generative-ai/pull/67) thanks to @stephentoub
- update NuGet packages
- bump version

## 2.2.3

### Changed

- bump version

## 2.2.2

### Changed

- update NuGet packages

## 2.2.1

### Changed

- bump version

## 2.2.0

### Changed

- bump version

## 2.1.8

### Changed

- bump version

## 2.1.7

### Changed

- bump version

## 2.1.6

### Changed

- bump version

## 2.1.5

### Changed

- bump version

## 2.1.4

### Changed

- bump version

## 2.1.3

### Changed

- bump version

## 2.1.2

### Changed

- bump version

## 2.1.1

### Changed

- bump version

## 2.1.0

### Changed

- update M.E.AI to 9.1.0-preview.1.25064.3 [#58](https://github.com/mscraftsman/generative-ai/pull/58) thanks to @stephentoub
- update NuGet packages
- drop .NET 6.0 targeting

## 2.0.2

### Changed

- remove non-existent `ResponseSchema` mapping
- bump version

## 2.0.1

### Changed

- bump version

## 2.0.0

### Changed

## 1.9.7

### Changed

- bump version

## 1.9.6

### Changed

- use explicit/aliases namespace
- update NuGet packages

## 1.9.5

### Changed

- Update M.E.AI to 9.0.1-preview.1.24570.5 [#48](https://github.com/mscraftsman/generative-ai/pull/48) thanks to @stephentoub

## 1.9.4

### Changed

- bump version

## 1.9.3

### Changed

- bump version

## 1.9.2

### Changed

- bump version

## 1.9.1

### Changed

- PR: Update Microsoft.Extensions.AI to 9.0.0-preview.9.24556.5 [#44](https://github.com/mscraftsman/generative-ai/pull/44) thanks to @stephentoub

## 1.9.0

### Changed

- bump version

## 1.8.3

### Changed

- bump version

## 1.8.2

### Added

- initial release