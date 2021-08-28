namespace DevilDaggersInfo.Web.BlazorWasm.Server.Transients;

public class ToolHelper : IToolHelper
{
	public ToolHelper(IFileSystemService fileSystemService)
	{
		Tools = JsonConvert.DeserializeObject<List<Tool>>(File.ReadAllText(Path.Combine(fileSystemService.GetPath(DataSubDirectory.Tools), "Tools.json"))) ?? throw new("Could not deserialize tools JSON.");
	}

	public List<Tool> Tools { get; } = new();

	public Tool GetToolByName(string name)
	{
		Tool? tool = Tools.Find(t => t.Name == name);
		if (tool == null)
			throw new($"Could not find tool with name {name}.");

		return tool;
	}
}
