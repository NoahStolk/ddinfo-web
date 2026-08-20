using DevilDaggersInfo.Web.ApiSpec.Admin.BackgroundServices;
using DevilDaggersInfo.Web.Core.Claims;
using DevilDaggersInfo.Web.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevilDaggersInfo.Web.Server.Controllers.Admin;

[Route("api/admin/background-services")]
[ApiController]
[Authorize(Roles = Roles.Admin)]
public sealed class BackgroundServicesController(BackgroundServiceMonitor backgroundServiceMonitor) : ControllerBase
{
	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult<List<GetBackgroundServiceEntry>> GetBackgroundServices()
	{
		return backgroundServiceMonitor.GetEntries();
	}
}
