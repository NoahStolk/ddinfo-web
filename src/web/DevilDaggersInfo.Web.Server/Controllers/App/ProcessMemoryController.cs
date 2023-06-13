using DevilDaggersInfo.Api.App;
using DevilDaggersInfo.Api.App.ProcessMemory;
using DevilDaggersInfo.Web.Server.Domain.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Web.Server.Controllers.App;

[Route("api/app/process-memory")]
[ApiController]
public class ProcessMemoryController : ControllerBase
{
	private readonly MarkerRepository _markerRepository;

	public ProcessMemoryController(MarkerRepository markerRepository)
	{
		_markerRepository = markerRepository;
	}

	[HttpGet("marker")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]

	// TODO: Make AppOperatingSystem required.
	public async Task<ActionResult<GetMarker>> GetMarker(AppOperatingSystem? appOperatingSystem, SupportedOperatingSystem? operatingSystem)
	{
		if (operatingSystem.HasValue)
		{
			appOperatingSystem = operatingSystem switch
			{
				SupportedOperatingSystem.Windows => AppOperatingSystem.Windows,
				SupportedOperatingSystem.Linux => AppOperatingSystem.Linux,
				_ => throw new UnsupportedOperatingSystemException($"Operating system '{operatingSystem}' is not supported."),
			};
		}

		return new GetMarker
		{
			Value = await _markerRepository.GetMarkerAsync(appOperatingSystem switch
			{
				AppOperatingSystem.Windows => "WindowsSteam",
				AppOperatingSystem.Linux => "LinuxSteam",
				_ => throw new UnsupportedOperatingSystemException($"Operating system '{appOperatingSystem}' is not supported."),
			}),
		};
	}
}
