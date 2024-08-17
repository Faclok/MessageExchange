using System.ComponentModel.DataAnnotations;

namespace MessageExchange;

public class Message
{
    public int Id { get; set; }
    public int SerialNumber { get; set; }
    public DateTime DateCreated { get; set; }

    [StringLength(128, ErrorMessage = "Длина строки не должна превышать 128 символов.")]
    public string Content { get; set; }
}
