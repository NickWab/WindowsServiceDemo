using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Serilog;

class Program
{
    static  IConfiguration   _configuration;

    public Program(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    static void Main(string[] args)
    {
        // Configure Serilog for logging
        Log.Logger = new LoggerConfiguration()
            .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json",optional:true,reloadOnChange:true);
        _configuration = builder.Build();
        
        string serviceName = _configuration["ServiceName"];


            try
            {
                if (IsServiceRunning(serviceName))
                {
                    Log.Information("Service is running.");
                }
                else
                {
                    Log.Information("Service is not running. Starting...");
                    StartService(serviceName);
                }

               
                Thread.Sleep(60000); // 1 minute for retry.
            }
            catch (Exception ex)
            {
                Log.Error($"Error: {ex.Message}");
                Thread.Sleep(60000); //  1 minute for retry.
            }
    }

    static bool IsServiceRunning(string serviceName)
    {
        Process process = new Process();
        process.StartInfo.FileName = "sc.exe";
        process.StartInfo.Arguments = $"query {serviceName}";
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.Start();
        string output = process.StandardOutput.ReadToEnd();
        process.WaitForExit();
    
        // Check if the service state is "RUNNING" in the output
        return output.Contains("STATE") && output.Contains("RUNNING");
    }

    static void StartService(string serviceName)
    {
        Process process = new Process();
        process.StartInfo.FileName = "sc.exe";
        process.StartInfo.Arguments = $"start {serviceName}";
        process.Start();
        process.WaitForExit();
    }
}
