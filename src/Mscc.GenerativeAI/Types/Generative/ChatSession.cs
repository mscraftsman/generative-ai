#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
#endif
using System.Runtime.CompilerServices;
using System.Text;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Contains an ongoing conversation with the model.
    /// </summary>
    /// <remarks>
    /// This ChatSession object collects the messages sent and received, in its ChatSession.History attribute.
    /// </remarks>
    public class ChatSession
    {
        private readonly GenerativeModel _model;
        private readonly GenerationConfig? _generationConfig;
        private readonly List<SafetySetting>? _safetySettings;
        private readonly List<Tool>? _tools;
        private readonly bool _enableAutomaticFunctionCalling;
        private List<ContentResponse> _history;
        private ContentResponse? _lastSent;
        private ContentResponse? _lastReceived;

        /// <summary>
        /// The chat history.
        /// </summary>
        public List<ContentResponse> History
        {
            get => _history;
            set
            {
                _history = value;
                _lastSent = null;
                _lastReceived = null;
            }
        }

        /// <summary>
        /// Returns the last received ContentResponse
        /// </summary>
        public ContentResponse? Last => _lastReceived;

        /// <summary>
        /// Constructor to start a chat session with history.
        /// </summary>
        /// <param name="model">The model to use in the chat.</param>
        /// <param name="history">A chat history to initialize the session with.</param>
        /// <param name="generationConfig">Optional. Configuration options for model generation and outputs.</param>
        /// <param name="safetySettings">Optional. A list of unique SafetySetting instances for blocking unsafe content.</param>
        /// <param name="tools">Optional. A list of Tools the model may use to generate the next response.</param>
        /// <param name="enableAutomaticFunctionCalling">Optional. </param>
        public ChatSession(GenerativeModel model,
            List<ContentResponse>? history = null,
            GenerationConfig? generationConfig = null,
            List<SafetySetting>? safetySettings = null,
            List<Tool>? tools = null,
            bool enableAutomaticFunctionCalling = false)
        {
            _model = model;
            History = history ?? new List<ContentResponse>();
            _generationConfig = generationConfig;
            _safetySettings = safetySettings;
            _tools = tools;
            _enableAutomaticFunctionCalling = enableAutomaticFunctionCalling;
        }

        /// <summary>
        /// Sends the conversation history with the added message and returns the model's response.
        /// Appends the request and response to the conversation history.
        /// </summary>
        /// <param name="request">The content request.</param>
        /// <param name="generationConfig">Optional. Overrides for the model's generation config.</param>
        /// <param name="safetySettings">Optional. Overrides for the model's safety settings.</param>
        /// <param name="tools">Optional. Overrides for the list of tools the model may use to generate the next response.</param>
        /// <param name="toolConfig">Optional. Overrides for the configuration of tools.</param>
        /// <returns>The model's response.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="request"/> is <see langword="null"/>.</exception>
        /// <exception cref="BlockedPromptException">Thrown when the model's response is blocked by a reason.</exception>
        /// <exception cref="StopCandidateException">Thrown when the model's response is stopped by the model's safety settings.</exception>
        /// <exception cref="ValueErrorException">Thrown when the candidate count is larger than 1.</exception>
        public async Task<GenerateContentResponse> SendMessage(GenerateContentRequest request,
            GenerationConfig? generationConfig = null,
            List<SafetySetting>? safetySettings = null,
            List<Tool>? tools = null,
            ToolConfig? toolConfig = null)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            // ThrowIfUnsupportedRequest<GenerateContentRequest>(request);

            generationConfig ??= _generationConfig;
            if (generationConfig?.CandidateCount > 1)
                throw new ValueErrorException("Can't chat with `CandidateCount > 1`");
            
            _lastSent = new ContentResponse { Role = Role.User, Parts = request.Contents[0].PartTypes! };
            History.Add(_lastSent);

            request.Contents = History.Select(x =>
                new Content { Role = x.Role, PartTypes = x.Parts }
            ).ToList();
            request.GenerationConfig ??= generationConfig;
            request.SafetySettings ??= safetySettings ?? _safetySettings;
            request.Tools ??= tools ?? _tools;
            request.ToolConfig ??= toolConfig;

            var response = await _model.GenerateContent(request);
            
            response.CheckResponse();

            if (_enableAutomaticFunctionCalling)
            {
                var result = HandleAutomaticFunctionCalling(response,
                    History,
                    generationConfig ?? _generationConfig,
                    safetySettings ?? _safetySettings,
                    _tools);
            }
            
            _lastReceived = new() { Role = Role.Model, Text = response.Text ?? string.Empty };
            History.Add(_lastReceived);
            return response;
        }

        /// <summary>
        /// Sends the conversation history with the added message and returns the model's response.
        /// Appends the request and response to the conversation history.
        /// </summary>
        /// <param name="prompt">The message or content sent.</param>
        /// <param name="generationConfig">Optional. Overrides for the model's generation config.</param>
        /// <param name="safetySettings">Optional. Overrides for the model's safety settings.</param>
        /// <param name="tools">Optional. Overrides for the list of tools the model may use to generate the next response.</param>
        /// <param name="toolConfig">Optional. Overrides for the configuration of tools.</param>
        /// <returns>The model's response.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="prompt"/> is <see langword="null"/>.</exception>
        public async Task<GenerateContentResponse> SendMessage(string prompt,
            GenerationConfig? generationConfig = null,
            List<SafetySetting>? safetySettings = null,
            List<Tool>? tools = null,
            ToolConfig? toolConfig = null)
        {
            if (prompt == null) throw new ArgumentNullException(nameof(prompt));

            var request = new GenerateContentRequest(prompt, 
                generationConfig ?? _generationConfig, 
                safetySettings ?? _safetySettings, 
                tools ?? _tools,
                toolConfig: toolConfig);
            return await SendMessage(request);
        }

        /// <summary>
        /// Sends the conversation history with the added message and returns the model's response.
        /// Appends the request and response to the conversation history.
        /// </summary>
        /// <param name="parts">The list of content parts sent.</param>
        /// <param name="generationConfig">Optional. Overrides for the model's generation config.</param>
        /// <param name="safetySettings">Optional. Overrides for the model's safety settings.</param>
        /// <param name="tools">Optional. Overrides for the list of tools the model may use to generate the next response.</param>
        /// <param name="toolConfig">Optional. Overrides for the configuration of tools.</param>
        /// <returns>The model's response.</returns>
        /// <exception cref="ValueErrorException">Thrown when the candidate count is larger than 1.</exception>
        public async Task<GenerateContentResponse> SendMessage(List<Part> parts,
            GenerationConfig? generationConfig = null,
            List<SafetySetting>? safetySettings = null,
            List<Tool>? tools = null,
            ToolConfig? toolConfig = null)
        {
            if (parts == null) throw new ArgumentNullException(nameof(parts));
            
            var request = new GenerateContentRequest(parts, 
                generationConfig ?? _generationConfig, 
                safetySettings ?? _safetySettings, 
                tools ?? _tools,
                toolConfig: toolConfig);
            return await SendMessage(request);
        }

        [Obsolete("This method has been replaced by strong-typed overloads and will be removed soon.")]
        public async Task<GenerateContentResponse> SendMessage(object content,
            GenerationConfig? generationConfig = null,
            List<SafetySetting>? safetySettings = null,
            List<Tool>? tools = null,
            ToolConfig? toolConfig = null)
        {
            if (content == null) throw new ArgumentNullException(nameof(content));
            if (!(content is String || content is List<Part>)) throw new ArgumentException(content.ToString(), nameof(content));

            generationConfig ??= _generationConfig;
            if (generationConfig?.CandidateCount > 1)
                throw new ValueErrorException("Can't chat with `CandidateCount > 1`");

            var role = Role.User;
            var parts = new List<Part>();
            if (content is String prompt)
            {
                parts.Add(new Part { Text = prompt });
            }
            if (content is List<Part> contentParts)
            {
                // role = Role.Function;
                parts = contentParts;
            }

            _lastSent = new ContentResponse { Role = role, Parts = parts };
            History.Add(_lastSent);
            
            var request = new GenerateContentRequest
            {
                Contents = History.Select(x =>
                    new Content { Role = x.Role, PartTypes = x.Parts }
                ).ToList(),
                GenerationConfig = generationConfig ?? _generationConfig,
                SafetySettings = safetySettings ?? _safetySettings,
                Tools = tools ?? _tools,
                ToolConfig = toolConfig
            };

            var response = await _model.GenerateContent(request);
            
            response.CheckResponse();

            if (_enableAutomaticFunctionCalling)
            {
                var result = HandleAutomaticFunctionCalling(response,
                    History,
                    generationConfig ?? _generationConfig,
                    safetySettings ?? _safetySettings,
                    _tools);
            }
            
            _lastReceived = new() { Role = Role.Model, Text = response.Text ?? string.Empty };
            History.Add(_lastReceived);
            return response;
        }

        /// <summary>
        /// Sends the conversation history with the added message and returns the model's response.
        /// </summary>
        /// <remarks>Appends the request and response to the conversation history.</remarks>
        /// <param name="request">The content request.</param>
        /// <param name="generationConfig">Optional. Overrides for the model's generation config.</param>
        /// <param name="safetySettings">Optional. Overrides for the model's safety settings.</param>
        /// <param name="tools">Optional. Overrides for the list of tools the model may use to generate the next response.</param>
        /// <param name="toolConfig">Optional. Overrides for the configuration of tools.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The model's response.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="request"/> is <see langword="null"/></exception>
        /// <exception cref="BlockedPromptException">Thrown when the <paramref name="request"/> is blocked by a reason.</exception>
        /// <exception cref="ValueErrorException">Thrown when the candidate count is larger than 1.</exception>
        public async IAsyncEnumerable<GenerateContentResponse> SendMessageStream(GenerateContentRequest request,
            GenerationConfig? generationConfig = null,
            List<SafetySetting>? safetySettings = null,
            List<Tool>? tools = null,
            ToolConfig? toolConfig = null, 
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            // ThrowIfUnsupportedRequest<GenerateContentRequest>(request);

            generationConfig ??= _generationConfig;
            if (generationConfig?.CandidateCount > 1)
                throw new ValueErrorException("Can't chat with `CandidateCount > 1`");
            
            _lastSent = new ContentResponse { Role = Role.User, Parts = request.Contents[0].PartTypes! };
            History.Add(_lastSent);

            request.Contents = History.Select(x =>
                new Content { Role = x.Role, PartTypes = x.Parts }
            ).ToList();
            request.GenerationConfig ??= generationConfig;
            request.SafetySettings ??= safetySettings ?? _safetySettings;
            request.Tools ??= tools ?? _tools;
            request.ToolConfig ??= toolConfig;

            var fullText = new StringBuilder();
            var response = _model.GenerateContentStream(request, cancellationToken:cancellationToken);
            await foreach (var item in response)
            {
                item.CheckResponse(true);

                if (cancellationToken.IsCancellationRequested)
                    yield break;

                fullText.Append(item.Text);
                yield return item;
            }
            _lastReceived = new() { Role = Role.Model, Text = fullText.ToString() };
            History.Add(_lastReceived);
        }

        /// <summary>
        /// Sends the conversation history with the added message and returns the model's response.
        /// Appends the request and response to the conversation history.
        /// </summary>
        /// <param name="prompt">The message sent.</param>
        /// <param name="generationConfig">Optional. Overrides for the model's generation config.</param>
        /// <param name="safetySettings">Optional. Overrides for the model's safety settings.</param>
        /// <param name="tools">Optional. Overrides for the list of tools the model may use to generate the next response.</param>
        /// <param name="toolConfig">Optional. Overrides for the configuration of tools.</param>
        /// <returns>The model's response.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="prompt"/> is <see langword="null"/>.</exception>
        public async IAsyncEnumerable<GenerateContentResponse> SendMessageStream(string prompt,
            GenerationConfig? generationConfig = null,
            List<SafetySetting>? safetySettings = null,
            List<Tool>? tools = null,
            ToolConfig? toolConfig = null)
        {
            if (prompt == null) throw new ArgumentNullException(nameof(prompt));

            var request = new GenerateContentRequest(prompt, 
                generationConfig ?? _generationConfig, 
                safetySettings ?? _safetySettings, 
                tools ?? _tools,
                toolConfig: toolConfig);
            await foreach (var response in SendMessageStream(request))
            {
                yield return response;
            }
        }

        /// <summary>
        /// Sends the conversation history with the added message and returns the model's response.
        /// Appends the request and response to the conversation history.
        /// </summary>
        /// <param name="parts">The list of content parts sent.</param>
        /// <param name="generationConfig">Optional. Overrides for the model's generation config.</param>
        /// <param name="safetySettings">Optional. Overrides for the model's safety settings.</param>
        /// <param name="tools">Optional. Overrides for the list of tools the model may use to generate the next response.</param>
        /// <param name="toolConfig">Optional. Overrides for the configuration of tools.</param>
        /// <returns>The model's response.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parts"/> is <see langword="null"/>.</exception>
        public async IAsyncEnumerable<GenerateContentResponse> SendMessageStream(List<Part> parts,
            GenerationConfig? generationConfig = null,
            List<SafetySetting>? safetySettings = null,
            List<Tool>? tools = null,
            ToolConfig? toolConfig = null)
        {
            if (parts == null) throw new ArgumentNullException(nameof(parts));

            var request = new GenerateContentRequest(parts, 
                generationConfig ?? _generationConfig, 
                safetySettings ?? _safetySettings, 
                tools ?? _tools,
                toolConfig: toolConfig);
            await foreach (var response in SendMessageStream(request))
            {
                yield return response;
            }
        }

        [Obsolete("This method has been replaced by strong-typed overloads and will be removed soon.")]
        public async IAsyncEnumerable<GenerateContentResponse> SendMessageStream(object content,
            GenerationConfig? generationConfig = null,
            List<SafetySetting>? safetySettings = null,
            List<Tool>? tools = null,
            ToolConfig? toolConfig = null, 
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            if (content == null) throw new ArgumentNullException(nameof(content));
            if (!(content is String || content is List<Part>)) throw new ArgumentException(content.ToString(), nameof(content));

            generationConfig ??= _generationConfig;
            if (generationConfig?.CandidateCount > 1)
                throw new ValueErrorException("Can't chat with `CandidateCount > 1`");

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

            _lastSent = new ContentResponse { Role = role, Parts = parts };
            History.Add(_lastSent);
            var request = new GenerateContentRequest
            {
                Contents = History.Select(x => 
                    new Content { Role = x.Role, PartTypes = x.Parts }
                ).ToList(),
                GenerationConfig = generationConfig ?? _generationConfig,
                SafetySettings = safetySettings ?? _safetySettings,
                Tools = tools ?? _tools,
                ToolConfig = toolConfig
            };

            var fullText = new StringBuilder();
            var responses = _model.GenerateContentStream(request, cancellationToken:cancellationToken);
            await foreach (var response in responses)
            {
                response.CheckResponse(true);

                if (cancellationToken.IsCancellationRequested)
                    yield break;

                fullText.Append(response.Text);
                yield return response;
            }
            _lastReceived = new() { Role = Role.Model, Text = fullText.ToString() };
            History.Add(_lastReceived);
        }

        /// <summary>
        /// Removes the last request/response pair from the chat history.
        /// </summary>
        /// <returns>Tuple with the last request/response pair.</returns>
        public (ContentResponse? Sent, ContentResponse? Received) Rewind()
        {
            (ContentResponse? Sent, ContentResponse? Received) result;
            var position = History.Count - 2;

            if (_lastReceived is null)
            {
                var entries = History.GetRange(position, 2);
                result = (entries.FirstOrDefault(), entries.LastOrDefault());
            }
            else
            {
                result = (_lastSent, _lastReceived);
                _lastSent = null;
                _lastReceived = null;
            }

            History.RemoveRange(position, 2);
            return result;
        }

        private (List<ContentResponse> history, ContentResponse content, GenerateContentResponse response) HandleAutomaticFunctionCalling(
            GenerateContentResponse response, 
            List<ContentResponse> history, 
            GenerationConfig? generationConfig, 
            List<SafetySetting>? safetySettings, 
            List<Tool>? tools)
        {
            throw new NotImplementedException();
            // var functionResponseParts = new List<Part>();
            // var functionCalls = GetFunctionCalls(response);
            // history.Add(response.Candidates[0].Content);
            //
            // foreach (var functionCall in functionCalls)
            // {
            //     var functionResponse = tools.Find(x =>
            //         x.FunctionDeclarations.Where(fd => fd.Name == functionCall.Name).Any());
            //     if (functionResponse is not null)
            //     {
            //         // functionResponseParts.Add(functionResponse);
            //     }
            // }
            //
            // var send = new ContentResponse() { Role = Role.User, Parts = functionResponseParts };
            // history.Add(send);
            //
            // var request = new GenerateContentRequest
            // {
            //     Contents = history.Select(x =>
            //         new Content { Role = x.Role, PartTypes = x.Parts }
            //     ).ToList(),
            //     GenerationConfig = generationConfig,
            //     SafetySettings = safetySettings,
            //     Tools = tools
            // };
            // response = _model.GenerateContent(request).Result;
            // response.CheckResponse();
            //
            // return (history, send, response);
        }

        private List<FunctionCall> GetFunctionCalls(GenerateContentResponse response)
        {
            if (response.Candidates.Count != 1)
            {
                throw new ValueErrorException(
                    $"Automatic function calling only works with 1 candidate, got {response.Candidates.Count}");
            }
            
            var parts = response.Candidates[0].Content.Parts;
            var functionCalls = parts
                .Where(x => x.FunctionCall != null)
                .Select(x => x.FunctionCall)
                .ToList();
            return functionCalls;
        }
    }
}