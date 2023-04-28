using DevilDaggersInfo.Api.Ddse.Spawnsets;
using DevilDaggersInfo.Core.Spawnset.Summary;
using DevilDaggersInfo.Web.Server.Converters.DomainToApi.Ddse;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Services.Caching;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DevilDaggersInfo.Web.Server.Controllers.Ddse;

[Obsolete("DDSE 2.46.1 will be removed.")]
[Route("api/ddse/spawnsets")]
[ApiController]
public class SpawnsetsController : ControllerBase
{
	private readonly ApplicationDbContext _dbContext;
	private readonly SpawnsetSummaryCache _spawnsetSummaryCache;

	public SpawnsetsController(ApplicationDbContext dbContext, SpawnsetSummaryCache spawnsetSummaryCache)
	{
		_dbContext = dbContext;
		_spawnsetSummaryCache = spawnsetSummaryCache;
	}

	[Obsolete("DDSE 2.46.1 will be removed.")]
	[HttpGet("/api/spawnsets/ddse")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public List<GetSpawnsetDdse> GetSpawnsetsObsolete(string? authorFilter = null, string? nameFilter = null)
	{
		IEnumerable<SpawnsetEntity> query = _dbContext.Spawnsets.AsNoTracking().Include(sf => sf.Player);

		if (!string.IsNullOrWhiteSpace(authorFilter))
		{
			// ! Navigation property.
			query = query.Where(sf => sf.Player!.PlayerName.Contains(authorFilter, StringComparison.InvariantCultureIgnoreCase));
		}

		if (!string.IsNullOrWhiteSpace(nameFilter))
			query = query.Where(sf => sf.Name.Contains(nameFilter, StringComparison.InvariantCultureIgnoreCase));

		List<int> spawnsetsWithCustomLeaderboardIds = _dbContext.CustomLeaderboards
			.AsNoTracking()
			.Select(cl => cl.SpawnsetId)
			.ToList();

		return query
			.Select(s =>
			{
				SpawnsetSummary spawnsetSummary = _spawnsetSummaryCache.GetSpawnsetSummaryById(s.Id);
				return s.ToDdseApi(spawnsetSummary, spawnsetsWithCustomLeaderboardIds.Contains(s.Id));
			})
			.ToList();
	}
}
