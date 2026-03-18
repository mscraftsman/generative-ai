using Microsoft.Extensions.Logging;
using Mscc.GenerativeAI.Types;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

namespace Mscc.GenerativeAI
{
    public sealed class LiveModel : BaseModel
    {
	    public LiveModel() : this(httpClientFactory: null, logger: null) { }
	    public LiveModel(IHttpClientFactory apiClient) : this(httpClientFactory: apiClient, logger: null) { }

	    public LiveModel(IHttpClientFactory httpClientFactory, ILogger logger) : base(httpClientFactory, logger)
	    {
		    Logger.LogLiveModelInvoking();
	    }

        /// <summary>
        /// Establishes a websocket connection to the specified model with the given configuration.
        /// </summary>
        /// <param name="model">
        /// The name of the model to connect to. For example "gemini-2.0-flash-live-preview-04-09".
        /// </param>
        /// <param name="config">
        /// The parameters for establishing a connection to the model.
        /// </param>
        /// <param name="cancellationToken">The cancellation token to use for the connection.</param>
        /// <returns> <see cref="AsyncSession"/> </returns>
        public async Task<AsyncSession> Connect(string model, 
            LiveConnectConfig config,
            CancellationToken cancellationToken = default)
        {
            var clientWebSocket = new ClientWebSocket();
            await SetRequestHeadersAsync(clientWebSocket, cancellationToken);
            Uri serverUri = GetServerUri();
            await clientWebSocket.ConnectAsync(serverUri, cancellationToken);

            string setupClientMessage = getSetupMessage(model, config);
            byte[] buffer = Encoding.UTF8.GetBytes(setupClientMessage);
            try
            {
                await clientWebSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true,
                    cancellationToken);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error sending setup message: {ex.Message}", ex);
            }

            return new AsyncSession(clientWebSocket, _httpClientFactory, IsVertexAI);
        }

        private async Task SetRequestHeadersAsync(ClientWebSocket clientWebSocket,
            CancellationToken cancellationToken = default)
        {
            if (IsVertexAI)
            {
                if (string.IsNullOrEmpty(_accessToken))
                {
                    throw new InvalidOperationException("Access token is required for Vertex AI.");
                }

                clientWebSocket.Options.SetRequestHeader("Authorization", $"Bearer {_accessToken}");
            }
            else
            {
                if (string.IsNullOrEmpty(_apiKey))
                {
                    throw new InvalidOperationException("An API key is required for Gemini API connections.");
                }

                clientWebSocket.Options.SetRequestHeader("x-goog-api-key", _apiKey);
            }

            if (RequestOptions?.Headers != null)
            {
                foreach (var header in RequestOptions.Headers)
                {
                    clientWebSocket.Options.SetRequestHeader(header.Key, string.Join(", ", header.Value));
                }
            }
        }

        private Uri GetServerUri()
        {
            string baseUrl = RequestOptions?.BaseUrl ?? (IsVertexAI ? BaseUrlVertexAi : BaseUrlGoogleAi);
            if (string.IsNullOrEmpty(baseUrl))
            {
                throw new InvalidOperationException("BaseUrl is not set.");
            }

            try
            {
                var baseUri = new Uri(ParseUrl(baseUrl));
                var uriBuilder = new UriBuilder(baseUri) { Scheme = baseUri.Scheme == "http" ? "ws" : "wss" };

                string wsBaseUrl = uriBuilder.Uri.ToString().TrimEnd('/');

                if (IsVertexAI)
                {
                    string apiVersion = RequestOptions?.ApiVersion ?? "v1beta1";
                    return new Uri(
                        $"{wsBaseUrl}/ws/google.cloud.aiplatform.{apiVersion}.LlmBidiService/BidiGenerateContent");
                }
                else
                {
                    string apiVersion = RequestOptions?.ApiVersion ?? "v1beta";
                    return new Uri(
                        $"{wsBaseUrl}/ws/google.ai.generativelanguage.{apiVersion}.GenerativeService.BidiGenerateContent");
                }
            }
            catch (UriFormatException e)
            {
                throw new InvalidOperationException("Failed to parse URL.", e);
            }
        }

        private string getSetupMessage(string model, LiveConnectConfig config)
        {
            var transformedModel = Transformers.TModel(this, model);
            if (IsVertexAI && transformedModel != null && transformedModel.StartsWith("publishers/"))
            {
                transformedModel = string.Format(
                    "projects/{0}/locations/{1}/{2}",
                    _projectId,
                    _region,
                    transformedModel
                );
            }

            LiveConnectParameters parameters = new LiveConnectParameters { Model = transformedModel, Config = config, };
            LiveConverters liveConverters = new LiveConverters(_httpClientFactory);
            string jsonString = JsonSerializer.Serialize(parameters, JsonConfig.LiveSerializerOptions);
            JsonNode? parameterNode = JsonNode.Parse(jsonString);
            if (parameterNode == null)
            {
                throw new InvalidOperationException("Failed to parse jsonString into a JsonNode.");
            }

            JsonNode body;
            if (IsVertexAI)
            {
                body = liveConverters.LiveConnectParametersToVertex(parameterNode);
            }
            else
            {
                body = liveConverters.LiveConnectParametersToMldev(parameterNode);
            }

            var finalMessage = new JsonObject
            {
                ["setup"] = body
            };

            return JsonSerializer.Serialize(finalMessage, JsonConfig.LiveSerializerOptions);
        }

        private bool IsVertexAI => _accessToken != null || (!string.IsNullOrEmpty(_projectId) && !string.IsNullOrEmpty(_region));
    }
}
