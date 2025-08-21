#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Request to generate a completion from the model.
    /// </summary>
    public class GenerateContentRequest
    {
        /// <summary>
        /// Required. The name of the Model to use for generating the completion.
        /// Format: models/{model}.
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// Required. The content of the current conversation with the model.
        /// For single-turn queries, this is a single instance. For multi-turn queries, this is a repeated field that contains conversation history + latest request.
        /// </summary>
        public List<Content> Contents { get; set; }

        /// <summary>
        /// Optional. Configuration options for model generation and outputs.
        /// </summary>
        public GenerationConfig? GenerationConfig { get; set; }

        /// <summary>
        /// Optional. A list of unique `<see cref="SafetySetting"/>` instances for blocking unsafe content.
        /// </summary>
        /// <remarks>
        /// This will be enforced on the `GenerateContentRequest.contents` and `GenerateContentResponse.candidates`.
        /// There should not be more than one setting for each `SafetyCategory` type.
        /// The API will block any contents and responses that fail to meet the thresholds set by these settings.
        /// This list overrides the default settings for each `SafetyCategory` specified in the safety_settings.
        /// If there is no `SafetySetting` for a given `SafetyCategory` provided in the list, the API will use the
        /// default safety setting for that category.
        /// Harm categories HARM_CATEGORY_HATE_SPEECH, HARM_CATEGORY_SEXUALLY_EXPLICIT, HARM_CATEGORY_DANGEROUS_CONTENT, HARM_CATEGORY_HARASSMENT, HARM_CATEGORY_CIVIC_INTEGRITY are supported.
        /// Refer to the [guide](https://ai.google.dev/gemini-api/docs/safety-settings) for detailed information on
        /// available safety settings. Also refer to the [Safety guidance](https://ai.google.dev/gemini-api/docs/safety-guidance)
        /// to learn how to incorporate safety considerations in your AI applications.
        /// </remarks>
        public List<SafetySetting>? SafetySettings { get; set; }

        /// <summary>
        /// Optional. Available for gemini-1.5-pro and gemini-1.0-pro-002.
        /// Instructions for the model to steer it toward better performance. For example, "Answer as concisely as possible" or "Don't use technical terms in your response".
        /// The text strings count toward the token limit.
        /// The role field of systemInstruction is ignored and doesn't affect the performance of the model. 
        /// </summary>
        /// <remarks>
        /// Note: only text should be used in parts and content in each part will be in a separate paragraph.
        /// </remarks>
        public Content? SystemInstruction { get; set; }

        /// <summary>
        /// Optional. Configuration of tools used by the model.
        /// </summary>
        public ToolConfig? ToolConfig { get; set; }

        /// <summary>
        /// Optional. A list of Tools the model may use to generate the next response.
        /// A <see cref="Tool"/> is a piece of code that enables the system to interact with external systems to perform an action, or set of actions, outside of knowledge and scope of the model. The only supported tool is currently Function.
        /// </summary>
        public Tools? Tools { get; set; }
        /// <summary>
        /// Optional. The name of the content cached to use as context to serve the prediction.
        /// Format: cachedContents/{cachedContent}
        /// </summary>
        public string? CachedContent { get; set; }

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

            Contents = new List<Content>
            {
                new Content { Role = Role.User, Parts = new List<IPart> { new TextData { Text = prompt } } }
            };
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

            Contents = new List<Content> { new Content { Parts = parts } };
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

            Contents = new List<Content>
            {
                new Content
                {
                    Role = Role.User,
                    Parts = new List<IPart> { new FileData { FileUri = file.Uri, MimeType = file.MimeType } }
                }
            };
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

            Contents = new List<Content> { new Content { PartTypes = parts } };
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

            if (useOnline)
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
            if (Contents[index] == null)
                throw new ArgumentNullException(nameof(index),
                    "The contents collection has no item at the specified index.");
            Contents[index].Parts?.Add(part);
        }
    }
}