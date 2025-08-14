using System;
using System.ServiceProcess;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Serilog;

// Note: This project requires the nuget package System.ServiceProcess.ServiceController

class Program
{
    static void Main(string[] args)
    {
        // Configure Serilog for logging
        Log.Logger = new LoggerConfiguration()
            .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        string serviceName = configuration["ServiceName"];

        while (true)
        {
            try
            {
                using (var sc = new ServiceController(serviceName))
                {
                    if (sc.Status == ServiceControllerStatus.Running)
                    {
                        Log.Information("Service '{ServiceName}' is running.", serviceName);
                    }
                    else
                    {
                        Log.Warning("Service '{ServiceName}' is not running. Current status: {Status}. Attempting to start...", serviceName, sc.Status);
                        sc.Start();
                        sc.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(30));
                        Log.Information("Service '{ServiceName}' started successfully.", serviceName);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while monitoring service '{ServiceName}'.", serviceName);
            }

            Thread.Sleep(60000); // 1 minute for retry.
        }
    }
}
