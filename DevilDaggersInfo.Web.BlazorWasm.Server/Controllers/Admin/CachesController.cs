using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.LeaderboardHistory;
using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.LeaderboardStatistics;
using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.ModArchives;
using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.SpawnsetHashes;
using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.SpawnsetSummaries;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using System;
using DevilDaggersInfo.Web.BlazorWasm.Client.Pages.Leaderboard;
using DevilDaggersInfo.Web.BlazorWasm.Server.InternalModels.Json;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Controllers.Admin;

[Route("api/admin/caches")]
[ApiController]
[Authorize(Roles = Roles.Admin)]
public class CachesController : ControllerBase
{
	private readonly LeaderboardStatisticsCache _leaderboardStatisticsCache;
	private readonly LeaderboardHistoryCache _leaderboardHistoryCache;
	private readonly ModArchiveCache _modArchiveCache;
	private readonly SpawnsetSummaryCache _spawnsetSummaryCache;
	private readonly SpawnsetHashCache _spawnsetHashCache;
	private readonly ILogger<CachesController> _logger;
	private readonly IFileSystemService _fileSystemService;

	public CachesController(
		LeaderboardStatisticsCache leaderboardStatisticsCache,
		LeaderboardHistoryCache leaderboardHistoryCache,
		ModArchiveCache modArchiveCache,
		SpawnsetSummaryCache spawnsetSummaryCache,
		SpawnsetHashCache spawnsetHashCache,
		ILogger<CachesController> logger,
		IFileSystemService fileSystemService)
	{
		_leaderboardStatisticsCache = leaderboardStatisticsCache;
		_leaderboardHistoryCache = leaderboardHistoryCache;
		_modArchiveCache = modArchiveCache;
		_spawnsetSummaryCache = spawnsetSummaryCache;
		_spawnsetHashCache = spawnsetHashCache;
		_logger = logger;
		_fileSystemService = fileSystemService;
	}

	[HttpPost("clear")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public ActionResult ClearCache([FromBody] string cacheType)
	{
		switch (cacheType)
		{
			case "LeaderboardHistory": _leaderboardHistoryCache.Clear(); break;
			case "LeaderboardStatistics": _leaderboardStatisticsCache.Initiate(); break;
			case "ModArchive": _modArchiveCache.Clear(); break;
			case "SpawnsetHash": _spawnsetHashCache.Clear(); break;
			case "SpawnsetSummary": _spawnsetSummaryCache.Clear(); break;
			default: return NotFound();
		}

		_logger.LogWarning("Memory cache '{cacheType}' was cleared.", cacheType);

		return Ok();
	}

	[HttpPost("history")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public ActionResult FixHistory([FromBody] string unused)
	{
		DateTime min = new(2018, 8, 1);
		DateTime max = new(2022, 1, 26, 12, 0, 0);

		foreach (string filePath in _fileSystemService.TryGetFiles(DataSubDirectory.LeaderboardHistory))
		{
			DateTime dateFromFile = HistoryUtils.HistoryJsonFileNameToDateTime(Path.GetFileName(filePath));
			if (dateFromFile < min || dateFromFile > max)
				continue;

			LeaderboardHistory history = _leaderboardHistoryCache.GetLeaderboardHistoryByFilePath(filePath);

			history.KillsGlobal -= 1_000_000_000;

			IoFile.WriteAllText(filePath, JsonConvert.SerializeObject(history, Formatting.None));
		}

		_logger.LogWarning("Fixed leaderboard history global kills.");

		return Ok();
	}
}
