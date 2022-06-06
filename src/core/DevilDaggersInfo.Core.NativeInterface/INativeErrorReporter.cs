namespace DevilDaggersInfo.Core.NativeInterface;

public interface INativeErrorReporter
{
	void ReportError(Exception exception);

	void ReportError(string message);
}
