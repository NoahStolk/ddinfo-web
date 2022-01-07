using DevilDaggersInfo.Web.BlazorWasm.Server.InternalModels;
using DevilDaggersInfo.Web.BlazorWasm.Server.InternalModels.Json;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Services;

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
		ToolEntity? tool = _dbContext.Tools.AsNoTracking().FirstOrDefault(t => t.Name == name);
		if (tool == null)
			throw new($"Could not find tool with name {name} in database.");

		return new()
		{
			Changelog = Changelogs.TryGetValue(tool.Name, out List<ChangelogEntry>? changelog) ? changelog : null,
			DisplayName = tool.DisplayName,
			Name = tool.Name,
			VersionNumber = Version.Parse(tool.CurrentVersionNumber),
			VersionNumberRequired = Version.Parse(tool.RequiredVersionNumber),
		};
	}
}
