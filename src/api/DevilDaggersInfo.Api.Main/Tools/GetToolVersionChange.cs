namespace DevilDaggersInfo.Api.Main.Tools;

public record GetToolVersionChange
{
	public required string Description { get; init; }

	public IReadOnlyList<GetToolVersionChange>? SubChanges { get; init; }
}
