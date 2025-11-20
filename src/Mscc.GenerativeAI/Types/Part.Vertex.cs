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
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// A datatype containing media that is part of a multi-part Content message.
    /// A part of a turn in a conversation with the model with a fixed MIME type. 
    /// It has one of the following mutually exclusive fields: 
    ///     1. text 
    ///     2. inline_data 
    ///     3. file_data 
    ///     4. functionResponse 
    ///     5. functionCall
    /// </summary>
    /// <remarks>
    /// Exactly one field within a Part should be set,
    /// representing the specific type of content being conveyed. Using multiple fields within the
    /// same `Part` instance is considered invalid.
    /// </remarks>
    [DebuggerDisplay("{Text}")]
    public partial class Part
    {
	    /// <summary>
	    /// Inline media bytes.
	    /// </summary>
	    public InlineData? InlineData { get; set; }

	    public Part() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        public Part(string text)
        {
            Text = text;
        }

        public Part(FileData fileData) : this()
        {
            FileData = fileData;
        }

        /// <summary>
		/// Inline text.
        /// The text content of the part.
        /// </summary>
        public string? Text
        {
            get
            {
                var value = TextData?.Text;
                if (string.IsNullOrEmpty(value)) { value = ExecutableCode?.Code;}
                if (string.IsNullOrEmpty(value)) { value = CodeExecutionResult?.Output;}
                return value;
            }
            set => TextData = new TextData { Text = value };
        }

        /// <remarks/>
        [DebuggerHidden]
        [JsonIgnore]
        internal TextData? TextData { get; set; }
        
        /// <summary>
        /// Media resolution for the input media.
        /// </summary>
        public PartMediaResolution? MediaResolution { get; set; }

        /// <summary>
        /// The ETag of the item.
        /// </summary>
        public virtual string? ETag { get; set; }

        // ToDo: Overloads for byte[] and Stream.
        public static InlineData FromBytes(string value, string mimeType)
        {
            if (string.IsNullOrEmpty(value)) return new InlineData();

            return new InlineData() { Data = value, MimeType = mimeType };
        }

        public static CodeExecutionResult FromCodeExecutionResult(Outcome outcome, string output)
        {
            return new CodeExecutionResult() { Outcome = outcome, Output = output };
        }

        public static ExecutableCode FromExecutableCode(string code, Language language)
        {
            if (string.IsNullOrEmpty(code)) return new ExecutableCode();

            return new ExecutableCode() { Code = code, Language = language };
        }

        public static FunctionCall FromFunctionCall(string name, string[] args)
        {
            if (string.IsNullOrEmpty(name)) return new FunctionCall();

            return new FunctionCall() { Name = name, Args = args };
        }

        public static FunctionResponse FromFunctionResponse(string name, dynamic response)
        {
            if (string.IsNullOrEmpty(name)) return new FunctionResponse();

            return new FunctionResponse() { Name = name, Response = response };
        }

        public static TextData FromText(string value)
        {
            if (string.IsNullOrEmpty(value)) return new TextData();

            return new TextData() { Text = value };
        }

        public static FileData FromUri(string uri, string mimetype)
        {
            return new FileData { FileUri = uri, MimeType = mimetype };
        }

        public static FileData FromUri(Uri uri, string mimetype)
        {
            if (uri is null) throw new ArgumentNullException(nameof(uri));
            return new FileData { FileUri = uri.AbsoluteUri, MimeType = mimetype };
        }

        public static VideoMetadata FromVideoMetadata(string startOffset, string endOffset, double fps = 1.0f)
        {
            return new VideoMetadata() { StartOffset = startOffset, EndOffset = endOffset, Fps = fps };
        }
    }

    /// <summary>
    /// A datatype containing data that is part of a multi-part `TuningContent` message.
    /// This is a subset of the Part used for model inference, with limited type support.
    /// A `Part` consists of data which has an associated datatype.
    /// A `Part` can only contain one of the accepted types in `Part.data`.
    /// </summary>
    [DebuggerDisplay("{Text}")]
    public class TuningPart : Part
    {
    }
}