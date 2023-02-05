namespace DevilDaggersInfo.App.Core.NativeInterface.Services.Linux;

/// <summary>
/// Platform-specific code for interacting with the Linux file system.
/// </summary>
public class LinuxFileSystemService : INativeFileSystemService
{
	public INativeFileSystemService.FileResult? OpenFile(string? extensionFilter)
	{
		// TODO: Open file dialog.
		throw new NotImplementedException();
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
