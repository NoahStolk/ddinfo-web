using Microsoft.AspNetCore.Mvc;

namespace DevilDaggersInfo.Web.Server.Controllers.Ddcl;

// TODO: Remove controller when DDCL 1.8.3 no longer needs a proper deprecation notice.
[Obsolete("DDCL 1.8.3 will be removed.")]
[Route("api/ddcl/custom-leaderboards")]
[ApiController]
public class CustomLeaderboardsController : ControllerBase
{
	[Obsolete("DDCL 1.8.3 will be removed.")]
	[HttpHead("/api/custom-leaderboards")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult CustomLeaderboardExistsBySpawnsetHashObsolete()
	{
		// Always return HTTP 200 since errors from this endpoint are now shown in the old client.
		return Ok();
	}
}
