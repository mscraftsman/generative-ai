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
    /// 
    /// </summary>
    public class GenerateContentRequest
    {
        /// <summary>
        /// Required. The content of the current conversation with the model.
        /// For single-turn queries, this is a single instance. For multi-turn queries, this is a repeated field that contains conversation history + latest request.
        /// </summary>
        public List<Content>? Contents { get; set; }
        /// <summary>
        /// Optional. Configuration options for model generation and outputs.
        /// </summary>
        public GenerationConfig? GenerationConfig { get; set; }
        /// <summary>
        /// Optional. A list of unique SafetySetting instances for blocking unsafe content.
        /// This will be enforced on the GenerateContentRequest.contents and GenerateContentResponse.candidates. There should not be more than one setting for each SafetyCategory type. The API will block any contents and responses that fail to meet the thresholds set by these settings. This list overrides the default settings for each SafetyCategory specified in the safetySettings. If there is no SafetySetting for a given SafetyCategory provided in the list, the API will use the default safety setting for that category. Harm categories HARM_CATEGORY_HATE_SPEECH, HARM_CATEGORY_SEXUALLY_EXPLICIT, HARM_CATEGORY_DANGEROUS_CONTENT, HARM_CATEGORY_HARASSMENT are supported.
        /// </summary>
        public List<SafetySetting>? SafetySettings { get; set; }
        /// <summary>
        /// Optional. A list of Tools the model may use to generate the next response.
        /// A Tool is a piece of code that enables the system to interact with external systems to perform an action, or set of actions, outside of knowledge and scope of the model. The only supported tool is currently Function.
        /// </summary>
        public List<Tool>? Tools { get; set; }

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
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="prompt"/> is <see langword="null"/>.</exception>
        public GenerateContentRequest(string prompt,
            GenerationConfig? generationConfig = null,
            List<SafetySetting>? safetySettings = null,
            List<Tool>? tools = null) : this()
        {
            if (prompt == null) throw new ArgumentNullException(nameof(prompt));

            Contents = new List<Content> { new Content
            {
                Role = Role.User,
                Parts = new List<IPart> { new TextData
                {
                    Text = prompt
                }}
            }};
            if (generationConfig != null) GenerationConfig = generationConfig;
            if (safetySettings != null) SafetySettings = safetySettings;
            if (tools != null) Tools = tools;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenerateContentRequest"/> class.
        /// </summary>
        /// <param name="parts"></param>
        /// <param name="generationConfig">Optional. Configuration options for model generation and outputs.</param>
        /// <param name="safetySettings">Optional. A list of unique SafetySetting instances for blocking unsafe content.</param>
        /// <param name="tools">Optional. A list of Tools the model may use to generate the next response.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="parts"/> is <see langword="null"/>.</exception>
        public GenerateContentRequest(List<IPart> parts,
            GenerationConfig? generationConfig = null,
            List<SafetySetting>? safetySettings = null,
            List<Tool>? tools = null)
        {
            if (parts == null) throw new ArgumentNullException(nameof(parts));

            Contents = new List<Content> { new Content
            {
                Parts = parts
            }};
            if (generationConfig != null) GenerationConfig = generationConfig;
            if (safetySettings != null) SafetySettings = safetySettings;
            if (tools != null) Tools = tools;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="GenerateContentRequest"/> class.
        /// </summary>
        /// <param name="file">The media file resource.</param>
        /// <param name="generationConfig">Optional. Configuration options for model generation and outputs.</param>
        /// <param name="safetySettings">Optional. A list of unique SafetySetting instances for blocking unsafe content.</param>
        /// <param name="tools">Optional. A list of Tools the model may use to generate the next response.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="file"/> is <see langword="null"/>.</exception>
        public GenerateContentRequest(FileResource file,
            GenerationConfig? generationConfig = null,
            List<SafetySetting>? safetySettings = null,
            List<Tool>? tools = null) : this()
        {
            if (file == null) throw new ArgumentNullException(nameof(file));

            Contents = new List<Content> { new Content
            {
                Role = Role.User,
                Parts = new List<IPart> { new FileData { FileUri = file.Uri, MimeType = file.MimeType } }
            }};
            if (generationConfig != null) GenerationConfig = generationConfig;
            if (safetySettings != null) SafetySettings = safetySettings;
            if (tools != null) Tools = tools;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenerateContentRequest"/> class.
        /// </summary>
        /// <param name="parts"></param>
        /// <param name="generationConfig">Optional. Configuration options for model generation and outputs.</param>
        /// <param name="safetySettings">Optional. A list of unique SafetySetting instances for blocking unsafe content.</param>
        /// <param name="tools">Optional. A list of Tools the model may use to generate the next response.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="parts"/> is <see langword="null"/>.</exception>
        public GenerateContentRequest(List<Part> parts,
            GenerationConfig? generationConfig = null,
            List<SafetySetting>? safetySettings = null,
            List<Tool>? tools = null)
        {
            if (parts == null) throw new ArgumentNullException(nameof(parts));

            Contents = new List<Content> { new Content
            {
                Parts = parts.Select(p => (IPart)p).ToList()
            }};
            if (generationConfig != null) GenerationConfig = generationConfig;
            if (safetySettings != null) SafetySettings = safetySettings;
            if (tools != null) Tools = tools;
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
        /// Adds a media file to the request.
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

            string base64data;
            mimeType ??= GenerativeAIExtensions.GetMimeType(uri);
            // Strangely, the MIME type is not checked for FileData but InlineData only.
            // GenerativeAIExtensions.GuardMimeType(mimeType);
            
            if (useOnline)
            {
                Contents[0].Parts.Add(new FileData
                {
                    FileUri = uri,
                    MimeType = mimeType
                });
            }
            
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
                base64data =  await GenerativeAIExtensions.ReadImageFileBase64Async(uri);
            }

            GenerativeAIExtensions.GuardMimeType(mimeType);
            Contents[0].Parts.Add(
                new InlineData { MimeType = mimeType, Data = base64data }
            );
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

            Contents[0].Parts.Add(
                new FileData { FileUri = file.Uri, MimeType = file.MimeType }
            );
        }
    }
}