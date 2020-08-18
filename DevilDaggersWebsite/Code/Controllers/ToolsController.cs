using DevilDaggersWebsite.Code.DataTransferObjects;
using DevilDaggersWebsite.Code.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net.Mime;
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
		public ActionResult<List<Tool>> GetTools(string? toolNameFilter = null)
		{
			IEnumerable<Tool> tools = ToolList.Tools;
			if (!string.IsNullOrEmpty(toolNameFilter))
				tools = tools.Where(t => t.Name.Contains(toolNameFilter, StringComparison.InvariantCulture));
			return tools.ToList();
		}

		[HttpGet("{toolName}/file")]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		[ProducesResponseType(404)]
		public ActionResult GetToolFile([Required] string toolName)
		{
			Tool tool = ToolList.Tools.FirstOrDefault(t => t.Name == toolName);
			if (tool == null)
				return new NotFoundObjectResult(new ProblemDetails { Title = $"Tool '{toolName}' was not found." });

			string path = Path.Combine("tools", tool.Name, $"{tool.Name}{tool.VersionNumber}.zip");
			if (!Io.File.Exists(Path.Combine(env.WebRootPath, path)))
				throw new Exception($"Tool file '{path}' does not exist.");

			return File(Io.File.ReadAllBytes(Path.Combine(env.WebRootPath, path)), MediaTypeNames.Application.Zip, $"{toolName}{tool.VersionNumber}.zip");
		}
	}
}