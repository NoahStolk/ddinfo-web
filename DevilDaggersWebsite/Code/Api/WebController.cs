using DevilDaggersCore.Website;
using DevilDaggersWebsite.Code.Tasks;
using DevilDaggersWebsite.Code.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Io = System.IO;

namespace DevilDaggersWebsite.Code.Api
{
	[Route("api/[controller]")]
	[ApiController]
	public class WebController : ControllerBase
	{
		[HttpGet("tools")]
		[ProducesResponseType(200)]
		public ActionResult<List<Tool>> GetTools()
			=> ToolList.Tools;

		[HttpGet("web-stats")]
		[ProducesResponseType(200)]
		public static WebStatsResult GetWebStats()
			=> new WebStatsResult(
				Io.File.GetLastWriteTime(Assembly.GetExecutingAssembly().Location),
				TaskInstanceKeeper.Instances.Select(kvp => new TaskResult(kvp.Key.Name, kvp.Value.LastTriggered, kvp.Value.LastFinished, kvp.Value.ExecutionTime, kvp.Value.Schedule)).ToList());
	}
}