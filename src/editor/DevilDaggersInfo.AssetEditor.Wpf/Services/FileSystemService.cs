using DevilDaggersInfo.Razor.Core.AssetEditor.Services;
using Microsoft.Win32;
using System.IO;

namespace DevilDaggersInfo.AssetEditor.Wpf.Services;

public class FileSystemService : IFileSystemService
{
	public IFileSystemService.FileResult? Open()
	{
		OpenFileDialog dialog = new();
		bool? openResult = dialog.ShowDialog();
		if (!openResult.HasValue || !openResult.Value)
			return null;

		return new(dialog.FileName, File.ReadAllBytes(dialog.FileName));
	}
}
