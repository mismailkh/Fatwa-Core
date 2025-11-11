using TestWorkerService;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        // services.AddHostedService<Worker>();
        services.AddHostedService<UserService>();
    })
    .Build();

await host.RunAsync();
