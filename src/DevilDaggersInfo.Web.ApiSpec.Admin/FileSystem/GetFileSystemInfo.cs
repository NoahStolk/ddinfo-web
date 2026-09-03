namespace DevilDaggersInfo.Web.ApiSpec.Admin.FileSystem;

public sealed record GetFileSystemInfo
{
	public required long TotalSize { get; init; }

	public required long AllowedSizeByModArchiveProcessor { get; init; }

	public required List<GetFileSystemEntry> Entries { get; init; }
}
