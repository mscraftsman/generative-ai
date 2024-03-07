#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
#endif

namespace Mscc.GenerativeAI
{
    public class ChatSession
    {
        private readonly GenerativeModel model;

        public List<ContentResponse> History { get; set; } = [];
        public List<Tool>? Tools { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="history"></param>
        public ChatSession(GenerativeModel model, List<ContentResponse>? history = null)
        {
            this.model = model;
            history ??= [];
            History = history;
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
                Contents = History.Select(x => new Content { Role = x.Role, PartTypes = x.Parts }).ToList()
                //GenerationConfig = model.GenerationConfig,
                //SafetySettings = model.SafetySettings,
                //Tools = model.Tools
            };

            var response = await model.GenerateContent(request);
            History.Add(new ContentResponse { Role = "model", Parts = new List<Part> { new Part { Text = response.Text } } });

            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prompt"></param>
        /// <returns></returns>
        public async Task<List<GenerateContentResponse>> SendMessageStream(string prompt)
        {
            return new List<GenerateContentResponse>();
        }
    }
}