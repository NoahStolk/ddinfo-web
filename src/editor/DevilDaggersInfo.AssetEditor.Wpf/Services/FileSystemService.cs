using DevilDaggersInfo.Razor.Core.AssetEditor.Services;
using Microsoft.Win32;
using Ookii.Dialogs.Wpf;
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
