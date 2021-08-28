using DevilDaggersInfo.Web.BlazorWasm.Server.Entities;
using DevilDaggersInfo.Web.BlazorWasm.Server.Enums;
using DevilDaggersInfo.Web.BlazorWasm.Server.Transients;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using Io = System.IO;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Controllers.Public;

[Route("api/tools")]
[ApiController]
public class ToolsController : ControllerBase
{
	private readonly IFileSystemService _fileSystemService;
	private readonly ApplicationDbContext _dbContext;
	private readonly IToolHelper _toolHelper;

	public ToolsController(IFileSystemService fileSystemService, ApplicationDbContext dbContext, IToolHelper toolHelper)
	{
		_fileSystemService = fileSystemService;
		_dbContext = dbContext;
		_toolHelper = toolHelper;
	}

	// This endpoint is still in use by DDSE/DDCL/DDAE.
	[Obsolete($"Use {nameof(GetTool)} instead.")]
	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult<List<GetTool>> GetToolsForTools(string? toolNameFilter = null)
	{
		IEnumerable<GetTool> tools = _toolHelper.Tools;
		if (!string.IsNullOrEmpty(toolNameFilter))
			tools = tools.Where(t => t.Name.Contains(toolNameFilter));
		return tools.ToList();
	}

	[HttpGet("{toolName}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public ActionResult<GetTool> GetTool([Required] string toolName)
	{
		GetTool? tool = _toolHelper.Tools.Find(t => t.Name == toolName);
		if (tool == null)
			return NotFound();

		string path = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.Tools), tool.Name, $"{tool.Name}{tool.VersionNumber}.zip");
		if (!Io.File.Exists(path))
			throw new($"Tool file '{path}' does not exist.");

		ToolStatisticEntity? toolStatistic = _dbContext.ToolStatistics
			.AsNoTracking()
			.FirstOrDefault(ts => ts.ToolName == tool.Name && ts.VersionNumber == tool.VersionNumber.ToString());

		tool.DownloadCount = toolStatistic?.DownloadCount ?? 0;
		tool.FileSize = (int)new FileInfo(path).Length;
		tool.SupportedOperatingSystems = new() { "Windows 64-bit" }; // TODO: Get this from database. Also, DDSE is actually supported on 32-bit, but DD itself isn't 32-bit anymore so probably not worth mentioning.

		return tool;
	}

	[HttpGet("{toolName}/file")]
	[ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public ActionResult GetToolFile([Required] string toolName)
	{
		GetTool? tool = _toolHelper.Tools.Find(t => t.Name == toolName);
		if (tool == null)
			return NotFound();

		string path = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.Tools), tool.Name, $"{tool.Name}{tool.VersionNumber}.zip");
		if (!Io.File.Exists(path))
			throw new($"Tool file '{path}' does not exist.");

		ToolStatisticEntity? toolStatistic = _dbContext.ToolStatistics.FirstOrDefault(ts => ts.ToolName == tool.Name && ts.VersionNumber == tool.VersionNumber.ToString());
		if (toolStatistic == null)
			_dbContext.ToolStatistics.Add(new ToolStatisticEntity { DownloadCount = 1, ToolName = tool.Name, VersionNumber = tool.VersionNumber.ToString() });
		else
			toolStatistic.DownloadCount++;

		_dbContext.SaveChanges();

		return File(Io.File.ReadAllBytes(path), MediaTypeNames.Application.Zip, $"{toolName}{tool.VersionNumber}.zip");
	}

	// TODO: Move to MemoryController.
	[HttpGet("devildaggerscustomleaderboards/settings")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult<GetDdclSettings> GetDdclSettingsForDdcl()
	{
		return JsonConvert.DeserializeObject<GetDdclSettings?>(Io.File.ReadAllText(Path.Combine(_fileSystemService.GetPath(DataSubDirectory.Tools), "DevilDaggersCustomLeaderboards", "Settings.json"))) ?? throw new("Could not deserialize DDCL settings JSON.");
	}
}
