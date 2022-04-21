using DevilDaggersInfo.Web.BlazorWasm.Client.Editor.Asset.Services;
using System.Windows;

namespace DevilDaggersInfo.Native.Editor.Asset.Wpf.Services;

public class ErrorReporter : IErrorReporter
{
	public void ReportError(Exception exception)
	{
		// TODO: Log exception.
		MessageBox.Show(exception.Message, exception.GetType().Name);
	}

	public void ReportError(string message)
	{
		MessageBox.Show(message, "Error");
	}
}
