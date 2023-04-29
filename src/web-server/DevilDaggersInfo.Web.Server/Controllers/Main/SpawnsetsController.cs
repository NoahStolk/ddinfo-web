using DevilDaggersInfo.Api.Main;
using DevilDaggersInfo.Api.Main.Spawnsets;
using DevilDaggersInfo.Common.Extensions;
using DevilDaggersInfo.Core.Spawnset.Summary;
using DevilDaggersInfo.Core.Wiki;
using DevilDaggersInfo.Web.Client;
using DevilDaggersInfo.Web.Server.Converters.DomainToApi.Main;
using DevilDaggersInfo.Web.Server.Domain.Converters.CoreToDomain;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using DevilDaggersInfo.Web.Server.Domain.Services.Caching;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

namespace DevilDaggersInfo.Web.Server.Controllers.Main;

[Route("api/spawnsets")]
[ApiController]
public class SpawnsetsController : ControllerBase
{
	private readonly ApplicationDbContext _dbContext;
	private readonly SpawnsetSummaryCache _spawnsetSummaryCache;
	private readonly ILogger<SpawnsetsController> _logger;

	public SpawnsetsController(ApplicationDbContext dbContext, SpawnsetSummaryCache spawnsetSummaryCache, ILogger<SpawnsetsController> logger)
	{
		_dbContext = dbContext;
		_spawnsetSummaryCache = spawnsetSummaryCache;
		_logger = logger;
	}

	[HttpPost("migrate")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult Migrate()
	{
		List<SpawnsetEntity> spawnsets = _dbContext.Spawnsets.ToList();
		foreach (SpawnsetEntity spawnset in spawnsets)
		{
			SpawnsetSummary summary = _spawnsetSummaryCache.GetSpawnsetSummaryById(spawnset.Id);
			spawnset.GameMode = summary.GameMode.ToDomain();
			spawnset.WorldVersion = summary.WorldVersion;
			spawnset.SpawnVersion = summary.SpawnVersion;
			spawnset.PreLoopSpawnCount = summary.PreLoopSection.SpawnCount;
			spawnset.PreLoopLength = summary.PreLoopSection.Length;
			spawnset.LoopSpawnCount = summary.LoopSection.SpawnCount;
			spawnset.LoopLength = summary.LoopSection.Length;
			spawnset.HandLevel = summary.HandLevel.ToDomain();
			spawnset.AdditionalGems = summary.AdditionalGems;
			spawnset.TimerStart = summary.TimerStart;
			spawnset.EffectiveHandLevel = summary.EffectivePlayerSettings.HandLevel.ToDomain();
			spawnset.EffectiveGemsOrHoming = summary.EffectivePlayerSettings.GemsOrHoming;
			spawnset.EffectiveHandMesh = summary.EffectivePlayerSettings.HandMesh.ToDomain();

			_dbContext.SaveChanges();
		}

		return Ok();
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public ActionResult<Page<GetSpawnsetOverview>> GetSpawnsets(
		bool practiceOnly,
		bool withCustomLeaderboardOnly,
		string? spawnsetFilter = null,
		string? authorFilter = null,
		[Range(0, 1000)] int pageIndex = 0,
		[Range(Constants.PageSizeMin, Constants.PageSizeMax)] int pageSize = Constants.PageSizeDefault,
		SpawnsetSorting? sortBy = null,
		bool ascending = false)
	{
		IQueryable<SpawnsetEntity> spawnsetsQuery = _dbContext.Spawnsets.AsNoTracking().Include(s => s.Player);

		if (practiceOnly)
			spawnsetsQuery = spawnsetsQuery.Where(s => s.IsPractice);

		if (withCustomLeaderboardOnly)
		{
			List<int> spawnsetsWithCustomLeaderboardIds = _dbContext.CustomLeaderboards
				.AsNoTracking()
				.Select(cl => cl.SpawnsetId)
				.ToList();

			spawnsetsQuery = spawnsetsQuery.Where(s => spawnsetsWithCustomLeaderboardIds.Contains(s.Id));
		}

		// Casing is ignored by default because of IQueryable.
		if (!string.IsNullOrWhiteSpace(spawnsetFilter))
			spawnsetsQuery = spawnsetsQuery.Where(s => s.Name.Contains(spawnsetFilter));

		if (!string.IsNullOrWhiteSpace(authorFilter))
		{
			// ! Navigation property.
			spawnsetsQuery = spawnsetsQuery.Where(s => s.Player!.PlayerName.Contains(authorFilter));
		}

		List<SpawnsetEntity> spawnsets = spawnsetsQuery.ToList();
		Dictionary<int, SpawnsetSummary> summaries = spawnsets.ToDictionary(s => s.Id, s => _spawnsetSummaryCache.GetSpawnsetSummaryById(s.Id));

		// ! Navigation property.
		spawnsets = (sortBy switch
		{
			SpawnsetSorting.Name => spawnsets.OrderBy(s => s.Name.ToLower(), ascending),
			SpawnsetSorting.AuthorName => spawnsets.OrderBy(s => s.Player!.PlayerName.ToLower(), ascending),
			SpawnsetSorting.LastUpdated => spawnsets.OrderBy(s => s.LastUpdated, ascending),
			SpawnsetSorting.GameMode => spawnsets.OrderBy(s => summaries[s.Id].GameMode, ascending),
			SpawnsetSorting.LoopLength => spawnsets.OrderBy(s => summaries[s.Id].LoopSection.Length, ascending),
			SpawnsetSorting.LoopSpawnCount => spawnsets.OrderBy(s => summaries[s.Id].LoopSection.SpawnCount, ascending),
			SpawnsetSorting.PreLoopLength => spawnsets.OrderBy(s => summaries[s.Id].PreLoopSection.Length, ascending),
			SpawnsetSorting.PreLoopSpawnCount => spawnsets.OrderBy(s => summaries[s.Id].PreLoopSection.SpawnCount, ascending),
			SpawnsetSorting.Hand => spawnsets.OrderBy(s => summaries[s.Id].HandLevel, ascending),
			SpawnsetSorting.AdditionalGems => spawnsets.OrderBy(s => summaries[s.Id].AdditionalGems, ascending),
			_ => spawnsets.OrderBy(s => s.Id, ascending),
		}).ToList();

		int totalSpawnsets = spawnsets.Count;
		int lastPageIndex = totalSpawnsets / pageSize;
		spawnsets = spawnsets
			.Skip(Math.Min(pageIndex, lastPageIndex) * pageSize)
			.Take(pageSize)
			.ToList();

		return new Page<GetSpawnsetOverview>
		{
			Results = spawnsets.ConvertAll(s => s.ToMainApi(summaries[s.Id])),
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
				}) ?? new(),
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

	// FORBIDDEN: Used by DDSE 2.45.0.0 ... 2.46.1.0.
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

		_logger.LogError("Default spawnset {name} does not exist in the file system.", name);
		return NotFound();
	}

	[HttpGet("by-author")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult<List<GetSpawnsetName>> GetSpawnsetsByAuthorId([Required] int playerId)
	{
		var spawnsets = _dbContext.Spawnsets
			.AsNoTracking()
			.Select(s => new { s.Id, s.PlayerId, s.Name, s.LastUpdated })
			.Where(s => s.PlayerId == playerId)
			.OrderByDescending(s => s.LastUpdated)
			.ToList();

		return spawnsets.ConvertAll(s => new GetSpawnsetName
		{
			Id = s.Id,
			Name = s.Name,
		});
	}
}
