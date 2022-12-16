using DevilDaggersInfo.Api.Ddcl.ProcessMemory;
using DevilDaggersInfo.Web.Server.Domain.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Web.Server.Controllers.Ddcl;

[Obsolete("DDCL 1.8.3 will be removed.")]
[Route("api/ddcl/process-memory")]
[ApiController]
public class ProcessMemoryController : ControllerBase
{
	private readonly MarkerRepository _markerRepository;

	public ProcessMemoryController(MarkerRepository markerRepository)
	{
		_markerRepository = markerRepository;
	}

	// FORBIDDEN: Used by ddstats-rust.
	[Obsolete("DDCL 1.8.3 will be removed.")]
	[HttpGet("/api/process-memory/marker")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<GetMarker>> GetMarkerObsolete([Required] SupportedOperatingSystem operatingSystem)
	{
		return new GetMarker
		{
			Value = await _markerRepository.GetMarkerAsync(operatingSystem switch
			{
				SupportedOperatingSystem.Windows => "WindowsSteam",
				SupportedOperatingSystem.Linux => "LinuxSteam",
				_ => throw new UnsupportedOperatingSystemException($"Operating system '{operatingSystem}' is not supported."),
			}),
		};
	}
}
