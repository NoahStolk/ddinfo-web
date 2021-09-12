using DevilDaggersWebsite.Dto;
using DevilDaggersWebsite.Enumerators;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IO;
using Io = System.IO;

namespace DevilDaggersWebsite.Api
{
	[Route("api/process-memory")]
	[ApiController]
	public class ProcessMemoryController : ControllerBase
	{
		private readonly IWebHostEnvironment _environment;

		public ProcessMemoryController(IWebHostEnvironment environment)
		{
			_environment = environment;
		}

		[HttpGet("marker")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public ActionResult<Marker> GetMarker(OperatingSystem operatingSystem)
		{
			DdclSettings ddclSettings = JsonConvert.DeserializeObject<DdclSettings?>(Io.File.ReadAllText(Path.Combine(_environment.WebRootPath, "tools", "DevilDaggersCustomLeaderboards", "Settings.json"))) ?? throw new("Could not deserialize DDCL settings JSON.");
			if (operatingSystem is OperatingSystem.Linux or OperatingSystem.Windows)
				return new Marker { Value = GetMarker(operatingSystem, ddclSettings) };

			return BadRequest($"Operating system '{operatingSystem}' is not supported.");

			static long GetMarker(OperatingSystem operatingSystem, DdclSettings ddclSettings) => operatingSystem switch
			{
				OperatingSystem.Linux => ddclSettings.MarkerLinuxSteam,
				OperatingSystem.Windows => ddclSettings.MarkerWindowsSteam,
				_ => 0,
			};
		}
	}
}
