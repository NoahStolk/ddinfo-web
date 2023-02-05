namespace DevilDaggersInfo.App.Core.NativeInterface.Services;

public interface INativeDialogService
{
	void ReportError(string title, string message, Exception? exception = null);

	void ReportMessage(string title, string message);
}
