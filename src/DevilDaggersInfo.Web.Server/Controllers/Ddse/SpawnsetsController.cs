using DevilDaggersInfo.Web.ApiSpec.Ddse.Spawnsets;
using DevilDaggersInfo.Web.Server.Converters.DomainToApi.Ddse;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

namespace DevilDaggersInfo.Web.Server.Controllers.Ddse;

[Route("api/ddse/spawnsets")]
[ApiController]
public class SpawnsetsController : ControllerBase
{
	private readonly ApplicationDbContext _dbContext;

	public SpawnsetsController(ApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<List<GetSpawnsetDdse>> GetSpawnsets(string? authorFilter = null, string? nameFilter = null)
	{
		return await GetSpawnsetsImpl(authorFilter, nameFilter);
	}

	[Obsolete("Support for DDSE 2.46.1 will be dropped in the future.")]
	[HttpGet("/api/spawnsets/ddse")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<List<GetSpawnsetDdse>> GetSpawnsetsObsolete(string? authorFilter = null, string? nameFilter = null)
	{
		return await GetSpawnsetsImpl(authorFilter, nameFilter);
	}

	[HttpGet("{spawnsetId}/file")]
	[ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult> GetSpawnsetFile([Required] int spawnsetId)
	{
		var spawnset = await _dbContext.Spawnsets.AsNoTracking().Select(s => new { s.Id, s.Name, s.File }).FirstOrDefaultAsync(s => s.Id == spawnsetId);
		if (spawnset == null)
			return NotFound();

		return File(spawnset.File, MediaTypeNames.Application.Octet, spawnset.Name);
	}

	private async Task<List<GetSpawnsetDdse>> GetSpawnsetsImpl(string? authorFilter, string? nameFilter)
	{
		IQueryable<SpawnsetEntity> spawnsetsQuery = _dbContext.Spawnsets.AsNoTracking().Include(sf => sf.Player);

		if (!string.IsNullOrWhiteSpace(authorFilter))
		{
			// ! Navigation property.
			spawnsetsQuery = spawnsetsQuery.Where(sf => sf.Player!.PlayerName.Contains(authorFilter));
		}

		if (!string.IsNullOrWhiteSpace(nameFilter))
			spawnsetsQuery = spawnsetsQuery.Where(sf => sf.Name.Contains(nameFilter));

		List<int> spawnsetsWithCustomLeaderboardIds = await _dbContext.CustomLeaderboards
			.Select(cl => cl.SpawnsetId)
			.ToListAsync();

		List<SpawnsetEntity> spawnsets = await spawnsetsQuery.ToListAsync();

		return spawnsets.ConvertAll(s => s.ToDdseApi(spawnsetsWithCustomLeaderboardIds.Contains(s.Id)));
	}
}
