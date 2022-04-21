namespace DevilDaggersInfo.Web.BlazorWasm.Client.Editor.Asset.Services;

public interface IFileSystemService
{
	FileResult? Open();

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
