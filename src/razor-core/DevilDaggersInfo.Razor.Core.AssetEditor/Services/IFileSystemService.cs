using DevilDaggersInfo.Core.Asset.Enums;

namespace DevilDaggersInfo.Razor.Core.AssetEditor.Services;

public interface IFileSystemService
{
	string GetAssetTypeFilter(AssetType assetType);

	string GetVertexShaderFilter();

	string GetFragmentShaderFilter();

	FileResult? Open(string extensionFilter);

	void Save(byte[] buffer);

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
