namespace DevilDaggersInfo.Web.Server.Domain.Models.Tools;

public class ToolVersionChange
{
	public string Description { get; init; } = string.Empty;

	public IReadOnlyList<ToolVersionChange>? SubChanges { get; init; }
}
