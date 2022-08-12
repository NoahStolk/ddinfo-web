using DevilDaggersInfo.Api.DdLive.Players;
using DevilDaggersInfo.Web.Server.Domain.Models.Players;
using DevilDaggersInfo.Web.Server.Domain.Repositories;

namespace DevilDaggersInfo.Web.Server.Controllers.DdLive;

[Route("api/ddlive/players")]
[ApiController]
public class PlayersController : ControllerBase
{
	private readonly PlayerRepository _playerRepository;

	public PlayersController(PlayerRepository playerRepository)
	{
		_playerRepository = playerRepository;
	}

	[HttpGet("common-names")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<ActionResult<List<GetCommonName>>> GetCommonNames()
	{
		List<PlayerCommonName> commonNames = await _playerRepository.GetCommonNamesAsync();
		return commonNames.ConvertAll(cn => new GetCommonName
		{
			CommonName = cn.CommonName,
			Id = cn.Id,
		});
	}
}
