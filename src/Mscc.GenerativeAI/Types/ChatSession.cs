#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
#endif

namespace Mscc.GenerativeAI
{
    public class ChatSession
    {
        private readonly GenerativeModel _model;
        // [JsonPropertyName("generation_config")]
        private readonly GenerationConfig? _generationConfig;
        // [JsonPropertyName("safety_settings")]
        private readonly List<SafetySetting>? _safetySettings;
        private readonly List<Tool>? _tools;

        public List<ContentResponse> History { get; set; }

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
            _model = model;
            History = history ?? [];
            _generationConfig = generationConfig;
            _safetySettings = safetySettings;
            _tools = tools;
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

            History.Add(new ContentResponse { Role = Role.User, Parts = new List<Part> { new() { Text = prompt } } });
            var request = new GenerateContentRequest
            {
                Contents = History.Select(x => 
                    new Content { Role = x.Role, PartTypes = x.Parts }).ToList(),
                GenerationConfig = _generationConfig,
                SafetySettings = _safetySettings,
                Tools = _tools
            };

            // ToDo: ?? => refactor to stream response.
            var response = await _model.GenerateContent(request);
            History.Add(new ContentResponse { Role = Role.Model, Parts = new List<Part> { new() { Text = response.Text } } });

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

            History.Add(new ContentResponse { Role = Role.User, Parts = new List<Part> { new Part { Text = prompt } } });
            var request = new GenerateContentRequest
            {
                Contents = History.Select(x => 
                    new Content { Role = x.Role, PartTypes = x.Parts }).ToList(),
                GenerationConfig = _generationConfig,
                SafetySettings = _safetySettings,
                Tools = _tools
            };

            // ToDo: ?? => refactor to stream response.
            // var response = await model.GenerateContentStream(request);
            // await foreach (var item in response)
            // {
            //     
            // }
            var stream = await _model.GenerateContentStream(request);
            var response = await JsonSerializer.DeserializeAsync<GenerateContentResponse>(stream, _model.DefaultJsonSerializerOptions());
            History.Add(new ContentResponse { Role = Role.Model, Parts = new List<Part> { new Part { Text = response.Text } } });
            return stream;
        }
    }
}