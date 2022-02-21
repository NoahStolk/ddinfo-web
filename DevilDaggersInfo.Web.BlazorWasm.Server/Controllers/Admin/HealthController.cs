using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.Health;
using Microsoft.AspNetCore.Authorization;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Controllers.Admin;

[Route("api/admin/health")]
[ApiController]
[Authorize(Roles = Roles.Admin)]
public class HealthController : ControllerBase
{
	private readonly ResponseTimeMonitor _responseTimeMonitor;

	public HealthController(ResponseTimeMonitor responseTimeMonitor)
	{
		_responseTimeMonitor = responseTimeMonitor;
	}

	[HttpGet("responses")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult<List<GetResponseTimeEntry>> GetResponseTimes(DateTime date)
	{
		return _responseTimeMonitor.GetLogEntries(DateOnly.FromDateTime(date));
	}
}
