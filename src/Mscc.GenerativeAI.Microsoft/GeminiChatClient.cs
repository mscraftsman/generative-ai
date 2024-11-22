#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
#endif
using Microsoft.Extensions.AI;
using System.Runtime.CompilerServices;

namespace Mscc.GenerativeAI.Microsoft;

public sealed class GeminiChatClient : IChatClient
{
    private const string providerName = "gemini";
    
    /// <summary>
    /// Gets the Gemini model that is used to communicate with.
    /// </summary>
    private readonly GenerativeModel _client;
    
    /// <inheritdoc/>
    public ChatClientMetadata Metadata { get; }

    /// <summary>
    /// Creates an instance of the Gemini API client.
    /// </summary>
    /// <param name="apiKey">API key provided by Google AI Studio</param>
    /// <param name="model">Model to use</param>
    public GeminiChatClient(string apiKey, string model = "")
    {
        var genAi = new GoogleAI(apiKey);
        _client = genAi.GenerativeModel(model);
        Metadata = new(providerName, null, model);
    }

    /// <inheritdoc/>
    public async Task<ChatCompletion> CompleteAsync(
        IList<ChatMessage> chatMessages, 
        ChatOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        if (chatMessages == null) throw new ArgumentNullException(nameof(chatMessages));

        var request = MicrosoftAi.AbstractionMapper.ToGeminiGenerateContentRequest(chatMessages, options);
        var requestOptions = MicrosoftAi.AbstractionMapper.ToGeminiGenerateContentRequestOptions(options);
		var response = await _client.GenerateContent(request, requestOptions);
		return MicrosoftAi.AbstractionMapper.ToChatCompletion(response) ?? new ChatCompletion([]);
    }

    /// <inheritdoc/>
    public async IAsyncEnumerable<StreamingChatCompletionUpdate> CompleteStreamingAsync(
        IList<ChatMessage> chatMessages, 
        ChatOptions? options = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        if (chatMessages == null) throw new ArgumentNullException(nameof(chatMessages));

        var request = MicrosoftAi.AbstractionMapper.ToGeminiGenerateContentRequest(chatMessages, options);
        var requestOptions = MicrosoftAi.AbstractionMapper.ToGeminiGenerateContentRequestOptions(options);
		await foreach (var response in _client.GenerateContentStream(request, requestOptions, cancellationToken))
			yield return MicrosoftAi.AbstractionMapper.ToStreamingChatCompletionUpdate(response);
    }

    /// <inheritdoc/>
    public object? GetService(Type serviceType, object? key)
        => key is null && serviceType?.IsInstanceOfType(this) is true ? this : null;
        
    /// <inheritdoc/>
    public void Dispose() { }
}