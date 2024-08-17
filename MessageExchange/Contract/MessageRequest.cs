using System.ComponentModel.DataAnnotations;

namespace MessageExchange.Contract;

public class MessageRequest
{
    public int SerialNumber { get; set; }

    [StringLength(128, ErrorMessage = "Длина строки не должна превышать 128 символов.")]
    public string Text { get; set; }
}
