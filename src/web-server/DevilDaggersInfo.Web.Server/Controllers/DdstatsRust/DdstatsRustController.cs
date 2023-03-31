using DevilDaggersInfo.Api.DdstatsRust;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Web.Server.Controllers.DdstatsRust;

[ApiController]
public class DdstatsRustController : ControllerBase
{
	private readonly ApplicationDbContext _dbContext;
	private readonly MarkerRepository _markerRepository;

	public DdstatsRustController(ApplicationDbContext dbContext, MarkerRepository markerRepository)
	{
		_dbContext = dbContext;
		_markerRepository = markerRepository;
	}

	[HttpGet("/api/integrations/ddstats-rust")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult<DdstatsRustAccessInfo> GetIntegration()
	{
		ToolEntity? ddstatsRust = _dbContext.Tools.AsNoTracking().FirstOrDefault(t => t.Name == "ddstats-rust");
		if (ddstatsRust == null)
			throw new("ddstats-rust not found in database.");

		return new DdstatsRustAccessInfo { RequiredVersion = ddstatsRust.RequiredVersionNumber };
	}

	// Used by DDCL 1.8.3.
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
