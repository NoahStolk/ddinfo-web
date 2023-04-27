using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using System.IO.Compression;
using System.Text;

namespace DevilDaggersInfo.Web.Server.Domain.Tests.TestImplementations;

public class TestFileSystemService : IFileSystemService
{
	private readonly Dictionary<string, byte[]> _files = new();

	public string[] TryGetFiles(DataSubDirectory subDirectory)
	{
		throw new NotImplementedException();
	}

	public string GetLeaderboardHistoryPathFromDate(DateTime dateTime)
	{
		throw new NotImplementedException();
	}

	public string GetPath(DataSubDirectory subDirectory)
	{
		return subDirectory.ToString();
	}

	public long GetDirectorySize(string path)
	{
		return _files.Where(f => f.Key.StartsWith(path)).Sum(f => f.Value.Length);
	}

	public string GetToolDistributionPath(string name, ToolPublishMethod publishMethod, ToolBuildType buildType, string version)
	{
		throw new NotImplementedException();
	}

	public bool DeleteFileIfExists(string path)
	{
		return _files.Remove(path);
	}

	public bool FileExists(string path)
	{
		return _files.ContainsKey(path);
	}

	public bool DeleteDirectoryIfExists(string path, bool recursive)
	{
		bool deleted = false;
		for (int i = _files.Count - 1; i >= 0; i--)
		{
			// TODO: Implement recursive.
			string p = _files.ElementAt(i).Key;
			if (!p.StartsWith(path))
				continue;

			_files.Remove(p);
			deleted = true;
		}

		return deleted;
	}

	public bool DirectoryExists(string path)
	{
		return _files.Any(f => f.Key.StartsWith(path));
	}

	public void MoveDirectory(string sourcePath, string destinationPath)
	{
		throw new NotImplementedException();
	}

	public void CreateDirectory(string path)
	{
		throw new NotImplementedException();
	}

	public string[] GetFiles(string path)
	{
		return _files.Where(f => f.Key.StartsWith(path)).Select(f => f.Key).ToArray();
	}

	public string[] GetFiles(string path, string searchPattern)
	{
		// TODO: Fix search pattern.
		return _files.Where(f => f.Key.StartsWith(path) && f.Key.Contains(searchPattern)).Select(f => f.Key).ToArray();
	}

	public byte[] ReadAllBytes(string path)
	{
		return ReadAllBytesImpl(path);
	}

	public async Task<byte[]> ReadAllBytesAsync(string path)
	{
		await Task.Yield();
		return ReadAllBytesImpl(path);
	}

	public async Task<byte[]> ReadAllBytesAsync(string path, CancellationToken cancellationToken)
	{
		await Task.Yield();
		return ReadAllBytesImpl(path);
	}

	private byte[] ReadAllBytesImpl(string path)
	{
		if (_files.TryGetValue(path, out byte[]? bytes))
			return bytes;

		throw new FileNotFoundException();
	}

	public void WriteAllBytes(string path, byte[] bytes)
	{
		WriteAllBytesImpl(path, bytes);
	}

	public async Task WriteAllBytesAsync(string path, byte[] bytes)
	{
		await Task.Yield();
		WriteAllBytesImpl(path, bytes);
	}

	public async Task WriteAllBytesAsync(string path, byte[] bytes, CancellationToken cancellationToken)
	{
		await Task.Yield();
		WriteAllBytesImpl(path, bytes);
	}

	private void WriteAllBytesImpl(string path, byte[] bytes)
	{
		_files[path] = bytes;
	}

	public void MoveFile(string sourcePath, string destinationPath)
	{
		throw new NotImplementedException();
	}

	public string ReadAllText(string path)
	{
		return Encoding.UTF8.GetString(ReadAllBytesImpl(path));
	}

	public async Task<string> ReadAllTextAsync(string path)
	{
		await Task.Yield();
		return Encoding.UTF8.GetString(ReadAllBytesImpl(path));
	}

	public async Task<string> ReadAllTextAsync(string path, CancellationToken cancellationToken)
	{
		await Task.Yield();
		return Encoding.UTF8.GetString(ReadAllBytesImpl(path));
	}

	public void WriteAllText(string path, string text)
	{
		throw new NotImplementedException();
	}

	public Task WriteAllTextAsync(string path, string text)
	{
		throw new NotImplementedException();
	}

	public Task WriteAllTextAsync(string path, string text, CancellationToken cancellationToken)
	{
		throw new NotImplementedException();
	}

	public async Task CreateZipFileAsync(string zipFilePath, Func<ZipArchive, Task> func)
	{
		using MemoryStream memoryStream = new();
		using ZipArchive archive = new(memoryStream, ZipArchiveMode.Create);
		await func(archive);
		WriteAllBytesImpl(zipFilePath, memoryStream.ToArray());
	}
}
