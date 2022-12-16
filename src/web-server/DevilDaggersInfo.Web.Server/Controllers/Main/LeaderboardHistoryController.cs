using DevilDaggersInfo.Api.Main.LeaderboardHistory;
using DevilDaggersInfo.Web.Server.Converters.DomainToApi.Main;
using DevilDaggersInfo.Web.Server.Domain.Models.LeaderboardHistory;
using DevilDaggersInfo.Web.Server.Domain.Services.Caching;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using Microsoft.AspNetCore.Mvc;

namespace DevilDaggersInfo.Web.Server.Controllers.Main;

[Route("api/leaderboard-history")]
[ApiController]
public class LeaderboardHistoryController : ControllerBase
{
	private readonly IFileSystemService _fileSystemService;
	private readonly ILeaderboardHistoryCache _leaderboardHistoryCache;

	public LeaderboardHistoryController(IFileSystemService fileSystemService, ILeaderboardHistoryCache leaderboardHistoryCache)
	{
		_fileSystemService = fileSystemService;
		_leaderboardHistoryCache = leaderboardHistoryCache;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public ActionResult<GetLeaderboardHistory> GetLeaderboardHistory(DateTime dateTime)
	{
		string historyPath = _fileSystemService.GetLeaderboardHistoryPathFromDate(dateTime);
		LeaderboardHistory history = _leaderboardHistoryCache.GetLeaderboardHistoryByFilePath(historyPath);
		return history.ToDto();
	}
}
