using DevilDaggersInfo.Api.Admin.Health;
using Microsoft.AspNetCore.Authorization;

namespace DevilDaggersInfo.Web.Server.Controllers.Admin;

[Route("api/admin/health")]
[ApiController]
[Authorize(Roles = Roles.Admin)]
public class HealthController : ControllerBase
{
	private readonly ResponseTimeMonitor _responseTimeMonitor;
	private readonly ILogger<HealthController> _logger;

	public HealthController(ResponseTimeMonitor responseTimeMonitor, ILogger<HealthController> logger)
	{
		_responseTimeMonitor = responseTimeMonitor;
		_logger = logger;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult<GetResponseTimes> GetResponseTimes(DateTime date)
	{
		return _responseTimeMonitor.GetLogEntries(DateOnly.FromDateTime(date));
	}

	[HttpPost("force-dump")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult ForceDump(string? unused)
	{
		_responseTimeMonitor.DumpLogs();

		_logger.LogInformation("Logs were dumped manually.");

		return Ok();
	}
}
