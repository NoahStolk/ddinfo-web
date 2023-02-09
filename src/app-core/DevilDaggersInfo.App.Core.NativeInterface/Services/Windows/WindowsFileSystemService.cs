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

		try
		{
			openDialog.Show(default);
		}
		catch (COMException)
		{
			// Return null when the user cancels the dialog.
			return null;
		}

		openDialog.GetResult(out IShellItem item);
		item.GetDisplayName(SIGDN.SIGDN_FILESYSPATH, out PWSTR name);
		Marshal.FreeCoTaskMem(new(name.Value));
		return name.ToString();
	}

	public unsafe string? CreateSaveFileDialog(string dialogTitle, string? extensionFilter)
	{
		PInvoke.CoCreateInstance(
			typeof(FileSaveDialog).GUID,
			null,
			CLSCTX.CLSCTX_INPROC_SERVER,
			out IFileSaveDialog saveDialog).ThrowOnFailure();

		try
		{
			saveDialog.Show(default);
		}
		catch (COMException)
		{
			// Return null when the user cancels the dialog.
			return null;
		}

		saveDialog.GetResult(out IShellItem item);
		item.GetDisplayName(SIGDN.SIGDN_FILESYSPATH, out PWSTR name);
		Marshal.FreeCoTaskMem(new(name.Value));
		return name.ToString();
	}

	public unsafe string? SelectDirectory()
	{
		PInvoke.CoCreateInstance(
			typeof(FileOpenDialog).GUID,
			null,
			CLSCTX.CLSCTX_INPROC_SERVER,
			out IFileOpenDialog openDialog).ThrowOnFailure();

		openDialog.SetOptions(FILEOPENDIALOGOPTIONS.FOS_PICKFOLDERS);

		try
		{
			openDialog.Show(default);
		}
		catch (COMException)
		{
			// Return null when the user cancels the dialog.
			return null;
		}

		openDialog.GetResult(out IShellItem item);
		item.GetDisplayName(SIGDN.SIGDN_FILESYSPATH, out PWSTR name);
		Marshal.FreeCoTaskMem(new(name.Value));
		return name.ToString();
	}
}
