using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.LeaderboardHistory;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.LeaderboardHistory;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.PlayerHistory;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Controllers.Public;

[Route("api/player-history")]
[ApiController]
public class PlayerHistoryController : ControllerBase
{
	private readonly IFileSystemService _fileSystemService;
	private readonly LeaderboardHistoryCache _leaderboardHistoryCache;

	public PlayerHistoryController(IFileSystemService fileSystemService, LeaderboardHistoryCache leaderboardHistoryCache)
	{
		_fileSystemService = fileSystemService;
		_leaderboardHistoryCache = leaderboardHistoryCache;
	}

	[HttpGet("progression")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public List<GetEntryHistory> GetPlayerProgressionById([Required, Range(1, 9999999)] int playerId)
	{
		List<GetEntryHistory> data = new();

		foreach (string leaderboardHistoryPath in _fileSystemService.TryGetFiles(DataSubDirectory.LeaderboardHistory))
		{
			GetLeaderboardHistory leaderboard = _leaderboardHistoryCache.GetLeaderboardHistoryByFilePath(leaderboardHistoryPath);
			GetEntryHistory? entry = leaderboard.Entries.Find(e => e.Id == playerId);

			// + 1 and - 1 are used to fix off-by-one errors in the history based on screenshots and videos. This is due to a rounding error in Devil Daggers itself.
			if (entry != null && !data.Any(e =>
				e.Time == entry.Time ||
				e.Time == entry.Time + 1 ||
				e.Time == entry.Time - 1))
			{
				data.Add(entry);
			}
		}

		return data;
	}

	[HttpGet("activity")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public List<GetPlayerActivity> GetPlayerActivityById([Required, Range(1, 9999999)] int playerId)
	{
		List<GetPlayerActivity> data = new();
		foreach (string leaderboardHistoryPath in _fileSystemService.TryGetFiles(DataSubDirectory.LeaderboardHistory))
		{
			GetLeaderboardHistory leaderboard = _leaderboardHistoryCache.GetLeaderboardHistoryByFilePath(leaderboardHistoryPath);
			GetEntryHistory? entry = leaderboard.Entries.Find(e => e.Id == playerId);
			if (entry?.DeathsTotal > 0)
			{
				data.Add(new()
				{
					DateTime = leaderboard.DateTime,
					DeathsTotal = entry.DeathsTotal,
				});
			}
		}

		return data;
	}
}
