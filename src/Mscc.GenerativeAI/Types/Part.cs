﻿using System.Diagnostics;
using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// A part of a turn in a conversation with the model with a fixed MIME type. 
    /// It has one of the following mutually exclusive fields: 
    ///     1. text 
    ///     2. inline_data 
    ///     3. file_data 
    ///     4. functionResponse 
    ///     5. functionCall
    /// </summary>
    [DebuggerDisplay("{Text}")]
    public class Part
    {
        /// <summary>
        /// A text part of a conversation with the model.
        /// </summary>
        public string Text {
            get { return TextData?.Text; }
            set { TextData = new TextData { Text = value }; } 
        }
        /// <remarks/>
        internal TextData TextData { get; set; }
        /// <summary>
        /// Raw media bytes sent directly in the request. 
        /// </summary>
        [JsonPropertyName("inline_data")]
        public InlineData InlineData { get; set; }
        /// <summary>
        /// URI based data.
        /// </summary>
        [JsonPropertyName("file_data")]
        public FileData FileData { get; set; }
        /// <summary>
        /// The result output of a FunctionCall.
        /// </summary>
        public FunctionResponse FunctionResponse { get; set; }
        /// <summary>
        /// A predicted FunctionCall returned from the model.
        /// </summary>
        public FunctionCall FunctionCall { get; set; }

        public FileData FromUri(string uri, string mimetype)
        {
            return new FileData { FileUri = uri, MimeType = mimetype };
        }
    }
}