using Microsoft.Extensions.Logging;
using Mscc.GenerativeAI.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Mscc.GenerativeAI
{
	/// <summary>
	/// The Gemini Interactions API is an experimental API that allows developers to build generative AI applications using Gemini models. Gemini is our most capable model, built from the ground up to be multimodal. It can generalize and seamlessly understand, operate across, and combine different types of information including language, images, audio, video, and code. You can use the Gemini API for use cases like reasoning across text and images, content generation, dialogue agents, summarization and classification systems, and more.
	/// </summary>
    public class InteractionsModel : BaseModel
    {
        internal override string Version => ApiVersion.V1Beta;

        /// <summary>
        /// Initializes a new instance of the <see cref="InteractionsModel"/> class.
        /// </summary>
        public InteractionsModel() : this(httpClientFactory: null, logger: null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InteractionsModel"/> class.
        /// </summary>
        public InteractionsModel(IHttpClientFactory apiClient) : this(httpClientFactory: apiClient, logger: null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InteractionsModel"/> class.
        /// </summary>
        /// <param name="httpClientFactory">Optional. The <see cref="IHttpClientFactory"/> to use for creating HttpClient instances.</param>
        /// <param name="logger">Optional. Logger instance used for logging</param>
        public InteractionsModel(IHttpClientFactory? httpClientFactory = null, ILogger? logger = null) : base(httpClientFactory, logger) { }

        /// <summary>
	    /// Creates a new interaction.
	    /// </summary>
	    /// <param name="model">The name of the `Model` used for generating the interaction. </param>
	    /// <param name="agent">The name of the `Agent` used for generating the interaction.</param>
	    /// <param name="input">The inputs for the interaction (common to both Model and Agent).</param>
	    /// <param name="systemInstruction">System instruction for the interaction.</param>
	    /// <param name="tools">A list of tool declarations the model may call during interaction.</param>
	    /// <param name="responseFormat">Enforces that the generated response is a JSON object that complies with the JSON schema specified in this field.</param>
	    /// <param name="responseMimeType">The mime type of the response. This is required if response_format is set.</param>
	    /// <param name="stream">Input only. Whether the interaction will be streamed.</param>
	    /// <param name="store">Input only. Whether to store the response and request for later retrieval.</param>
	    /// <param name="background">Whether to run the model interaction in the background.</param>
	    /// <param name="generationConfig">Configuration parameters for the model interaction. Alternative to `agent_config`. Only applicable when `model` is set.</param>
	    /// <param name="agentConfig">Configuration for the agent. Alternative to `generation_config`. Only applicable when `agent` is set.</param>
	    /// <param name="previousInteractionId">The ID of the previous interaction, if any.</param>
	    /// <param name="responseModalities">The requested modalities of the response (TEXT, IMAGE, AUDIO).</param>
	    /// <param name="requestOptions">Options for the request.</param>
	    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
	    /// <returns>An Interaction resource.</returns>
        public async Task<InteractionResource> Create(InteractionRequest request,
	        RequestOptions? requestOptions = null, 
	        CancellationToken cancellationToken = default)
        {
	        var url = "{BaseUrlGoogleAi}/interactions";
	        return await PostAsync<InteractionRequest, InteractionResource>(request, url, string.Empty, requestOptions, HttpCompletionOption.ResponseContentRead, cancellationToken);
        }

        /// <summary>
	    /// Creates a new interaction.
	    /// </summary>
	    /// <param name="model">The name of the `Model` used for generating the interaction. </param>
	    /// <param name="agent">The name of the `Agent` used for generating the interaction.</param>
	    /// <param name="input">The inputs for the interaction (common to both Model and Agent).</param>
	    /// <param name="systemInstruction">System instruction for the interaction.</param>
	    /// <param name="tools">A list of tool declarations the model may call during interaction.</param>
	    /// <param name="responseFormat">Enforces that the generated response is a JSON object that complies with the JSON schema specified in this field.</param>
	    /// <param name="responseMimeType">The mime type of the response. This is required if response_format is set.</param>
	    /// <param name="stream">Input only. Whether the interaction will be streamed.</param>
	    /// <param name="store">Input only. Whether to store the response and request for later retrieval.</param>
	    /// <param name="background">Whether to run the model interaction in the background.</param>
	    /// <param name="generationConfig">Configuration parameters for the model interaction. Alternative to `agent_config`. Only applicable when `model` is set.</param>
	    /// <param name="agentConfig">Configuration for the agent. Alternative to `generation_config`. Only applicable when `agent` is set.</param>
	    /// <param name="previousInteractionId">The ID of the previous interaction, if any.</param>
	    /// <param name="responseModalities">The requested modalities of the response (TEXT, IMAGE, AUDIO).</param>
	    /// <param name="requestOptions">Options for the request.</param>
	    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
	    /// <returns>An Interaction resource.</returns>
        public async Task<InteractionResource> Create(string? model = null,
	        string? agent = null,
	        List<InteractionContent>? input = null,
            string? systemInstruction = null,
	        Tools? tools = null,
	        object? responseFormat = null,
	        string? responseMimeType = null,
            bool? stream = false,
            bool? store = false,
	        bool? background = false,
	        GenerationConfig? generationConfig = null,
	        object? agentConfig = null,
	        string? previousInteractionId = null,
	        List<ResponseModality>? responseModalities = null,
	        RequestOptions? requestOptions = null, 
	        CancellationToken cancellationToken = default)
        {
	        var request = new InteractionRequest
	        {
		        Model = model, 
		        Agent = agent,
		        InputListContent = input,
		        SystemInstruction = systemInstruction
	        };
	        return await Create(request, requestOptions, cancellationToken);
        }

        /// <summary>
        /// Creates a new interaction.
        /// </summary>
        /// <param name="model">The name of the `Model` used for generating the interaction. </param>
        /// <param name="agent">The name of the `Agent` used for generating the interaction.</param>
        /// <param name="input">The inputs for the interaction (common to both Model and Agent).</param>
        /// <param name="systemInstruction">System instruction for the interaction.</param>
        /// <param name="tools">A list of tool declarations the model may call during interaction.</param>
        /// <param name="responseFormat">Enforces that the generated response is a JSON object that complies with the JSON schema specified in this field.</param>
        /// <param name="responseMimeType">The mime type of the response. This is required if response_format is set.</param>
        /// <param name="stream">Input only. Whether the interaction will be streamed.</param>
        /// <param name="store">Input only. Whether to store the response and request for later retrieval.</param>
        /// <param name="background">Whether to run the model interaction in the background.</param>
        /// <param name="generationConfig">Configuration parameters for the model interaction. Alternative to `agent_config`. Only applicable when `model` is set.</param>
        /// <param name="agentConfig">Configuration for the agent. Alternative to `generation_config`. Only applicable when `agent` is set.</param>
        /// <param name="previousInteractionId">The ID of the previous interaction, if any.</param>
        /// <param name="responseModalities">The requested modalities of the response (TEXT, IMAGE, AUDIO).</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>An Interaction resource.</returns>
        public async Task<InteractionResource> Create(string? model = null,
	        string? agent = null,
	        InteractionContent? input = null,
	        string? systemInstruction = null,
	        Tools? tools = null,
	        object? responseFormat = null,
	        string? responseMimeType = null,
	        bool? stream = false,
	        bool? store = false,
	        bool? background = false,
	        GenerationConfig? generationConfig = null,
	        object? agentConfig = null,
	        string? previousInteractionId = null,
	        List<ResponseModality>? responseModalities = null,
	        RequestOptions? requestOptions = null,
	        CancellationToken cancellationToken = default)
        {
	        var request = new InteractionRequest
	        {
		        Model = model, 
		        Agent = agent,
		        InputContent = input,
		        SystemInstruction = systemInstruction
	        };
	        return await Create(request, requestOptions, cancellationToken);
        }

        /// <summary>
        /// Creates a new interaction.
        /// </summary>
        /// <param name="model">The name of the `Model` used for generating the interaction. </param>
        /// <param name="agent">The name of the `Agent` used for generating the interaction.</param>
        /// <param name="input">The inputs for the interaction (common to both Model and Agent).</param>
        /// <param name="systemInstruction">System instruction for the interaction.</param>
        /// <param name="tools">A list of tool declarations the model may call during interaction.</param>
        /// <param name="responseFormat">Enforces that the generated response is a JSON object that complies with the JSON schema specified in this field.</param>
        /// <param name="responseMimeType">The mime type of the response. This is required if response_format is set.</param>
        /// <param name="stream">Input only. Whether the interaction will be streamed.</param>
        /// <param name="store">Input only. Whether to store the response and request for later retrieval.</param>
        /// <param name="background">Whether to run the model interaction in the background.</param>
        /// <param name="generationConfig">Configuration parameters for the model interaction. Alternative to `agent_config`. Only applicable when `model` is set.</param>
        /// <param name="agentConfig">Configuration for the agent. Alternative to `generation_config`. Only applicable when `agent` is set.</param>
        /// <param name="previousInteractionId">The ID of the previous interaction, if any.</param>
        /// <param name="responseModalities">The requested modalities of the response (TEXT, IMAGE, AUDIO).</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>An Interaction resource.</returns>
        public async Task<InteractionResource> Create(string? model = null,
	        string? agent = null,
	        string? input = null,
	        string? systemInstruction = null,
	        Tools? tools = null,
	        object? responseFormat = null,
	        string? responseMimeType = null,
	        bool? stream = false,
	        bool? store = false,
	        bool? background = false,
	        GenerationConfig? generationConfig = null,
	        object? agentConfig = null,
	        string? previousInteractionId = null,
	        List<ResponseModality>? responseModalities = null,
	        RequestOptions? requestOptions = null, 
	        CancellationToken cancellationToken = default)
        {
	        var request = new InteractionRequest
	        {
		        Model = model, 
		        Agent = agent,
		        InputString = input,
		        SystemInstruction = systemInstruction
	        };
	        return await Create(request, requestOptions, cancellationToken);
        }

        /// <summary>
        /// Creates a new interaction.
        /// </summary>
        /// <param name="model">The name of the `Model` used for generating the interaction. </param>
        /// <param name="agent">The name of the `Agent` used for generating the interaction.</param>
        /// <param name="input">The inputs for the interaction (common to both Model and Agent).</param>
        /// <param name="systemInstruction">System instruction for the interaction.</param>
        /// <param name="tools">A list of tool declarations the model may call during interaction.</param>
        /// <param name="responseFormat">Enforces that the generated response is a JSON object that complies with the JSON schema specified in this field.</param>
        /// <param name="responseMimeType">The mime type of the response. This is required if response_format is set.</param>
        /// <param name="stream">Input only. Whether the interaction will be streamed.</param>
        /// <param name="store">Input only. Whether to store the response and request for later retrieval.</param>
        /// <param name="background">Whether to run the model interaction in the background.</param>
        /// <param name="generationConfig">Configuration parameters for the model interaction. Alternative to `agent_config`. Only applicable when `model` is set.</param>
        /// <param name="agentConfig">Configuration for the agent. Alternative to `generation_config`. Only applicable when `agent` is set.</param>
        /// <param name="previousInteractionId">The ID of the previous interaction, if any.</param>
        /// <param name="responseModalities">The requested modalities of the response (TEXT, IMAGE, AUDIO).</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>An Interaction resource.</returns>
        // ToDo: implement multi-turn handling
        public async Task<InteractionResource> Create(string? model = null,
	        string? agent = null,
	        List<InteractionTurn>? input = null,
	        string? systemInstruction = null,
	        Tools? tools = null,
	        object? responseFormat = null,
	        string? responseMimeType = null,
	        bool? stream = false,
	        bool? store = false,
	        bool? background = false,
	        GenerationConfig? generationConfig = null,
	        object? agentConfig = null,
	        string? previousInteractionId = null,
	        List<ResponseModality>? responseModalities = null,
	        RequestOptions? requestOptions = null, 
	        CancellationToken cancellationToken = default)
        {
	        var request = new InteractionRequest
	        {
		        Model = model, 
		        Agent = agent,
		        InputListTurn = input,
		        SystemInstruction = systemInstruction
	        };
	        return await Create(request, requestOptions, cancellationToken);
        }

        /// <summary>
        /// Retrieves the full details of a single interaction based on its `Interaction.id`.
        /// </summary>
        /// <param name="id">The unique identifier of the interaction to retrieve.</param>
        /// <param name="stream">If set to true, the generated content will be streamed incrementally.</param>
        /// <param name="lastEventId">Optional. If set, resumes the interaction stream from the next chunk after the event marked by the event id. Can only be used if `stream` is true.</param>
        /// <param name="apiVersion">Which version of the API to use.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>An Interaction resource.</returns>
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
        /// Deletes the interaction by id.
        /// </summary>
        /// <param name="id">The unique identifier of the interaction to delete.</param>
        /// <param name="apiVersion">Which version of the API to use.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>If successful, the response is empty.</returns>
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
        /// Cancels an interaction by id. This only applies to background interactions that are still running.
        /// </summary>
        /// <param name="id">The unique identifier of the interaction to retrieve.</param>
        /// <param name="apiVersion">Which version of the API to use.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>An Interaction resource.</returns>
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