using DevilDaggersInfo.Core.Spawnset.Summary;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using System.Collections.Concurrent;

namespace DevilDaggersInfo.Web.Server.Domain.Services.Caching;

public class SpawnsetSummaryCache
{
	private readonly ConcurrentDictionary<string, SpawnsetSummary> _cache = new();

	private readonly IFileSystemService _fileSystemService;

	public SpawnsetSummaryCache(IFileSystemService fileSystemService)
	{
		_fileSystemService = fileSystemService;
	}

	public SpawnsetSummary GetSpawnsetSummaryByFilePath(string filePath)
	{
		string name = Path.GetFileNameWithoutExtension(filePath);
		if (_cache.TryGetValue(name, out SpawnsetSummary? summary))
			return summary;

		if (!SpawnsetSummary.TryParse(_fileSystemService.ReadAllBytes(filePath), out SpawnsetSummary? spawnsetSummary))
			throw new($"Failed to get spawnset summary from spawnset file: '{name}'.");

		_cache.TryAdd(name, spawnsetSummary);
		return spawnsetSummary;
	}

	public void Clear()
		=> _cache.Clear();

	public int GetCount()
		=> _cache.Count;
}
