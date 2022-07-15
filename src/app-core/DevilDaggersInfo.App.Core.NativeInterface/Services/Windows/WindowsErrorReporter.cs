namespace DevilDaggersInfo.App.Core.NativeInterface.Services.Windows;

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
