using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace MessageExchange;

public class WebSocketHandler : IWebSocketHandler
{
    private readonly List<WebSocket> _sockets = new List<WebSocket>();

    public void AddSocket(WebSocket socket)
    {
        _sockets.Add(socket);
    }

    public async Task SendAllAsync(string message)
    {
        var buffer = Encoding.UTF8.GetBytes(message);
        var segment = new ArraySegment<byte>(buffer);

        foreach (var socket in _sockets)
        {
            if (socket.State == WebSocketState.Open)
            {
                await socket.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }

    public Task SendAllAsync<T>(T message)
    {
        return SendAllAsync(JsonSerializer.Serialize(message));
    }
}
