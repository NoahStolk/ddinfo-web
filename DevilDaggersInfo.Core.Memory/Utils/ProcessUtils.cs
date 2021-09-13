namespace DevilDaggersInfo.Core.Memory.Utils;

public static class ProcessUtils
{
	public static Process? GetDevilDaggersProcess(SupportedOs supportedOs)
	{
		string processWindowTitle = GetProcessWindowTitle(supportedOs);
		foreach (Process process in Process.GetProcessesByName(GetProcessName(supportedOs)))
		{
			if (process.MainWindowTitle == processWindowTitle)
				return process;
		}

		return null;
	}

	public static string GetProcessName(SupportedOs supportedOs) => supportedOs switch
	{
		SupportedOs.Windows => "dd",
		SupportedOs.Linux => "devildaggers",
		_ => throw new OperatingSystemNotSupportedException(),
	};

	public static string GetProcessWindowTitle(SupportedOs supportedOs) => supportedOs switch
	{
		SupportedOs.Windows => "Devil Daggers",
		SupportedOs.Linux => string.Empty,
		_ => throw new OperatingSystemNotSupportedException(),
	};
}
