#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
#endif
using mea = Microsoft.Extensions.AI;
using System.Runtime.CompilerServices;

namespace Mscc.GenerativeAI.Microsoft;

public sealed class GeminiChatClient : mea.IChatClient
{
    private const string ProviderName = "gemini";
    
    /// <summary>
    /// Gets the Gemini model that is used to communicate with.
    /// </summary>
    private readonly GenerativeModel _client;
    private readonly mea.ChatClientMetadata _metadata;

    /// <summary>
    /// Creates a new instance of the <see cref="GeminiChatClient"/> class for the specified Gemini API client.
    /// </summary>
    /// <param name="client">The underlying client.</param>
    /// <exception cref="ArgumentNullException">Thrown when the specified client is null.</exception>
    public GeminiChatClient(GenerativeModel client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _metadata = new(ProviderName, null, client.Model);
    }
    
    /// <summary>
    /// Creates an instance of the Gemini API client using Google AI.
    /// </summary>
    /// <param name="apiKey">API key provided by Google AI Studio</param>
    /// <param name="model">Model to use.</param>
    public GeminiChatClient(string apiKey, string model = Model.Gemini25Pro)
    {
        var genAi = new GoogleAI(apiKey);
        _client = genAi.GenerativeModel(model);
        _metadata = new(ProviderName, null, model);
    }

    /// <summary>
    /// Creates an instance of the Gemini API client using Vertex AI.
    /// </summary>
    /// <param name="projectId">Identifier of the Google Cloud project.</param>
    /// <param name="region">Optional. Region to use (default: "us-central1").</param>
    /// <param name="model">Model to use.</param>
    public GeminiChatClient(string projectId, string? region = null, string model = Model.Gemini25Pro)
    {
        var genAi = new VertexAI(projectId: projectId, region: region);
        _client = genAi.GenerativeModel(model);
        _metadata = new(ProviderName, null, model);
    }
    
    /// <inheritdoc/>
    public async Task<mea.ChatResponse> GetResponseAsync(
        IEnumerable<mea.ChatMessage> messages, 
        mea.ChatOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        if (messages == null) throw new ArgumentNullException(nameof(messages));

        var request = AbstractionMapper.ToGeminiGenerateContentRequest(this, messages, options);
        var requestOptions = AbstractionMapper.ToGeminiGenerateContentRequestOptions(options);
		var response = await _client.GenerateContent(request, requestOptions, cancellationToken);
		return AbstractionMapper.ToChatResponse(response) ?? new mea.ChatResponse([]);
    }

    /// <inheritdoc/>
    public async IAsyncEnumerable<mea.ChatResponseUpdate> GetStreamingResponseAsync(
        IEnumerable<mea.ChatMessage> messages, 
        mea.ChatOptions? options = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        if (messages == null) throw new ArgumentNullException(nameof(messages));

        var request = AbstractionMapper.ToGeminiGenerateContentRequest(this, messages, options);
        var requestOptions = AbstractionMapper.ToGeminiGenerateContentRequestOptions(options);
		await foreach (var response in _client.GenerateContentStream(request, requestOptions, cancellationToken))
			yield return AbstractionMapper.ToChatResponseUpdate(response) ?? new mea.ChatResponseUpdate();
    }

    /// <inheritdoc/>
    object? mea.IChatClient.GetService(Type serviceType, object? serviceKey) =>
        serviceKey is not null ? null :
        serviceType == typeof(mea.ChatClientMetadata) ? _metadata :
        serviceType == typeof(GenerativeModel) ? _client :
        serviceType?.IsInstanceOfType(this) is true ? this :
        null;
        
    /// <inheritdoc/>
    public void Dispose() { }
}