using DevilDaggersInfo.Api.Admin;
using DevilDaggersInfo.Api.Admin.CustomLeaderboards;
using DevilDaggersInfo.Web.Server.Domain.Admin.Converters.DomainToApi;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DevilDaggersInfo.Web.Server.Domain.Admin.Repositories;

public class CustomLeaderboardRepository
{
	private readonly ApplicationDbContext _dbContext;

	public CustomLeaderboardRepository(ApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<Page<GetCustomLeaderboardForOverview>> GetCustomLeaderboardsAsync(string? filter, int pageIndex, int pageSize, CustomLeaderboardSorting? sortBy, bool ascending)
	{
		IQueryable<CustomLeaderboardEntity> customLeaderboardsQuery = _dbContext.CustomLeaderboards
			.AsNoTracking()
			.Include(cl => cl.Spawnset);

		if (!string.IsNullOrWhiteSpace(filter))
		{
			// ! Navigation property.
			customLeaderboardsQuery = customLeaderboardsQuery.Where(cl => cl.Spawnset!.Name.Contains(filter));
		}

		// ! Navigation property.
		customLeaderboardsQuery = sortBy switch
		{
			CustomLeaderboardSorting.RankSorting => customLeaderboardsQuery.OrderBy(cl => cl.RankSorting, ascending).ThenBy(cl => cl.Id),
			CustomLeaderboardSorting.DateCreated => customLeaderboardsQuery.OrderBy(cl => cl.DateCreated, ascending).ThenBy(cl => cl.Id),
			CustomLeaderboardSorting.IsFeatured => customLeaderboardsQuery.OrderBy(cl => cl.IsFeatured, ascending).ThenBy(cl => cl.Id),
			CustomLeaderboardSorting.SpawnsetName => customLeaderboardsQuery.OrderBy(cl => cl.Spawnset!.Name, ascending).ThenBy(cl => cl.Id),
			CustomLeaderboardSorting.TimeBronze => customLeaderboardsQuery.OrderBy(cl => cl.Bronze, ascending).ThenBy(cl => cl.Id),
			CustomLeaderboardSorting.TimeSilver => customLeaderboardsQuery.OrderBy(cl => cl.Silver, ascending).ThenBy(cl => cl.Id),
			CustomLeaderboardSorting.TimeGolden => customLeaderboardsQuery.OrderBy(cl => cl.Golden, ascending).ThenBy(cl => cl.Id),
			CustomLeaderboardSorting.TimeDevil => customLeaderboardsQuery.OrderBy(cl => cl.Devil, ascending).ThenBy(cl => cl.Id),
			CustomLeaderboardSorting.TimeLeviathan => customLeaderboardsQuery.OrderBy(cl => cl.Leviathan, ascending).ThenBy(cl => cl.Id),
			_ => customLeaderboardsQuery.OrderBy(cl => cl.Id, ascending),
		};

		List<CustomLeaderboardEntity> customLeaderboards = await customLeaderboardsQuery
			.Skip(pageIndex * pageSize)
			.Take(pageSize)
			.ToListAsync();

		return new Page<GetCustomLeaderboardForOverview>
		{
			Results = customLeaderboards.ConvertAll(cl => cl.ToAdminApiOverview()),
			TotalResults = _dbContext.CustomLeaderboards.Count(),
		};
	}

	public async Task<GetCustomLeaderboard> GetCustomLeaderboardAsync(int id)
	{
		// ! Navigation property.
		CustomLeaderboardEntity? customLeaderboard = await _dbContext.CustomLeaderboards
			.AsNoTracking()
			.Include(cl => cl.Spawnset)
				.ThenInclude(sf => sf!.Player)
			.FirstOrDefaultAsync(cl => cl.Id == id);

		if (customLeaderboard == null)
			throw new NotFoundException($"Leaderboard with ID '{id}' was not found.");

		return customLeaderboard.ToAdminApi();
	}
}
