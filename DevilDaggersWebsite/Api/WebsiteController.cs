using DevilDaggersWebsite.Dto;
using DevilDaggersWebsite.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Reflection;
using Io = System.IO;

namespace DevilDaggersWebsite.Api
{
	[Route("api/website")]
	[ApiController]
	public class WebsiteController : ControllerBase
	{
		[HttpGet("stats")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public ActionResult<WebStatsResult> GetStats()
			=> new WebStatsResult(
				Io.File.GetLastWriteTime(Assembly.GetExecutingAssembly().Location),
				TaskInstanceKeeper.Instances.Select(kvp => new TaskResult(kvp.Key.Name, kvp.Value.LastTriggered, kvp.Value.LastFinished, kvp.Value.ExecutionTime, kvp.Value.Schedule)).ToList());
	}
}
