using MessageExchange.Contract;
using MessageExchange.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace MessageExchange.Controllers;

/// <summary>
/// Controller for managing messages.
/// </summary>
/// <param name="logger">The logger instance.</param>
/// <param name="messageRepository">The message repository instance.</param>
[ApiController]
[Route("api/[controller]")]
public partial class MessageController(IWebSocketHandler webSocket, ILogger<Message> logger, IMessageRepository messageRepository) : ControllerBase
{
    private readonly IMessageRepository _messageRepository = messageRepository;
    private readonly IWebSocketHandler _socketHandler = webSocket;
    private readonly ILogger _logger = logger;


    /// <summary>
    /// Retrieves a collection of messages within the specified date range.
    /// </summary>
    /// <param name="startDate">The start date for filtering messages. If null, defaults to 10 minutes ago.</param>
    /// <param name="endDate">The end date for filtering messages. If null, defaults to the current time.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of messages.</returns>
    [HttpGet]
    public async Task<IEnumerable<Message>> Get(DateTime? startDate = null, DateTime? endDate = null)
    {
        if (startDate is null)
            startDate = DateTime.UtcNow.AddMinutes(-10);
        else
            startDate = startDate.Value.ToUniversalTime();

        if (endDate is null)
            endDate = DateTime.UtcNow;
        else
            endDate = endDate.Value.ToUniversalTime();


        _logger.LogInformation("Request {startDate} {endDate}", startDate, endDate);

        var messages = await _messageRepository.GetByDateAsync(startDate.Value, endDate.Value);
        return messages;
    }

    /// <summary>
    /// Creates a new message.
    /// </summary>
    /// <param name="request">The request containing the message details.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    [HttpPost]
    public async Task Create([FromBody] MessageRequest request)
    {
        _logger.LogInformation("Request to create a message {@request}", request);

        Message message = new()
        {
            Content = request.Text,
            DateCreated = DateTime.UtcNow,
            SerialNumber = request.SerialNumber,
        };

        await _messageRepository.CreateAsync(message);

        await _socketHandler.SendAllAsync(message);
    }
}
