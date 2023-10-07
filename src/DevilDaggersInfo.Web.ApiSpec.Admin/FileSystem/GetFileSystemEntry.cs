namespace DevilDaggersInfo.Web.ApiSpec.Admin.FileSystem;

public record GetFileSystemEntry
{
	public required string Name { get; init; }

	public required int Count { get; init; }

	public required long Size { get; init; }
}
