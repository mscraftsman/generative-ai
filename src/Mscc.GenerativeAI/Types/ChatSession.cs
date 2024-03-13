#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
#endif
using System.Runtime.CompilerServices;

namespace Mscc.GenerativeAI
{
    public class ChatSession
    {
        private readonly GenerativeModel _model;
        private readonly GenerationConfig? _generationConfig;
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
            History = history ?? new List<ContentResponse>();
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

            History.Add(new ContentResponse { Role = Role.User, Parts = new List<Part> { new Part { Text = prompt } } });
            var request = new GenerateContentRequest
            {
                Contents = History.Select(x =>
                    new Content { Role = x.Role, PartTypes = x.Parts }
                ).ToList(),
                GenerationConfig = _generationConfig,
                SafetySettings = _safetySettings,
                Tools = _tools
            };

            var response = await _model.GenerateContent(request);
            History.Add(new ContentResponse { Role = Role.Model, Parts = new List<Part> { new Part { Text = response.Text } } });
            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async IAsyncEnumerable<GenerateContentResponse> SendMessageStream(object content, 
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            if (content == null) throw new ArgumentNullException(nameof(content));
            if (!(content is String || content is List<Part>)) throw new ArgumentException(content.ToString(), nameof(content));

            var role = Role.User;
            var parts = new List<Part>();
            if (content is String prompt)
            {
                role = Role.User;
                parts.Add(new Part { Text = prompt });
            }
            if (content is List<Part> contentParts)
            {
                role = Role.Function;
                parts = contentParts;
            }

            History.Add(new ContentResponse { Role = role, Parts = parts });
            var request = new GenerateContentRequest
            {
                Contents = History.Select(x => 
                    new Content { Role = x.Role, PartTypes = x.Parts }
                ).ToList(),
                GenerationConfig = _generationConfig,
                SafetySettings = _safetySettings,
                Tools = _tools
            };

            var response = _model.GenerateContentStream(request, cancellationToken);
            await foreach (var item in response)
            {
                if (cancellationToken.IsCancellationRequested)
                    yield break;
                History.Add(new ContentResponse { Role = Role.Model, Parts = item?.Candidates?[0]?.Content?.Parts });
                yield return item;
            }
        }
    }
}