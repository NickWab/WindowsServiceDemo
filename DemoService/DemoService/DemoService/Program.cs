using DemoService;

IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService(config =>
    {
        config.ServiceName = "DemoServiceApp";
    })
    .ConfigureServices(services => { services.AddHostedService<Worker>(); })
    .Build();

host.Run();