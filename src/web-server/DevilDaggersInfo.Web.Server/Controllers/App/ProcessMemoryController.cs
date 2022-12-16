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
	public async Task<ActionResult<GetMarker>> GetMarker([Required] SupportedOperatingSystem operatingSystem)
		=> await GetMarkerRepo(operatingSystem);

	private async Task<GetMarker> GetMarkerRepo(SupportedOperatingSystem operatingSystem) => new()
	{
		Value = await _markerRepository.GetMarkerAsync(operatingSystem switch
		{
			SupportedOperatingSystem.Windows => "WindowsSteam",
			SupportedOperatingSystem.Linux => "LinuxSteam",
			_ => throw new UnsupportedOperatingSystemException($"Operating system '{operatingSystem}' is not supported."),
		}),
	};
}
