using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;

namespace MessageExchange.Repositories;

public class MessageRepository : IMessageRepository
{
    private readonly string _connectionString;
    private readonly IDbConnection _dbConnection;

    private readonly ILogger _logger;
    private static bool _isInit = false;

    public MessageRepository(string connectionString,ILogger<MessageRepository> logger)
    {
        _logger = logger;
        _connectionString = connectionString;
        _dbConnection = new SqlConnection(connectionString);
        
        if (!_isInit)
        {
            CteateTable();
            _isInit = true;
        }
    }

    public Task CreateAsync(Message message)
    {
            var sqlQuery = "INSERT INTO Messages (SerialNumber, DateCreated, Content) VALUES(@SerialNumber, @DateCreated, @Content)";
            _logger.LogInformation("Sql query {sqlQuery}",sqlQuery);

            return _dbConnection.ExecuteAsync(sqlQuery, message);

            // если нужно получить id
            //var sqlQuery = "INSERT INTO Users (SerialNumber, DateCreation,Text) VALUES(@SerialNumber, @DateCreation, @Text); SELECT CAST(SCOPE_IDENTITY() as int)";
            //int? messageId = db.Query<int>(sqlQuery, message).FirstOrDefault();
            //message.Id = messageId.Value;
    }

    public Task<IEnumerable<Message>> GetByDateAsync(DateTime startDate, DateTime endDate)
    {
            var sqlQuery = """
                SELECT *
                FROM Messages
                WHERE DateCreated BETWEEN @startDate AND @endDate;
                """;

            _logger.LogInformation("Sql query {sqlQuery}", sqlQuery);
            return _dbConnection.QueryAsync<Message>(sqlQuery, new { startDate, endDate });
    }

    ~MessageRepository()
    {
        _dbConnection.Dispose();
    }

    public void CteateTable()
    {
        var sqlQuery = @"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Messages') SELECT 1 ELSE SELECT 0";

        _logger.LogInformation("Sql query {sqlQuery}", sqlQuery);
        // Проверка существования таблицы
        var tableExists = _dbConnection.ExecuteScalar<bool>(sqlQuery);

        if (!tableExists)
        {
            sqlQuery = """
                CREATE TABLE Messages (
                    Id INT PRIMARY KEY IDENTITY,
                    SerialNumber INT NOT NULL,
                    DateCreated DATETIME NOT NULL,
                    Content NVARCHAR(128) NOT NULL
                );
                """;
            _logger.LogInformation("Sql query {sqlQuery}", sqlQuery);
            // Создание таблицы, если она не существует
            _dbConnection.Execute(sqlQuery);
        }
        else
        {
            Console.WriteLine("Таблица уже существует.");
        }
    }

}
