using FATWA_DOMAIN.Interfaces;
using FATWA_INFRASTRUCTURE.Database;
using FATWA_INFRASTRUCTURE.Repository;
using Microsoft.EntityFrameworkCore;
using WorkflowSlaService;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        IConfiguration configuration = hostContext.Configuration;

        var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("LocalDBConnection"));
        services.AddScoped<DatabaseContext>(s => new DatabaseContext(optionsBuilder.Options));
        services.AddHostedService<Worker>();
        services.AddScoped<IAuditLog, AuditLogRepository>();
    })
    .Build();

await host.RunAsync();
