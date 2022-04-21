namespace DevilDaggersInfo.Web.BlazorWasm.Client.Editor.Asset.Services;

public interface IErrorReporter
{
	void ReportError(Exception exception);

	void ReportError(string message);
}
