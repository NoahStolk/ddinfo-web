using DevilDaggersInfo.App.Core.NativeInterface.Native.Windows;
using System.Runtime.InteropServices;

namespace DevilDaggersInfo.App.Core.NativeInterface.Services.Windows;

/// <summary>
/// Platform-specific code for interacting with the Windows file system.
/// </summary>
public class WindowsFileSystemService : INativeFileSystemService
{
	public string? GetFilePathFromDialog(string dialogTitle, string? extensionFilter)
	{
		OpenFileName ofn = new()
		{
			filter = extensionFilter, // Example: "Log files\0*.log\0Batch files\0*.bat\0",
			file = new(new char[256]),
			maxFile = 256,
			fileTitle = new(new char[64]),
			maxFileTitle = 64,
			initialDir = "C:\\",
			title = "Open file",
		};
		ofn.structSize = Marshal.SizeOf(ofn);

		if (!NativeMethods.GetOpenFileName(ofn) || ofn.file == null)
			return null;

		return ofn.file;
	}

	public string? SelectDirectory()
	{
		// TODO: Open directory dialog.
		throw new NotImplementedException();
	}
}
