using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.LeaderboardHistory;
using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.LeaderboardStatistics;
using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.ModArchives;
using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.SpawnsetHashes;
using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.SpawnsetSummaries;
using DevilDaggersInfo.Web.BlazorWasm.Server.InternalModels.Json;
using Microsoft.AspNetCore.Authorization;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Controllers.Admin;

[Route("api/admin")]
[ApiController]
[Authorize(Roles = Roles.Admin)]
public class AdminController : ControllerBase
{
	private readonly LeaderboardStatisticsCache _leaderboardStatisticsCache;
	private readonly LeaderboardHistoryCache _leaderboardHistoryCache;
	private readonly ModArchiveCache _modArchiveCache;
	private readonly SpawnsetSummaryCache _spawnsetSummaryCache;
	private readonly SpawnsetHashCache _spawnsetHashCache;
	private readonly ILogger<AdminController> _logger;
	private readonly IFileSystemService _fileSystemService;

	public AdminController(
		LeaderboardStatisticsCache leaderboardStatisticsCache,
		LeaderboardHistoryCache leaderboardHistoryCache,
		ModArchiveCache modArchiveCache,
		SpawnsetSummaryCache spawnsetSummaryCache,
		SpawnsetHashCache spawnsetHashCache,
		ILogger<AdminController> logger,
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

	[HttpPost("clear-cache")]
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

		_logger.LogInformation("Memory cache '{cacheType}' was cleared.", cacheType);

		return Ok();
	}

	[HttpPost("convert-history-to-binary")]
	public ActionResult BuildHistoryBinaries(string? unused)
	{
		const uint version = 0;

		foreach (string path in Directory.GetFiles(_fileSystemService.GetPath(DataSubDirectory.LeaderboardHistory), "*.json"))
		{
			LeaderboardHistory lh = JsonConvert.DeserializeObject<LeaderboardHistory>(IoFile.ReadAllText(path)) ?? throw new("can't parse json");

			using MemoryStream ms = new();
			using BinaryWriter bw = new(ms);
			bw.Write(version);
			bw.Write(lh.DateTime.Ticks);
			bw.Write(lh.Players);
			bw.Write(lh.TimeGlobal);
			bw.Write(lh.KillsGlobal);
			bw.Write(lh.GemsGlobal);
			bw.Write(lh.DeathsGlobal);
			bw.Write(lh.DaggersHitGlobal);
			bw.Write(lh.DaggersFiredGlobal);

			bw.Write(lh.Entries.Count);
			for (int i = 0; i < lh.Entries.Count; i++)
			{
				EntryHistory entryHistory = lh.Entries[i];
				bw.Write(entryHistory.Rank);
				bw.Write(entryHistory.Id);
				bw.Write(entryHistory.Username);
				bw.Write(entryHistory.Time);
				bw.Write(entryHistory.Kills);
				bw.Write(entryHistory.Gems);
				bw.Write(entryHistory.DeathType);
				bw.Write(entryHistory.DaggersHit);
				bw.Write(entryHistory.DaggersFired);
				bw.Write(entryHistory.TimeTotal);
				bw.Write(entryHistory.KillsTotal);
				bw.Write(entryHistory.GemsTotal);
				bw.Write(entryHistory.DeathsTotal);
				bw.Write(entryHistory.DaggersHitTotal);
				bw.Write(entryHistory.DaggersFiredTotal);
			}

			string newPath = path[0..^5] + ".bin";
			IoFile.WriteAllBytes(newPath, ms.ToArray());
		}

		_logger.LogInformation("All leaderboard history files have been converted to binary.");

		return Ok();
	}
}
