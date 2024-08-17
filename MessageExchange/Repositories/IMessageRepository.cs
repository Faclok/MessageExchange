namespace MessageExchange.Repositories;

public interface IMessageRepository
{
    public Task CreateAsync(Message message);

    public Task<IEnumerable<Message>> GetByDateAsync(DateTime startDateTime, DateTime EndDateTime);
}
