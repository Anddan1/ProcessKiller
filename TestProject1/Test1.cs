using Microsoft.VisualStudio.TestPlatform.TestHost;
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

[TestFixture]
public class ProgramTests
{
    private string processName;
    private int maxLifetimeMinutes;
    private int monitoringFrequencyMinutes;
    private string logFilePath;

    [SetUp]
    public void SetUp()
    {
        processName = "test_process";
        maxLifetimeMinutes = 2;
        monitoringFrequencyMinutes = 1;
        logFilePath = Path.Combine(Environment.CurrentDirectory, "ProcessMonitorLog.txt");
    }

    [TearDown]
    public void Cleanup()
    {
        if (File.Exists(logFilePath))
        {
            File.Delete(logFilePath);
        }
    }

    [Test]
    public void Test_ProcessMonitor()
    {
        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = "dotnet",
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            Arguments = $"run --project {Path.Combine(Environment.CurrentDirectory, "TestProject1.csproj")}"
        };
        Process process = new Process { StartInfo = psi };
        process.Start();

        Thread.Sleep(2000); 
        process.StandardInput.WriteLine($"{processName} {maxLifetimeMinutes} {monitoringFrequencyMinutes}");
        process.StandardInput.Flush();
        Thread.Sleep(2000); 
        process.StandardInput.WriteLine("q");
        process.StandardInput.Flush();
        process.WaitForExit();

        // Assert
        Assert.IsFalse(File.Exists(logFilePath), "Log file should not exist.");
    }

    [Test]
    public void Test_ProcessMonitor_ValidArguments()
    {
        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = "dotnet",
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            Arguments = $"run --project {Path.Combine(Environment.CurrentDirectory, "TestProject1.csproj")}"
        };
        Process process = new Process { StartInfo = psi };
        process.Start();

        try
        {
            Thread.Sleep(2000); 
            process.StandardInput.WriteLine($"{processName} {maxLifetimeMinutes} {monitoringFrequencyMinutes}");
            process.StandardInput.Flush();
            Thread.Sleep(2000); 

            Assert.IsTrue(process.HasExited, "Process should have exited.");
            Assert.IsFalse(File.Exists(logFilePath), "Log file should not exist.");
        }
        finally
        {
            process.Kill();
            process.WaitForExit();
        }
    }

    [Test]
    public void Test_ProcessMonitor_InvalidArguments()
    {
        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = "dotnet",
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            Arguments = $"run --project {Path.Combine(Environment.CurrentDirectory, "TestProject1.csproj")}"
        };
        Process process = new Process { StartInfo = psi };
        process.Start();

        try
        {
            Thread.Sleep(2000); 
            process.StandardInput.WriteLine($"{processName} invalidArgument {monitoringFrequencyMinutes}");
            process.StandardInput.Flush();
            Thread.Sleep(2000); 

            Assert.IsTrue(process.HasExited, "Process should have exited due to invalid arguments.");
            Assert.IsFalse(File.Exists(logFilePath), "Log file should not exist.");
        }
        finally
        {
            process.WaitForExit();
            process.Close();
        }
    }



    [Test]
    public void Test_ProcessMonitor_NoProcessFound()
    {
        string processName = "NonExistentProcess";
        int maxLifetimeMinutes = 60;
        int monitoringFrequencyMinutes = 5;

        Thread.Sleep(2000); 

        Assert.IsFalse(File.Exists(logFilePath), "Log file should not exist.");
    }


    [Test]
    public void Test_ProcessMonitor_ManualExit()
    {
        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = "dotnet",
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            Arguments = $"run --project {Path.Combine(Environment.CurrentDirectory, "TestProject1.csproj")}"
        };
        Process process = new Process { StartInfo = psi };
        process.Start();

        try
        {
            Thread.Sleep(2000); 
            process.StandardInput.WriteLine($"{processName} {maxLifetimeMinutes} {monitoringFrequencyMinutes}");
            process.StandardInput.Flush();
            Thread.Sleep(2000); 

            process.StandardInput.WriteLine("q");
            process.StandardInput.Flush();

            Assert.IsTrue(process.HasExited, "Process should have exited due to manual exit.");
            Assert.IsFalse(File.Exists(logFilePath), "Log file should not exist.");
        }
        finally
        {
            process.Kill();
            process.WaitForExit();
        }
    }

    [Test]
    public void Test_ProcessMonitor_MultipleProcesses()
    {
        
        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = "dotnet",
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            Arguments = $"run --project {Path.Combine(Environment.CurrentDirectory, "TestProject1.csproj")}"
        };
        Process process1 = new Process { StartInfo = psi };
        Process process2 = new Process { StartInfo = psi };
        process1.Start();
        process2.Start();

        try
        {
            Thread.Sleep(2000); 
            process1.StandardInput.WriteLine($"{processName} {maxLifetimeMinutes} {monitoringFrequencyMinutes}");
            process1.StandardInput.Flush();
            Thread.Sleep(2000); 

            Assert.IsTrue(process1.HasExited, "Process 1 should have exited.");
            Assert.IsTrue(process2.HasExited, "Process 2 should have exited.");
            Assert.IsFalse(File.Exists(logFilePath), "Log file should not exist.");
        }
        finally
        {
            process1.Kill();
            process1.WaitForExit();
            process2.Kill();
            process2.WaitForExit();
        }
    }

    [Test]
    public void Test_ProcessMonitor_ProcessTermination()
    {
        string processName = "dummyProcess";
        int maxLifetimeMinutes = 1;
        int monitoringFrequencyMinutes = 1;

        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = "dotnet",
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            Arguments = $"run --project {Path.Combine(Environment.CurrentDirectory, "TestProject1.csproj")}"
        };
        Process process = new Process { StartInfo = psi };
        process.Start();

        try
        {
            Thread.Sleep(2000); 
            process.StandardInput.WriteLine($"{processName} {maxLifetimeMinutes} {monitoringFrequencyMinutes}");
            process.StandardInput.Flush();
            Thread.Sleep(2000); 

            
            Assert.IsTrue(process.HasExited, "Process should have exited.");
            Assert.IsFalse(File.Exists(logFilePath), "Log file should not exist.");
        }
        finally
        {
            process.WaitForExit();
            process.Close();
        }
    }



}

