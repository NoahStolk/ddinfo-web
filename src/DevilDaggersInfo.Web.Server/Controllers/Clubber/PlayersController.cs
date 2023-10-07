using DevilDaggersInfo.Api.Clubber.Players;
using DevilDaggersInfo.Web.Server.Converters.DomainToApi.Clubber;
using DevilDaggersInfo.Web.Server.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Web.Server.Controllers.Clubber;

[Route("api/clubber/players")]
[ApiController]
public class PlayersController : ControllerBase
{
	private readonly PlayerHistoryRepository _playerHistoryRepository;
	private readonly PlayerRepository _playerRepository;

	public PlayersController(PlayerHistoryRepository playerHistoryRepository, PlayerRepository playerRepository)
	{
		_playerHistoryRepository = playerHistoryRepository;
		_playerRepository = playerRepository;
	}

	[HttpGet("{id}/country-code")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<GetPlayerCountryCode>> GetPlayerCountryCodeById([Required] int id)
	{
		return new GetPlayerCountryCode
		{
			CountryCode = await _playerRepository.GetPlayerCountryCodeAsync(id),
		};
	}

	[HttpGet("{id}/history")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public GetPlayerHistory GetPlayerHistoryById([Required, Range(1, int.MaxValue)] int id)
	{
		return _playerHistoryRepository.GetPlayerHistoryById(id).ToClubberApi();
	}
}
