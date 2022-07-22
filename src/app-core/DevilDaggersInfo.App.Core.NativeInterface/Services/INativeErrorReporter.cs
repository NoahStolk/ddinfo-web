namespace DevilDaggersInfo.App.Core.NativeInterface.Services;

public interface INativeErrorReporter
{
	void ReportError(string title, string message, Exception? exception = null);
}
