using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

class Program
{
    
    private static int monitoringFrequencyInMinutes;

    static void Main(string[] args)
    {
        if (args.Length < 3)
        {
            Console.WriteLine("Usage: ProcessMonitor <processName> <maxLifetimeMinutes> <monitoringFrequencyMinutes>");
            return;
        }

        string processName = args[0];
        int maxLifetimeMinutes;

        if (!int.TryParse(args[1], out maxLifetimeMinutes))
        {
            Console.WriteLine("Invalid maxLifetimeMinutes value.");
            return;
        }

        int monitoringFrequencyMinutes;

        if (!int.TryParse(args[2], out monitoringFrequencyMinutes))
        {
            Console.WriteLine("Invalid monitoringFrequencyMinutes value.");
            return;
        }

        Console.WriteLine($"Starting process monitor for {processName} (max lifetime: {maxLifetimeMinutes} minutes, monitoring frequency: {monitoringFrequencyMinutes} minutes)...");
        Console.WriteLine("Press 'q' to exit.");

        do
        {
            Process[] processes = Process.GetProcessesByName(processName);

            if (processes.Length > 0)
            {
                foreach (Process process in processes)
                {
                    TimeSpan processAge = DateTime.Now - process.StartTime;

                    if (processAge.TotalMinutes > maxLifetimeMinutes)
                    {
                        Console.WriteLine($"Process {process.ProcessName} (PID: {process.Id}) has been running for {processAge.TotalMinutes} minutes (threshold: {maxLifetimeMinutes} minutes)");
                        process.Kill();
                        string log = $"Process {processName} with PID {process.Id} was killed at {DateTime.Now}";
                        Console.WriteLine(log);
                        string logFilePath = Path.Combine(Environment.CurrentDirectory, "ProcessMonitorLog.txt");
                        File.AppendAllText(logFilePath, log + Environment.NewLine);
                    }
                }
            }

            Thread.Sleep(monitoringFrequencyInMinutes * 60 * 1000);

        } while (!Console.KeyAvailable || Console.ReadKey(true).Key != ConsoleKey.Q);
    }
}