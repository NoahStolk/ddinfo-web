namespace DevilDaggersInfo.Api.App.Updates;

public record GetLatestVersionFile
{
	public required byte[] ZipFileContents { get; init; }
}
