using DevilDaggersInfo.Api.Main;
using DevilDaggersInfo.Api.Main.Spawnsets;
using DevilDaggersInfo.Common.Extensions;
using DevilDaggersInfo.Core.Spawnset.Summary;
using DevilDaggersInfo.Core.Wiki;
using DevilDaggersInfo.Web.Client;
using DevilDaggersInfo.Web.Server.Converters.DomainToApi.Main;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Models.Spawnsets;
using DevilDaggersInfo.Web.Server.Domain.Services.Caching;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Security.Cryptography;

namespace DevilDaggersInfo.Web.Server.Controllers.Main;

[Route("api/spawnsets")]
[ApiController]
public class SpawnsetsController : ControllerBase
{
	private readonly ApplicationDbContext _dbContext;
	private readonly IFileSystemService _fileSystemService;
	private readonly SpawnsetSummaryCache _spawnsetSummaryCache;
	private readonly SpawnsetHashCache _spawnsetHashCache;
	private readonly ILogger<SpawnsetsController> _logger;

	public SpawnsetsController(ApplicationDbContext dbContext, IFileSystemService fileSystemService, SpawnsetSummaryCache spawnsetSummaryCache, SpawnsetHashCache spawnsetHashCache, ILogger<SpawnsetsController> logger)
	{
		_dbContext = dbContext;
		_fileSystemService = fileSystemService;
		_spawnsetSummaryCache = spawnsetSummaryCache;
		_spawnsetHashCache = spawnsetHashCache;
		_logger = logger;
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

		Dictionary<int, SpawnsetSummary> summaries = new();
		foreach (string filePath in _fileSystemService.TryGetFiles(DataSubDirectory.Spawnsets))
		{
			string name = Path.GetFileNameWithoutExtension(filePath);
			SpawnsetEntity? spawnset = spawnsets.Find(s => s.Name == name);
			if (spawnset == null)
				continue;

			summaries[spawnset.Id] = _spawnsetSummaryCache.GetSpawnsetSummaryByFilePath(filePath);
		}

		// In case a spawnset doesn't have a summary; remove it.
		spawnsets = spawnsets.Where(s => summaries.ContainsKey(s.Id)).ToList();

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
			Results = spawnsets.ConvertAll(s => s.ToGetSpawnsetOverview(summaries[s.Id])),
			TotalResults = totalSpawnsets,
		};
	}

	[HttpGet("by-hash")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<GetSpawnsetByHash>> GetSpawnsetByHash([FromQuery] byte[] hash)
	{
		SpawnsetHashCacheData? data = await _spawnsetHashCache.GetSpawnsetAsync(hash);
		if (data == null)
			return NotFound();

		SpawnsetEntity? spawnset = _dbContext.Spawnsets
			.AsNoTracking()
			.Include(s => s.Player)
			.FirstOrDefault(s => s.Name == data.Name);
		if (spawnset == null)
		{
			_logger.LogWarning("Spawnset {name} was found in hash cache but not in database.", data.Name);
			return NotFound();
		}

		CustomLeaderboardEntity? customLeaderboard = _dbContext.CustomLeaderboards
			.AsNoTracking()
			.FirstOrDefault(cl => cl.SpawnsetId == spawnset.Id);

		var customEntries = customLeaderboard == null ? null : _dbContext.CustomEntries
			.AsNoTracking()
			.Select(ce => new { ce.Id, ce.CustomLeaderboardId, ce.Time })
			.Where(ce => ce.CustomLeaderboardId == customLeaderboard.Id)
			.ToList();

		// ! Navigation property.
		return new GetSpawnsetByHash
		{
			AuthorName = spawnset.Player!.PlayerName,
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
		string path = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.Spawnsets), fileName);
		if (!_fileSystemService.FileExists(path))
			return NotFound();

		byte[] spawnsetBytes = await _fileSystemService.ReadAllBytesAsync(path);
		return MD5.HashData(spawnsetBytes);
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
		string path = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.Spawnsets), fileName);
		if (!_fileSystemService.FileExists(path))
			return NotFound();

		return File(await _fileSystemService.ReadAllBytesAsync(path), MediaTypeNames.Application.Octet, fileName);
	}

	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<GetSpawnset>> GetSpawnsetById([Required] int id)
	{
		SpawnsetEntity? spawnsetEntity = _dbContext.Spawnsets
			.AsNoTracking()
			.Include(s => s.Player)
			.FirstOrDefault(s => s.Id == id);
		if (spawnsetEntity == null)
			return NotFound();

		string path = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.Spawnsets), spawnsetEntity.Name);
		if (!_fileSystemService.FileExists(path))
			return NotFound();

		var customLeaderboard = _dbContext.CustomLeaderboards
			.AsNoTracking()
			.Select(cl => new { cl.Id, cl.SpawnsetId })
			.FirstOrDefault(cl => cl.SpawnsetId == spawnsetEntity.Id);

		return spawnsetEntity.ToGetSpawnset(customLeaderboard?.Id, await _fileSystemService.ReadAllBytesAsync(path));
	}

	[HttpGet("default")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<byte[]>> GetDefaultSpawnset(GameVersion gameVersion)
	{
		string fileName = gameVersion switch
		{
			GameVersion.V1_0 => "V1",
			GameVersion.V2_0 => "V2",
			_ => "V3",
		};

		string path = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.Spawnsets), fileName);
		if (!_fileSystemService.FileExists(path))
		{
			_logger.LogError("Default spawnset {name} does not exist in the file system.", fileName);
			return NotFound();
		}

		return await _fileSystemService.ReadAllBytesAsync(path);
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
