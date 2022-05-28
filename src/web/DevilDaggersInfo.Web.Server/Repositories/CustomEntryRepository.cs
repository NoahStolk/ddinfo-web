namespace DevilDaggersInfo.Web.Server.Repositories;

public class CustomEntryRepository
{
	private readonly ApplicationDbContext _dbContext;
	private readonly IFileSystemService _fileSystemService;

	public CustomEntryRepository(ApplicationDbContext dbContext, IFileSystemService fileSystemService)
	{
		_dbContext = dbContext;
		_fileSystemService = fileSystemService;
	}

	public (string FileName, byte[] Contents) GetCustomEntryReplayById(int id)
	{
		string path = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.CustomEntryReplays), $"{id}.ddreplay");
		if (!IoFile.Exists(path))
			throw new NotFoundException($"Replay file with ID '{id}' could not be found.");

		var customEntry = _dbContext.CustomEntries
			.AsNoTracking()
			.Select(ce => new
			{
				ce.Id,
				ce.CustomLeaderboard.SpawnsetId,
				SpawnsetName = ce.CustomLeaderboard.Spawnset.Name,
				ce.PlayerId,
				ce.Player.PlayerName,
			})
			.FirstOrDefault(ce => ce.Id == id);
		if (customEntry == null)
			throw new NotFoundException($"Custom entry replay '{id}' could not be found.");

		string fileName = $"{customEntry.SpawnsetId}-{customEntry.SpawnsetName}-{customEntry.PlayerId}-{customEntry.PlayerName}.ddreplay";
		return (fileName, IoFile.ReadAllBytes(path));
	}
}
