using MessageExchange.Repositories;
using Serilog;


namespace MessageExchange;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        #region Configuration Serilog
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        builder.Host.UseSerilog();
        #endregion

        string connectionString = "Server=(localdb)\\mssqllocaldb;Database=messagedb;Trusted_Connection=True";
        builder.Services.AddScoped<IMessageRepository>(provider => new MessageRepository(connectionString, provider.GetRequiredService<ILogger<MessageRepository>>()));

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            var filePath = Path.Combine(AppContext.BaseDirectory, "MessageExchange.xml");
            c.IncludeXmlComments(filePath);
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.UseWebSockets();

        app.Run();
    }
}
