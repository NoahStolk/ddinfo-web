using DevilDaggersInfo.Web.ApiSpec.Tools;
using DevilDaggersInfo.Web.ApiSpec.Tools.Updates;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Web.Server.Controllers.Tools;

[Obsolete("This is only here for backwards compatibility with ddinfo-tools 0.10.3.0 and older.")]
[Route("api/app/updates")]
[ApiController]
public class UpdatesController : ControllerBase
{
	[Obsolete("This is only here for backwards compatibility with ddinfo-tools 0.10.3.0 and older.")]
	[HttpGet("latest")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<GetLatestVersion>> GetLatest([Required] AppOperatingSystem appOperatingSystem)
	{
		return new GetLatestVersion
		{
			FileSize = 0,
			VersionNumber = "0.0.0.0",
		};
	}

	[Obsolete("This is only here for backwards compatibility with ddinfo-tools 0.10.3.0 and older.")]
	[HttpGet("latest-file")]
	[ProducesResponseType(typeof(FileStreamResult), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult> GetLatestFile([Required] AppOperatingSystem appOperatingSystem)
	{
		return NotFound();
	}
}
