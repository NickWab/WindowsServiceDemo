using DemoService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.WindowsServices;

IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService()
    .ConfigureServices((hostContext, services) =>
    {
        services.AddHostedService<Worker>();
        services.Configure<WindowsServiceLifetimeOptions>(options =>
        {
            options.ServiceName = hostContext.Configuration.GetValue<string>("ServiceName");
        });
    })
    .Build();

host.Run();