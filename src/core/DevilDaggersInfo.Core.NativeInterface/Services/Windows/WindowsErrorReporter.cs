using DevilDaggersInfo.Core.NativeInterface.Services;

namespace DevilDaggersInfo.Core.NativeInterface.Services.Windows;

public class WindowsErrorReporter : INativeErrorReporter
{
	public void ReportError(Exception exception)
	{
		// TODO: Log exception.
	}

	public void ReportError(string message)
	{
	}
}
