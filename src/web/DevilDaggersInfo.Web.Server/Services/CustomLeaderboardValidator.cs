using DevilDaggersInfo.Web.Shared.Dto.Admin.CustomLeaderboards;

namespace DevilDaggersInfo.Web.Server.Services;

public class CustomLeaderboardValidator
{
	private readonly ApplicationDbContext _dbContext;
	private readonly IFileSystemService _fileSystemService;

	public CustomLeaderboardValidator(ApplicationDbContext dbContext, IFileSystemService fileSystemService)
	{
		_dbContext = dbContext;
		_fileSystemService = fileSystemService;
	}

	public void ValidateCustomLeaderboard(int spawnsetId, CustomLeaderboardCategory category, AddCustomLeaderboardDaggers customLeaderboardDaggers, bool isFeatured)
	{
		if (!Enum.IsDefined(category))
			throw new CustomLeaderboardValidationException($"Category '{category}' is not defined.");

		if (isFeatured)
		{
			foreach (double dagger in new double[] { customLeaderboardDaggers.Leviathan, customLeaderboardDaggers.Devil, customLeaderboardDaggers.Golden, customLeaderboardDaggers.Silver, customLeaderboardDaggers.Bronze })
			{
				const double min = 1;
				const double max = 1500;
				if (dagger < min || dagger > max)
					throw new CustomLeaderboardValidationException($"All daggers times must be between {min} and {max} for featured leaderboards.");
			}

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

		if (category == CustomLeaderboardCategory.Survival && !spawnsetBinary.HasEndLoop())
			throw new CustomLeaderboardValidationException($"Custom leaderboard with category '{category}' must have an end loop.");

		if (category is CustomLeaderboardCategory.Pacifist or CustomLeaderboardCategory.TimeAttack && !spawnsetBinary.HasSpawns())
			throw new CustomLeaderboardValidationException($"Custom leaderboard with category '{category}' must have spawns.");
	}
}
