using DevilDaggersWebsite.Entities;
using DevilDaggersWebsite.Enumerators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace DevilDaggersWebsite.Api
{
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
		public ActionResult<Dto.Marker> GetMarker(OperatingSystem operatingSystem)
		{
			string? name = operatingSystem switch
			{
				OperatingSystem.Windows => "WindowsSteam",
				OperatingSystem.Linux => "LinuxSteam",
				_ => null,
			};

			if (name == null)
				return BadRequest($"Operating system '{operatingSystem}' is not supported.");

			Marker? marker = _dbContext.Markers.FirstOrDefault(m => m.Name == name);
			if (marker == null)
				throw new($"Marker key '{name}' was not found in database.");

			return new Dto.Marker { Value = marker.Value };
		}
	}
}
