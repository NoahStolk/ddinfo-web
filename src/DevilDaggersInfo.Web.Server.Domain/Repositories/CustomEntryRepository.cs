using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using Microsoft.EntityFrameworkCore;

namespace DevilDaggersInfo.Web.Server.Domain.Repositories;

public sealed class CustomEntryRepository(ApplicationDbContext dbContext, IFileSystem fileSystem)
{
	public async Task<byte[]> GetCustomEntryReplayBufferByIdAsync(int id)
	{
		byte[]? contents = await fileSystem.ReadAllBytesAsync(DataSubDirectory.CustomEntryReplays, $"{id}.ddreplay");
		return contents ?? throw new NotFoundException($"Replay file with ID '{id}' could not be found.");
	}

	public async Task<(string FileName, byte[] Contents)> GetCustomEntryReplayByIdAsync(int id)
	{
		byte[]? contents = await fileSystem.ReadAllBytesAsync(DataSubDirectory.CustomEntryReplays, $"{id}.ddreplay");
		if (contents == null)
			throw new NotFoundException($"Replay file with ID '{id}' could not be found.");

		// ! Navigation property.
		var customEntry = await dbContext.CustomEntries
			.AsNoTracking()
			.Select(ce => new
			{
				ce.Id,
				ce.CustomLeaderboard!.SpawnsetId,
				SpawnsetName = ce.CustomLeaderboard.Spawnset!.Name,
				ce.PlayerId,
				ce.Player!.PlayerName,
			})
			.FirstOrDefaultAsync(ce => ce.Id == id);
		if (customEntry == null)
			throw new NotFoundException($"Custom entry replay '{id}' could not be found.");

		string fileName = $"{customEntry.SpawnsetId}-{customEntry.SpawnsetName}-{customEntry.PlayerId}-{customEntry.PlayerName}.ddreplay";
		return (fileName, contents);
	}

	public async Task<List<int>> GetExistingCustomEntryReplayIdsAsync(List<int> ids)
	{
		List<int> existingIds = [];
		foreach (int id in ids)
		{
			if (await fileSystem.ExistsAsync(DataSubDirectory.CustomEntryReplays, $"{id}.ddreplay"))
				existingIds.Add(id);
		}

		return existingIds;
	}
}
