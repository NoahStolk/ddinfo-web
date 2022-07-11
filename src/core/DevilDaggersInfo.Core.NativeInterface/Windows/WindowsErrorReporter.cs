using DevilDaggersInfo.Core.NativeInterface;

namespace DevilDaggersInfo.Core.NativeInterface.Windows;

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
