using DevilDaggersInfo.Web.Core.Claims;
using DevilDaggersInfo.Web.Server.Domain.Admin.Repositories;
using DevilDaggersInfo.Web.Server.Domain.Admin.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Web.Server.Controllers.Admin;

[Route("api/admin/markers")]
[ApiController]
[Authorize(Roles = Roles.CustomLeaderboards)]
public class MarkersController : ControllerBase
{
	private readonly MarkerRepository _markerRepository;
	private readonly MarkerService _markerService;

	public MarkersController(MarkerRepository markerRepository, MarkerService markerService)
	{
		_markerRepository = markerRepository;
		_markerService = markerService;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<ActionResult<List<string>>> GetMarkers()
	{
		return await _markerRepository.GetMarkerNamesAsync();
	}

	[HttpPut("{name}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult> EditMarker(string name, [Required, FromBody] long value)
	{
		await _markerService.EditMarkerAsync(name, value);
		return Ok();
	}
}
