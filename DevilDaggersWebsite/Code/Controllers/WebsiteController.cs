using DevilDaggersCore.Website;
using DevilDaggersWebsite.Code.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Reflection;
using Io = System.IO;

namespace DevilDaggersWebsite.Code.Controllers
{
	[Route("api/website")]
	[ApiController]
	public class WebsiteController : ControllerBase
	{
		[HttpGet("stats")]
		[ProducesResponseType(200)]
		public ActionResult<WebStatsResult> GetStats()
			=> new WebStatsResult(
				Io.File.GetLastWriteTime(Assembly.GetExecutingAssembly().Location),
				TaskInstanceKeeper.Instances.Select(kvp => new TaskResult(kvp.Key.Name, kvp.Value.LastTriggered, kvp.Value.LastFinished, kvp.Value.ExecutionTime, kvp.Value.Schedule)).ToList());
	}
}