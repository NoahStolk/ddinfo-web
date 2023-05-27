using DevilDaggersInfo.Api.Admin;
using DevilDaggersInfo.Api.Admin.Players;
using DevilDaggersInfo.Web.Client;
using DevilDaggersInfo.Web.Core.Claims;
using DevilDaggersInfo.Web.Server.Domain.Admin.Repositories;
using DevilDaggersInfo.Web.Server.Domain.Admin.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Web.Server.Controllers.Admin;

[Route("api/admin/players")]
[ApiController]
public class PlayersController : ControllerBase
{
	private readonly PlayerRepository _playerRepository;
	private readonly PlayerService _playerService;

	public PlayersController(PlayerRepository playerRepository, PlayerService playerService)
	{
		_playerRepository = playerRepository;
		_playerService = playerService;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[Authorize(Roles = Roles.Players)]
	public async Task<ActionResult<Page<GetPlayerForOverview>>> GetPlayers(
		[Range(0, 1000)] int pageIndex = 0,
		[Range(Constants.PageSizeMin, Constants.PageSizeMax)] int pageSize = Constants.PageSizeDefault,
		PlayerSorting? sortBy = null,
		bool ascending = false)
		=> await _playerRepository.GetPlayersAsync(pageIndex, pageSize, sortBy, ascending);

	[HttpGet("names")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[Authorize(Roles = $"{Roles.Players},{Roles.Mods},{Roles.Spawnsets}")]
	public async Task<ActionResult<List<GetPlayerName>>> GetPlayerNames()
		=> await _playerRepository.GetPlayerNamesAsync();

	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[Authorize(Roles = Roles.Players)]
	public async Task<ActionResult<GetPlayer>> GetPlayerById(int id)
		=> await _playerRepository.GetPlayerAsync(id);

	[HttpPost]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status409Conflict)]
	[Authorize(Roles = Roles.Players)]
	public async Task<ActionResult> AddPlayer(AddPlayer addPlayer)
	{
		await _playerService.AddPlayerAsync(addPlayer);
		return Ok();
	}

	[HttpPut("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[Authorize(Roles = Roles.Players)]
	public async Task<ActionResult> EditPlayerById(int id, EditPlayer editPlayer)
	{
		await _playerService.EditPlayerAsync(id, editPlayer);
		return Ok();
	}

	[HttpDelete("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[Authorize(Roles = Roles.Players)]
	public async Task<ActionResult> DeletePlayerById(int id)
	{
		await _playerService.DeletePlayerAsync(id);
		return Ok();
	}
}
