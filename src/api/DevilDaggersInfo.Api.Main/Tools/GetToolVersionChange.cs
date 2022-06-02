namespace DevilDaggersInfo.Api.Main.Tools;

public record GetToolVersionChange
{
	public string Description { get; init; } = null!;

	public IReadOnlyList<GetToolVersionChange>? SubChanges { get; init; }
}
