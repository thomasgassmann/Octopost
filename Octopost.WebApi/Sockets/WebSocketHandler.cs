namespace Octopost.WebApi.Sockets
{
    using System;
    using System.Net.WebSockets;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public abstract class WebSocketHandler
    {
        public WebSocketHandler(WebSocketConnectionManager webSocketConnectionManager) =>
            this.WebSocketConnectionManager = webSocketConnectionManager;

        protected WebSocketConnectionManager WebSocketConnectionManager { get; }

        public abstract Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer);

        public virtual Task OnConnected(WebSocket socket)
        {
            this.WebSocketConnectionManager.AddSocket(socket);
            return Task.FromResult(default(int));
        }

        public virtual async Task OnDisconnected(WebSocket socket) =>
            await this.WebSocketConnectionManager.RemoveSocket(this.WebSocketConnectionManager.GetId(socket));

        public async Task SendMessageAsync(WebSocket socket, string message)
        {
            if (socket.State != WebSocketState.Open)
            {
                return;
            }

            var buffer = new ArraySegment<byte>(Encoding.ASCII.GetBytes(message), 0, message.Length);
            await socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public async Task SendMessageAsync(string socketId, string message) =>
            await this.SendMessageAsync(this.WebSocketConnectionManager.GetSocketById(socketId), message);

        public async Task SendMessageToAllAsync(string message)
        {
            foreach (var pair in this.WebSocketConnectionManager.GetAll())
            {
                if (pair.Value.State == WebSocketState.Open)
                {
                    await this.SendMessageAsync(pair.Value, message);
                }
            }
        }
    }
}
