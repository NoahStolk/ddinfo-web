namespace DevilDaggersInfo.Web.ApiSpec.Main.Tools;

public record GetToolDownloadCount
{
	public required string Version { get; init; }

	public required int Count { get; init; }
}
