using DevilDaggersInfo.Core.Common.Extensions;
using DevilDaggersInfo.Core.Wiki;
using DevilDaggersInfo.Web.ApiSpec.Main;
using DevilDaggersInfo.Web.ApiSpec.Main.Spawnsets;
using DevilDaggersInfo.Web.Client;
using DevilDaggersInfo.Web.Server.Converters.DomainToApi.Main;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Extensions;
using DevilDaggersInfo.Web.Server.Domain.Models.Spawnsets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

namespace DevilDaggersInfo.Web.Server.Controllers.Main;

[Route("api/spawnsets")]
[ApiController]
public sealed class SpawnsetsController : ControllerBase
{
	private readonly ApplicationDbContext _dbContext;
	private readonly ILogger<SpawnsetsController> _logger;

	public SpawnsetsController(ApplicationDbContext dbContext, ILogger<SpawnsetsController> logger)
	{
		_dbContext = dbContext;
		_logger = logger;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<Page<GetSpawnsetOverview>>> GetSpawnsets(
		bool withCustomLeaderboardOnly,
		string? spawnsetFilter = null,
		string? authorFilter = null,
		[Range(0, 1000)] int pageIndex = 0,
		[Range(Constants.PageSizeMin, Constants.PageSizeMax)] int pageSize = Constants.PageSizeDefault,
		SpawnsetSorting? sortBy = null,
		bool ascending = false)
	{
		IQueryable<SpawnsetEntity> spawnsetsQuery = _dbContext.Spawnsets.AsNoTracking();

		if (withCustomLeaderboardOnly)
			spawnsetsQuery = spawnsetsQuery.Where(s => _dbContext.CustomLeaderboards.Any(cl => cl.SpawnsetId == s.Id));

		// Casing is ignored by default because of IQueryable.
		if (!string.IsNullOrWhiteSpace(spawnsetFilter))
		{
			spawnsetFilter = spawnsetFilter.Trim();
			spawnsetsQuery = spawnsetsQuery.Where(s => s.Name.Contains(spawnsetFilter));
		}

		if (!string.IsNullOrWhiteSpace(authorFilter))
		{
			authorFilter = authorFilter.Trim();

			// ! Navigation property.
			spawnsetsQuery = spawnsetsQuery.Where(s => s.Player!.PlayerName.Contains(authorFilter));
		}

		int totalSpawnsets = await spawnsetsQuery.CountAsync();

		// Sorting is applied to the query so the database does it, which means LOWER() is not needed: the column
		// collation is already case insensitive, and wrapping the column would prevent an index from being used.
		// ! Navigation property.
		spawnsetsQuery = sortBy switch
		{
			SpawnsetSorting.Name => spawnsetsQuery.OrderBy(s => s.Name, ascending),
			SpawnsetSorting.AuthorName => spawnsetsQuery.OrderBy(s => s.Player!.PlayerName, ascending),
			SpawnsetSorting.LastUpdated => spawnsetsQuery.OrderBy(s => s.LastUpdated, ascending),
			SpawnsetSorting.GameMode => spawnsetsQuery.OrderBy(s => s.GameMode, ascending),
			SpawnsetSorting.LoopLength => spawnsetsQuery.OrderBy(s => s.LoopLength, ascending),
			SpawnsetSorting.LoopSpawnCount => spawnsetsQuery.OrderBy(s => s.LoopSpawnCount, ascending),
			SpawnsetSorting.PreLoopLength => spawnsetsQuery.OrderBy(s => s.PreLoopLength, ascending),
			SpawnsetSorting.PreLoopSpawnCount => spawnsetsQuery.OrderBy(s => s.PreLoopSpawnCount, ascending),
			SpawnsetSorting.Hand => spawnsetsQuery.OrderBy(s => s.HandLevel, ascending),
			SpawnsetSorting.AdditionalGems => spawnsetsQuery.OrderBy(s => s.AdditionalGems, ascending),
			_ => spawnsetsQuery.OrderBy(s => s.Id, ascending),
		};

		int lastPageIndex = totalSpawnsets / pageSize;

		// Only the columns the overview needs are selected. Materialising the entity would also fetch the spawnset
		// file, which is a BLOB of up to 70 KiB per row that the overview never uses.
		// ! Navigation property.
		List<SpawnsetOverview> spawnsets = await spawnsetsQuery
			.Skip(Math.Min(pageIndex, lastPageIndex) * pageSize)
			.Take(pageSize)
			.Select(s => new SpawnsetOverview
			{
				Id = s.Id,
				Name = s.Name,
				AuthorName = s.Player!.PlayerName,
				LastUpdated = s.LastUpdated,
				GameMode = s.GameMode,
				LoopLength = s.LoopLength,
				LoopSpawnCount = s.LoopSpawnCount,
				PreLoopLength = s.PreLoopLength,
				PreLoopSpawnCount = s.PreLoopSpawnCount,
				EffectiveHandLevel = s.EffectiveHandLevel,
				EffectiveGemsOrHoming = s.EffectiveGemsOrHoming,
			})
			.ToListAsync();

		return new Page<GetSpawnsetOverview>
		{
			Results = spawnsets.ConvertAll(s => s.ToMainApi()),
			TotalResults = totalSpawnsets,
		};
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

		CustomLeaderboardEntity? customLeaderboard = await _dbContext.CustomLeaderboards
			.AsNoTracking()
			.FirstOrDefaultAsync(cl => cl.SpawnsetId == spawnset.Id);

		var customEntries = customLeaderboard == null ? null : await _dbContext.CustomEntries
			.AsNoTracking()
			.Select(ce => new { ce.Id, ce.CustomLeaderboardId, ce.Time })
			.Where(ce => ce.CustomLeaderboardId == customLeaderboard.Id)
			.ToListAsync();

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

	[HttpGet("hash")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<byte[]>> GetSpawnsetHash([Required] string fileName)
	{
		var spawnset = await _dbContext.Spawnsets.AsNoTracking().Select(s => new { s.Name, s.Md5Hash }).FirstOrDefaultAsync(s => s.Name == fileName);
		if (spawnset == null)
			return NotFound();

		return spawnset.Md5Hash;
	}

	[HttpGet("total-data")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult<GetTotalSpawnsetData> GetTotalSpawnsetData()
	{
		return new GetTotalSpawnsetData
		{
			Count = _dbContext.Spawnsets.AsNoTracking().Select(s => s.Id).Count(),
		};
	}

	[HttpGet("{fileName}/file")]
	[ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult> GetSpawnsetFile([Required] string fileName)
	{
		var spawnset = await _dbContext.Spawnsets.AsNoTracking().Select(s => new { s.Name, s.File }).FirstOrDefaultAsync(s => s.Name == fileName);
		if (spawnset == null)
			return NotFound();

		return File(spawnset.File, MediaTypeNames.Application.Octet, fileName);
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

		return spawnsetEntity.ToMainApi(customLeaderboard?.Id, spawnsetEntity.File);
	}

	[HttpGet("default")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<byte[]>> GetDefaultSpawnset(GameVersion gameVersion)
	{
		string name = gameVersion switch
		{
			GameVersion.V1_0 => "V1",
			GameVersion.V2_0 => "V2",
			_ => "V3",
		};

		var spawnsetEntity = await _dbContext.Spawnsets
			.AsNoTracking()
			.Select(s => new { s.Name, s.File })
			.FirstOrDefaultAsync(s => s.Name == name);
		if (spawnsetEntity != null)
			return spawnsetEntity.File;

		_logger.LogError("Default spawnset {Name} does not exist in the file system.", name);
		return NotFound();
	}

	[HttpGet("by-author")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<ActionResult<List<GetSpawnsetName>>> GetSpawnsetsByAuthorId([Required] int playerId)
	{
		var spawnsets = await _dbContext.Spawnsets
			.AsNoTracking()
			.Select(s => new { s.Id, s.PlayerId, s.Name, s.LastUpdated })
			.Where(s => s.PlayerId == playerId)
			.OrderByDescending(s => s.LastUpdated)
			.ToListAsync();

		return spawnsets.ConvertAll(s => new GetSpawnsetName
		{
			Id = s.Id,
			Name = s.Name,
		});
	}
}
