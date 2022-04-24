using DevilDaggersInfo.Core.Asset.Enums;
using DevilDaggersInfo.Core.Asset.Extensions;
using DevilDaggersInfo.Razor.Core.AssetEditor.Services;
using Microsoft.Win32;
using Ookii.Dialogs.Wpf;
using System.IO;

namespace DevilDaggersInfo.AssetEditor.Wpf.Services;

/// <summary>
/// Platform-specific code for interacting with the OS file system.
/// </summary>
public class FileSystemService : IFileSystemService
{
	public string GetAssetTypeFilter(AssetType assetType)
	{
		string fileExtension = assetType.GetFileExtension();

		string filter = assetType switch
		{
			AssetType.Audio => "Audio",
			AssetType.Mesh => "Mesh",
			AssetType.ObjectBinding => "Text",
			AssetType.Shader => "Shader",
			AssetType.Texture => "Texture",
			_ => throw new NotSupportedException($"Asset type '{assetType}' is not supported."),
		};

		return $"{filter} files (*{fileExtension})|*{fileExtension}";
	}

	public IFileSystemService.FileResult? Open(string extensionFilter)
	{
		OpenFileDialog dialog = new()
		{
			Filter = extensionFilter,
		};
		bool? openResult = dialog.ShowDialog();
		if (!openResult.HasValue || !openResult.Value)
			return null;

		return new(dialog.FileName, File.ReadAllBytes(dialog.FileName));
	}

	public void Save(byte[] buffer)
	{
		SaveFileDialog dialog = new();
		bool? saveResult = dialog.ShowDialog();
		if (!saveResult.HasValue || !saveResult.Value)
			return;

		File.WriteAllBytes(dialog.FileName, buffer);
	}

	public string? SelectDirectory()
	{
		VistaFolderBrowserDialog folderDialog = new();
		if (folderDialog.ShowDialog() == true)
			return folderDialog.SelectedPath;

		return null;
	}
}
