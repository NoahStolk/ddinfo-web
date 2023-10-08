namespace DevilDaggersInfo.Web.ApiSpec.Main.Tools;

[Obsolete("This data is no longer available.")]
public record GetToolVersionChange
{
	public string Description { get; init; } = string.Empty;

	public IReadOnlyList<GetToolVersionChange>? SubChanges { get; init; }
}
