using DevilDaggersInfo.Web.ApiSpec.Main.LeaderboardHistory;
using DevilDaggersInfo.Web.Server.Converters.DomainToApi.Main;
using DevilDaggersInfo.Web.Server.Domain.Models.LeaderboardHistory;
using DevilDaggersInfo.Web.Server.Domain.Services.Caching;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using DevilDaggersInfo.Web.Server.Domain.Utils;
using Microsoft.AspNetCore.Mvc;

namespace DevilDaggersInfo.Web.Server.Controllers.Main;

[Route("api/leaderboard-history")]
[ApiController]
public sealed class LeaderboardHistoryController(IFileSystem fileSystem, ILeaderboardHistoryCache leaderboardHistoryCache) : ControllerBase
{
	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<GetLeaderboardHistory>> GetLeaderboardHistory(DateTime dateTime)
	{
		string historyFileName = HistoryUtils.GetHistoryFileNameFromDate(await HistoryUtils.GetHistoryFileNamesAsync(fileSystem), dateTime);
		LeaderboardHistory history = await leaderboardHistoryCache.GetLeaderboardHistoryAsync(historyFileName);
		return history.ToMainApi();
	}
}
