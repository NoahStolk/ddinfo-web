namespace DevilDaggersInfo.Web.BlazorWasm.Server.Transients;

public class ToolHelper : IToolHelper
{
	private readonly ApplicationDbContext _dbContext;

	public ToolHelper(ApplicationDbContext dbContext, IFileSystemService fileSystemService)
	{
		_dbContext = dbContext;

		Changelogs = JsonConvert.DeserializeObject<Dictionary<string, List<ChangelogEntry>>>(File.ReadAllText(Path.Combine(fileSystemService.GetPath(DataSubDirectory.Tools), "Changelogs.json"))) ?? throw new("Could not deserialize Changelogs.json.");
	}

	public Dictionary<string, List<ChangelogEntry>> Changelogs { get; } = new();

	public Tool GetToolByName(string name)
	{
		ToolEntity? toolEntity = _dbContext.Tools.AsNoTracking().FirstOrDefault(t => t.Name == name);
		if (toolEntity == null)
			throw new($"Could not find tool with name {name} in database.");

		return GetToolFromEntity(toolEntity);
	}

	public Tool GetToolFromEntity(ToolEntity tool) => new()
	{
		Changelog = Changelogs.TryGetValue(tool.Name, out List<ChangelogEntry>? changelog) ? changelog : new List<ChangelogEntry>(),
		DisplayName = tool.DisplayName,
		Name = tool.Name,
		VersionNumber = Version.Parse(tool.CurrentVersionNumber),
		VersionNumberRequired = Version.Parse(tool.RequiredVersionNumber),
	};
}
