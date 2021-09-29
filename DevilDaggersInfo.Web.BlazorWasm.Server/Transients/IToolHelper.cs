namespace DevilDaggersInfo.Web.BlazorWasm.Server.Transients;

public interface IToolHelper
{
	Dictionary<string, List<ChangelogEntry>> Changelogs { get; }

	Tool GetToolByName(string name);

	Tool GetToolFromEntity(ToolEntity tool);
}
