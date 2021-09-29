using DevilDaggersInfo.Web.BlazorWasm.Server.Converters.Public;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Tools;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Tools.Obsolete;

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
	// TODO: Remove this endpoint, and stop using Tool classes (which correspond to the Tools.json format) as DTOs.
	[Obsolete($"Use {nameof(GetTool)} instead.")]
	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult<List<GetToolObsolete>> GetToolsForTools(string? toolNameFilter = null)
	{
		IQueryable<ToolEntity> tools = _dbContext.Tools.AsNoTracking();
		if (!string.IsNullOrEmpty(toolNameFilter))
			tools = tools.Where(t => t.Name.Contains(toolNameFilter));

		List<Tool> toolModels = tools.ToList().ConvertAll(t => _toolHelper.GetToolFromEntity(t));

		return toolModels
			.Select(t => new GetToolObsolete
			{
				Changelog = t.Changelog
					.Select(ce => new GetChangelogEntryObsolete
					{
						Changes = ce.Changes.Select(c => ToGetChange(c)).ToList(),
						Date = ce.Date,
						VersionNumber = ce.VersionNumber,
					})
					.ToList(),
				DisplayName = t.DisplayName,
				Name = t.Name,
				VersionNumber = t.VersionNumber,
				VersionNumberRequired = t.VersionNumberRequired,
			})
			.ToList();

		static GetChangeObsolete ToGetChange(Change change)
		{
			return new GetChangeObsolete
			{
				Description = change.Description,
				SubChanges = change.SubChanges?.Select(c => ToGetChange(c)).ToList(),
			};
		}
	}

	[HttpGet("{toolName}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public ActionResult<GetTool> GetTool([Required] string toolName)
	{
		Tool? tool = _toolHelper.GetToolByName(toolName);
		if (tool == null)
			return NotFound();

		string path = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.Tools), tool.Name, $"{tool.Name}{tool.VersionNumber}.zip");
		if (!IoFile.Exists(path))
			throw new($"Tool file '{path}' does not exist.");

		List<ToolStatisticEntity> toolStatistics = _dbContext.ToolStatistics
			.AsNoTracking()
			.Where(ts => ts.ToolName == tool.Name)
			.ToList();

		return tool.ToGetTool(toolStatistics, (int)new FileInfo(path).Length);
	}

	[HttpGet("{toolName}/file")]
	[ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public ActionResult GetToolFile([Required] string toolName)
	{
		Tool? tool = _toolHelper.GetToolByName(toolName);
		if (tool == null)
			return NotFound();

		string path = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.Tools), tool.Name, $"{tool.Name}{tool.VersionNumber}.zip");
		if (!IoFile.Exists(path))
			throw new($"Tool file '{path}' does not exist.");

		ToolStatisticEntity? toolStatistic = _dbContext.ToolStatistics.FirstOrDefault(ts => ts.ToolName == tool.Name && ts.VersionNumber == tool.VersionNumber.ToString());
		if (toolStatistic == null)
			_dbContext.ToolStatistics.Add(new ToolStatisticEntity { DownloadCount = 1, ToolName = tool.Name, VersionNumber = tool.VersionNumber.ToString() });
		else
			toolStatistic.DownloadCount++;

		_dbContext.SaveChanges();

		return File(IoFile.ReadAllBytes(path), MediaTypeNames.Application.Zip, $"{toolName}{tool.VersionNumber}.zip");
	}

	// TODO: Move to MemoryController.
	[HttpGet("devildaggerscustomleaderboards/settings")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult<GetDdclSettings> GetDdclSettingsForDdcl()
	{
		return JsonConvert.DeserializeObject<GetDdclSettings?>(IoFile.ReadAllText(Path.Combine(_fileSystemService.GetPath(DataSubDirectory.Tools), "DevilDaggersCustomLeaderboards", "Settings.json"))) ?? throw new("Could not deserialize DDCL settings JSON.");
	}
}
