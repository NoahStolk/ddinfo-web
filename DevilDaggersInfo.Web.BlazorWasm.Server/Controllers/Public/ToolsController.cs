using DevilDaggersInfo.Web.BlazorWasm.Server.Entities;
using DevilDaggersInfo.Web.BlazorWasm.Server.Transients;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Tools;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using Io = System.IO;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Controllers.Public;

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

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult<List<GetTool>> GetToolsForTools(string? toolNameFilter = null)
	{
		IEnumerable<GetTool> tools = _toolHelper.Tools;
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
		GetTool? tool = _toolHelper.Tools.Find(t => t.Name == toolName);
		if (tool == null)
			return new NotFoundObjectResult(new ProblemDetails { Title = $"Tool '{toolName}' was not found." });

		string path = Path.Combine("tools", tool.Name, $"{tool.Name}{tool.VersionNumber}.zip");
		if (!Io.File.Exists(Path.Combine(_environment.WebRootPath, path)))
			throw new Exception($"Tool file '{path}' does not exist.");

		ToolStatisticEntity? toolStatistic = _dbContext.ToolStatistics.FirstOrDefault(ts => ts.ToolName == tool.Name && ts.VersionNumber == tool.VersionNumber.ToString());
		if (toolStatistic == null)
			_dbContext.ToolStatistics.Add(new ToolStatisticEntity { DownloadCount = 1, ToolName = tool.Name, VersionNumber = tool.VersionNumber.ToString() });
		else
			toolStatistic.DownloadCount++;

		_dbContext.SaveChanges();

		return File(Io.File.ReadAllBytes(Path.Combine(_environment.WebRootPath, path)), MediaTypeNames.Application.Zip, $"{toolName}{tool.VersionNumber}.zip");
	}

	// TODO: Move to MemoryController.
	[HttpGet("devildaggerscustomleaderboards/settings")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult<GetDdclSettings> GetDdclSettingsForDdcl()
	{
		return JsonConvert.DeserializeObject<GetDdclSettings?>(Io.File.ReadAllText(Path.Combine(_environment.WebRootPath, "tools", "DevilDaggersCustomLeaderboards", "Settings.json"))) ?? throw new("Could not deserialize DDCL settings JSON.");
	}
}
