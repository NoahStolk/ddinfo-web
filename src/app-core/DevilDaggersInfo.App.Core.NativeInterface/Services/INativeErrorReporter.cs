namespace DevilDaggersInfo.Core.NativeInterface.Services;

public interface INativeErrorReporter
{
	void ReportError(Exception exception);

	void ReportError(string message);
}
