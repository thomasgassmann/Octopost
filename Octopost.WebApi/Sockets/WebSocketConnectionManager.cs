namespace Octopost.WebApi.Sockets
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Net.WebSockets;
    using System.Threading;
    using System.Threading.Tasks;

    public class WebSocketConnectionManager
    {
        private ConcurrentDictionary<string, WebSocket> sockets = new ConcurrentDictionary<string, WebSocket>();

        public WebSocket GetSocketById(string id) =>
            this.sockets.FirstOrDefault(p => p.Key == id).Value;

        public ConcurrentDictionary<string, WebSocket> GetAll() => this.sockets;

        public string GetId(WebSocket socket) =>
            this.sockets.FirstOrDefault(p => p.Value == socket).Key;

        public void AddSocket(WebSocket socket) =>
            this.sockets.TryAdd(this.CreateConnectionId(), socket);

        public async Task RemoveSocket(string id)
        {
            this.sockets.TryRemove(id, out var socket);
            await socket.CloseAsync(closeStatus: WebSocketCloseStatus.NormalClosure,
                                    statusDescription: "Closed by the WebSocketManager",
                                    cancellationToken: CancellationToken.None);
        }

        private string CreateConnectionId() =>
            Guid.NewGuid().ToString();
    }
}
