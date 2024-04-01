using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;
using Serilog;

class Program
{
    static void Main(string[] args)
    {
        // Configure Serilog for logging
        Log.Logger = new LoggerConfiguration()
            .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        string serviceName = "DemoServiceApp";

        while (true)
        {
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
    }

    static bool IsServiceRunning(string serviceName)
    {
        Process process = new Process();
        process.StartInfo.FileName = "sc.exe";
        process.StartInfo.Arguments = $"query {serviceName} | findstr RUNNING";
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.Start();
        string output = process.StandardOutput.ReadToEnd();
        process.WaitForExit();
        return output.Contains("RUNNING");
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
