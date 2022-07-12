namespace DevilDaggersInfo.Core.NativeInterface.Services;

public interface INativeFileSystemService
{
	FileResult? OpenFile(string? extensionFilter);

	void SaveDataToFile(byte[] data);

	string? SelectDirectory();

	public class FileResult
	{
		public FileResult(string path, byte[] contents)
		{
			Path = path;
			Contents = contents;
		}

		public string Path { get; }

		public byte[] Contents { get; }
	}
}
