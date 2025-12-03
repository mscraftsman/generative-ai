# Release Notes

## 2.9.4

### Added

- add model `gemini-3-pro-image-preview` add model `nano-banana-pro-preview` (31388f3)
- add model `gemini-3-pro-preview` (9981e7e)
- add license header check (af98ac8)
- add license header for code files (f9dd69b)
- Add support for thought_signature in function calls #145 (e0197cd)
- add property `Shape` (17dc6dd)
- add devcontainer for better Codespace (.NET 10) (255e48d)
- add RequestOptions (d82aa66)
- add ILogger functionality (b7b8a98)
- apply `RetryDelay` to retry strategy for HTTP 429 #136 (7066bc9)
- feat: Handle Cloud Storage URIs in AddMedia() (da1d52c)
- feat: Handle HTTP 429 with Retry-After header (457331e)
- provide `Description` in enums (c61f4a0)
- test enum value DescriptionAttribute (87cb93a)
- add Vertex AI client (eaad8bf)
- test enum DescriptionAttribute (141738e)
- use enum property #130 (89d187b)
- support compute tokens (70124b6)
- support count tokens (177cfc0)
- support Function call argument streaming for all languages (0d73195)
- Successfully created the `Console.Issue124` console project in the `samples` directory. (bafb29f)
- Merge branch 'main' into fix_double_encoding (def3a33)
- Merge pull request #136 from mscraftsman/feature/handle-http-429-retry-after (cdd77c6)
- Merge pull request #137 (748a981)
- Merge pull request #139 (f20c366)
- Merge pull request #141 (8c929d4)
- Merge pull request #142 from mscraftsman/feature-issue-124 (949a079)
- Merge pull request #143 from xoofx/fix-thought (d03a3e3)
- Apply PR 146 to current code base (d2886fd)
- Merge pull request #146 from PederHP/fix_double_encoding (248a25d)
- Merge pull request #147 from PederHP/thought_issue_rebased (b488013)
- Merge pull request #152 (f85b42e)

### Changed

- remove model `gemini-2.0-flash-preview-image-generation`
- remove model `imagen-3.0-generate-001`
- remove model `imagen-3.0-generate-002-exp`
- remove model `imagen-4.0-generate-preview-05-20`
- remove model `imagen-4.0-ultra-generate-exp-05-20`
- remove model `imagen-4.0-ultra-generate-preview-06-06`
- remove model `veo-2.0-generate-001`
- remove model `veo-3.0-fast-generate-preview`
- remove model `lyria-base-001` (bad5036)
- change model (e2a109b)
- change `RetrievedContext` (98c90c9)
- extend `FinishReason` (7c2c905)
- use `JsonStringEnumConverter` instead of `EnumMemberAttribute` (52a11c9)
- Address code review: add null checks in test reflection code (f8957aa)
- Put check of thought before regular response (76be9c3)
- Refactor DataContent instantiation for readability (22b2d80)
- Change data encoding to Base64 for InlineData (532ee13)
- Complete review of Thought signatures #149 (e0197cd)
- Bump Microsoft.Extensions.AI from 10.0.0 to 10.0.1 (4441a00)
- upgrade to .NET 10 (938919a)
- revision 20251117 (b779025)
- revision 20251201 (167df08)
- upgrade NuGet packages (be8daf0)
- Bump Microsoft.Extensions.AI.Abstractions from 10.0.0 to 10.0.1 (b622e5f)
- Bump Google.Apis.Auth from 1.72.0 to 1.73.0 (f11aa7d)
- rename file for Jules and other AI agents. (88db222)
- change label (0eaabff)
- adjust editor configuration (2316b3e)
- use CSV files ;-) (2dd41aa)
- rename project (e086174)
- remove duplicate enum (dbac130)
- update model reference (2199919)
- read more env vars to allow testing with multiple clients. (639d5b7)
- read env var (622a395)
- Formatting (ea6c3bb)
- Formatting (030f84a)
- chore: update test (c8e2259)
- revert tests (ef143d8)

### Fixed

- fix thinking responses when they don't include a ThoughtSignature.
- fix broken retry mechanism #144 (c07809a)
- Fix thinking responses when they don't include a ThoughtSignature. (c8cc181)
- fixes (97a4da3)
- Fix thought_signature preservation for image parts in MEAI mapping (59f1c9f)
- Enhance ThoughtSignature handling in AbstractionMapper for improved serialization and preservation (99a8bc4)

## Changelog

Changes across all versions have been documented in the [Changelog](CHANGELOG.md).
