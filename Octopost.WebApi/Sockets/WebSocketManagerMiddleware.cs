namespace Octopost.WebApi.Sockets
{
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Net.WebSockets;
    using System.Threading;
    using System.Threading.Tasks;

    public class WebSocketManagerMiddleware
    {
        private readonly RequestDelegate next;

        private readonly WebSocketHandler webSocketHandler;

        public WebSocketManagerMiddleware(RequestDelegate next, WebSocketHandler webSocketHandler)
        {
            this.next = next;
            this.webSocketHandler = webSocketHandler;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.WebSockets.IsWebSocketRequest)
            {
                await this.next.Invoke(context);
                return;
            }

            var socket = await context.WebSockets.AcceptWebSocketAsync();
            await this.webSocketHandler.OnConnected(socket);
            await this.Receive(socket, async (result, buffer) =>
            {
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    await this.webSocketHandler.ReceiveAsync(socket, result, buffer);
                    return;
                }

                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    await this.webSocketHandler.OnDisconnected(socket);
                    return;
                }

            });

            await this.next.Invoke(context);
        }

        private async Task Receive(WebSocket socket, Action<WebSocketReceiveResult, byte[]> handleMessage)
        {
            var buffer = new byte[1024 * 4];
            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                handleMessage(result, buffer);
            }
        }
    }
}
