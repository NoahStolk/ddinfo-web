namespace DevilDaggersInfo.Web.ApiSpec.Admin.FileSystem;

public sealed record GetFileSystemEntry
{
	public required string Name { get; init; }

	public required int Count { get; init; }

	public required long Size { get; init; }
}
