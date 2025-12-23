/*
 * Copyright 2024-2025 Jochen Kirstätter
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      https://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// Request to generate a completion from the model.
    /// </summary>
    public partial class GenerateContentRequest
    {
        /// <summary>
        /// The ETag of the item.
        /// </summary>
        public virtual string? ETag { get; set; }

        /// <summary>
        /// Optional. The labels with user-defined metadata for the request.
        /// </summary>
        /// <remarks>
        /// It is used for billing and reporting only.
        /// Label keys and values can be no longer than 63 characters (Unicode codepoints) and
        /// can only contain lowercase letters, numeric characters, underscores, and dashes.
        /// International characters are allowed. Label values are optional. Label keys must start with a letter.
        /// </remarks>
        public virtual IDictionary<string, string>? Labels { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenerateContentRequest"/> class.
        /// </summary>
        public GenerateContentRequest() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenerateContentRequest"/> class.
        /// </summary>
        /// <param name="prompt">String to process.</param>
        /// <param name="generationConfig">Optional. Configuration options for model generation and outputs.</param>
        /// <param name="safetySettings">Optional. A list of unique SafetySetting instances for blocking unsafe content.</param>
        /// <param name="tools">Optional. A list of Tools the model may use to generate the next response.</param>
        /// <param name="systemInstruction">Optional. </param>
        /// <param name="toolConfig">Optional. Configuration of tools.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="prompt"/> is <see langword="null"/>.</exception>
        public GenerateContentRequest(string prompt,
            GenerationConfig? generationConfig = null,
            List<SafetySetting>? safetySettings = null,
            Tools? tools = null,
            Content? systemInstruction = null,
            ToolConfig? toolConfig = null) : this()
        {
            if (prompt == null) throw new ArgumentNullException(nameof(prompt));

            Contents = [new Content { Role = Role.User, Parts = [new TextData { Text = prompt }] }];
            if (generationConfig != null) GenerationConfig = generationConfig;
            if (safetySettings != null) SafetySettings = safetySettings;
            if (tools != null) Tools = tools;
            if (systemInstruction != null) SystemInstruction = systemInstruction;
            if (toolConfig != null) ToolConfig = toolConfig;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenerateContentRequest"/> class.
        /// </summary>
        /// <param name="parts"></param>
        /// <param name="generationConfig">Optional. Configuration options for model generation and outputs.</param>
        /// <param name="safetySettings">Optional. A list of unique SafetySetting instances for blocking unsafe content.</param>
        /// <param name="tools">Optional. A list of Tools the model may use to generate the next response.</param>
        /// <param name="systemInstruction">Optional. </param>
        /// <param name="toolConfig">Optional. Configuration of tools.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="parts"/> is <see langword="null"/>.</exception>
        public GenerateContentRequest(List<IPart> parts,
            GenerationConfig? generationConfig = null,
            List<SafetySetting>? safetySettings = null,
            Tools? tools = null,
            Content? systemInstruction = null,
            ToolConfig? toolConfig = null) : this()
        {
            if (parts == null) throw new ArgumentNullException(nameof(parts));

            Contents = [new Content { Parts = parts }];
            if (generationConfig != null) GenerationConfig = generationConfig;
            if (safetySettings != null) SafetySettings = safetySettings;
            if (tools != null) Tools = tools;
            if (systemInstruction != null) SystemInstruction = systemInstruction;
            if (toolConfig != null) ToolConfig = toolConfig;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenerateContentRequest"/> class.
        /// </summary>
        /// <param name="file">The media file resource.</param>
        /// <param name="generationConfig">Optional. Configuration options for model generation and outputs.</param>
        /// <param name="safetySettings">Optional. A list of unique SafetySetting instances for blocking unsafe content.</param>
        /// <param name="tools">Optional. A list of Tools the model may use to generate the next response.</param>
        /// <param name="systemInstruction">Optional. </param>
        /// <param name="toolConfig">Optional. Configuration of tools.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="file"/> is <see langword="null"/>.</exception>
        public GenerateContentRequest(FileResource file,
            GenerationConfig? generationConfig = null,
            List<SafetySetting>? safetySettings = null,
            Tools? tools = null,
            Content? systemInstruction = null,
            ToolConfig? toolConfig = null) : this()
        {
            if (file == null) throw new ArgumentNullException(nameof(file));

            Contents =
            [
	            new Content
	            {
		            Role = Role.User,
		            Parts = [new FileData { FileUri = file.Uri, MimeType = file.MimeType }]
	            }
            ];
            if (generationConfig != null) GenerationConfig = generationConfig;
            if (safetySettings != null) SafetySettings = safetySettings;
            if (tools != null) Tools = tools;
            if (systemInstruction != null) SystemInstruction = systemInstruction;
            if (toolConfig != null) ToolConfig = toolConfig;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenerateContentRequest"/> class.
        /// </summary>
        /// <param name="parts"></param>
        /// <param name="generationConfig">Optional. Configuration options for model generation and outputs.</param>
        /// <param name="safetySettings">Optional. A list of unique SafetySetting instances for blocking unsafe content.</param>
        /// <param name="tools">Optional. A list of Tools the model may use to generate the next response.</param>
        /// <param name="systemInstruction">Optional. </param>
        /// <param name="toolConfig">Optional. Configuration of tools.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="parts"/> is <see langword="null"/>.</exception>
        public GenerateContentRequest(List<Part> parts,
            GenerationConfig? generationConfig = null,
            List<SafetySetting>? safetySettings = null,
            Tools? tools = null,
            Content? systemInstruction = null,
            ToolConfig? toolConfig = null) : this()
        {
            if (parts == null) throw new ArgumentNullException(nameof(parts));

            Contents = [new Content { PartTypes = parts }];
            if (generationConfig != null) GenerationConfig = generationConfig;
            if (safetySettings != null) SafetySettings = safetySettings;
            if (tools != null) Tools = tools;
            if (systemInstruction != null) SystemInstruction = systemInstruction;
            if (toolConfig != null) ToolConfig = toolConfig;
        }

        /// <summary>
        /// Adds a <see cref="Content"/> object to the request.
        /// </summary>
        /// <param name="content"></param>
        public void AddContent(Content content)
        {
            Contents ??= [];
	        Contents.Add(content);
        }

        /// <summary>
        /// Adds a media file or a base64-encoded string to the request.
        /// </summary>
        /// <remarks>
        /// Depending on the <paramref name="useOnline"/> flag, either an <see cref="InlineData"/>
        /// or <see cref="FileData"/> part will be added to the request.
        /// Standard URLs are supported and the resource is downloaded if <paramref name="useOnline"/> is <see langword="false"/>.
        /// </remarks>
        /// <param name="uri">The URI of the media file.</param>
        /// <param name="mimeType">The IANA standard MIME type to check.</param>
        /// <param name="useOnline">Flag indicating whether the file shall be used online or read from the local file system.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="uri"/> is <see langword="null"/>.</exception>
        public async Task AddMedia(string uri, string? mimeType = null, bool useOnline = false)
        {
            if (uri == null) throw new ArgumentNullException(nameof(uri));

            if (useOnline || uri.StartsWith("gs://", StringComparison.OrdinalIgnoreCase))
            {
                mimeType ??= GenerativeAIExtensions.GetMimeType(uri);
                // Strangely, the MIME type is not checked for FileData but InlineData only.
                // GenerativeAIExtensions.GuardMimeType(mimeType);
                AddPart(new FileData { FileUri = uri, MimeType = mimeType });
                await Task.CompletedTask;
            }
            else
            {
                string base64data;
                if (uri.IsValidBase64String())
                {
                    base64data = uri;
                    if (mimeType == null)
                        throw new ArgumentNullException(nameof(mimeType),
                            "MIME type for base64-encoded string is missing.");
                }
                else
                {
                    mimeType ??= GenerativeAIExtensions.GetMimeType(uri);
                    if (File.Exists(uri))
                    {
#if NET472_OR_GREATER || NETSTANDARD2_0
                        base64data = Convert.ToBase64String(File.ReadAllBytes(uri));
#else
                        base64data = Convert.ToBase64String(await File.ReadAllBytesAsync(uri));
#endif
                    }
                    else
                    {
                        base64data = await GenerativeAIExtensions.ReadImageFileBase64Async(uri);
                    }
                }

                GenerativeAIExtensions.GuardMimeType(mimeType);
                AddPart(new InlineData { MimeType = mimeType, Data = base64data });
            }
        }

        /// <summary>
        /// Adds a media file resource to the request.
        /// </summary>
        /// <param name="file">The media file resource.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="file"/> is <see langword="null"/>.</exception>
        /// <exception cref="NotSupportedException">Thrown when the MIME type of <paramref name="file"/>> is not supported by the API.</exception>
        public void AddMedia(FileResource file)
        {
            if (file == null) throw new ArgumentNullException(nameof(file));
            // Strangely, the MIME type is not checked for FileData but InlineData only.
            // GenerativeAIExtensions.GuardMimeType(file.MimeType);

            AddPart(new FileData { FileUri = file.Uri, MimeType = file.MimeType });
        }

        /// <summary>
        /// Adds a <see cref="Part"/> object to the Content at the specified <param name="index"></param>.
        /// </summary>
        /// <param name="part">Part object to add to the <see cref="Contents"/> collection.</param>
        /// <param name="index">Zero-based index of element in the Contents collection.</param>
        public void AddPart(IPart part, int index = 0)
        {
            if (Contents?[index] == null)
                throw new ArgumentNullException(nameof(index),
                    "The contents collection has no item at the specified index.");
            Contents[index].Parts?.Add(part);
        }
    }
}