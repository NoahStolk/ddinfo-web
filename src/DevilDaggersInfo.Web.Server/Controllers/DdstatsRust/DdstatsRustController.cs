using DevilDaggersInfo.Web.ApiSpec.DdstatsRust;
using DevilDaggersInfo.Web.Server.Domain.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Web.Server.Controllers.DdstatsRust;

[ApiController]
public class DdstatsRustController : ControllerBase
{
	private readonly MarkerRepository _markerRepository;

	public DdstatsRustController(MarkerRepository markerRepository)
	{
		_markerRepository = markerRepository;
	}

	[HttpGet("/api/integrations/ddstats-rust")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult<DdstatsRustAccessInfo> GetIntegration()
	{
		return new DdstatsRustAccessInfo { RequiredVersion = "0.6.10.5" };
	}

	[HttpGet("/api/process-memory/marker")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<GetMarker>> GetMarker([Required] SupportedOperatingSystem operatingSystem)
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
