using DevilDaggersInfo.Api.Clubber;
using DevilDaggersInfo.Web.Server.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Web.Server.Controllers.Clubber;

[Route("api/clubber/players")]
[ApiController]
public class PlayersController : ControllerBase
{
	private readonly PlayerRepository _playerRepository;

	public PlayersController(PlayerRepository playerRepository)
	{
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
}
