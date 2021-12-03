using DevilDaggersWebsite.Entities;
using DevilDaggersWebsite.Transients;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
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
		private readonly IWebHostEnvironment _environment;
		private readonly ApplicationDbContext _dbContext;
		private readonly IToolHelper _toolHelper;

		public ToolsController(IWebHostEnvironment environment, ApplicationDbContext dbContext, IToolHelper toolHelper)
		{
			_environment = environment;
			_dbContext = dbContext;
			_toolHelper = toolHelper;
		}

		[HttpGet("{toolName}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult<Dto.Tool> GetTool(string toolName)
		{
			Dto.Tool? tool = _toolHelper.GetToolByName(toolName);
			return tool == null ? NotFound($"Tool '{toolName}' was not found.") : tool;
		}

		[HttpGet("{toolName}/file")]
		[ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult GetToolFile([Required] string toolName)
		{
			Dto.Tool? tool = _toolHelper.GetToolByName(toolName);
			if (tool == null)
				return new NotFoundObjectResult(new ProblemDetails { Title = $"Tool '{toolName}' was not found." });

			string path = Path.Combine("tools", tool.Name, $"{tool.Name}{tool.VersionNumber}.zip");
			if (!Io.File.Exists(Path.Combine(_environment.WebRootPath, path)))
				throw new Exception($"Tool file '{path}' does not exist.");

			ToolStatistic? toolStatistic = _dbContext.ToolStatistics.FirstOrDefault(ts => ts.ToolName == tool.Name && ts.VersionNumber == tool.VersionNumber.ToString());
			if (toolStatistic == null)
				_dbContext.ToolStatistics.Add(new ToolStatistic { DownloadCount = 1, ToolName = tool.Name, VersionNumber = tool.VersionNumber.ToString() });
			else
				toolStatistic.DownloadCount++;

			_dbContext.SaveChanges();

			return File(Io.File.ReadAllBytes(Path.Combine(_environment.WebRootPath, path)), MediaTypeNames.Application.Zip, $"{toolName}{tool.VersionNumber}.zip");
		}
	}
}
