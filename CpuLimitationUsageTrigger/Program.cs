using System.Management;

// Set trigger for CPU usage to 80%
int cpuTrigger = 20;

while (true)
{
    try
    {
        // Collect hardware info
        string processorName = string.Empty;
        string chassisType = string.Empty;
        ulong totalMemory = 0;
        ulong totalStorage = 0;

        using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Processor"))
        {
            foreach (var obj in searcher.Get())
            {
                processorName = obj["Name"].ToString();
                break;
            }
        }

        using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_SystemEnclosure"))
        {
            foreach (var obj in searcher.Get())
            {
                chassisType = obj["ChassisTypes"].ToString();
                break;
            }
        }

        using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem"))
        {
            foreach (var obj in searcher.Get())
            {
                totalMemory = Convert.ToUInt64(obj["TotalVisibleMemorySize"]);
                totalStorage = Convert.ToUInt64(obj["TotalVisibleMemorySize"]);
                break;
            }
        }

        

        // Check trigger for CPU usage
        using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PerfFormattedData_PerfOS_Processor WHERE Name='_Total'"))
        {
            foreach (var obj in searcher.Get())
            {
                int cpuUsage = Convert.ToInt32(obj["PercentProcessorTime"]);
                if (cpuUsage >= cpuTrigger)
                {
                    // Do something when CPU usage reaches trigger
                }
                break;
            }
        }

        // Wait for set interval before uploading again
        await Task.Delay(TimeSpan.FromSeconds(30));
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}