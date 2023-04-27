using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using DevilDaggersInfo.Web.Server.Utils;
using System.IO.Compression;

namespace DevilDaggersInfo.Web.Server.Services;

public class FileSystemService : IFileSystemService
{
	public FileSystemService()
	{
		foreach (DataSubDirectory e in (DataSubDirectory[])Enum.GetValues(typeof(DataSubDirectory)))
			Directory.CreateDirectory(GetPath(e));
	}

	public string[] TryGetFiles(DataSubDirectory subDirectory)
	{
		try
		{
			return Directory.GetFiles(GetPath(subDirectory));
		}
		catch
		{
			return Array.Empty<string>();
		}
	}

	public string GetLeaderboardHistoryPathFromDate(DateTime dateTime)
	{
		string[] paths = TryGetFiles(DataSubDirectory.LeaderboardHistory);
		foreach (string path in paths.Where(p => p.EndsWith(".bin")).OrderByDescending(p => p))
		{
			if (HistoryUtils.HistoryFileNameToDateTime(Path.GetFileName(path)) <= dateTime)
				return path;
		}

		return paths[0];
	}

	public string GetPath(DataSubDirectory subDirectory)
	{
		return Path.Combine("Data", subDirectory.ToString());
	}

	public long GetDirectorySize(string path)
	{
		DirectoryInfo directoryInfo = new(path);
		return directoryInfo.EnumerateFiles("*.*", SearchOption.AllDirectories).Sum(fi => fi.Length);
	}

	public long GetFileSize(string path)
	{
		return new FileInfo(path).Length;
	}

	public string GetToolDistributionPath(string name, ToolPublishMethod publishMethod, ToolBuildType buildType, string version)
	{
		return Path.Combine(GetPath(DataSubDirectory.Tools), $"{name}-{version}-{buildType}-{publishMethod}.zip");
	}

	public bool DeleteFileIfExists(string path)
	{
		bool exists = File.Exists(path);
		if (exists)
			File.Delete(path);

		return exists;
	}

	public bool FileExists(string path)
	{
		return File.Exists(path);
	}

	public bool DeleteDirectoryIfExists(string path, bool recursive)
	{
		bool exists = Directory.Exists(path);
		if (exists)
			Directory.Delete(path, recursive);

		return exists;
	}

	public bool DirectoryExists(string path)
	{
		return Directory.Exists(path);
	}

	public void MoveDirectory(string sourcePath, string destinationPath)
	{
		Directory.Move(sourcePath, destinationPath);
	}

	public void CreateDirectory(string path)
	{
		Directory.CreateDirectory(path);
	}

	public string[] GetFiles(string path)
	{
		return Directory.GetFiles(path);
	}

	public string[] GetFiles(string path, string searchPattern)
	{
		return Directory.GetFiles(path, searchPattern);
	}

	public byte[] ReadAllBytes(string path)
	{
		return File.ReadAllBytes(path);
	}

	public async Task<byte[]> ReadAllBytesAsync(string path)
	{
		return await File.ReadAllBytesAsync(path);
	}

	public async Task<byte[]> ReadAllBytesAsync(string path, CancellationToken cancellationToken)
	{
		return await File.ReadAllBytesAsync(path, cancellationToken);
	}

	public void WriteAllBytes(string path, byte[] bytes)
	{
		File.WriteAllBytes(path, bytes);
	}

	public async Task WriteAllBytesAsync(string path, byte[] bytes)
	{
		await File.WriteAllBytesAsync(path, bytes);
	}

	public async Task WriteAllBytesAsync(string path, byte[] bytes, CancellationToken cancellationToken)
	{
		await File.WriteAllBytesAsync(path, bytes, cancellationToken);
	}

	public void MoveFile(string sourcePath, string destinationPath)
	{
		File.Move(sourcePath, destinationPath);
	}

	public string ReadAllText(string path)
	{
		return File.ReadAllText(path);
	}

	public async Task<string> ReadAllTextAsync(string path)
	{
		return await File.ReadAllTextAsync(path);
	}

	public async Task<string> ReadAllTextAsync(string path, CancellationToken cancellationToken)
	{
		return await File.ReadAllTextAsync(path, cancellationToken);
	}

	public void WriteAllText(string path, string text)
	{
		File.WriteAllText(path, text);
	}

	public async Task WriteAllTextAsync(string path, string text)
	{
		await File.WriteAllTextAsync(path, text);
	}

	public async Task WriteAllTextAsync(string path, string text, CancellationToken cancellationToken)
	{
		await File.WriteAllTextAsync(path, text, cancellationToken);
	}

	public async Task CreateZipFileAsync(string zipFilePath, Func<ZipArchive, Task> func)
	{
		using ZipArchive archive = ZipFile.Open(zipFilePath, ZipArchiveMode.Create);
		await func(archive);
	}

	public ZipArchive OpenZipArchive(string zipFilePath)
	{
		return ZipFile.Open(zipFilePath, ZipArchiveMode.Read);
	}
}
