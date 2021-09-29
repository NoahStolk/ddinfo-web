using DevilDaggersWebsite.Dto;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IO;
using Io = System.IO;

namespace DevilDaggersWebsite.Api
{
	[Route("api/integrations")]
	[ApiController]
	public class IntegrationsController : ControllerBase
	{
		private readonly IWebHostEnvironment _environment;

		public IntegrationsController(IWebHostEnvironment environment)
		{
			_environment = environment;
		}

		[HttpGet("ddstats-rust")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public ActionResult<DdstatsRustAccessInfo> GetDdstatsRustAccessInfo()
		{
			return JsonConvert.DeserializeObject<DdstatsRustAccessInfo?>(Io.File.ReadAllText(Path.Combine(_environment.WebRootPath, "integrations", "ddstats-rust.json"))) ?? throw new("Could not deserialize DdstatsRustAccessInfo.");
		}
	}
}
