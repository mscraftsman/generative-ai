using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json.Nodes;

namespace Mscc.GenerativeAI.Types
{
    public partial class AsyncSession : IAsyncDisposable
    {
        private readonly WebSocket _webSocket;
        private readonly IHttpClientFactory? _httpClientFactory;
        private readonly bool _isVertexAI;
        private int _isDisposed = 0; // 0 = false, 1 = true. Used with Interlocked.

        public AsyncSession(WebSocket webSocket, IHttpClientFactory? httpClientFactory, bool isVertexAI)
        {
            _webSocket = webSocket ?? throw new ArgumentNullException(nameof(webSocket));
            _httpClientFactory = httpClientFactory;
            _isVertexAI = isVertexAI;
        }

        /// <summary>
        /// Sends non-realtime, turn-based content to the model.
        /// <para>
        /// There are two ways to send messages to the live API:
        /// <c>SendClientContentAsync</c> and <c>SendRealtimeInputAsync</c>.
        /// </para>
        /// <para>
        /// <c>SendClientContentAsync</c> messages are added to the model context
        /// <b>in order</b>. Because <c>SendClientContentAsync</c> guarantees the order
        /// of messages between the client and the server, the model cannot respond as
        /// quickly as with <c>SendRealtimeInputAsync</c>. This is most noticeable when
        /// sending objects that require significant preprocessing time (typically images).
        /// </para>
        /// <para>
        /// <c>SendRealtimeInputAsync</c> sends a list of <see cref="Content"/> objects,
        /// which offers more options than the <see cref="Blob"/> objects sent by
        /// <c>SendClientContentAsync</c>.
        /// </para>
        /// <para>
        /// The main use cases for <c>SendClientContentAsync</c> over
        /// <c>SendRealtimeInputAsync</c> are:
        /// <list type="number">
        /// <item>
        /// Prefilling a conversation context (including sending anything that can't be
        /// represented as a realtime message) before starting a realtime conversation.
        /// </item>
        /// <item>
        /// Conducting a non-realtime conversation with the live API.
        /// </item>
        /// </list>
        /// <b>Caution:</b> Interleaving <c>SendClientContentAsync</c> and
        /// <c>SendRealtimeInputAsync</c> in the same conversation is not recommended and
        /// can lead to unexpected behavior.
        /// </para>
        /// </summary>
        /// <param name="clientContent">The client content to send to the model.</param>
        /// <param name="cancellationToken">The cancellation token to use for the send operation.</param>
        /// <returns></returns>
        public async Task SendClientContent(LiveSendClientContentParameters clientContent,
            CancellationToken cancellationToken = default)
        {
            LiveClientMessage liveClientMessage = new LiveClientMessage
            {
                ClientContent = new LiveClientContent
                {
                    Turns = clientContent.Turns,
                    TurnComplete = clientContent.TurnComplete
                }
            };

            await Send(liveClientMessage, cancellationToken);
        }

        /// <summary>
        /// Sends realtime input to the model. With <c>SendRealtimeInputAsync</c>,
        /// Google's GenAI Live API will respond to audio automatically based on voice
        /// activity detection (VAD). <c>SendRealtimeInputAsync</c> is optimized for
        /// responsiveness at the expense of deterministic ordering of the conversation
        /// messages. Response tokens are added to the context as they become available.
        /// </summary>
        /// <param name="realtimeInput">The realtime input to send to the model.</param>
        /// <param name="cancellationToken">The cancellation token to use for the send operation.</param>
        /// <returns></returns>
        public async Task SendRealtimeInput(LiveSendRealtimeInputParameters realtimeInput,
            CancellationToken cancellationToken = default)
        {
            LiveClientMessage liveClientMessage = new LiveClientMessage
            {
                RealtimeInput = realtimeInput
            };
            await Send(liveClientMessage, cancellationToken);
        }

        /// <summary>
        /// Sends tool response to the model.
        /// </summary>
        /// <param name="toolResponse">Tool response to send to the model.</param>
        /// <param name="cancellationToken">The cancellation token to use for the send operation.</param>
        public async Task SendToolResponseAsync(LiveSendToolResponseParameters toolResponse,
            CancellationToken cancellationToken = default)
        {
            LiveClientMessage liveClientMessage = new LiveClientMessage
            {
                ToolResponse = new LiveClientToolResponse
                {
                    FunctionResponses = toolResponse.FunctionResponses
                }
            };
            await Send(liveClientMessage, cancellationToken);
        }

        /// <summary>
        /// Receives model responses from the server.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to use for the send operation.</param>
        /// <returns>
        /// A <see cref="LiveServerMessage"/> containing the model's response, or <c>null</c> if the
        /// connection has been gracefully closed.
        /// </returns>
        /// <exception cref="InvalidOperationException">Thrown if an empty or invalid message is received.</exception>
        /// <exception cref="WebSocketException">Thrown for underlying WebSocket errors that are not a graceful close.</exception>
        public async Task<LiveServerMessage?> Receive(CancellationToken cancellationToken = default)
        {
            if (_isDisposed == 1)
            {
                return null;
            }

            switch (_webSocket.State)
            {
                case WebSocketState.Connecting:
                    throw new InvalidOperationException(
                        "Cannot receive data while the WebSocket is still connecting. Ensure that the ConnectAsync method has completed.");
                case WebSocketState.Open:
                    break; // Proceed with receiving.
                default:
                    return null;
            }

            var buffer = new byte[4096];
            var messageBuilder = new StringBuilder();
            WebSocketReceiveResult result;

            try
            {
                do
                {
                    result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);
                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        return null;
                    }

                    messageBuilder.Append(Encoding.UTF8.GetString(buffer, 0, result.Count));
                } while (!result.EndOfMessage);
            }
            catch (WebSocketException ex) when (ex.WebSocketErrorCode == WebSocketError.ConnectionClosedPrematurely)
            {
                return null;
            }

            var messageString = messageBuilder.ToString();
            if (string.IsNullOrEmpty(messageString))
            {
                throw new InvalidOperationException("Received an empty message from the server.");
            }

            var serverMessage = LiveServerMessage.FromJson(messageString);
            if (serverMessage == null)
            {
                throw new InvalidOperationException("Failed to deserialize server message because it is null.");
            }

            return serverMessage;
        }

        private async Task Send(LiveClientMessage liveClientMessage, CancellationToken cancellationToken = default)
        {
            string jsonString = JsonSerializer.Serialize(liveClientMessage, JsonConfig.LiveSerializerOptions);
            JsonNode? liveClientMessageNode = JsonNode.Parse(jsonString);
            if (liveClientMessageNode == null)
            {
                throw new InvalidOperationException("Failed to parse liveClientMessage into a JsonNode.");
            }

            LiveConverters liveConverters = new LiveConverters();
            JsonNode body;
            if (_isVertexAI)
            {
                body = liveConverters.LiveClientMessageToVertex(liveClientMessageNode);
            }
            else
            {
                body = liveConverters.LiveClientMessageToMldev(liveClientMessageNode);
            }

            string jsonMessage = JsonSerializer.Serialize(body, JsonConfig.LiveSerializerOptions);
            byte[] buffer = Encoding.UTF8.GetBytes(jsonMessage);

            if (_webSocket.State != WebSocketState.Open)
            {
                throw new InvalidOperationException($"WebSocket is not open. State: {_webSocket.State}");
            }

            await _webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true,
                cancellationToken);
        }

        /// <summary>
        /// Closes the WebSocket connection gracefully. This method is thread-safe and idempotent.
        /// </summary>
        public async Task Close()
        {
	        // Atomically check and set the disposed flag to ensure this block runs only once.
	        // Critical to avoid race conditions in multi-threaded scenarios.
            if (Interlocked.CompareExchange(ref _isDisposed, 1, 0) != 0)
            {
                return;
            }

            try
            {
                if (_webSocket.State == WebSocketState.Open || _webSocket.State == WebSocketState.Connecting)
                {
                    await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                }
                else if (_webSocket.State == WebSocketState.CloseReceived)
                {
                    await _webSocket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "Acknowledging server close",
                        CancellationToken.None);
                }
                // For other states (None, CloseSent, Closed, Aborted), no action is needed.
            }
            catch (Exception ex) when (ex is ObjectDisposedException || ex is InvalidOperationException ||
                                       ex is WebSocketException || ex is OperationCanceledException ||
                                       ex is IOException)
            {
                // Suppress exceptions during cleanup as the primary goal is to release resources.
                // Optionally, these exceptions can be logged for debugging purposes.
            }
            finally
            {
                _webSocket.Dispose();
            }
        }

        /// <summary>
        /// Asynchronously disposes the session by closing the WebSocket connection.
        /// </summary>
        public async ValueTask DisposeAsync()
        {
            await Close();
        }
    }
}
