namespace DevilDaggersInfo.Web.ApiSpec.Main.Tools;

public record GetToolVersionChange
{
	public required string Description { get; init; }

	public required IReadOnlyList<GetToolVersionChange>? SubChanges { get; init; }
}
