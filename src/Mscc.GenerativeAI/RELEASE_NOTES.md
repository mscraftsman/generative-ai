# Release Notes

## 3.0.1

### Added

- add model `gemini-3-flash-preview` (36873ad)
- add handling of `CreatedAt` (920782c)
- map `PromptFeedback` (319b715)
- map `CitationMetadata` (f16b007)
- add property `DisplayName` (7f6d835)
- add type `StreamableHttpTransport` (33ecdf0)
- add property `VeoDataMixtureRatio` (0a187d2)
- add tool `McpServer` (53886fd)
- add tool `ToolParallelAiSearch` (4345b06)
- add `RetryStatusCodes` to AdditionalProperties (0f95334)
- provide initial dictionary of function names (00184a6)
- add new samples to solution (e99f48b)
- add sample for issue #163 (41e940f)
- add sample for #162 (e6188b7)
- add test for #163 (e4415ff)
- add globbing (9a1fbd1)
- add set constraints (aa7b10f)
- add XML comments (acef44b)
- add `AsBuilder` extension method (bc2e811)

### Changed

- remove extension method `AsBuilder` (f79b75f)
- refactor lazy instantiation (6c02503)
- split `Directory.Build.props` (e09bd76)
- improve mapping of `UsageMetadata` (1e67c92)
- refactor to version 3.0.0 (1914d6a)
- parse Cloud Storage URLs (d82b62b)
- chore: add XML comment (851c52f)
- chore: formatting (e89cf32)
- upgrade NuGet packages (ac6c10b)
- upgrade NuGet packages (c15d2cf)
- update Dev Container (3141863)
- use GitHub variable (0cc22b9)
- extend build pipeline (bec6e5e)
- update using statements (1390b4e)
- sync latest models (835e612)
- sync with latest Discovery APIs (95b92e4)
- revision 20251219 (3600258)
- revision 20251218 (db39bc6)

### Fixed

- refactor `ToChatMessage` improving handling of `ThoughtSignature` #163 (ddfaa8d)
- Merge pull request #165 from mscraftsman/fix-issue-164-12176495308426868827 (c8d748e)
- Fix: Return multiple embeddings for multiple values (3600715)
- Merge pull request #161 from mscraftsman/alpha (3ca39eb)

## Changelog

Changes across all versions have been documented in the [Changelog](CHANGELOG.md).
