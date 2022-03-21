using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.CustomLeaderboards;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Services;

public class CustomLeaderboardValidatorService
{
	private readonly ApplicationDbContext _dbContext;
	private readonly IFileSystemService _fileSystemService;

	public CustomLeaderboardValidatorService(ApplicationDbContext dbContext, IFileSystemService fileSystemService)
	{
		_dbContext = dbContext;
		_fileSystemService = fileSystemService;
	}

	public void ValidateCustomLeaderboard(int spawnsetId, CustomLeaderboardCategory category, AddCustomLeaderboardDaggers? customLeaderboardDaggers, bool isFeatured)
	{
		if (!Enum.IsDefined(category))
			throw new CustomLeaderboardValidationException($"Category '{category}' is not defined.");

		if (isFeatured && customLeaderboardDaggers == null)
			throw new CustomLeaderboardValidationException("Daggers are required for featured leaderboards.");

		if (customLeaderboardDaggers != null)
		{
			if (category.IsAscending())
			{
				if (customLeaderboardDaggers.Leviathan >= customLeaderboardDaggers.Devil)
					throw new CustomLeaderboardValidationException("For ascending leaderboards, Leviathan time must be smaller than Devil time.");
				if (customLeaderboardDaggers.Devil >= customLeaderboardDaggers.Golden)
					throw new CustomLeaderboardValidationException("For ascending leaderboards, Devil time must be smaller than Golden time.");
				if (customLeaderboardDaggers.Golden >= customLeaderboardDaggers.Silver)
					throw new CustomLeaderboardValidationException("For ascending leaderboards, Golden time must be smaller than Silver time.");
				if (customLeaderboardDaggers.Silver >= customLeaderboardDaggers.Bronze)
					throw new CustomLeaderboardValidationException("For ascending leaderboards, Silver time must be smaller than Bronze time.");
			}
			else
			{
				if (customLeaderboardDaggers.Leviathan <= customLeaderboardDaggers.Devil)
					throw new CustomLeaderboardValidationException("For descending leaderboards, Leviathan time must be greater than Devil time.");
				if (customLeaderboardDaggers.Devil <= customLeaderboardDaggers.Golden)
					throw new CustomLeaderboardValidationException("For descending leaderboards, Devil time must be greater than Golden time.");
				if (customLeaderboardDaggers.Golden <= customLeaderboardDaggers.Silver)
					throw new CustomLeaderboardValidationException("For descending leaderboards, Golden time must be greater than Silver time.");
				if (customLeaderboardDaggers.Silver <= customLeaderboardDaggers.Bronze)
					throw new CustomLeaderboardValidationException("For descending leaderboards, Silver time must be greater than Bronze time.");
			}
		}

		var spawnset = _dbContext.Spawnsets
			.AsNoTracking()
			.Select(sf => new { sf.Id, sf.Name })
			.FirstOrDefault(sf => sf.Id == spawnsetId);
		if (spawnset == null)
			throw new CustomLeaderboardValidationException($"Spawnset with ID '{spawnsetId}' does not exist.");

		string spawnsetFilePath = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.Spawnsets), spawnset.Name);
		if (!File.Exists(spawnsetFilePath))
			throw new InvalidOperationException($"Spawnset file '{spawnset.Name}' does not exist. Spawnset with ID '{spawnsetId}' does not have a file which should never happen.");

		if (!SpawnsetBinary.TryParse(File.ReadAllBytes(spawnsetFilePath), out SpawnsetBinary? spawnsetBinary))
			throw new InvalidOperationException($"Could not parse survival file '{spawnset.Name}'. Please review the file. Also review how this file ended up in the 'spawnsets' directory, as it should not be possible to upload non-survival files from within the Admin pages.");

		GameMode requiredGameMode = category.GetRequiredGameModeForCategory();
		if (spawnsetBinary.GameMode != requiredGameMode)
			throw new CustomLeaderboardValidationException($"Game mode must be '{requiredGameMode}' when the custom leaderboard category is '{category}'. The spawnset has game mode '{spawnsetBinary.GameMode}'.");

		if (spawnsetBinary.TimerStart != 0)
			throw new CustomLeaderboardValidationException("Cannot create a leaderboard for spawnset that uses the TimerStart value. This value is meant for practice and it is confusing to use it with custom leaderboards, as custom leaderboards always use the 'actual' timer value.");

		if (category == CustomLeaderboardCategory.Survival && !spawnsetBinary.HasEndLoop())
			throw new CustomLeaderboardValidationException($"Custom leaderboard with category {category} must have an end loop.");

		if (category is CustomLeaderboardCategory.Pacifist or CustomLeaderboardCategory.TimeAttack && !spawnsetBinary.HasSpawns())
			throw new CustomLeaderboardValidationException($"Custom leaderboard with category {category} must have spawns.");
	}
}
