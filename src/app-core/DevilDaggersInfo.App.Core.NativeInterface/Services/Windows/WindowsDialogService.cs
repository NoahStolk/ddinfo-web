using DevilDaggersInfo.App.Core.NativeInterface.Native.Windows;

namespace DevilDaggersInfo.App.Core.NativeInterface.Services.Windows;

public class WindowsDialogService : INativeDialogService
{
	public void ReportError(string title, string message, Exception? exception = null)
	{
		if (exception != null)
			message += Environment.NewLine + exception.Message;

		// TODO: Log exception.
		NativeMethods.MessageBox(IntPtr.Zero, message, title, 0);
	}

	public void ReportMessage(string title, string message)
	{
		NativeMethods.MessageBox(IntPtr.Zero, message, title, 0);
	}

	public bool? PromptYesNo(string title, string message)
	{
		int result = NativeMethods.MessageBox(IntPtr.Zero, message, title, 0x00000004 /* MB_YESNO */);

		return result switch
		{
			/* IDYES */
			6 => true,
			/* IDNO */
			7 => false,
			_ => null,
		};
	}
}
