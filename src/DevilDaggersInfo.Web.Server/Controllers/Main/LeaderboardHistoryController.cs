using DevilDaggersInfo.Web.ApiSpec.Main.LeaderboardHistory;
using DevilDaggersInfo.Web.Server.Converters.DomainToApi.Main;
using DevilDaggersInfo.Web.Server.Domain.Models.LeaderboardHistory;
using DevilDaggersInfo.Web.Server.Domain.Services.Caching;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using Microsoft.AspNetCore.Mvc;

namespace DevilDaggersInfo.Web.Server.Controllers.Main;

[Route("api/leaderboard-history")]
[ApiController]
public sealed class LeaderboardHistoryController(IFileSystemService fileSystemService, ILeaderboardHistoryCache leaderboardHistoryCache) : ControllerBase
{
	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public ActionResult<GetLeaderboardHistory> GetLeaderboardHistory(DateTime dateTime)
	{
		string historyPath = fileSystemService.GetLeaderboardHistoryPathFromDate(dateTime);
		LeaderboardHistory history = leaderboardHistoryCache.GetLeaderboardHistoryByFilePath(historyPath);
		return history.ToMainApi();
	}
}
