namespace DevilDaggersInfo.Api.App.Updates;

public record GetLatestVersion
{
	public required string VersionNumber { get; init; }

	public required int FileSize { get; init; }
}
