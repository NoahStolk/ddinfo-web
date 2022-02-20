using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.Health;
using Microsoft.AspNetCore.Authorization;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Controllers.Admin;

[Route("api/admin/health")]
[ApiController]
[Authorize(Roles = Roles.Admin)]
public class HealthController : ControllerBase
{
	private readonly ResponseTimeMonitor _responseTimeMonitor;
	private readonly ILogger<CachesController> _logger;

	public HealthController(
		ResponseTimeMonitor responseTimeMonitor,
		ILogger<CachesController> logger)
	{
		_responseTimeMonitor = responseTimeMonitor;
		_logger = logger;
	}

	[HttpGet("responses")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult<List<GetResponseTimeEntry>> GetResponseTimes(DateTime startDateTime, DateTime endDateTime)
	{
		return _responseTimeMonitor.GetEntries(startDateTime, endDateTime);
	}

	[HttpPost("responses/clear")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult ClearResponseTimes(string? empty)
	{
		_logger.LogWarning("Response times were cleared ({count}).", _responseTimeMonitor.GetCount());

		_responseTimeMonitor.Clear();

		return Ok();
	}
}
