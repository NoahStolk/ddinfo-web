using DevilDaggersInfo.Web.ApiSpec.Tools.Spawnsets;
using DevilDaggersInfo.Web.Server.Converters.DomainToApi.Tools;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Web.Server.Controllers.Tools;

[Route("api/app/spawnsets")]
[ApiController]
public class SpawnsetsController : ControllerBase
{
	private readonly ApplicationDbContext _dbContext;

	public SpawnsetsController(ApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<GetSpawnset>> GetSpawnsetById([Required] int id)
	{
		SpawnsetEntity? spawnsetEntity = await _dbContext.Spawnsets
			.AsNoTracking()
			.Include(s => s.Player)
			.FirstOrDefaultAsync(s => s.Id == id);
		if (spawnsetEntity == null)
			return NotFound();

		var customLeaderboard = await _dbContext.CustomLeaderboards
			.AsNoTracking()
			.Select(cl => new { cl.Id, cl.SpawnsetId })
			.FirstOrDefaultAsync(cl => cl.SpawnsetId == spawnsetEntity.Id);

		return spawnsetEntity.ToAppApi(customLeaderboard?.Id, spawnsetEntity.File);
	}

	[HttpGet("{id}/buffer")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<GetSpawnsetBuffer>> GetSpawnsetBufferById([Required] int id)
	{
		var spawnset = await _dbContext.Spawnsets
			.AsNoTracking()
			.Select(s => new { s.Id, s.Name, s.File })
			.FirstOrDefaultAsync(s => s.Id == id);
		if (spawnset == null)
			throw new NotFoundException();

		return new GetSpawnsetBuffer { Data = spawnset.File };
	}

	[HttpGet("by-hash")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<GetSpawnsetByHash>> GetSpawnsetByHash([FromQuery] byte[] hash)
	{
		// ! Navigation property.
		var spawnset = await _dbContext.Spawnsets
			.AsNoTracking()
			.Include(s => s.Player)
			.Select(s => new
			{
				s.Md5Hash,
				s.Player!.PlayerName,
				s.Id,
				s.Name,
			})
			.FirstOrDefaultAsync(s => s.Md5Hash == hash);
		if (spawnset == null)
			return NotFound();

		CustomLeaderboardEntity? customLeaderboard = _dbContext.CustomLeaderboards
			.AsNoTracking()
			.FirstOrDefault(cl => cl.SpawnsetId == spawnset.Id);

		var customEntries = customLeaderboard == null ? null : _dbContext.CustomEntries
			.AsNoTracking()
			.Select(ce => new { ce.Id, ce.CustomLeaderboardId, ce.Time })
			.Where(ce => ce.CustomLeaderboardId == customLeaderboard.Id)
			.ToList();

		return new GetSpawnsetByHash
		{
			AuthorName = spawnset.PlayerName,
			CustomLeaderboard = customLeaderboard == null ? null : new GetSpawnsetByHashCustomLeaderboard
			{
				CustomLeaderboardId = customLeaderboard.Id,
				CustomEntries = customEntries?.ConvertAll(ce => new GetSpawnsetByHashCustomEntry
				{
					HasReplay = false,
					CustomEntryId = ce.Id,
					Time = ce.Time,
				}) ?? [],
			},
			SpawnsetId = spawnset.Id,
			Name = spawnset.Name,
		};
	}
}
