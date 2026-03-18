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
        /// </summary>
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
        /// Sends realtime input to the model.
        /// </summary>
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
            byte[] buffer = JsonSerializer.SerializeToUtf8Bytes(liveClientMessage, JsonConfig.LiveSerializerOptions);

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
            }
            catch (Exception ex) when (ex is ObjectDisposedException || ex is InvalidOperationException ||
                                       ex is WebSocketException || ex is OperationCanceledException ||
                                       ex is IOException)
            {
                // Suppress exceptions during cleanup
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
