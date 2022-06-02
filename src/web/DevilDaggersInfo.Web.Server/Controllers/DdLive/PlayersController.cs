using DevilDaggersInfo.Api.DdLive.Players;

namespace DevilDaggersInfo.Web.Server.Controllers.DdLive;

[Route("api/ddlive/players")]
[ApiController]
public class PlayersController : ControllerBase
{
	private readonly ApplicationDbContext _dbContext;

	public PlayersController(ApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	[HttpGet("/api/players/common-names")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult<List<GetCommonName>> GetCommonNames()
	{
		return _dbContext.Players
			.AsNoTracking()
			.Select(p => new { p.Id, p.CommonName })
			.Where(p => p.CommonName != null)
			.Select(p => new GetCommonName
			{
				Id = p.Id,
				CommonName = p.CommonName!,
			})
			.ToList();
	}
}
