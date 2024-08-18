using System.Net.WebSockets;

namespace MessageExchange
{
    public interface IWebSocketHandler
    {
        void AddSocket(WebSocket socket);
        Task SendAllAsync(string message);
        Task SendAllAsync<T>(T message);
    }
}