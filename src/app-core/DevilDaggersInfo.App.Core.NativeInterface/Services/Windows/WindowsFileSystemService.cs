using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.System.Com;
using Windows.Win32.UI.Shell;

namespace DevilDaggersInfo.App.Core.NativeInterface.Services.Windows;

/// <summary>
/// Platform-specific code for interacting with the Windows file system.
/// </summary>
[SupportedOSPlatform("windows6.0.6000")]
public class WindowsFileSystemService : INativeFileSystemService
{
	public unsafe string? CreateOpenFileDialog(string dialogTitle, string? extensionFilter)
	{
		PInvoke.CoCreateInstance(
			typeof(FileOpenDialog).GUID,
			null,
			CLSCTX.CLSCTX_INPROC_SERVER,
			out IFileOpenDialog openDialog).ThrowOnFailure();

		openDialog.Show(default);
		openDialog.GetResult(out IShellItem item);
		item.GetDisplayName(SIGDN.SIGDN_FILESYSPATH, out PWSTR name);
		Marshal.FreeCoTaskMem(new IntPtr(name.Value));
		return name.ToString();
	}

	public unsafe string? CreateSaveFileDialog(string dialogTitle, string? extensionFilter)
	{
		PInvoke.CoCreateInstance(
			typeof(FileSaveDialog).GUID,
			null,
			CLSCTX.CLSCTX_INPROC_SERVER,
			out IFileSaveDialog saveDialog).ThrowOnFailure();

		saveDialog.Show(default);
		saveDialog.GetResult(out IShellItem item);
		item.GetDisplayName(SIGDN.SIGDN_FILESYSPATH, out PWSTR name);
		Marshal.FreeCoTaskMem(new IntPtr(name.Value));
		return name.ToString();
	}

	public string? SelectDirectory()
	{
		// TODO: Open directory dialog.
		throw new NotImplementedException();
	}
}
