using Microsoft.AspNetCore.Mvc;

namespace MessageExchange.Controllers;

public class WebSocketController(IWebSocketHandler handler) : ControllerBase
{
    private readonly IWebSocketHandler _handler = handler;

    [HttpGet("/ws")]
    public async Task Get()
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            var socketFinishedTcs = new TaskCompletionSource<object>();
            _handler.AddSocket(webSocket);

            await socketFinishedTcs.Task;
        }
        else
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }
}
