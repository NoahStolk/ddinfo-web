namespace DevilDaggersInfo.App.Core.NativeInterface.Services;

public interface INativeFileSystemService
{
	FileResult? OpenFile(string? extensionFilter);

	void SaveDataToFile(byte[] data);

	string? SelectDirectory();

	public record FileResult(string Path, byte[] Contents);
}
