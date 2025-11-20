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
    public class LiveModel : BaseModel
    {
	    public LiveModel() : this(httpClientFactory: null, logger: null) { }
	    public LiveModel(IHttpClientFactory apiClient) : this(httpClientFactory: apiClient, logger: null) { }

	    public LiveModel(IHttpClientFactory httpClientFactory, ILogger logger) : base(httpClientFactory, logger)
	    {
		    Logger.LogLiveModelInvoking();
	    }

#if false
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

            return new AsyncSession(clientWebSocket, httpClientFactory);
        }

        private async Task SetRequestHeadersAsync(ClientWebSocket clientWebSocket,
            CancellationToken cancellationToken = default)
        {
            if (apiClient.VertexAI)
            {
                if (apiClient.Credentials == null)
                {
                    throw new InvalidOperationException("GoogleAuth credentials are required for Vertex AI.");
                }

                string accessToken =
                    await apiClient.Credentials.GetAccessTokenForRequestAsync(cancellationToken: cancellationToken);
                if (string.IsNullOrEmpty(accessToken))
                {
                    throw new InvalidOperationException("Failed to retrieve access token from credentials.");
                }

                clientWebSocket.Options.SetRequestHeader("Authorization", $"Bearer {accessToken}");
            }
            else
            {
                if (string.IsNullOrEmpty(apiClient.ApiKey))
                {
                    throw new InvalidOperationException("An API key is required for Gemini API connections.");
                }

                clientWebSocket.Options.SetRequestHeader("x-goog-api-key", apiClient.ApiKey);
            }

            foreach (var header in apiClient?.HttpOptions?.Headers ?? new Dictionary<string, string>())
            {
                clientWebSocket.Options.SetRequestHeader(header.Key, header.Value);
            }
        }

        private Uri GetServerUri()
        {
            string baseUrl = apiClient.HttpOptions?.BaseUrl;
            if (string.IsNullOrEmpty(baseUrl))
            {
                throw new InvalidOperationException("BaseUrl is not set in Client.");
            }

            try
            {
                var baseUri = new Uri(baseUrl);
                var uriBuilder = new UriBuilder(baseUri) { Scheme = baseUri.Scheme == "http" ? "ws" : "wss" };

                string wsBaseUrl = uriBuilder.Uri.ToString().TrimEnd('/');

                if (apiClient.VertexAI)
                {
                    string apiVersion = apiClient.HttpOptions?.ApiVersion ?? "v1beta1";
                    return new Uri(
                        $"{wsBaseUrl}/ws/google.cloud.aiplatform.{apiVersion}.LlmBidiService/BidiGenerateContent");
                }
                else
                {
                    string apiVersion = apiClient.HttpOptions?.ApiVersion ?? "v1beta";
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
            var transformedModel = Transformers.TModel(this.apiClient, model);
            if (apiClient.VertexAI && transformedModel != null && transformedModel.StartsWith("publishers/"))
            {
                transformedModel = string.Format(
                    "projects/{0}/locations/{1}/{2}",
                    apiClient.Project,
                    apiClient.Location,
                    transformedModel
                );
            }

            LiveConnectParameters parameters = new LiveConnectParameters { Model = transformedModel, Config = config, };
            LiveConverters liveConverters = new LiveConverters(apiClient);
            string jsonString = JsonSerializer.Serialize(parameters);
            JsonNode? parameterNode = JsonNode.Parse(jsonString);
            if (parameterNode == null)
            {
                throw new InvalidOperationException("Failed to parse jsonString into a JsonNode.");
            }

            JsonNode body;
            if (apiClient.VertexAI)
            {
                body = liveConverters.LiveConnectParametersToVertex(apiClient, parameterNode, new JsonObject());
            }
            else
            {
                body = liveConverters.LiveConnectParametersToMldev(apiClient, parameterNode, new JsonObject());
            }

            body?.AsObject().Remove("config");
            return JsonSerializer.Serialize(body, JsonConfig.JsonSerializerOptions);
        }
#endif
    }
}