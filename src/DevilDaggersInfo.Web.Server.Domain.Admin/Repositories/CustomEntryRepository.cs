using DevilDaggersInfo.Web.ApiSpec.Admin;
using DevilDaggersInfo.Web.ApiSpec.Admin.CustomEntries;
using DevilDaggersInfo.Web.Server.Domain.Admin.Converters.DomainToApi;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DevilDaggersInfo.Web.Server.Domain.Admin.Repositories;

public class CustomEntryRepository
{
	private readonly ApplicationDbContext _dbContext;

	public CustomEntryRepository(ApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<Page<GetCustomEntryForOverview>> GetCustomEntriesAsync(string? filter, int pageIndex, int pageSize, CustomEntrySorting? sortBy, bool ascending)
	{
		// ! Navigation property.
		IQueryable<CustomEntryEntity> customEntriesQuery = _dbContext.CustomEntries
			.AsNoTracking()
		.Include(ce => ce.Player)
			.Include(ce => ce.CustomLeaderboard)
				.ThenInclude(cl => cl!.Spawnset);

		if (!string.IsNullOrWhiteSpace(filter))
		{
			// ! Navigation property.
			customEntriesQuery = customEntriesQuery.Where(ce => ce.Player!.PlayerName.Contains(filter) || ce.CustomLeaderboard!.Spawnset!.Name.Contains(filter));
		}

		// ! Navigation property.
		customEntriesQuery = sortBy switch
		{
			CustomEntrySorting.ClientVersion => customEntriesQuery.OrderBy(ce => ce.ClientVersion, ascending).ThenBy(ce => ce.Id),
			CustomEntrySorting.DaggersFired => customEntriesQuery.OrderBy(ce => ce.DaggersFired, ascending).ThenBy(ce => ce.Id),
			CustomEntrySorting.DaggersHit => customEntriesQuery.OrderBy(ce => ce.DaggersHit, ascending).ThenBy(ce => ce.Id),
			CustomEntrySorting.DeathType => customEntriesQuery.OrderBy(ce => ce.DeathType, ascending).ThenBy(ce => ce.Id),
			CustomEntrySorting.EnemiesAlive => customEntriesQuery.OrderBy(ce => ce.EnemiesAlive, ascending).ThenBy(ce => ce.Id),
			CustomEntrySorting.EnemiesKilled => customEntriesQuery.OrderBy(ce => ce.EnemiesKilled, ascending).ThenBy(ce => ce.Id),
			CustomEntrySorting.GemsCollected => customEntriesQuery.OrderBy(ce => ce.GemsCollected, ascending).ThenBy(ce => ce.Id),
			CustomEntrySorting.GemsDespawned => customEntriesQuery.OrderBy(ce => ce.GemsDespawned, ascending).ThenBy(ce => ce.Id),
			CustomEntrySorting.GemsEaten => customEntriesQuery.OrderBy(ce => ce.GemsEaten, ascending).ThenBy(ce => ce.Id),
			CustomEntrySorting.GemsTotal => customEntriesQuery.OrderBy(ce => ce.GemsTotal, ascending).ThenBy(ce => ce.Id),
			CustomEntrySorting.HomingStored => customEntriesQuery.OrderBy(ce => ce.HomingStored, ascending).ThenBy(ce => ce.Id),
			CustomEntrySorting.HomingEaten => customEntriesQuery.OrderBy(ce => ce.HomingEaten, ascending).ThenBy(ce => ce.Id),
			CustomEntrySorting.LevelUpTime2 => customEntriesQuery.OrderBy(ce => ce.LevelUpTime2, ascending).ThenBy(ce => ce.Id),
			CustomEntrySorting.LevelUpTime3 => customEntriesQuery.OrderBy(ce => ce.LevelUpTime3, ascending).ThenBy(ce => ce.Id),
			CustomEntrySorting.LevelUpTime4 => customEntriesQuery.OrderBy(ce => ce.LevelUpTime4, ascending).ThenBy(ce => ce.Id),
			CustomEntrySorting.PlayerName => customEntriesQuery.OrderBy(ce => ce.Player!.PlayerName, ascending).ThenBy(ce => ce.Id),
			CustomEntrySorting.SpawnsetName => customEntriesQuery.OrderBy(ce => ce.CustomLeaderboard!.Spawnset!.Name, ascending).ThenBy(ce => ce.Id),
			CustomEntrySorting.SubmitDate => customEntriesQuery.OrderBy(ce => ce.SubmitDate, ascending).ThenBy(ce => ce.Id),
			CustomEntrySorting.Time => customEntriesQuery.OrderBy(ce => ce.Time, ascending).ThenBy(ce => ce.Id),
			_ => customEntriesQuery.OrderBy(ce => ce.Id, ascending),
		};

		List<CustomEntryEntity> customEntries = await customEntriesQuery
			.Skip(pageIndex * pageSize)
			.Take(pageSize)
			.ToListAsync();

		return new Page<GetCustomEntryForOverview>
		{
			Results = customEntries.ConvertAll(ce => ce.ToAdminApiOverview()),
			TotalResults = await customEntriesQuery.CountAsync(),
		};
	}

	public async Task<GetCustomEntry> GetCustomEntryAsync(int id)
	{
		CustomEntryEntity? customEntry = await _dbContext.CustomEntries
			.AsNoTracking()
			.Include(ce => ce.CustomLeaderboard)
			.FirstOrDefaultAsync(cl => cl.Id == id);

		if (customEntry == null)
			throw new NotFoundException();

		return customEntry.ToAdminApi();
	}
}
