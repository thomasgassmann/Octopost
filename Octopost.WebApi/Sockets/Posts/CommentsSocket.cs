namespace Octopost.WebApi.Sockets.Posts
{
    using System;
    using System.Net.WebSockets;
    using System.Threading.Tasks;

    public class CommentsSocket : WebSocketHandler
    {
        public CommentsSocket(WebSocketConnectionManager webSocketConnectionManager) : base(webSocketConnectionManager)
        {
        }

        public override Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            throw new NotImplementedException();
        }
    }
}
