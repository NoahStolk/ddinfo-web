using DevilDaggersInfo.Web.Core.Claims;
using Microsoft.AspNetCore.Authorization;

namespace DevilDaggersInfo.Web.Server.Controllers.Admin;

[Route("api/admin/markers")]
[ApiController]
[Authorize(Roles = Roles.CustomLeaderboards)]
public class MarkersController : ControllerBase
{
	private readonly ApplicationDbContext _dbContext;
	private readonly ILogger<MarkersController> _logger;

	public MarkersController(ApplicationDbContext dbContext, ILogger<MarkersController> logger)
	{
		_dbContext = dbContext;
		_logger = logger;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<ActionResult<List<string>>> GetMarkers()
	{
		return await _dbContext.Markers.AsNoTracking().Select(m => m.Name).ToListAsync();
	}

	[HttpPut("{name}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult> EditMarker(string name, [Required] long value)
	{
		MarkerEntity? marker = await _dbContext.Markers.FirstOrDefaultAsync(m => m.Name == name);
		if (marker == null)
			throw new NotFoundException();

		long oldValue = marker.Value;
		marker.Value = value;
		await _dbContext.SaveChangesAsync();

		_logger.LogWarning("Marker '{markerName}' was updated from '{old}' to '{new}'.", marker.Name, $"0x{oldValue:X16}", $"0x{marker.Value:X16}");

		return Ok();
	}
}
