using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.ProcessMemory;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Controllers.Public;

[Route("api/process-memory")]
[ApiController]
public class ProcessMemoryController : ControllerBase
{
	private readonly ApplicationDbContext _dbContext;

	public ProcessMemoryController(ApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	[HttpGet("marker")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public ActionResult<Marker> GetMarker(SupportedOperatingSystem operatingSystem)
	{
		string? name = operatingSystem switch
		{
			SupportedOperatingSystem.Windows => "WindowsSteam",
			SupportedOperatingSystem.Linux => "LinuxSteam",
			_ => null,
		};

		if (name == null)
			return BadRequest($"Operating system '{operatingSystem}' is not supported.");

		MarkerEntity? marker = _dbContext.Markers.FirstOrDefault(m => m.Name == name);
		if (marker == null)
			throw new($"Marker key '{name}' was not found in database.");

		return new Marker { Value = marker.Value };
	}
}
