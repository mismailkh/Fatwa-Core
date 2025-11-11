using FATWACONSUMER.RABBITMQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace FATWACONSUMER
{
    class Program
    {

        static void Main(string[] args)
        {
            // Create a Service Collection for DI
            var serviceCollection = new ServiceCollection();
            // Build Configuration
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            IConfiguration configuration;
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env}.json", true, true)
                .Build();
            
            // Add the Configuration to the service Collection
            serviceCollection.AddSingleton<IConfiguration>(configuration);

            Log.Logger = new LoggerConfiguration()
                  .MinimumLevel.Debug()
                  .WriteTo.File("FatwaConsumerlog.json")
                  .CreateLogger();
           
            RabbitMQPullConsumer client = new RabbitMQPullConsumer(configuration);
            Log.Information("Starting up");
            client.GetToken();
            client.CreateConnection();
            client.ProcessMessages();
            Log.Error("Consumer Stop");
        }
    }
}