using DevilDaggersInfo.Api.Main.Players;
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
public class PlayersController : ControllerBase
{
	private readonly PlayerCustomLeaderboardStatisticsRepository _playerCustomLeaderboardStatisticsRepository;
	private readonly PlayerRepository _playerRepository;
	private readonly PlayerHistoryRepository _playerHistoryRepository;
	private readonly PlayerProfileService _profileService;
	private readonly PlayerProfileRepository _profileRepository;

	public PlayersController(
		PlayerCustomLeaderboardStatisticsRepository playerCustomLeaderboardStatisticsRepository,
		PlayerRepository playerRepository,
		PlayerHistoryRepository playerHistoryRepository,
		PlayerProfileService profileService,
		PlayerProfileRepository profileRepository)
	{
		_playerCustomLeaderboardStatisticsRepository = playerCustomLeaderboardStatisticsRepository;
		_playerRepository = playerRepository;
		_playerHistoryRepository = playerHistoryRepository;
		_profileService = profileService;
		_profileRepository = profileRepository;
	}

	[HttpGet("leaderboard")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<ActionResult<List<GetPlayerForLeaderboard>>> GetPlayersForLeaderboard()
	{
		List<Domain.Models.Players.PlayerForLeaderboard> players = await _playerRepository.GetPlayersForLeaderboardAsync();
		return players.ConvertAll(p => p.ToMainApi());
	}

	[HttpGet("settings")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<ActionResult<List<GetPlayerForSettings>>> GetPlayersForSettings()
	{
		List<Domain.Models.Players.PlayerForSettings> players = await _playerRepository.GetPlayersForSettingsAsync();
		return players.ConvertAll(p => p.ToMainApi());
	}

	// FORBIDDEN: Used by DDLIVE.
	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<GetPlayer>> GetPlayerById([Required] int id)
	{
		Domain.Models.Players.Player player = await _playerRepository.GetPlayerAsync(id);
		return player.ToMainApi();
	}

	// FORBIDDEN: Used by DDLIVE.
	[HttpGet("{id}/history")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public GetPlayerHistory GetPlayerHistoryById([Required, Range(1, int.MaxValue)] int id)
	{
		return _playerHistoryRepository.GetPlayerHistoryById(id);
	}

	[HttpGet("{id}/custom-leaderboard-statistics")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<ActionResult<List<GetPlayerCustomLeaderboardStatistics>>> GetCustomLeaderboardStatisticsByPlayerId([Required, Range(1, int.MaxValue)] int id)
	{
		return await _playerCustomLeaderboardStatisticsRepository.GetCustomLeaderboardStatisticsByPlayerIdAsync(id);
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
		return await _profileRepository.GetProfileAsync(User, id);
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
		await _profileService.UpdateProfileAsync(User, id, editPlayerProfile);
		return Ok();
	}
}
