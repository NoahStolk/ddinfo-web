namespace DevilDaggersInfo.App.Core.NativeInterface.Services;

public interface INativeFileSystemService
{
	string? GetFilePathFromDialog(string dialogTitle, string? extensionFilter);

	string? SelectDirectory();
}
