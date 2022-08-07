using DevilDaggersInfo.Api.Admin.BackgroundServices;
using DevilDaggersInfo.Web.Core.Claims;
using Microsoft.AspNetCore.Authorization;

namespace DevilDaggersInfo.Web.Server.Controllers.Admin;

[Route("api/admin/background-services")]
[ApiController]
[Authorize(Roles = Roles.Admin)]
public class BackgroundServicesController : ControllerBase
{
	private readonly BackgroundServiceMonitor _backgroundServiceMonitor;

	public BackgroundServicesController(BackgroundServiceMonitor backgroundServiceMonitor)
	{
		_backgroundServiceMonitor = backgroundServiceMonitor;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult<List<GetBackgroundServiceEntry>> GetBackgroundServices()
	{
		return _backgroundServiceMonitor.GetEntries();
	}
}
