using DevilDaggersInfo.Web.ApiSpec.Main.Players;
using DevilDaggersInfo.Web.Server.Converters.DomainToApi.Main;
using DevilDaggersInfo.Web.Server.Domain.Main.Repositories;
using DevilDaggersInfo.Web.Server.Domain.Main.Services;
using DevilDaggersInfo.Web.Server.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Web.Server.Controllers.Main;

[Route("api/players")]
[ApiController]
public sealed class PlayersController(
	PlayerCustomLeaderboardStatisticsRepository playerCustomLeaderboardStatisticsRepository,
	PlayerHistoryRepository playerHistoryRepository,
	PlayerProfileRepository profileRepository,
	PlayerProfileService profileService,
	PlayerRepository playerRepository)
	: ControllerBase
{
	[HttpGet("leaderboard")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<ActionResult<List<GetPlayerForLeaderboard>>> GetPlayersForLeaderboard()
	{
		List<Domain.Models.Players.PlayerForLeaderboard> players = await playerRepository.GetPlayersForLeaderboardAsync();
		return players.ConvertAll(p => p.ToMainApi());
	}

	[HttpGet("settings")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<ActionResult<List<GetPlayerForSettings>>> GetPlayersForSettings()
	{
		List<Domain.Models.Players.PlayerForSettings> players = await playerRepository.GetPlayersForSettingsAsync();
		return players.ConvertAll(p => p.ToMainApi());
	}

	// FORBIDDEN: Used by DDLIVE.
	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<GetPlayer>> GetPlayerById([Required] int id)
	{
		Domain.Models.Players.Player player = await playerRepository.GetPlayerAsync(id);
		return player.ToMainApi();
	}

	// FORBIDDEN: Used by DDLIVE.
	[HttpGet("{id}/history")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public GetPlayerHistory GetPlayerHistoryById([Required, Range(1, int.MaxValue)] int id)
	{
		return playerHistoryRepository.GetPlayerHistoryById(id).ToMainApi();
	}

	[HttpGet("{id}/custom-leaderboard-statistics")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<ActionResult<List<GetPlayerCustomLeaderboardStatistics>>> GetCustomLeaderboardStatisticsByPlayerId([Required, Range(1, int.MaxValue)] int id)
	{
		return await playerCustomLeaderboardStatisticsRepository.GetCustomLeaderboardStatisticsByPlayerIdAsync(id);
	}

	[Authorize]
	[HttpGet("{id}/profile")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<GetPlayerProfile>> GetProfileByPlayerId([Required] int id)
	{
		return await profileRepository.GetProfileAsync(User, id);
	}

	[Authorize]
	[HttpPut("{id}/profile")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult> UpdateProfileByPlayerId([Required] int id, EditPlayerProfile editPlayerProfile)
	{
		await profileService.UpdateProfileAsync(User, id, editPlayerProfile);
		return Ok();
	}
}
