using Microsoft.Extensions.Logging;
using Mscc.GenerativeAI.Types;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// The Gemini Interactions API is an experimental API that allows developers to build generative AI applications using Gemini models.
    /// Gemini is a highly capable multimodal model that can understand and process various types of information, including language, images, audio, video, and code.
    /// The API supports use cases like reasoning across text and images, content generation, dialogue agents, summarization, and classification.
    /// </summary>
    public class InteractionsModel : BaseModel
    {
        internal override string Version => ApiVersion.V1Beta;

        /// <summary>
        /// Initializes a new instance of the <see cref="InteractionsModel"/> class.
        /// </summary>
        public InteractionsModel() : this(httpClientFactory: null, logger: null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InteractionsModel"/> class with a specified <see cref="IHttpClientFactory"/>.
        /// </summary>
        /// <param name="apiClient">The <see cref="IHttpClientFactory"/> to use for making API requests.</param>
        public InteractionsModel(IHttpClientFactory apiClient) : this(httpClientFactory: apiClient, logger: null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InteractionsModel"/> class with optional <see cref="IHttpClientFactory"/> and <see cref="ILogger"/>.
        /// </summary>
        /// <param name="httpClientFactory">Optional. The <see cref="IHttpClientFactory"/> to use for creating HttpClient instances.</param>
        /// <param name="logger">Optional. The <see cref="ILogger"/> instance for logging.</param>
        public InteractionsModel(IHttpClientFactory? httpClientFactory = null, ILogger? logger = null) : base(httpClientFactory, logger) { }

        /// <summary>
        /// Creates a new interaction based on the provided request.
        /// </summary>
        /// <param name="request">The request object containing all parameters for the interaction.</param>
        /// <param name="requestOptions">Optional. Options for configuring the request, such as timeout and retry settings.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>An <see cref="InteractionResource"/> representing the created interaction.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the request is null.</exception>
        public async Task<InteractionResource> Create(InteractionRequest request,
	        RequestOptions? requestOptions = null, 
	        CancellationToken cancellationToken = default)
        {
	        if (request == null) throw new ArgumentNullException(nameof(request));
	        ThrowIfUnsupportedRequest(request);

	        var url = "{BaseUrlGoogleAi}/interactions";
	        return await PostAsync<InteractionRequest, InteractionResource>(request, url, string.Empty, requestOptions, HttpCompletionOption.ResponseContentRead, cancellationToken);
        }

        /// <summary>
        /// Creates a new interaction with the specified parameters.
        /// </summary>
        /// <param name="model">The name of the `Model` to use for the interaction.</param>
        /// <param name="agent">The name of the `Agent` to use for the interaction.</param>
        /// <param name="input">The input string for the interaction.</param>
        /// <param name="systemInstruction">A system instruction to guide the model's behavior.</param>
        /// <param name="tools">A list of tool declarations the model may call.</param>
        /// <param name="responseFormat">Enforces the response to be a JSON object matching a specified schema.</param>
        /// <param name="responseMimeType">The MIME type of the response, required if `responseFormat` is set.</param>
        /// <param name="stream">If true, the interaction will be streamed.</param>
        /// <param name="store">If true, the request and response will be stored for later retrieval.</param>
        /// <param name="background">If true, the interaction will run in the background.</param>
        /// <param name="generationConfig">Configuration for the model's generation process.</param>
        /// <param name="agentConfig">Configuration for the agent.</param>
        /// <param name="previousInteractionId">The ID of the previous interaction in a conversation.</param>
        /// <param name="responseModalities">The requested modalities for the response (e.g., TEXT, IMAGE).</param>
        /// <param name="requestOptions">Optional. Options for configuring the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>An <see cref="InteractionResource"/> representing the created interaction.</returns>
        public async Task<InteractionResource> Create(string? model = null,
	        string? agent = null,
	        string? input = null,
	        string? systemInstruction = null,
	        Tools? tools = null,
	        object? responseFormat = null,
	        string? responseMimeType = null,
	        bool? stream = null,
	        bool? store = null,
	        bool? background = null,
	        GenerationConfig? generationConfig = null,
	        object? agentConfig = null,
	        string? previousInteractionId = null,
	        List<ResponseModality>? responseModalities = null,
	        RequestOptions? requestOptions = null, 
	        CancellationToken cancellationToken = default)
        {
	        var request = new InteractionRequest
	        {
		        InputString = input,
		        Model = model, 
		        Agent = agent,
		        SystemInstruction = systemInstruction,
		        Tools = tools,
		        ResponseFormat = responseFormat,
		        ResponseMimeType = responseMimeType,
		        Store = store,
		        Background = background,
		        GenerationConfig = generationConfig,
		        AgentConfig = agentConfig,
		        PreviousInteractionId = previousInteractionId,
		        ResponseModalities = responseModalities
	        };
	        return await Create(request, requestOptions, cancellationToken);
        }

        /// <summary>
        /// Creates a new interaction with the specified parameters, using structured content as input.
        /// </summary>
        /// <param name="model">The name of the `Model` to use for the interaction.</param>
        /// <param name="agent">The name of the `Agent` to use for the interaction.</param>
        /// <param name="input">The structured content for the interaction.</param>
        /// <param name="systemInstruction">A system instruction to guide the model's behavior.</param>
        /// <param name="tools">A list of tool declarations the model may call.</param>
        /// <param name="responseFormat">Enforces the response to be a JSON object matching a specified schema.</param>
        /// <param name="responseMimeType">The MIME type of the response, required if `responseFormat` is set.</param>
        /// <param name="stream">If true, the interaction will be streamed.</param>
        /// <param name="store">If true, the request and response will be stored for later retrieval.</param>
        /// <param name="background">If true, the interaction will run in the background.</param>
        /// <param name="generationConfig">Configuration for the model's generation process.</param>
        /// <param name="agentConfig">Configuration for the agent.</param>
        /// <param name="previousInteractionId">The ID of the previous interaction in a conversation.</param>
        /// <param name="responseModalities">The requested modalities for the response (e.g., TEXT, IMAGE).</param>
        /// <param name="requestOptions">Optional. Options for configuring the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>An <see cref="InteractionResource"/> representing the created interaction.</returns>
        public async Task<InteractionResource> Create(string? model = null,
	        string? agent = null,
	        InteractionContent? input = null,
	        string? systemInstruction = null,
	        Tools? tools = null,
	        object? responseFormat = null,
	        string? responseMimeType = null,
	        bool? stream = null,
	        bool? store = null,
	        bool? background = null,
	        GenerationConfig? generationConfig = null,
	        object? agentConfig = null,
	        string? previousInteractionId = null,
	        List<ResponseModality>? responseModalities = null,
	        RequestOptions? requestOptions = null,
	        CancellationToken cancellationToken = default)
        {
	        var request = new InteractionRequest
	        {
		        InputContent = input,
		        Model = model, 
		        Agent = agent,
		        SystemInstruction = systemInstruction,
		        Tools = tools,
		        ResponseFormat = responseFormat,
		        ResponseMimeType = responseMimeType,
		        Store = store,
		        Background = background,
		        GenerationConfig = generationConfig,
		        AgentConfig = agentConfig,
		        PreviousInteractionId = previousInteractionId,
		        ResponseModalities = responseModalities
	        };
	        return await Create(request, requestOptions, cancellationToken);
        }

        /// <summary>
        /// Creates a new interaction with a list of content parts as input.
        /// </summary>
        /// <param name="model">The name of the `Model` to use for the interaction.</param>
        /// <param name="agent">The name of the `Agent` to use for the interaction.</param>
        /// <param name="input">A list of content parts for the interaction.</param>
        /// <param name="systemInstruction">A system instruction to guide the model's behavior.</param>
        /// <param name="tools">A list of tool declarations the model may call.</param>
        /// <param name="responseFormat">Enforces the response to be a JSON object matching a specified schema.</param>
        /// <param name="responseMimeType">The MIME type of the response, required if `responseFormat` is set.</param>
        /// <param name="stream">If true, the interaction will be streamed.</param>
        /// <param name="store">If true, the request and response will be stored for later retrieval.</param>
        /// <param name="background">If true, the interaction will run in the background.</param>
        /// <param name="generationConfig">Configuration for the model's generation process.</param>
        /// <param name="agentConfig">Configuration for the agent.</param>
        /// <param name="previousInteractionId">The ID of the previous interaction in a conversation.</param>
        /// <param name="responseModalities">The requested modalities for the response (e.g., TEXT, IMAGE).</param>
        /// <param name="requestOptions">Optional. Options for configuring the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>An <see cref="InteractionResource"/> representing the created interaction.</returns>
        public async Task<InteractionResource> Create(string? model = null,
	        string? agent = null,
	        List<InteractionContent>? input = null,
            string? systemInstruction = null,
	        Tools? tools = null,
	        object? responseFormat = null,
	        string? responseMimeType = null,
            bool? stream = null,
            bool? store = null,
	        bool? background = null,
	        GenerationConfig? generationConfig = null,
	        object? agentConfig = null,
	        string? previousInteractionId = null,
	        List<ResponseModality>? responseModalities = null,
	        RequestOptions? requestOptions = null, 
	        CancellationToken cancellationToken = default)
        {
	        var request = new InteractionRequest
	        {
		        InputListContent = input,
		        Model = model, 
		        Agent = agent,
		        SystemInstruction = systemInstruction,
		        Tools = tools,
		        ResponseFormat = responseFormat,
		        ResponseMimeType = responseMimeType,
		        Store = store,
		        Background = background,
		        GenerationConfig = generationConfig,
		        AgentConfig = agentConfig,
		        PreviousInteractionId = previousInteractionId,
		        ResponseModalities = responseModalities
	        };
	        return await Create(request, requestOptions, cancellationToken);
        }

        /// <summary>
        /// Creates a new interaction with a history of conversation turns as input.
        /// </summary>
        /// <param name="model">The name of the `Model` to use for the interaction.</param>
        /// <param name="agent">The name of the `Agent` to use for the interaction.</param>
        /// <param name="input">A list of conversation turns representing the history.</param>
        /// <param name="systemInstruction">A system instruction to guide the model's behavior.</param>
        /// <param name="tools">A list of tool declarations the model may call.</param>
        /// <param name="responseFormat">Enforces the response to be a JSON object matching a specified schema.</param>
        /// <param name="responseMimeType">The MIME type of the response, required if `responseFormat` is set.</param>
        /// <param name="stream">If true, the interaction will be streamed.</param>
        /// <param name="store">If true, the request and response will be stored for later retrieval.</param>
        /// <param name="background">If true, the interaction will run in the background.</param>
        /// <param name="generationConfig">Configuration for the model's generation process.</param>
        /// <param name="agentConfig">Configuration for the agent.</param>
        /// <param name="previousInteractionId">The ID of the previous interaction in a conversation.</param>
        /// <param name="responseModalities">The requested modalities for the response (e.g., TEXT, IMAGE).</param>
        /// <param name="requestOptions">Optional. Options for configuring the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>An <see cref="InteractionResource"/> representing the created interaction.</returns>
        public async Task<InteractionResource> Create(string? model = null,
	        string? agent = null,
	        List<InteractionTurn>? input = null,
	        string? systemInstruction = null,
	        Tools? tools = null,
	        object? responseFormat = null,
	        string? responseMimeType = null,
	        bool? stream = null,
	        bool? store = null,
	        bool? background = null,
	        GenerationConfig? generationConfig = null,
	        object? agentConfig = null,
	        string? previousInteractionId = null,
	        List<ResponseModality>? responseModalities = null,
	        RequestOptions? requestOptions = null, 
	        CancellationToken cancellationToken = default)
        {
	        var request = new InteractionRequest
	        {
		        InputListTurn = input,
		        Model = model, 
		        Agent = agent,
		        SystemInstruction = systemInstruction,
		        Tools = tools,
		        ResponseFormat = responseFormat,
		        ResponseMimeType = responseMimeType,
		        Store = store,
		        Background = background,
		        GenerationConfig = generationConfig,
		        AgentConfig = agentConfig,
		        PreviousInteractionId = previousInteractionId,
		        ResponseModalities = responseModalities
	        };
	        return await Create(request, requestOptions, cancellationToken);
        }

        /// <summary>
        /// Creates a new interaction and streams the response as it is generated.
        /// </summary>
        /// <param name="request">The request object containing all parameters for the interaction.</param>
        /// <param name="requestOptions">Optional. Options for configuring the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>An asynchronous stream of <see cref="InteractionSseEvent"/> chunks.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the request is null.</exception>
        public async IAsyncEnumerable<InteractionSseEvent> CreateStream(InteractionRequest request,
	        RequestOptions? requestOptions = null, 
	        [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
	        if (request == null) throw new ArgumentNullException(nameof(request));
	        ThrowIfUnsupportedRequest(request);

	        request.Stream = true;

	        var url = "{BaseUrlGoogleAi}/interactions";
	        await foreach (var item in PostStreamAsync<InteractionRequest, InteractionSseEvent>(request, url, string.Empty, requestOptions, cancellationToken))
	        {
		        if (cancellationToken.IsCancellationRequested)
			        yield break;
		        yield return item;
	        }
        }

        /// <summary>
        /// Creates a new interaction and streams the response.
        /// </summary>
        /// <param name="model">The name of the `Model` to use for the interaction.</param>
        /// <param name="agent">The name of the `Agent` to use for the interaction.</param>
        /// <param name="input">The input string for the interaction.</param>
        /// <param name="systemInstruction">A system instruction to guide the model's behavior.</param>
        /// <param name="tools">A list of tool declarations the model may call.</param>
        /// <param name="responseFormat">Enforces the response to be a JSON object matching a specified schema.</param>
        /// <param name="responseMimeType">The MIME type of the response, required if `responseFormat` is set.</param>
        /// <param name="stream">If true, the interaction will be streamed.</param>
        /// <param name="store">If true, the request and response will be stored for later retrieval.</param>
        /// <param name="background">If true, the interaction will run in the background.</param>
        /// <param name="generationConfig">Configuration for the model's generation process.</param>
        /// <param name="agentConfig">Configuration for the agent.</param>
        /// <param name="previousInteractionId">The ID of the previous interaction in a conversation.</param>
        /// <param name="responseModalities">The requested modalities for the response (e.g., TEXT, IMAGE).</param>
        /// <param name="requestOptions">Optional. Options for configuring the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>An asynchronous stream of <see cref="InteractionSseEvent"/> chunks.</returns>
        public async IAsyncEnumerable<InteractionSseEvent> CreateStream(string? model = null,
	        string? agent = null,
	        string? input = null,
	        string? systemInstruction = null,
	        Tools? tools = null,
	        object? responseFormat = null,
	        string? responseMimeType = null,
	        bool? stream = null,
	        bool? store = null,
	        bool? background = null,
	        GenerationConfig? generationConfig = null,
	        object? agentConfig = null,
	        string? previousInteractionId = null,
	        List<ResponseModality>? responseModalities = null,
	        RequestOptions? requestOptions = null, 
	        [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
	        var request = new InteractionRequest
	        {
		        InputString = input,
		        Model = model, 
		        Agent = agent,
		        SystemInstruction = systemInstruction,
		        Tools = tools,
		        ResponseFormat = responseFormat,
		        ResponseMimeType = responseMimeType,
		        Store = store,
		        Background = background,
		        GenerationConfig = generationConfig,
		        AgentConfig = agentConfig,
		        PreviousInteractionId = previousInteractionId,
		        ResponseModalities = responseModalities
	        };

	        await foreach (var item in CreateStream(request, requestOptions, cancellationToken))
	        {
		        if (cancellationToken.IsCancellationRequested)
			        yield break;
		        yield return item;
	        }
        }

        /// <summary>
        /// Creates a new interaction with structured content and streams the response.
        /// </summary>
        /// <param name="model">The name of the `Model` to use for the interaction.</param>
        /// <param name="agent">The name of the `Agent` to use for the interaction.</param>
        /// <param name="input">The structured content for the interaction.</param>
        /// <param name="systemInstruction">A system instruction to guide the model's behavior.</param>
        /// <param name="tools">A list of tool declarations the model may call.</param>
        /// <param name="responseFormat">Enforces the response to be a JSON object matching a specified schema.</param>
        /// <param name="responseMimeType">The MIME type of the response, required if `responseFormat` is set.</param>
        /// <param name="stream">If true, the interaction will be streamed.</param>
        /// <param name="store">If true, the request and response will be stored for later retrieval.</param>
        /// <param name="background">If true, the interaction will run in the background.</param>
        /// <param name="generationConfig">Configuration for the model's generation process.</param>
        /// <param name="agentConfig">Configuration for the agent.</param>
        /// <param name="previousInteractionId">The ID of the previous interaction in a conversation.</param>
        /// <param name="responseModalities">The requested modalities for the response (e.g., TEXT, IMAGE).</param>
        /// <param name="requestOptions">Optional. Options for configuring the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>An asynchronous stream of <see cref="InteractionSseEvent"/> chunks.</returns>
        public async IAsyncEnumerable<InteractionSseEvent> CreateStream(string? model = null,
	        string? agent = null,
	        InteractionContent? input = null,
	        string? systemInstruction = null,
	        Tools? tools = null,
	        object? responseFormat = null,
	        string? responseMimeType = null,
	        bool? stream = null,
	        bool? store = null,
	        bool? background = null,
	        GenerationConfig? generationConfig = null,
	        object? agentConfig = null,
	        string? previousInteractionId = null,
	        List<ResponseModality>? responseModalities = null,
	        RequestOptions? requestOptions = null, 
	        [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
	        var request = new InteractionRequest
	        {
		        InputContent = input,
		        Model = model, 
		        Agent = agent,
		        SystemInstruction = systemInstruction,
		        Tools = tools,
		        ResponseFormat = responseFormat,
		        ResponseMimeType = responseMimeType,
		        Store = store,
		        Background = background,
		        GenerationConfig = generationConfig,
		        AgentConfig = agentConfig,
		        PreviousInteractionId = previousInteractionId,
		        ResponseModalities = responseModalities
	        };

	        await foreach (var item in CreateStream(request, requestOptions, cancellationToken))
	        {
		        if (cancellationToken.IsCancellationRequested)
			        yield break;
		        yield return item;
	        }
        }

        /// <summary>
        /// Creates a new interaction with a list of content parts and streams the response.
        /// </summary>
        /// <param name="model">The name of the `Model` to use for the interaction.</param>
        /// <param name="agent">The name of the `Agent` to use for the interaction.</param>
        /// <param name="input">A list of content parts for the interaction.</param>
        /// <param name="systemInstruction">A system instruction to guide the model's behavior.</param>
        /// <param name="tools">A list of tool declarations the model may call.</param>
        /// <param name="responseFormat">Enforces the response to be a JSON object matching a specified schema.</param>
        /// <param name="responseMimeType">The MIME type of the response, required if `responseFormat` is set.</param>
        /// <param name="stream">If true, the interaction will be streamed.</param>
        /// <param name="store">If true, the request and response will be stored for later retrieval.</param>
        /// <param name="background">If true, the interaction will run in the background.</param>
        /// <param name="generationConfig">Configuration for the model's generation process.</param>
        /// <param name="agentConfig">Configuration for the agent.</param>
        /// <param name="previousInteractionId">The ID of the previous interaction in a conversation.</param>
        /// <param name="responseModalities">The requested modalities for the response (e.g., TEXT, IMAGE).</param>
        /// <param name="requestOptions">Optional. Options for configuring the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>An asynchronous stream of <see cref="InteractionSseEvent"/> chunks.</returns>
        public async IAsyncEnumerable<InteractionSseEvent> CreateStream(string? model = null,
	        string? agent = null,
	        List<InteractionContent>? input = null,
	        string? systemInstruction = null,
	        Tools? tools = null,
	        object? responseFormat = null,
	        string? responseMimeType = null,
	        bool? stream = null,
	        bool? store = null,
	        bool? background = null,
	        GenerationConfig? generationConfig = null,
	        object? agentConfig = null,
	        string? previousInteractionId = null,
	        List<ResponseModality>? responseModalities = null,
	        RequestOptions? requestOptions = null, 
	        [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
	        var request = new InteractionRequest
	        {
		        InputListContent = input,
		        Model = model, 
		        Agent = agent,
		        SystemInstruction = systemInstruction,
		        Tools = tools,
		        ResponseFormat = responseFormat,
		        ResponseMimeType = responseMimeType,
		        Store = store,
		        Background = background,
		        GenerationConfig = generationConfig,
		        AgentConfig = agentConfig,
		        PreviousInteractionId = previousInteractionId,
		        ResponseModalities = responseModalities
	        };

	        await foreach (var item in CreateStream(request, requestOptions, cancellationToken))
	        {
		        if (cancellationToken.IsCancellationRequested)
			        yield break;
		        yield return item;
	        }
        }

        /// <summary>
        /// Creates a new interaction with a history of conversation turns and streams the response.
        /// </summary>
        /// <param name="model">The name of the `Model` to use for the interaction.</param>
        /// <param name="agent">The name of the `Agent` to use for the interaction.</param>
        /// <param name="input">A list of conversation turns representing the history.</param>
        /// <param name="systemInstruction">A system instruction to guide the model's behavior.</param>
        /// <param name="tools">A list of tool declarations the model may call.</param>
        /// <param name="responseFormat">Enforces the response to be a JSON object matching a specified schema.</param>
        /// <param name="responseMimeType">The MIME type of the response, required if `responseFormat` is set.</param>
        /// <param name="stream">If true, the interaction will be streamed.</param>
        /// <param name="store">If true, the request and response will be stored for later retrieval.</param>
        /// <param name="background">If true, the interaction will run in the background.</param>
        /// <param name="generationConfig">Configuration for the model's generation process.</param>
        /// <param name="agentConfig">Configuration for the agent.</param>
        /// <param name="previousInteractionId">The ID of the previous interaction in a conversation.</param>
        /// <param name="responseModalities">The requested modalities for the response (e.g., TEXT, IMAGE).</param>
        /// <param name="requestOptions">Optional. Options for configuring the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>An asynchronous stream of <see cref="InteractionSseEvent"/> chunks.</returns>
        public async IAsyncEnumerable<InteractionSseEvent> CreateStream(string? model = null,
	        string? agent = null,
	        List<InteractionTurn>? input = null,
	        string? systemInstruction = null,
	        Tools? tools = null,
	        object? responseFormat = null,
	        string? responseMimeType = null,
	        bool? stream = null,
	        bool? store = null,
	        bool? background = null,
	        GenerationConfig? generationConfig = null,
	        object? agentConfig = null,
	        string? previousInteractionId = null,
	        List<ResponseModality>? responseModalities = null,
	        RequestOptions? requestOptions = null, 
	        [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
	        var request = new InteractionRequest
	        {
		        InputListTurn = input,
		        Model = model, 
		        Agent = agent,
		        SystemInstruction = systemInstruction,
		        Tools = tools,
		        ResponseFormat = responseFormat,
		        ResponseMimeType = responseMimeType,
		        Store = store,
		        Background = background,
		        GenerationConfig = generationConfig,
		        AgentConfig = agentConfig,
		        PreviousInteractionId = previousInteractionId,
		        ResponseModalities = responseModalities
	        };

	        await foreach (var item in CreateStream(request, requestOptions, cancellationToken))
	        {
		        if (cancellationToken.IsCancellationRequested)
			        yield break;
		        yield return item;
	        }
        }

        /// <summary>
        /// Retrieves the full details of a single interaction by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the interaction to retrieve.</param>
        /// <param name="stream">If true, the generated content will be streamed incrementally.</param>
        /// <param name="lastEventId">Optional. If set, resumes the stream from the event after the specified ID. Requires `stream` to be true.</param>
        /// <param name="apiVersion">The API version to use for the request.</param>
        /// <param name="requestOptions">Optional. Options for configuring the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>An <see cref="InteractionResource"/> with the details of the interaction.</returns>
        public async Task<InteractionResource> Get(string id,
	        bool? stream = false,
	        string? lastEventId = null,
	        string? apiVersion = null,
	        RequestOptions? requestOptions = null, 
	        CancellationToken cancellationToken = default)
        {
	        var url = $"{BaseUrlGoogleAi}/interactions/{id}";
	        url = ParseUrl(url);
	        using var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);
	        var response = await SendAsync(httpRequest, requestOptions, cancellationToken);
	        await response.EnsureSuccessAsync(cancellationToken);
	        return await Deserialize<InteractionResource>(response);
        }

        /// <summary>
        /// Deletes an interaction by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the interaction to delete.</param>
        /// <param name="apiVersion">The API version to use for the request.</param>
        /// <param name="requestOptions">Optional. Options for configuring the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>An empty string if the deletion is successful.</returns>
        public async Task<string> Delete(string id,
	        string? apiVersion = null,
	        RequestOptions? requestOptions = null, 
	        CancellationToken cancellationToken = default)
        {
	        var url = $"{BaseUrlGoogleAi}/interactions/{id}";
	        url = ParseUrl(url);
	        using var httpRequest = new HttpRequestMessage(HttpMethod.Delete, url);
	        var response = await SendAsync(httpRequest, requestOptions, cancellationToken);
	        await response.EnsureSuccessAsync(cancellationToken);
#if NET472_OR_GREATER || NETSTANDARD2_0
	        return await response.Content.ReadAsStringAsync();
#else
            return await response.Content.ReadAsStringAsync(cancellationToken);
#endif
        }
        
        /// <summary>
        /// Cancels a running background interaction by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the interaction to cancel.</param>
        /// <param name="apiVersion">The API version to use for the request.</param>
        /// <param name="requestOptions">Optional. Options for configuring the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>An <see cref="InteractionResource"/> representing the canceled interaction.</returns>
        public async Task<InteractionResource> Cancel(string id,
	        string? apiVersion = null,
	        RequestOptions? requestOptions = null, 
	        CancellationToken cancellationToken = default)
        {
	        var request = new InteractionResource();
	        
	        var url = "{BaseUrlGoogleAi}/interactions/{id}/cancel";
	        return await PostAsync<InteractionResource, InteractionResource>(request, url, string.Empty, requestOptions, HttpCompletionOption.ResponseContentRead, cancellationToken);
        }
    }
}