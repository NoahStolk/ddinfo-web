namespace DevilDaggersInfo.Razor.Core.AssetEditor.Services;

public interface IErrorReporter
{
	void ReportError(Exception exception);

	void ReportError(string message);
}
