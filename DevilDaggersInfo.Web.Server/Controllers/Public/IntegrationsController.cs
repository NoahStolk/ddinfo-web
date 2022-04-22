using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Integrations;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Controllers.Public;

[Route("api/integrations")]
[ApiController]
public class IntegrationsController : ControllerBase
{
	private readonly ApplicationDbContext _dbContext;

	public IntegrationsController(ApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	[HttpGet("ddstats-rust")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult<DdstatsRustAccessInfo> GetDdstatsRustAccessInfo()
	{
		ToolEntity? ddstatsRust = _dbContext.Tools.AsNoTracking().FirstOrDefault(t => t.Name == "ddstats-rust");
		if (ddstatsRust == null)
			throw new("ddstats-rust not found in database.");

		return new DdstatsRustAccessInfo
		{
			RequiredVersion = Version.Parse(ddstatsRust.RequiredVersionNumber),
		};
	}
}
