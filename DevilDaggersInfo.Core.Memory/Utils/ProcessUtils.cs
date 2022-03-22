namespace DevilDaggersInfo.Core.Memory.Utils;

public static class ProcessUtils
{
	public static Process? GetDevilDaggersProcess(SupportedOs supportedOs)
	{
		if (supportedOs == SupportedOs.Linux)
			return Array.Find(Process.GetProcesses(), p => p.ProcessName.StartsWith("devildaggers"));

		if (supportedOs == SupportedOs.Windows)
			return Array.Find(Process.GetProcessesByName("dd"), p => p.MainWindowTitle == "Devil Daggers");

		throw new PlatformNotSupportedException();
	}
}
