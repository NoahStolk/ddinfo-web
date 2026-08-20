using DevilDaggersInfo.Web.Core.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevilDaggersInfo.Web.Server.Controllers.Admin;

[Route("api/admin/logging")]
[ApiController]
[Authorize(Roles = Roles.Admin)]
public sealed class LoggingController(ILogger<LoggingController> logger) : ControllerBase
{
	[HttpPost("test-exception")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult TestException(string? message)
	{
		throw new InvalidOperationException("Test exception:" + message);
	}

	[HttpPost("log-error")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult LogError(string? message)
	{
		logger.LogError("Test log error: {Message}", message);
		return Ok();
	}

	[HttpPost("log-warning")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult LogWarning(string? message)
	{
		logger.LogWarning("Test log warning: {Message}", message);
		return Ok();
	}

	[HttpPost("log-info")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult LogInfo(string? message)
	{
		logger.LogInformation("Test log info: {Message}", message);
		return Ok();
	}
}
