using DevilDaggersInfo.Web.BlazorWasm.Server.Enums;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Tools;
using Newtonsoft.Json;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Transients;

public class ToolHelper : IToolHelper
{
	public ToolHelper(IFileSystemService fileSystemService)
	{
		Tools = JsonConvert.DeserializeObject<List<GetTool>?>(File.ReadAllText(Path.Combine(fileSystemService.GetPath(DataSubDirectory.Tools), "Tools.json"))) ?? throw new("Could not deserialize tools JSON.");
	}

	public List<GetTool> Tools { get; } = new();

	public GetTool GetToolByName(string name)
	{
		GetTool? tool = Tools.Find(t => t.Name == name);
		if (tool == null)
			throw new($"Could not find tool with name {name}.");

		return tool;
	}
}
