using DevilDaggersInfo.Api.DdstatsRust;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DevilDaggersInfo.Web.Server.Controllers.DdstatsRust;

[Route("api/ddstats-rust/access")]
[ApiController]
public class AccessController : ControllerBase
{
	private readonly ApplicationDbContext _dbContext;

	public AccessController(ApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	[HttpGet("/api/integrations/ddstats-rust")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult<DdstatsRustAccessInfo> GetDdstatsRustAccessInfo()
	{
		ToolEntity? ddstatsRust = _dbContext.Tools.AsNoTracking().FirstOrDefault(t => t.Name == "ddstats-rust");
		if (ddstatsRust == null)
			throw new("ddstats-rust not found in database.");

		return new DdstatsRustAccessInfo { RequiredVersion = ddstatsRust.RequiredVersionNumber };
	}
}
