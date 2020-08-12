using DevilDaggersCore.Website;
using DevilDaggersWebsite.Code.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using Io = System.IO;

namespace DevilDaggersWebsite.Code.Controllers
{
	[Route("api/tools")]
	[ApiController]
	public class ToolsController : ControllerBase
	{
		private readonly IWebHostEnvironment env;

		public ToolsController(IWebHostEnvironment env)
		{
			this.env = env;
		}

		[HttpGet]
		[ProducesResponseType(200)]
		public ActionResult<List<Tool>> GetTools()
			=> ToolList.Tools;

		[HttpGet("{toolName}/path")]
		[ProducesResponseType(200)]
		[ProducesResponseType(404)]
		public ActionResult<string> GetToolPath([Required] string toolName)
		{
			Tool tool = ToolList.Tools.FirstOrDefault(t => t.Name == toolName);
			if (tool == null)
				return new NotFoundObjectResult(new ProblemDetails { Title = $"Tool '{toolName}' was not found." });

			string path = Path.Combine("tools", toolName, $"{toolName}{tool.VersionNumber}.zip");

			if (!Io.File.Exists(Path.Combine(env.WebRootPath, path)))
				throw new Exception($"Tool file '{path}' does not exist.");

			return path;
		}
	}
}