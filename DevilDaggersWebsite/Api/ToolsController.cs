using DevilDaggersWebsite.Dto;
using DevilDaggersWebsite.Entities;
using DevilDaggersWebsite.Transients;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net.Mime;
using Io = System.IO;

namespace DevilDaggersWebsite.Api
{
	[Route("api/tools")]
	[ApiController]
	public class ToolsController : ControllerBase
	{
		private readonly IWebHostEnvironment _env;
		private readonly ApplicationDbContext _dbContext;
		private readonly IToolHelper _toolHelper;

		public ToolsController(IWebHostEnvironment env, ApplicationDbContext dbContext, IToolHelper toolHelper)
		{
			_env = env;
			_dbContext = dbContext;
			_toolHelper = toolHelper;
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public ActionResult<List<Tool>> GetTools(string? toolNameFilter = null)
		{
			IEnumerable<Tool> tools = _toolHelper.Tools;
			if (!string.IsNullOrEmpty(toolNameFilter))
				tools = tools.Where(t => t.Name.Contains(toolNameFilter));
			return tools.ToList();
		}

		[HttpGet("{toolName}/file")]
		[ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult GetToolFile([Required] string toolName)
		{
			Tool? tool = _toolHelper.Tools.Find(t => t.Name == toolName);
			if (tool == null)
				return new NotFoundObjectResult(new ProblemDetails { Title = $"Tool '{toolName}' was not found." });

			string path = Path.Combine("tools", tool.Name, $"{tool.Name}{tool.VersionNumber}.zip");
			if (!Io.File.Exists(Path.Combine(_env.WebRootPath, path)))
				throw new Exception($"Tool file '{path}' does not exist.");

			ToolStatistic? toolStatistic = _dbContext.ToolStatistics.FirstOrDefault(ts => ts.ToolName == tool.Name && ts.VersionNumber == tool.VersionNumber.ToString());
			if (toolStatistic == null)
				_dbContext.ToolStatistics.Add(new ToolStatistic { DownloadCount = 1, ToolName = tool.Name, VersionNumber = tool.VersionNumber.ToString() });
			else
				toolStatistic.DownloadCount++;

			_dbContext.SaveChanges();

			return File(Io.File.ReadAllBytes(Path.Combine(_env.WebRootPath, path)), MediaTypeNames.Application.Zip, $"{toolName}{tool.VersionNumber}.zip");
		}

		[HttpGet("devildaggerscustomleaderboards/settings")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public ActionResult<DdclSettings> GetDdclSettings()
		{
			return JsonConvert.DeserializeObject<DdclSettings?>(Io.File.ReadAllText(Path.Combine(_env.WebRootPath, "tools", "DevilDaggersCustomLeaderboards", "Settings.json"))) ?? throw new("Could not deserialize DDCL settings JSON.");
		}
	}
}
