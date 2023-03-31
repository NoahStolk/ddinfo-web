using DevilDaggersInfo.Web.Server.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace DevilDaggersInfo.Web.Server.Controllers.Ddcl;

[Obsolete("DDCL 1.8.3 will be removed.")]
[Route("api/ddcl/custom-leaderboards")]
[ApiController]
public class CustomLeaderboardsController : ControllerBase
{
	[Obsolete("DDCL 1.8.3 will be removed.")]
	[HttpHead("/api/custom-leaderboards")]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public ActionResult CustomLeaderboardExistsBySpawnsetHashObsolete()
	{
		throw new BadRequestException("DDCL 1.8.3 is no longer supported. Go to https://www.devildaggers.info/ and download DDINFO TOOLS to connect to custom leaderboards");
	}
}
