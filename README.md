# ProcessKiller

The NUnit tests:

Test_ProcessMonitor_ValidArguments: Verify that the process monitor starts successfully and operates as expected when provided with valid arguments for process name, max lifetime minutes, and monitoring frequency minutes. Assert that the monitor is running and performing the desired actions.

Test_ProcessMonitor_InvalidArguments: Test the behavior when invalid or missing arguments are provided to the process monitor. Assert that appropriate error messages are displayed and the monitor does not start.

Test_ProcessMonitor_ProcessTermination: Test the scenario where the monitored process exceeds the maximum allowed lifetime and should be terminated. Start a dummy process and configure the process monitor with a short max lifetime and monitoring frequency. Assert that the process is killed by the monitor and verify the log file contents.

Test_ProcessMonitor_NoProcessFound: Test the behavior when the process monitor cannot find any processes matching the specified name. Start a different process or provide a non-existent process name. Assert that the monitor does not take any action and continues to run.

Test_ProcessMonitor_ManualExit: Test the scenario where the user manually exits the process monitor by pressing the 'q' key. Assert that the monitor exits gracefully and stops monitoring.

Test_ProcessMonitor_MultipleProcesses: Test the behavior when multiple processes with the specified name are running simultaneously. Start multiple dummy processes and configure the monitor accordingly. Assert that the monitor correctly handles each process individually and terminates any processes that exceed the maximum lifetime.

Test_ProcessMonitor_LogFileCleanup: Test the cleanup behavior of the process monitor. Start a dummy process and configure the monitor with a short max lifetime and monitoring frequency. After the process is terminated, assert that the log file is created and contains the expected information. Then, wait for a sufficient amount of time for the log file cleanup mechanism to trigger, and assert that the log file no longer exists.
