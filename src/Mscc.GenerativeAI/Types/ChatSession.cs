#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
#endif
using System.IO;

namespace Mscc.GenerativeAI
{
    public class ChatSession
    {
        private readonly GenerativeModel model;
        // [JsonPropertyName("generation_config")]
        public GenerationConfig? generationConfig;
        // [JsonPropertyName("safety_settings")]
        public List<SafetySetting>? safetySettings;
        public List<Tool>? tools;

        public List<ContentResponse> History { get; set; } = [];

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="history"></param>
        public ChatSession(GenerativeModel model,
            List<ContentResponse>? history = null,
            GenerationConfig? generationConfig = null,
            List<SafetySetting>? safetySettings = null,
            List<Tool>? tools = null)
        {
            this.model = model;
            history ??= [];
            History = history;
            this.generationConfig = generationConfig;
            this.safetySettings = safetySettings;
            this.tools = tools;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prompt"></param>
        /// <returns></returns>
        public async Task<GenerateContentResponse> SendMessage(string prompt)
        {
            if (prompt == null) throw new ArgumentNullException(nameof(prompt));
            if (string.IsNullOrEmpty(prompt)) throw new ArgumentException(prompt, nameof(prompt));

            History.Add(new ContentResponse { Role = "user", Parts = new List<Part> { new Part { Text = prompt } } });
            var request = new GenerateContentRequest
            {
                Contents = History.Select(x => new Content { Role = x.Role, PartTypes = x.Parts }).ToList(),
                GenerationConfig = generationConfig,
                SafetySettings = safetySettings,
                Tools = tools
            };

            // ToDo: ?? => refactor to stream response.
            var response = await model.GenerateContent(request);
            History.Add(new ContentResponse { Role = "model", Parts = new List<Part> { new Part { Text = response.Text } } });

            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prompt"></param>
        /// <returns></returns>
        public async Task<Stream> SendMessageStream(string prompt)
        {
            if (prompt == null) throw new ArgumentNullException(nameof(prompt));
            if (string.IsNullOrEmpty(prompt)) throw new ArgumentException(prompt, nameof(prompt));

            History.Add(new ContentResponse { Role = "user", Parts = new List<Part> { new Part { Text = prompt } } });
            var request = new GenerateContentRequest
            {
                Contents = History.Select(x => new Content { Role = x.Role, PartTypes = x.Parts }).ToList(),
                GenerationConfig = generationConfig,
                SafetySettings = safetySettings,
                Tools = tools
            };

            // ToDo: ?? => refactor to stream response.
            // var response = await model.GenerateContentStream(request);
            // await foreach (var item in response)
            // {
            //     
            // }
            var stream = await model.GenerateContentStream(request);
            var response = await JsonSerializer.DeserializeAsync<GenerateContentResponse>(stream, model.DefaultJsonSerializerOptions());
            History.Add(new ContentResponse { Role = "model", Parts = new List<Part> { new Part { Text = response.Text } } });
            return stream;
        }
    }
}