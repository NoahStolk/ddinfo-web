namespace DevilDaggersInfo.Core.Memory.Utils;

public static class ProcessUtils
{
	public static Process? GetDevilDaggersProcess(SupportedOs supportedOs)
	{
		if (supportedOs == SupportedOs.Linux)
		{
			foreach (Process process in Process.GetProcesses())
			{
				if (process.ProcessName.StartsWith("devildaggers"))
					return process;
			}

			return null;
		}
		else if (supportedOs == SupportedOs.Windows)
		{
			foreach (Process process in Process.GetProcessesByName("dd"))
			{
				if (process.MainWindowTitle == "Devil Daggers")
					return process;
			}

			return null;
		}
		else
		{
			throw new PlatformNotSupportedException();
		}
	}
}
