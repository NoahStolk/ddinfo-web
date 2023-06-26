using DevilDaggersInfo.Api.Admin;
using DevilDaggersInfo.Api.Admin.Spawnsets;
using DevilDaggersInfo.Web.Server.Domain.Admin.Converters.DomainToApi;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DevilDaggersInfo.Web.Server.Domain.Admin.Repositories;

public class SpawnsetRepository
{
	private readonly ApplicationDbContext _dbContext;

	public SpawnsetRepository(ApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<Page<GetSpawnsetForOverview>> GetSpawnsetsAsync(string? filter, int pageIndex, int pageSize, SpawnsetSorting? sortBy, bool ascending)
	{
		IQueryable<SpawnsetEntity> spawnsetsQuery = _dbContext.Spawnsets.AsNoTracking().Include(s => s.Player);

		if (!string.IsNullOrWhiteSpace(filter))
		{
			// ! Navigation property.
			spawnsetsQuery = spawnsetsQuery.Where(s => s.Name.Contains(filter) || s.Player!.PlayerName.Contains(filter));
		}

		// ! Navigation property.
		spawnsetsQuery = sortBy switch
		{
			SpawnsetSorting.Author => spawnsetsQuery.OrderBy(s => s.Player!.PlayerName, ascending).ThenBy(s => s.Id),
			SpawnsetSorting.HtmlDescription => spawnsetsQuery.OrderBy(s => s.HtmlDescription, ascending).ThenBy(s => s.Id),
			SpawnsetSorting.IsPractice => spawnsetsQuery.OrderBy(s => s.IsPractice, ascending).ThenBy(s => s.Id),
			SpawnsetSorting.LastUpdated => spawnsetsQuery.OrderBy(s => s.LastUpdated, ascending).ThenBy(s => s.Id),
			SpawnsetSorting.MaxDisplayWaves => spawnsetsQuery.OrderBy(s => s.MaxDisplayWaves, ascending).ThenBy(s => s.Id),
			SpawnsetSorting.Name => spawnsetsQuery.OrderBy(s => s.Name, ascending).ThenBy(s => s.Id),
			_ => spawnsetsQuery.OrderBy(s => s.Id, ascending),
		};

		List<SpawnsetEntity> spawnsets = await spawnsetsQuery
			.Skip(pageIndex * pageSize)
			.Take(pageSize)
			.ToListAsync();

		return new Page<GetSpawnsetForOverview>
		{
			Results = spawnsets.ConvertAll(s => s.ToAdminApiOverview()),
			TotalResults = spawnsetsQuery.Count(),
		};
	}

	public async Task<List<GetSpawnsetName>> GetSpawnsetNamesAsync()
	{
		var spawnsets = await _dbContext.Spawnsets
			.AsNoTracking()
			.Select(s => new { s.Id, s.Name })
			.ToListAsync();

		return spawnsets.ConvertAll(s => new GetSpawnsetName
		{
			Id = s.Id,
			Name = s.Name,
		});
	}

	public async Task<GetSpawnset> GetSpawnset(int id)
	{
		SpawnsetEntity? spawnset = await _dbContext.Spawnsets
			.AsNoTracking()
			.FirstOrDefaultAsync(p => p.Id == id);
		if (spawnset == null)
			throw new NotFoundException();

		return spawnset.ToAdminApi();
	}
}
