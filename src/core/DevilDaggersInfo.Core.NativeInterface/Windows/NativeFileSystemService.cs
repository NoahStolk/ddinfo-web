using DevilDaggersInfo.Core.NativeInterface.Windows.Native;
using System.Runtime.InteropServices;

namespace DevilDaggersInfo.Core.NativeInterface.Windows;

/// <summary>
/// Platform-specific code for interacting with the OS file system.
/// </summary>
public class NativeFileSystemService : INativeFileSystemService
{
	public INativeFileSystemService.FileResult? OpenFile(string? extensionFilter)
	{
		OpenFileName ofn = new()
		{
			customFilter = extensionFilter,
			filter = extensionFilter, // "Log files\0*.log\0Batch files\0*.bat\0",
			file = new(new char[256]),
			maxFile = 256,
			fileTitle = new(new char[64]),
			maxFileTitle = 64,
			initialDir = "C:\\",
			title = "Open file called using platform invoke...",
			defExt = "txt", // ?
		};
		ofn.structSize = Marshal.SizeOf(ofn);

		if (!NativeMethods.GetOpenFileName(ofn))
			return null;

		if (ofn.file == null)
			return null;

		return new(ofn.file, File.ReadAllBytes(ofn.file));
	}

	public void SaveDataToFile(byte[] data)
	{
		// TODO: Save file dialog.
		throw new NotImplementedException();
	}

	public string? SelectDirectory()
	{
		// TODO: Open directory dialog.
		throw new NotImplementedException();
	}
}
