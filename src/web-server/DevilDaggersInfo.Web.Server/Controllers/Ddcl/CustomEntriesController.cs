using DevilDaggersInfo.Web.Server.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace DevilDaggersInfo.Web.Server.Controllers.Ddcl;

// TODO: Remove controller when DDCL 1.8.3 no longer needs a proper deprecation notice.
[Obsolete("DDCL 1.8.3 will be removed.")]
[Route("api/ddcl/custom-entries")]
[ApiController]
public class CustomEntriesController : ControllerBase
{
	[Obsolete("DDCL 1.8.3 will be removed.")]
	[HttpPost("/api/custom-entries/submit")]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public ActionResult SubmitScoreForDdclObsolete()
	{
		throw new BadRequestException("DDCL 1.8.3 is no longer supported. Go to https://www.devildaggers.info/ and download DDINFO TOOLS to connect to custom leaderboards.");
	}
}
