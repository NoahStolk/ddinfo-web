using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using Microsoft.EntityFrameworkCore;

namespace DevilDaggersInfo.Web.Server.Domain.Repositories;

public class CustomEntryRepository
{
	private readonly ApplicationDbContext _dbContext;
	private readonly IFileSystemService _fileSystemService;

	public CustomEntryRepository(ApplicationDbContext dbContext, IFileSystemService fileSystemService)
	{
		_dbContext = dbContext;
		_fileSystemService = fileSystemService;
	}

	public async Task<byte[]> GetCustomEntryReplayBufferByIdAsync(int id)
	{
		string path = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.CustomEntryReplays), $"{id}.ddreplay");
		if (!_fileSystemService.FileExists(path))
			throw new NotFoundException($"Replay file with ID '{id}' could not be found.");

		return await _fileSystemService.ReadAllBytesAsync(path);
	}

	public async Task<(string FileName, byte[] Contents)> GetCustomEntryReplayByIdAsync(int id)
	{
		string path = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.CustomEntryReplays), $"{id}.ddreplay");
		if (!_fileSystemService.FileExists(path))
			throw new NotFoundException($"Replay file with ID '{id}' could not be found.");

		// ! Navigation property.
		var customEntry = _dbContext.CustomEntries
			.AsNoTracking()
			.Select(ce => new
			{
				ce.Id,
				ce.CustomLeaderboard!.SpawnsetId,
				SpawnsetName = ce.CustomLeaderboard.Spawnset!.Name,
				ce.PlayerId,
				ce.Player!.PlayerName,
			})
			.FirstOrDefault(ce => ce.Id == id);
		if (customEntry == null)
			throw new NotFoundException($"Custom entry replay '{id}' could not be found.");

		string fileName = $"{customEntry.SpawnsetId}-{customEntry.SpawnsetName}-{customEntry.PlayerId}-{customEntry.PlayerName}.ddreplay";
		return (fileName, await _fileSystemService.ReadAllBytesAsync(path));
	}

	public List<int> GetExistingCustomEntryReplayIds(List<int> ids)
	{
		return ids
			.Where(id => _fileSystemService.FileExists(Path.Combine(_fileSystemService.GetPath(DataSubDirectory.CustomEntryReplays), $"{id}.ddreplay")))
			.Select(id => id)
			.ToList();
	}
}
